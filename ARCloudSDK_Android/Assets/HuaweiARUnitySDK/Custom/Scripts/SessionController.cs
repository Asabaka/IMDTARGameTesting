using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuaweiARUnitySDK;

public class SessionController : MonoBehaviour
{
    public ARConfigBase config;
   // public BackGroundRenderer backGroundRenderer;
    public GameObject anchorPrefab;
    private bool surport = false;
    private bool isSessionCreated = false;
    private List<GameObject> anchorObjs;

    public bool IsSessionCreated { get => isSessionCreated; }

    // Start is called before the first frame update
    void Start()
    {
        anchorObjs = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        ARSession.Update();
    }

    public bool CheckAbility()
    {
        if (surport) return true;
        AREnginesAvaliblity ability = AREnginesSelector.Instance.CheckDeviceExecuteAbility();
        if ((AREnginesAvaliblity.HUAWEI_AR_ENGINE & ability) != 0)
        {
            AREnginesSelector.Instance.SetAREngine(AREnginesType.HUAWEI_AR_ENGINE);
            surport = true;
            return true;
        }
        surport = false;
     //   backGroundRenderer.enabled = false;
        return false;
    }

    public void CreateSession()
    {
        if (!surport)
        {
            CheckAbility();
        }
        if (!surport)
        {
            Debug.LogError("该设备不支持HuaWeiAREngine！");
            return;
        }
        if (isSessionCreated) return;
        if (AndroidPermissionsRequest.IsPermissionGranted("android.permission.CAMERA"))
        {
          //  backGroundRenderer.enabled = true;
            ARSession.CreateSession(); //create session
            isSessionCreated = true; //flag to indicate session is created
            ARSession.Config(config); //config with  HandARTrackingConfig
            ARSession.SetCameraTextureNameAuto(); //set external texture to receive camera feed automatically
            ARSession.SetDisplayGeometry(Screen.width, Screen.height); // set display width and height
            ARSession.Resume(); //resume session
            
        }
        else
        {
            Debug.LogError("没有相机权限！");
        }
    }

    public GameObject AddAnchor(Pose pose)
    {
        //return ARSession.AddAnchor(pose);
        //var anchor = ARSession.AddAnchor(pose);
        GameObject go = Instantiate(anchorPrefab, pose.position, pose.rotation);
        //go.GetComponent<HuaWeiARAnchor>().anchor = anchor;
        anchorObjs.Add(go);
        return go;
    }

    public void StopSession()
    {
        foreach (var obj in anchorObjs)
        {
            Destroy(obj);
        }
        anchorObjs.Clear();
        //ARSession.Stop();
    }

    public void DestroySession()
    {
        StopSession();
        ARSession.Stop();
        isSessionCreated = false;
    }

}
