﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HttpRequestManager : MonoSingleton<HttpRequestManager>
{
    /*
    UnityWebRequest uwr = new UnityWebRequest();
    uwr.url = "http://www.mysite.com";
    uwr.method = UnityWebRequest.kHttpVerbGET;   // can be set to any custom method, common constants privided

    uwr.useHttpContinue = false;
    uwr.chunkedTransfer = false;
    uwr.redirectLimit = 0;  // disable redirects
    uwr.timeout = 60;       // don't make this small, web requests do take some time 
    */

    /// <summary>
    /// GET请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="action"></param>
    public void Get(string url, Action<UnityWebRequest> actionResult)
    {
        StartCoroutine(_Get(url, actionResult));
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="downloadFilePathAndName">储存文件的路径和文件名 like 'Application.persistentDataPath+"/unity3d.html"'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求对象</param>
    /// <returns></returns>
    public void DownloadFile(string url, string downloadFilePathAndName, Action<UnityWebRequest> actionResult)
    {
        StartCoroutine(_DownloadFile(url, downloadFilePathAndName, actionResult));
    }

    /// <summary>
    /// 请求图片
    /// </summary>
    /// <param name="url">图片地址,like 'http://www.my-server.com/image.png '</param>
    /// <param name="action">请求发起后处理回调结果的委托,处理请求结果的图片</param>
    /// <returns></returns>
    public void GetTexture(string url, Action<Texture2D> actionResult)
    {
        StartCoroutine(_GetTexture(url, actionResult));
    }

    /// <summary>
    /// 请求AssetBundle
    /// </summary>
    /// <param name="url">AssetBundle地址,like 'http://www.my-server.com/myData.unity3d'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AssetBundle</param>
    /// <returns></returns>
    public void GetAssetBundle(string url, Action<AssetBundle> actionResult)
    {
        StartCoroutine(_GetAssetBundle(url, actionResult));
    }

    /// <summary>
    /// 请求服务器地址上的音效
    /// </summary>
    /// <param name="url">没有音效地址,like 'http://myserver.com/mysound.wav'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AudioClip</param>
    /// <param name="audioType">音效类型</param>
    /// <returns></returns>
    public void GetAudioClip(string url, Action<AudioClip> actionResult, AudioType audioType = AudioType.WAV)
    {
        StartCoroutine(_GetAudioClip(url, actionResult, audioType));
    }

    /// <summary>
    /// 向服务器提交post请求
    /// </summary>
    /// <param name="serverURL">服务器请求目标地址,like "http://www.my-server.com/myform"</param>
    /// <param name="lstformData">form表单参数</param>
    /// <param name="lstformData">处理返回结果的委托,处理请求对象</param>
    /// <returns></returns>
    public void Post(string serverURL, List<IMultipartFormSection> lstformData, Action<UnityWebRequest> actionResult)
    {
        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));

        StartCoroutine(_Post(serverURL, lstformData, actionResult));
    }

    /// <summary>
    /// 向服务器提交post请求
    /// </summary>
    /// <param name="serverURL">服务器请求目标地址,like "http://www.my-server.com/myform"</param>
    /// <param name="headers">请求头</param>
    /// <param name="parameters">请求参数</param>
    /// <param name="actionResult">回调action</param>
    /// <returns></returns>
    public void Post(string serverURL, Dictionary<string, string> headers, Dictionary<string, string> parameters, Action<bool, string> actionResult, string contentType = "application/json")
    {
        StartCoroutine(_Post(serverURL, headers, parameters, actionResult, contentType));
    }


    /// <summary>
    /// 向服务器提交postFormData请求
    /// </summary>
    /// <param name="serverURL"></param>
    /// <param name="headers"></param>
    /// <param name="parameters"></param>
    /// <param name="actionResult"></param>
    /// <param name="contentType"></param>
    public void PostFormData(string serverURL, Dictionary<string, string> headers, Dictionary<string, byte[]> parameters, Action<bool, string> actionResult, string contentType = "application/json")
    {
        StartCoroutine(_PostFormData(serverURL, headers, parameters, actionResult, contentType));
    }

    /// <summary>
    /// 通过PUT方式将字节流传到服务器
    /// </summary>
    /// <param name="url">服务器目标地址 like 'http://www.my-server.com/upload' </param>
    /// <param name="contentBytes">需要上传的字节流</param>
    /// <param name="resultAction">处理返回结果的委托</param>
    /// <returns></returns>
    public void UploadByPut(string url, byte[] contentBytes, Action<bool> actionResult)
    {
        StartCoroutine(_UploadByPut(url, contentBytes, actionResult, ""));
    }

    /// <summary>
    /// GET请求
    /// </summary>
    /// <param name="url">请求地址,like 'http://www.my-server.com/ '</param>
    /// <param name="action">请求发起后处理回调结果的委托</param>
    /// <returns></returns>
    IEnumerator _Get(string url, Action<UnityWebRequest> actionResult)
    {
        using (UnityWebRequest uwr = UnityWebRequest.Get(url))
        {
            yield return uwr.SendWebRequest();
            actionResult?.Invoke(uwr);
        }
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="downloadFilePathAndName">储存文件的路径和文件名 like 'Application.persistentDataPath+"/unity3d.html"'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求对象</param>
    /// <returns></returns>
    IEnumerator _DownloadFile(string url, string downloadFilePathAndName, Action<UnityWebRequest> actionResult)
    {
        var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
        uwr.downloadHandler = new DownloadHandlerFile(downloadFilePathAndName);
        yield return uwr.SendWebRequest();
        actionResult?.Invoke(uwr);
    }

    /// <summary>
    /// 请求图片
    /// </summary>
    /// <param name="url">图片地址,like 'http://www.my-server.com/image.png '</param>
    /// <param name="action">请求发起后处理回调结果的委托,处理请求结果的图片</param>
    /// <returns></returns>
    IEnumerator _GetTexture(string url, Action<Texture2D> actionResult)
    {
        UnityWebRequest uwr = new UnityWebRequest(url);
        DownloadHandlerTexture downloadTexture = new DownloadHandlerTexture(true);
        uwr.downloadHandler = downloadTexture;
        yield return uwr.SendWebRequest();
        Texture2D t = null;
        if (!(uwr.isNetworkError || uwr.isHttpError))
        {
            t = downloadTexture.texture;
        }
        actionResult?.Invoke(t);
    }

    /// <summary>
    /// 请求AssetBundle
    /// </summary>
    /// <param name="url">AssetBundle地址,like 'http://www.my-server.com/myData.unity3d'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AssetBundle</param>
    /// <returns></returns>
    IEnumerator _GetAssetBundle(string url, Action<AssetBundle> actionResult)
    {
        UnityWebRequest www = new UnityWebRequest(url);
        DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(www.url, uint.MaxValue);
        www.downloadHandler = handler;
        yield return www.SendWebRequest();
        AssetBundle bundle = null;
        if (!(www.isNetworkError || www.isHttpError))
        {
            bundle = handler.assetBundle;
        }
        actionResult?.Invoke(bundle);
    }

    /// <summary>
    /// 请求服务器地址上的音效
    /// </summary>
    /// <param name="url">没有音效地址,like 'http://myserver.com/mysound.wav'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AudioClip</param>
    /// <param name="audioType">音效类型</param>
    /// <returns></returns>
    IEnumerator _GetAudioClip(string url, Action<AudioClip> actionResult, AudioType audioType = AudioType.WAV)
    {
        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(url, audioType))
        {
            yield return uwr.SendWebRequest();
            if (!(uwr.isNetworkError || uwr.isHttpError))
            {
                actionResult?.Invoke(DownloadHandlerAudioClip.GetContent(uwr));
            }
        }
    }

    /// <summary>
    /// 向服务器提交post请求
    /// </summary>
    /// <param name="serverURL">服务器请求目标地址,like "http://www.my-server.com/myform"</param>
    /// <param name="lstformData">form表单参数</param>
    /// <param name="lstformData">处理返回结果的委托</param>
    /// <returns></returns>
    IEnumerator _Post(string serverURL, List<IMultipartFormSection> lstformData, Action<UnityWebRequest> actionResult)
    {
        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));
        UnityWebRequest uwr = UnityWebRequest.Post(serverURL, lstformData);
        yield return uwr.SendWebRequest();
        actionResult?.Invoke(uwr);
    }

    /// <summary>
    /// 向服务器提交post请求
    /// </summary>
    /// <param name="serverURL">服务器请求目标地址,like "http://www.my-server.com/myform"</param>
    /// <param name="headers">请求头</param>
    /// <param name="parameters">请求参数</param>
    /// <param name="actionResult">回调action</param>
    /// <returns></returns>
    IEnumerator _Post(string serverURL, Dictionary<string, string> headers, Dictionary<string,string> parameters, Action<bool, string> actionResult, string contentType)
    {
        string json = "{";
        foreach (KeyValuePair<string, string> post_arg in parameters)
        {
            json += "\"" + post_arg.Key + "\"" + ":" + "\"" + post_arg.Value + "\"" + ",";
        }

        if (json.EndsWith(",", StringComparison.Ordinal))
        {
            json = json.Substring(0, json.Length - 1) + "}";
        }

        Debug.Log("serverURL:" + serverURL +  "    " + "parameters:" + json);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        //headers["Content-Type"] = contentType;

        UnityWebRequest request = UnityWebRequest.Put(serverURL, bodyRaw);
        request.method = "POST";
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.uploadHandler.contentType = contentType;
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        if (headers != null)
        {
            foreach (var item in headers)
            {
                request.SetRequestHeader(item.Key, item.Value);
            }
        }
        yield return request.SendWebRequest();
        if (request.isHttpError || request.isNetworkError)
        {
            Debug.Log(request.error);
            actionResult?.Invoke(false, request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            actionResult?.Invoke(true, request.downloadHandler.text);
        }
    }

    /// <summary>
    /// PostFormData
    /// </summary>
    /// <param name="serverURL"></param>
    /// <param name="headers"></param>
    /// <param name="parameters"></param>
    /// <param name="actionResult"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    IEnumerator _PostFormData(string serverURL, Dictionary<string, string> headers, Dictionary<string, byte[]> parameters, Action<bool, string> actionResult, string contentType)
    {

        //List<IMultipartFormSection> iparams = new List<IMultipartFormSection>();
        //foreach (var item in parameters)
        //{
        //    iparams.Add(new MultipartFormFileSection(item.Key, item.Value));
        //    Debug.Log(item.Key);
        //    Debug.Log(item.Value.Length);
        //}

        //UnityWebRequest request = UnityWebRequest.Post(serverURL, iparams);

        WWWForm form = new WWWForm();
        foreach (var item in parameters)
        {
            form.AddBinaryData(item.Key, item.Value, "test", "application/octet-stream");
        }

        UnityWebRequest request = UnityWebRequest.Post(serverURL, form);

        if (headers != null)
        {
            foreach (var item in headers)
            {
                request.SetRequestHeader(item.Key, item.Value);
            }
        }

        //request.SetRequestHeader("Content-Type", "multipart/form-data");

        yield return request.SendWebRequest();
        if (request.isHttpError || request.isNetworkError)
        {
            Debug.Log(request.error);
            actionResult?.Invoke(false, request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            actionResult?.Invoke(true, request.downloadHandler.text);
        }
    }

    /// <summary>
    /// 通过PUT方式将字节流传到服务器
    /// </summary>
    /// <param name="url">服务器目标地址 like 'http://www.my-server.com/upload' </param>
    /// <param name="contentBytes">需要上传的字节流</param>
    /// <param name="resultAction">处理返回结果的委托</param>
    /// <param name="resultAction">设置header文件中的Content-Type属性</param>
    /// <returns></returns>
    IEnumerator _UploadByPut(string url, byte[] contentBytes, Action<bool> actionResult, string contentType = "application/octet-stream")
    {
        UnityWebRequest uwr = new UnityWebRequest();
        UploadHandler uploader = new UploadHandlerRaw(contentBytes);

        // Sends header: "Content-Type: custom/content-type";
        uploader.contentType = contentType;

        uwr.uploadHandler = uploader;

        yield return uwr.SendWebRequest();

        bool res = true;
        if (uwr.isNetworkError || uwr.isHttpError)
        {
            res = false;
        }
        actionResult?.Invoke(res);
    }
}
