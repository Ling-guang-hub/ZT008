/**
 * 
 * 网络请求的post对象
 * 
 * ***/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class A_RodMoveGangBundle 
{
    //post请求地址
    public string URL;
    //post的数据表单
    public WWWForm Link;
    //post成功回调
    public Action<UnityWebRequest> GangCoaming;
    //post失败回调
    public Action GangLake;
    public A_RodMoveGangBundle(string url,WWWForm  form,Action<UnityWebRequest> success,Action fail)
    {
        URL = url;
        Link = form;
        GangCoaming = success;
        GangLake = fail;
    }
}
