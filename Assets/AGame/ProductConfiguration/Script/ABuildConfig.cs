using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class ABuildConfig : ScriptableObject
{
    [ReadOnly]
    public string BuildTarget;
    [LabelText("应用图标"), PreviewField(50, ObjectFieldAlignment.Left), ReadOnly]
    public Texture2D LogoTexture;
    [ReadOnly]
    public string Version;
    [LabelText("屏幕方向"), ReadOnly]
    public int ScreenOrientation = 0;
    [LabelText("是否加密状态"), ReadOnly]
    public bool IsEncrypt;
    [ReadOnly]
    public string BaseUrl;
    [ReadOnly]
    public string GameName;
    [ReadOnly]
    public string PackageName;
    [ReadOnly]
    public int BuildCode;
    [ReadOnly]
    public string Applovin_SDK_KEY;
    [ReadOnly]
    public string Applovin_REWARD_ID;
    [ReadOnly]
    public string Applovin_INTER_ID;
    [ReadOnly]
    public string Adjust_APP_ID;
    [ReadOnly]
    public string GameCode;
    [ReadOnly]
    public string Rate_ID;
    [ReadOnly]
    public string ZT_LoginPlatform;
    [ReadOnly]
    public string ZT_ID;
}