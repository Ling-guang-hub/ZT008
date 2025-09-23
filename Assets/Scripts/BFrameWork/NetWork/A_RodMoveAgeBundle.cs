/***
 * 
 * 网络请求的get对象
 * 
 * **/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class A_RodMoveAgeBundle 
{
    //get的url
    public string Gun;
    //get成功的回调
    public Action<UnityWebRequest> AgeCoaming;
    //get失败的回调
    public Action AgeLake;
    public A_RodMoveAgeBundle(string url,Action<UnityWebRequest> success,Action fail)
    {
        Gun = url;
        AgeCoaming = success;
        AgeLake = fail;
    }
   
}
