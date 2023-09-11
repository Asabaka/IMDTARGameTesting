using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuaweiARUnitySDK;


public class HuaWeiARAnchor : MonoBehaviour
{
    // Start is called before the first frame update
    public ARAnchor anchor;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (anchor == null) return;
        var pose = anchor.GetPose();
        transform.position = pose.position;
        transform.rotation = pose.rotation;
    }
}
