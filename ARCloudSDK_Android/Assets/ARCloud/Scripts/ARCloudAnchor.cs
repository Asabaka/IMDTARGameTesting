using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Vuplex.WebView;
using UnityEngine.Networking;

public enum ActiveType
{
    GameObject,
    Camera,
    Origin
}

public class ARCloudAnchor : MonoBehaviour
{
    private int deleteIndex;//ɾ���õ�key
    private GameObject arGo;
    private int m_SelectedPrefabIndex = 0;
    private PrefabSource m_PrefabSource;
    private string downloadurl;

    public GameObject ArGo { get => arGo; }
    public int SelectedPrefabIndex { get => m_SelectedPrefabIndex; }
    public string DownLoadUrl { get => downloadurl; }
    public PrefabSource prefabSource { get => m_PrefabSource; }

    public string uuid;//ΨһID
    private void Start()
    {
    }
    private void Update()
    {
    }


    public void Active(ActiveType type, int goIndex = -1, PrefabSource prefabSource = PrefabSource.Custom, int putInIndex = 0, string UUID = "", string path = "")
    {
        if (type == ActiveType.GameObject)
        {
            StartCoroutine(LoadAssetFromServer(path, goIndex, putInIndex));

        }
   
        if (string.IsNullOrEmpty(UUID))
        {
            uuid = System.Guid.NewGuid().ToString();
        }
        else
        {
            uuid = UUID;
        }
    }

    IEnumerator LoadAssetFromServer(string path, int goIndex, int putInIndex)
    {
        Debug.Log("模型地址" + path);
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(path);
        //2���ȴ����������з�����
        yield return request.SendWebRequest();
        //3������������֮�󣬾�Ҫ��DownloadHandlerAssetBundle���л�ȡһ��request���õ���������һ��AssetBundle�����

        AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);
        ARCloudManager.Instance.AddBundle(ab);

        string[] s = path.Split('/');
        string assetName = s[s.Length - 1];//path���һ��б�ܺ�ƴ�ӵľ�����Դ����

        //4���û�ȡ����AssetBundle����ȥ������Դ������GameObject
        var obj = ab.LoadAsset<GameObject>(assetName);
        //5��ʵ���������GameObject����
        if (obj != null)
        {
            GameObject go = Instantiate(obj);
            m_SelectedPrefabIndex = goIndex;
            SetObjParent(go, putInIndex);
        }

        ARCloudManager.Instance.CurrentLoadedAssetCount++;

        if (ARCloudManager.Instance.CurrentLoadedAssetCount == ARCloudManager.Instance.NeedLoadAssetCount)
        {
            ARCloudManager.Instance.isLoadingAssets = false;
            ARCloudManager.Instance.DelayHidLoadingObj();
            ARCloudManager.Instance.TipShow("");
            ARCloudManager.Instance.TipHide();
        }

    }
    void SetObjParent(GameObject go, int putInIndex)
    {
        go.transform.SetParent(transform, false);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        arGo = go;


    }
}

