using System;
using System.IO;
using System.Text;
using LitJson;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class AGameBuildWindow : OdinMenuEditorWindow
{
    [MenuItem("AGame/BuildWindow")]
    public static void ShowWindow()
    {
        var window = GetWindow<AGameBuildWindow>();
        window.Show();
    }
    
    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.Add("产品配置", new ABuildConfigInfo());
        return tree;
    }
    
}

[Serializable]
public class ABuildConfigInfo
{
    [Title("产品配置")]
    [LabelText("当前编辑器平台"), ReadOnly]
    public BuildTarget CurrentBuildTarget;
    
    [ReadOnly, ShowInInspector]
    public string BuildTarget
    {
        get => _buildConfig.BuildTarget;
        set => _buildConfig.BuildTarget = value.Trim();
    }

    [LabelText("是否加密状态"), ReadOnly, ShowInInspector]
    public bool IsEncrypt
    {
        get => _buildConfig.IsEncrypt;
        set
        {
            _buildConfig.IsEncrypt = value;
            _DESBtnText = value ? "解密" : "加密";
        }
    }
    [LabelText("应用图标"), PreviewField(50, ObjectFieldAlignment.Left), ShowInInspector]
    public Texture2D LogoTexture
    {
        get => _buildConfig.LogoTexture;
        set => _buildConfig.LogoTexture = value;
    }
    [LabelText("屏幕方向"), ShowInInspector]
    public UIOrientation ScreenOrientation
    {
        get => (UIOrientation)_buildConfig.ScreenOrientation;
        set => _buildConfig.ScreenOrientation = (int)value;
    }
    [ShowInInspector]
    public string Version
    {
        get => _buildConfig.Version;
        set => _buildConfig.Version = value.Trim();
    }
    [ShowInInspector]
    public string BaseUrl
    {
        get => _buildConfig.BaseUrl;
        set => _buildConfig.BaseUrl = value.Trim();
    }
    [ShowInInspector]
    public string GameName
    {
        get => _buildConfig.GameName;
        set => _buildConfig.GameName = value.Trim();
    }
    [ShowInInspector]
    public string PackageName
    {
        get => _buildConfig.PackageName;
        set => _buildConfig.PackageName = value.Trim();
    }
    [ShowInInspector]
    public int BuildCode
    {
        get => _buildConfig.BuildCode;
        set => _buildConfig.BuildCode = value;
    }
    [ShowInInspector]
    public string Applovin_SDK_KEY
    {
        get => _buildConfig.Applovin_SDK_KEY;
        set => _buildConfig.Applovin_SDK_KEY = value.Trim();
    }
    [ShowInInspector]
    public string Applovin_REWARD_ID
    {
        get => _buildConfig.Applovin_REWARD_ID;
        set => _buildConfig.Applovin_REWARD_ID = value.Trim();
    }
    [ShowInInspector]
    public string Applovin_INTER_ID
    {
        get => _buildConfig.Applovin_INTER_ID;
        set => _buildConfig.Applovin_INTER_ID = value.Trim();
    }
    [ShowInInspector]
    public string Adjust_APP_ID
    {
        get => _buildConfig.Adjust_APP_ID;
        set => _buildConfig.Adjust_APP_ID = value.Trim();
    }
    [ShowInInspector]
    public string GameCode
    {
        get => _buildConfig.GameCode;
        set => _buildConfig.GameCode = value.Trim();
    }
    [ShowInInspector]
    public string Rate_ID
    {
        get => _buildConfig.Rate_ID;
        set => _buildConfig.Rate_ID = value.Trim();
    }
    [ShowInInspector]
    public string ZT_LoginPlatform
    {
        get => _buildConfig.ZT_LoginPlatform;
        set => _buildConfig.ZT_LoginPlatform = value.Trim();
    }
    [ShowInInspector]
    public string ZT_ID
    {
        get => _buildConfig.ZT_ID;
        set => _buildConfig.ZT_ID = value.Trim();
    }
    
    private ABuildConfig _buildConfig;
    private string _buildPath = "Assets/AGame/ProductConfiguration/Resources/AGame/ABuildConfig.asset";
    private string _DESBtnText = "加密";
    public ABuildConfigInfo()
    {
        _buildConfig = AssetDatabase.LoadAssetAtPath<ABuildConfig>(_buildPath);
        if (_buildConfig == null)
        {
            _buildConfig = ScriptableObject.CreateInstance<ABuildConfig>();
        }
        
        // 初始化时获取当前平台
        CurrentBuildTarget = EditorUserBuildSettings.activeBuildTarget;
        if (CurrentBuildTarget == UnityEditor.BuildTarget.Android)
        {
            BuildTarget = "Android";
            ZT_LoginPlatform = "0";
        }
        else if (CurrentBuildTarget == UnityEditor.BuildTarget.iOS)
        {
            BuildTarget = "iOS";
            ZT_LoginPlatform = "1";
        }
        
        _DESBtnText = _buildConfig.IsEncrypt ? "解密" : "加密";
    }
    
    [Button("$_DESBtnText", ButtonSizes.Large)]
    private void DESBtnClick()
    {
        IsEncrypt = !IsEncrypt;
        if (IsEncrypt)
        {
            Applovin_SDK_KEY = AUtility.Crypto.EncryptDES(Applovin_SDK_KEY, PackageName);
            _DESBtnText = "解密";
        }
        else
        {
            Applovin_SDK_KEY = AUtility.Crypto.DecryptDES(Applovin_SDK_KEY, PackageName);
            _DESBtnText = "加密";
        }
    }

    [Button("构建", ButtonSizes.Large)]
    private void Build()
    {
        
        BuildCode++;
        
        // 设置屏幕方向
        PlayerSettings.defaultInterfaceOrientation = ScreenOrientation;
        
        //version
        PlayerSettings.bundleVersion = Version;
        PlayerSettings.productName = GameName;
        //PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
        PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new Texture2D[] { LogoTexture });
        if (CurrentBuildTarget == UnityEditor.BuildTarget.Android)
        {
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel28;
            //PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel33;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            AndroidArchitecture aac = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
            PlayerSettings.Android.targetArchitectures = aac;
            PlayerSettings.Android.bundleVersionCode = BuildCode;
            EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, PackageName);
        }
        else if (CurrentBuildTarget == UnityEditor.BuildTarget.iOS)
        {
            PlayerSettings.iOS.buildNumber = BuildCode.ToString();
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, PackageName);
        }
        
        AppLovinSettings.Instance.SdkKey = Applovin_SDK_KEY;
        EditorUtility.SetDirty(AppLovinSettings.Instance);
        
        DownloadConfig();
        
        EditorUtility.SetDirty(_buildConfig);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    private void DownloadConfig()
    {
        var ConfigUrl = BConsumer.ArchiveKey.ExciteGun;
#if UNITY_IOS
        string url = BaseUrl + ConfigUrl + GameCode + "&channel=" + "AppStore" + "&version=" + Version;
#elif UNITY_ANDROID
        string url = BaseUrl + ConfigUrl + GameCode + "&channel=" + "GooglePlay" + "&version=" + Version;
#else
        string url = BaseUrl + ConfigUrl + GameCode + "&channel=" + "GooglePlay" + "&version=" + Version;
#endif
        A_RodMoveTrickle.Instance.ArmyAge(url,
            (data) =>
            {
                Debug.Log("ServerData 成功" + data.downloadHandler.text);
                PlayerPrefs.SetString("OnlineData", data.downloadHandler.text);
                RootData rootData = JsonMapper.ToObject<RootData>(data.downloadHandler.text);
                //if (rootData.data.apple_pie != "apple")
                {
                    rootData.data.apple_pie = "apple";
                    string locationStr = JsonMapper.ToJson(rootData);
                    EditorUtility.DisplayDialog("改值成功", "", "确定");
                    WriteJsonFromStreamingAssetsPath("/" + "LocationJson" + "/" + "LocationData.txt", locationStr);
                    Debug.Log("Build Success");
                    // if (GameObject.Find("NetWorkManager"))
                    // {
                    //     DestroyImmediate(GameObject.Find("NetWorkManager"));
                    // }
                }
            },
            () =>
            {
                Debug.Log("ServerData 失败");
                EditorUtility.DisplayDialog("改值失败", "请检查网络", "确定");
                // if (GameObject.Find("NetWorkManager"))
                // {
                //     DestroyImmediate(GameObject.Find("NetWorkManager"));
                // }
                return;
            });
    }
    
    public static string ReadJsonFromStreamingAssetsPath(string jsonName)
    {
        string url = Application.streamingAssetsPath + jsonName;
        url = url.Replace("StreamingAssets", "Resources");
        Encoding endoning = Encoding.UTF8;//识别Json数据内容中文字段
        StreamReader streamReader = new StreamReader(url, endoning);
        string jsonData = streamReader.ReadToEnd();
        streamReader.Close();
        return jsonData;
    }
    public static void WriteJsonFromStreamingAssetsPath(string jsonName, string jsonString)
    {
        string url = Application.streamingAssetsPath + jsonName;
        url = url.Replace("StreamingAssets", "Resources");
        StreamWriter streamWrite = new StreamWriter(url);
        streamWrite.WriteLine(jsonString);
        streamWrite.Dispose();
        streamWrite.Close();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

