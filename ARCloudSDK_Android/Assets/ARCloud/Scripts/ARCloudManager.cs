using DG.Tweening;
using LitJson;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using DG.Tweening;

public class ARCloudManager : MonoSingleton<ARCloudManager>
{
    public UIController m_UIController;
    public Transform m_ARCloudParent;//����ת��������ϵ�ľ���
    public Camera arCamera;
  //  public ARAnchorManager m_ArAnchorManager;
    public ARSession m_ARSession;
    // public AROcclusionManager m_ArOcclusionManager;
    //  public ARMeshManager m_ARMeshManager;
    public SessionController huaweiARSessionController;
    public Text tipText;
    public GameObject mARPrefab;
    public bool isLoadingAssets = false;//�Ƿ����ڼ�����Դ
  //  public GameObject loadingObj;//������Դ�ĵȴ�ģ��
    private bool isScanSuccess = false;
    private int itemIndex = 0;
    private string m_CurDataId = "";
    private GameObject editGo;
    private Pose m_CameraUpPose;
    private Matrix4x4 m_CameraUpLocalToWorldMt;
    private ARAnchor m_CameraAnchor;
    private GameObject m_EditOrigin;
    private List<GameObject> editObjs;
    private List<GameObject> m_EditAnchors;
    private ARAnchor m_ARCloudOriginAnchor;
    private List<AssetBundle> loadedBundles;
    // Start is called before the first frame update
    void Start()
    {
        editObjs = new List<GameObject>();
        m_EditAnchors = new List<GameObject>();
        loadedBundles = new List<AssetBundle>();
#if UNITY_EDITOR
        m_ARSession.enabled = true;
#elif UNITY_IOS
        m_ARSession.enabled = true;
        Input.gyro.enabled =false;
#elif UNITY_ANDROID
        m_ARSession.enabled = false;//����
        Input.gyro.enabled =false;
        StartCoroutine(CheckAndroidSupport());

#endif
        Invoke("CaptureCameraAndLoadData", 1f);
        m_UIController.ShowScanObj();
    }
    IEnumerator CheckAndroidSupport()
    {

        TipShow("Checking for AR support...");

        yield return ARSession.CheckAvailability();

        if (ARSession.state == ARSessionState.NeedsInstall)
        {
            TipShow("Your device supports AR, but requires a software update.");
            TipShow("Attempting install...");
            TipHide();
            yield return ARSession.Install();
        }

        if (ARSession.state == ARSessionState.Ready)
        {
            TipShow("该设备支持AR");
            TipHide();
            // To start the ARSession, we just need to enable it.
            m_ARSession.enabled = true;
        }
        else
        {
            switch (ARSession.state)
            {
                case ARSessionState.Unsupported:
                    TipShow("Your device does not support AR.");
                    TipHide();
                    break;
                case ARSessionState.NeedsInstall:
                    TipShow("The software update failed, or you declined the update.");
                    TipHide();
                    // In this case, we enable a button which allows the user
                    // to try again in the event they decline the update the first time.
                    break;
            }

            TipShow("\n[Start non-AR experience instead]");
            TipHide();
            //
            // Start a non-AR fallback experience here...
            //
        }
    }
    float time = 0;
    // Update is called once per frame
    void Update()
    {
        if (!isLoadingAssets)
        {
            // ���û���ڼ�����Դ������ɨ��У׼
            time += Time.deltaTime;
            if (time > 5)
            {
                time = 0;
                CaptureCameraAndLoadData();
            }
          //  m_UIController.loading.gameObject.SetActive(false);

        }
        else
        {
            //����ڼ�����Դ��
        //    m_UIController.loading.gameObject.SetActive(true);

        }
   

    }

    public void OnClickAdd()
    {
        foreach (var item in m_EditAnchors)
        {
            item.transform.position = item.transform.position + new Vector3(0, 1, 0);
        }
        foreach (var item in editObjs)
        {
            item.transform.position = item.transform.position + new Vector3(0, 1, 0);
        }
        TipShow(m_EditAnchors.Count.ToString() + "POS" + m_EditAnchors[0].transform.GetChild(1).transform.position);
        //   TipShow(editObjs.Count.ToString() + "POS" + editObjs[0].transform.GetChild(1).transform.position);
    }
    public void OnClickdic()
    {
        foreach (var item in m_EditAnchors)
        {
            item.transform.position = item.transform.position + new Vector3(0, -1, 0);
        }
        foreach (var item in editObjs)
        {
            item.transform.position = item.transform.position + new Vector3(0, -1, 0);
        }
        TipShow(m_EditAnchors.Count.ToString() + "POS" + m_EditAnchors[0].transform.GetChild(1).transform.position);
        //  TipShow(editObjs.Count.ToString() + "POS" + editObjs[0].transform.GetChild(1).transform.position);
    }
    public void DelayHidLoadingObj()
    {
        StartCoroutine(HideLoadingObj());
    }
    IEnumerator HideLoadingObj()
    {
        yield return new WaitForSeconds(0.1f);
      //  loadingObj.SetActive(false);
        m_UIController.ShowScanSuccess();
    }
    Coroutine captureCoroutine = null;
    public void CaptureCameraAndLoadData()
    {
        if (captureCoroutine != null)
        {
            StopCoroutine(captureCoroutine);
        }
        captureCoroutine = StartCoroutine(CaptureShotAndLoadData());
    }
    IEnumerator CaptureShotAndLoadData()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("");
        var width = 1080;
        var height = width * Screen.height / Screen.width;
        RenderTexture rt;
        Rect rect;
        if (Application.platform == RuntimePlatform.Android)
        {
            rt = new RenderTexture(width, height, 3);
            rect = new Rect(new Vector2(0, 0), new Vector2(width, height));
        }
        else
        {
            rt = new RenderTexture(Screen.width, Screen.height, 3);
            rect = new Rect(new Vector2(0, 0), new Vector2(Screen.width, Screen.height));
        }
        arCamera.targetTexture = rt;
        arCamera.Render();
        RenderTexture.active = rt;
        Texture2D screenShot;
        if (Application.platform == RuntimePlatform.Android)
        {
            screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
        }
        else
        {
            screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        }
        screenShot.ReadPixels(rect, 0, 0);// ע�����ʱ�����Ǵ�RenderTexture.active�ж�ȡ����  ��ֻ�������߳�ִ��
        screenShot.Apply();

        arCamera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors  
        GameObject.Destroy(rt);

#if UNITY_EDITOR
        if (editGo != null)
        {
            Destroy(editGo);
            m_CameraUpPose = Pose.identity;
        }
        m_CameraUpPose = new Pose(arCamera.transform.position, arCamera.transform.rotation);
        m_CameraUpLocalToWorldMt = arCamera.transform.localToWorldMatrix;
        editGo = Instantiate(mARPrefab, arCamera.transform.position, arCamera.transform.rotation);
        editGo.GetComponent<ARCloudAnchor>().Active(ActiveType.Camera);
#else
        if(Input.gyro.enabled == true)
        {
            if (editGo != null)
            {
                Destroy(editGo);
                m_CameraUpPose = Pose.identity;
            }
            m_CameraUpPose = new Pose(arCamera.transform.position, arCamera.transform.rotation);
            m_CameraUpLocalToWorldMt = arCamera.transform.localToWorldMatrix;
            editGo = Instantiate(mARPrefab, arCamera.transform.position, arCamera.transform.rotation);
            editGo.GetComponent<ARCloudAnchor>().Active(ActiveType.Camera);
        }
        else
        {
            if (m_CameraAnchor != null)
            {
              //  m_ArAnchorManager.RemoveAnchor(m_CameraAnchor);
                RemoveAnchor(m_CameraAnchor, true);
                m_CameraUpPose = Pose.identity;
            }
            m_CameraUpPose = new Pose(arCamera.transform.position, arCamera.transform.rotation);
            m_CameraUpLocalToWorldMt = arCamera.transform.localToWorldMatrix;
            m_CameraAnchor = AddAnchor(mARPrefab, m_CameraUpPose);
           // m_CameraAnchor = m_ArAnchorManager.AddAnchor(m_CameraUpPose);
            m_CameraAnchor.gameObject.GetComponent<ARCloudAnchor>().Active(ActiveType.Camera);
        }
#endif

        string url = "http://wxgame.jicf.net:29004//ARcloud/scene/scan";

        byte[] imageData = { };

#if UNITY_EDITOR
        imageData = ImgFromPath("Assets/ARCloud/ARCloud3.jpg");
#else
        imageData = screenShot.EncodeToJPG();
#endif
        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "scene_id", "Null" }
        };

        Dictionary<string, byte[]> bParameters = new Dictionary<string, byte[]>
        {
            { "file", imageData }
        };

        Dictionary<string, string> sParameters = new Dictionary<string, string>
        {
#if UNITY_EDITOR
            { "model", "iPhone10,6" }
#else
            { "model", SystemInfo.deviceModel }
#endif
        };

        PostFormData(url, headers, bParameters, sParameters, OnScanResponseAndLoadData);

    }
    
    public void PostFormData(string serverURL, Dictionary<string, string> headers, Dictionary<string, byte[]> bParameters, Dictionary<string, string> sParameters, Action<bool, string, byte[]> actionResult, string contentType = "multipart/form-data")
    {
        //  TipShow("���ڸ�����������ͼƬ");
        StartCoroutine(_PostFormData(serverURL, headers, bParameters, sParameters, actionResult, contentType));
    }
    IEnumerator _PostFormData(string serverURL, Dictionary<string, string> headers, Dictionary<string, byte[]> bParameters, Dictionary<string, string> sParameters, Action<bool, string, byte[]> actionResult, string contentType)
    {
        Debug.Log(serverURL);
        WWWForm form = new WWWForm();
        if (headers != null)
        {
            foreach (var item in headers)
            {
                form.AddField(item.Key, item.Value);
            }
        }
        if (bParameters != null)
        {
            foreach (var item in bParameters)
            {
                form.AddBinaryData(item.Key, item.Value);
            }
        }
        if (sParameters != null)
        {
            foreach (var item in sParameters)
            {
                form.AddField(item.Key, item.Value);
            }
        }
        UnityWebRequest request = UnityWebRequest.Post(serverURL, form);
        request.useHttpContinue = false;

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
            //   TipShow("����������ʧ��"+ request.error);
            actionResult?.Invoke(false, request.error, request.downloadHandler.data);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            // TipShow("���������ͳɹ�");
            actionResult?.Invoke(true, request.downloadHandler.text, request.downloadHandler.data);
        }
    }
    public void OnScanResponseAndLoadData(bool success, string result, byte[] data)
    {
        if (success)
        {
            // m_UIController.SetDebugInfo(result);

            JsonData resultData = JsonMapper.ToObject(result);
            //  TipShow("ɨ�蹹������Code:"+ resultData["code"].ToString());
            if (resultData["code"].ToString() == "200")
            {
                JsonData rawMatrixArray = resultData["data"]["camera_position"];
                Matrix4x4 matrix = new Matrix4x4();
                float scale = float.Parse(resultData["data"]["scale"].ToString());
                matrix.SetRow(0, new Vector4(float.Parse(rawMatrixArray[0].ToString()), float.Parse(rawMatrixArray[1].ToString()), float.Parse(rawMatrixArray[2].ToString()), scale * float.Parse(rawMatrixArray[3].ToString())));
                matrix.SetRow(1, new Vector4(float.Parse(rawMatrixArray[4].ToString()), float.Parse(rawMatrixArray[5].ToString()), float.Parse(rawMatrixArray[6].ToString()), scale * float.Parse(rawMatrixArray[7].ToString())));
                matrix.SetRow(2, new Vector4(float.Parse(rawMatrixArray[8].ToString()), float.Parse(rawMatrixArray[9].ToString()), float.Parse(rawMatrixArray[10].ToString()), scale * float.Parse(rawMatrixArray[11].ToString())));
                matrix.SetRow(3, new Vector4(0, 0, 0, 1));
                var newSceneID = resultData["data"]["scenes_id"].ToString();
                if (newSceneID != m_CurDataId) {
                    ResetAR();
                }
                m_CurDataId = newSceneID;
                MatchARCloudAndLoadData(matrix);

                isScanSuccess = true;
                //      TipShow("ɨ�蹹������ɹ�");
                return;
            }
            else
            {
                //   TipShow("OnScanResponseAndLoadDat ����ʧ�ܣ�"+ result);
            }
        }
    }
 
    public void MatchARCloudAndLoadData(Matrix4x4 cameraMatrix)
    {
        //ARCloud����ԭ�����
        var arCloudOriginMatrix = m_CameraUpLocalToWorldMt * m_ARCloudParent.localToWorldMatrix * cameraMatrix;

        var arCloudOriginRotation = arCloudOriginMatrix.ExtractRotation();
        Matrix4x4 oldOrign = Matrix4x4.zero;
#if UNITY_EDITOR

        if (m_EditOrigin != null)
        {
            oldOrign = m_EditOrigin.transform.worldToLocalMatrix;
            //�µ�ԭ�����ԭ����ƫ����
            //Pose newOriginPosOffset = m_EditOrigin.transform.InverseTransformPose(new Pose(arCloudOriginMatrix.ExtractPosition(), arCloudOriginRotation));
            GameObject.Destroy(m_EditOrigin);

        }
        m_EditOrigin = Instantiate(mARPrefab, arCloudOriginMatrix.ExtractPosition(), arCloudOriginRotation);
        m_EditOrigin.transform.localScale = arCloudOriginMatrix.ExtractScale();
        if (oldOrign != Matrix4x4.zero)
        {
            RelocationAllAnchor(oldOrign, m_EditOrigin.transform);
        }
        m_EditOrigin.GetComponent<ARCloudAnchor>().Active(ActiveType.Origin);
        Debug.Log("m_EditOrigin" + m_EditOrigin.transform.name);
#else
        if(Input.gyro.enabled == true)
        {
            m_EditOrigin = Instantiate(mARPrefab, arCloudOriginMatrix.ExtractPosition(), arCloudOriginRotation);
            m_EditOrigin.transform.localScale = arCloudOriginMatrix.ExtractScale();
            m_EditOrigin.GetComponent<ARCloudAnchor>().Active(ActiveType.Origin);
             Debug.Log("m_EditOrigin2"+m_EditOrigin.transform.name);
        }
        else
        {
            if(huaweiARSessionController != null && huaweiARSessionController.CheckAbility() && huaweiARSessionController.IsSessionCreated)
            {
                m_EditOrigin = Instantiate(mARPrefab, arCloudOriginMatrix.ExtractPosition(), arCloudOriginRotation);
                m_EditOrigin.transform.localScale = arCloudOriginMatrix.ExtractScale();
                m_EditOrigin.GetComponent<ARCloudAnchor>().Active(ActiveType.Origin);
            }
            else
            {
                if (m_ARCloudOriginAnchor != null)
                {
                    oldOrign = m_ARCloudOriginAnchor.transform.worldToLocalMatrix;
                    //�µ�ԭ�����ԭ����ƫ����
                    //Pose newOriginPosOffset =  m_ARCloudOriginAnchor.transform.InverseTransformPose(new Pose(arCloudOriginMatrix.ExtractPosition(), arCloudOriginRotation));

                    // m_ArAnchorManager.RemoveAnchor(m_ARCloudOriginAnchor);
                    RemoveAnchor(m_ARCloudOriginAnchor, true);
                }

              //   m_ARCloudOriginAnchor = m_ArAnchorManager.AddAnchor(new Pose(arCloudOriginMatrix.ExtractPosition(), arCloudOriginRotation));
                m_ARCloudOriginAnchor = AddAnchor(mARPrefab, new Pose(arCloudOriginMatrix.ExtractPosition(), arCloudOriginRotation));
                m_ARCloudOriginAnchor.transform.localScale = arCloudOriginMatrix.ExtractScale();
                if (oldOrign != Matrix4x4.zero) {
                    RelocationAllAnchor(oldOrign, m_ARCloudOriginAnchor.transform);
                }
                m_ARCloudOriginAnchor.gameObject.GetComponent<ARCloudAnchor>().Active(ActiveType.Origin);
                Debug.Log("m_ARCloudOriginAnchor"+m_ARCloudOriginAnchor.transform.name);
            }
        }
#endif

        if (!isScanSuccess)
        {
            LoadARCloudData();


        }


    }
    public void LoadARCloudData()
    {
        string url = "http://wxgame.jicf.net:29004/file/download/";
        Dictionary<string, string> sParameters = new Dictionary<string, string>
        {
            //{ "fileid", m_CurDataId },
            { "fileid", m_CurDataId },
            { "filetype",  "AR" }
        };
        PostFormData(url, null, null, sParameters, OnLoadResponse);

    }
    public ARAnchor AddAnchor(GameObject prefab, Pose pos)
    {
        GameObject obj = Instantiate(prefab, pos.position, pos.rotation);
        if (obj.GetComponent<ARAnchor>() == null)
        {
            obj.AddComponent<ARAnchor>();
        }
        return obj.GetComponent<ARAnchor>();
    }
    public void DelayAddAnchorComponent(GameObject obj)
    {
        StartCoroutine(IEDelayAddAnchor(obj));
    }

    IEnumerator IEDelayAddAnchor(GameObject obj)
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("delayaddanchor" + obj.name);
        if (obj.GetComponent<ARAnchor>() == null)
        {
            obj.AddComponent<ARAnchor>();
        }
    }
    public ARAnchor AddAnchorComponent(GameObject obj)
    {
        if (obj.GetComponent<ARAnchor>() == null)
        {
            obj.AddComponent<ARAnchor>();
        }
        return obj.GetComponent<ARAnchor>();

    }
    public void AddBundle(AssetBundle ab)
    {
        loadedBundles.Add(ab);
    }
    public void ClearBundles()
    {
        foreach (var item in loadedBundles)
        {
            item.Unload(true);
        }
        loadedBundles.Clear();
    }
    public void ResetAR()
    {
        RemoveAllAnchor();
        isScanSuccess = false;
        isLoadingAssets = false;
        CurrentLoadedAssetCount = 0;
        NeedLoadAssetCount = 0;
        ClearBundles();
        if (captureCoroutine != null)
        {
            StopCoroutine(captureCoroutine);
        }
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }
    void RemoveEditObj(GameObject obj)
    {
        if (!editObjs.Contains(obj)) return;
        editObjs.Remove(obj);
        Destroy(obj);
    }
    void RemoveAnchor(GameObject anchor)
    {
        if (!m_EditAnchors.Contains(anchor)) return;
        m_EditAnchors.Remove(anchor);
        Destroy(anchor);
    }
    public void RemoveAllAnchor()
    {
#if UNITY_EDITOR
        for (int i = editObjs.Count - 1; i >= 0; i--)
        {
            var obj = editObjs[i];
            RemoveEditObj(obj);
        }
#else
        for (int i = m_EditAnchors.Count - 1; i >= 0; i--)
        {
            var anchor = m_EditAnchors[i];
            RemoveAnchor(anchor);
        }
      
#endif
    }
    public void RemoveAnchor(ARAnchor anchor, bool isDestoryObj)
    {
        Destroy(anchor);
        if (isDestoryObj)
        {
            Destroy(anchor.gameObject);
        }
    }
    public void OnLoadResponse(bool success, string result, byte[] data)
    {
        if (success)
        {
            m_UIController.HideScanObj();
            if (data.Length > 0)
            {
                var arCloudData = BinaryTool.ProtoDeSerialize<ARCloudData>(data);
                LoadData(arCloudData);
            }
            else
            {
                //  m_UIController.SetDebugInfo("����ʧ��:" + data.Length);
                //if (m_UIController.loadFailure != null) {
                //    m_UIController.loadFailure.gameObject.SetActive(true);
                //    m_UIController.loadFailure.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack).OnComplete(
                //    delegate {
                //        m_UIController.loadFailure.transform.DOLocalMoveY(1500f, 1.5f).SetEase(Ease.InOutQuad);
                //    }
                //    );
                //}

            }
        }
        else
        {
            //  m_UIController.SetDebugInfo("����ʧ��:" + data.Length);
            //if (m_UIController.loadFailure != null) {
            //    m_UIController.loadFailure.gameObject.SetActive(true);
            //    m_UIController.loadFailure.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack).OnComplete(
            //    delegate {
            //        m_UIController.loadFailure.transform.DOLocalMoveY(1500f, 1.5f).SetEase(Ease.InOutQuad);
            //    }
            //    );
            //}

        }



    }
    private int needLoadAssetCount = 0;//��Ҫ���ص�����
    int currentLoadedAssetCount = 0;//��ǰ�Ѿ����ص�����
    public int NeedLoadAssetCount { get => needLoadAssetCount; set => needLoadAssetCount = value; }
    public int CurrentLoadedAssetCount { get => currentLoadedAssetCount; set => currentLoadedAssetCount = value; }

    void LoadData(ARCloudData data)
    {
        isLoadingAssets = true;
      //  loadingObj.SetActive(true);
        NeedLoadAssetCount = data.ARCloudPoints.Count;
        foreach (var point in data.ARCloudPoints)
        {
            PlantLoadedPointPlayer(point);

        }
    }
    
    void PlantLoadedPointPlayer(ARCloudPoint point)
    {

        var pointLocalPose = new Pose(new Vector3((float)point.PosX, (float)point.PosY, (float)point.PosZ), Quaternion.Euler((float)point.Pitch, (float)point.Yaw, (float)point.Roll));
#if UNITY_EDITOR
        var pointPose = m_EditOrigin.transform.TransformPose(pointLocalPose);

        var go = Instantiate(mARPrefab, pointPose.position, pointPose.rotation);
        go.transform.localScale = new Vector3((float)point.Scale_X, (float)point.Scale_Y, (float)point.Scale_Z);
        editObjs.Add(go);
        itemIndex++;


        string prefabUrl = point.DownLoadUrl;
        string[] s = prefabUrl.Split('/');
        string assetName = s[s.Length - 1];//path���һ��б�ܺ�ƴ�ӵľ�����Դ����
        string userNumber = s[s.Length - 3];
 
        prefabUrl = ConStr.WEBURL+ "/AR/user/" + userNumber + "/armodelandroid/" + assetName;
        Debug.Log("资源下载地址" + prefabUrl);
        go.GetComponent<ARCloudAnchor>().Active(ActiveType.GameObject, point.PrefabIndex, point.Source, itemIndex, point.UUID, prefabUrl);


#else
                if(Input.gyro.enabled == true)
                {
                    var pointPose = m_EditOrigin.transform.TransformPose(pointLocalPose);
                    var go = Instantiate(mARPrefab, pointPose.position, pointPose.rotation);
                    go.transform.localScale = new Vector3((float)point.Scale_X, (float)point.Scale_Y, (float)point.Scale_Z);
                    editObjs.Add(go);
                    itemIndex++;
                    Debug.Log("gyro");
                    go.GetComponent<ARCloudAnchor>().Active(ActiveType.GameObject, point.PrefabIndex,point.Source, itemIndex, point.UUID,point.DownLoadUrl);
                }
                else
                {
                    if(huaweiARSessionController != null && huaweiARSessionController.CheckAbility() && huaweiARSessionController.IsSessionCreated)
                    {
                        var pointPose1 = m_EditOrigin.transform.TransformPose(pointLocalPose);
                        var anchor1 = huaweiARSessionController.AddAnchor(pointPose1);
                        itemIndex++;
                        anchor1.transform.localScale = new Vector3((float)point.Scale_X, (float)point.Scale_Y, (float)point.Scale_Z);
                        anchor1.gameObject.GetComponent<ARCloudAnchor>().Active(ActiveType.GameObject, point.PrefabIndex,point.Source, itemIndex, point.UUID,point.DownLoadUrl);
                        return;
                    }
           
            var pointPose = m_ARCloudOriginAnchor.transform.TransformPose(pointLocalPose);
                //  var anchor = m_ArAnchorManager.AddAnchor(pointPose);
                  
                    var anchor = AddAnchor(mARPrefab, pointPose);
                    anchor.transform.localScale = new Vector3((float)point.Scale_X, (float)point.Scale_Y, (float)point.Scale_Z);

                    itemIndex++;
                    string prefabUrl = point.DownLoadUrl;
#if UNITY_IOS

#elif UNITY_ANDROID
                        string[] s = prefabUrl.Split('/');
                        string assetName = s[s.Length - 1];//path���һ��б�ܺ�ƴ�ӵľ�����Դ����
                         string userNumber = s[s.Length - 3];
                        
                         prefabUrl = ConStr.WEBURL+ "/AR/user/" + userNumber + "/armodelandroid/" + assetName;
                          Debug.Log("资源下载地址" + prefabUrl);
                       
#endif
                        
                        anchor.gameObject.GetComponent<ARCloudAnchor>().Active(ActiveType.GameObject, point.PrefabIndex,point.Source,0,point.UUID, prefabUrl);
                       
                         m_EditAnchors.Add(anchor.gameObject);
                        
                        return;

                }
#endif


    }
    /// �ض�λ����ê��
    /// </summary>
    /// <param name="offsetPos">ƫ����</param>
    public void RelocationAllAnchor(Matrix4x4 oldOrign, Transform newOrign)
    {
#if UNITY_EDITOR
        foreach (var item in editObjs)
        {
            Matrix4x4 itemMatrix = item.transform.localToWorldMatrix;
            Matrix4x4 arCloudLocalPoseMatrix = oldOrign * itemMatrix;
            Pose arCloudLocalPose = new Pose(arCloudLocalPoseMatrix.GetColumn(3), arCloudLocalPoseMatrix.rotation);
            Pose newPos = newOrign.transform.TransformPose(arCloudLocalPose);
            item.transform.rotation = newPos.rotation;
            item.transform.position = newPos.position;
        }
#else
        for (int i = m_EditAnchors.Count - 1; i >= 0; i--)
        {

            var anchor = m_EditAnchors[i];
      
            RemoveAnchor(anchor.GetComponent<ARAnchor>(), false);
            Matrix4x4 itemMatrix = anchor.transform.localToWorldMatrix;
            Matrix4x4 arCloudLocalPoseMatrix = oldOrign * itemMatrix;
            Pose arCloudLocalPose = new Pose(arCloudLocalPoseMatrix.GetColumn(3), arCloudLocalPoseMatrix.rotation);
            Pose newPos = newOrign.transform.TransformPose(arCloudLocalPose);
            anchor.transform.DOMove(newPos.position, 0.1f);
            anchor.transform.DORotate(newPos.rotation.eulerAngles,0.1f);
            //anchor.transform.rotation = newPos.rotation;
            //anchor.transform.position = newPos.position;
            // AddAnchorComponent(obj);
            DelayAddAnchorComponent(anchor);
        }
      
#endif

    }
    public void DelaySetPos(GameObject obj, Pose offsetPos)
    {
        StartCoroutine(IEDelaySetPos(obj, offsetPos));
    }
    IEnumerator IEDelaySetPos(GameObject obj, Pose offsetPos)
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("delaysetpos" + offsetPos.position);
        Pose newPos = obj.transform.TransformPose(offsetPos);
        obj.transform.rotation = newPos.rotation;
        obj.transform.position = newPos.position;
    }
    public byte[] ImgFromPath(string path)
    {
        try
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, (int)fs.Length);
            //string base64String = Convert.ToBase64String(buffer);
            return buffer;
        }
        catch (Exception e)
        {
            Debug.LogError("ImgToBytes ת��ʧ��:" + e.Message);
            return new byte[] { };
        }
    }

    public void TipShow(string tipContent)
    {
        tipText.gameObject.SetActive(true);
        tipText.text = tipContent;
        tipText.GetComponent<Text>().text = tipContent;
    }

    public void TipHide()
    {

        Invoke("TipActive", 3);
    }

    public void TipActive()
    {
        tipText.gameObject.SetActive(false);
    }
}
[ProtoContract]
[System.Serializable]
public class ARCloudData
{
    [ProtoMember(1)]
    public List<ARCloudPoint> ARCloudPoints { get; set; }
}
[ProtoContract]
[System.Serializable]
public enum PrefabSource
{
    Public,
    Custom,
}

[ProtoContract]
[System.Serializable]
public class ARCloudPoint
{
    [ProtoMember(1)]
    public double PosX { get; set; }
    [ProtoMember(2)]
    public double PosY { get; set; }
    [ProtoMember(3)]
    public double PosZ { get; set; }
    [ProtoMember(4)]
    public double Roll { get; set; }
    [ProtoMember(5)]
    public double Pitch { get; set; }
    [ProtoMember(6)]
    public double Yaw { get; set; }
    [ProtoMember(7)]
    public double Scale_X { get; set; }
    [ProtoMember(8)]
    public double Scale_Y { get; set; }
    [ProtoMember(9)]
    public double Scale_Z { get; set; }
    [ProtoMember(10)]
    public int PrefabIndex { get; set; }
    [ProtoMember(11)]
    public string PrefabPath { get; set; }
    [ProtoMember(12)]
    public PrefabSource Source { get; set; }
    [ProtoMember(13)]
    public string UUID { get; set; }
    [ProtoMember(14)]
    public string DownLoadUrl { get; set; }
}
