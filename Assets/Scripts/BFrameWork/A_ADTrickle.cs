using UnityEngine;
using System;

public class A_ADTrickle : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("SdkKey")]    public string FixCat= "";
[UnityEngine.Serialization.FormerlySerializedAs("MAX_REWARD_ID")]    public string MAX_REWARD_ID= "8cc44f23f3a78029";
[UnityEngine.Serialization.FormerlySerializedAs("MAX_INTER_ID")]    public string MAX_INTER_ID= "6e02aadb78d3ce0c";
[UnityEngine.Serialization.FormerlySerializedAs("isTest")]    public bool ByPlow= false;
    public static A_ADTrickle Religion{ get; private set; }

    // 广告加载状态
    private bool BetMobileMyBefore= false;
    private bool BetAccidentallyMyBefore= false;

    Action<bool> AxMobileMyPollution;
    bool ByMobileMyPollution= false;


    private void Awake()
    {
        Religion = this;
        LadeBedSDK();
    }

    // 初始化MAX SDK
    private void LadeBedSDK()
    {
        var buildConfig = AUtility.Config.GetTargetBuildConfig();
        if (buildConfig == null || string.IsNullOrEmpty(buildConfig.Applovin_SDK_KEY)
            || string.IsNullOrEmpty(buildConfig.Applovin_REWARD_ID)
            || string.IsNullOrEmpty(buildConfig.Applovin_INTER_ID))
        {
            Debug.LogError("没有配置Applovin_SDK_KEY 或 Applovin_REWARD_ID 或 Applovin_INTER_ID");
            return;
        }
        FixCat = buildConfig.Applovin_SDK_KEY;
        MAX_REWARD_ID = buildConfig.Applovin_REWARD_ID;
        MAX_INTER_ID = buildConfig.Applovin_INTER_ID;
        
        MaxSdk.SetSdkKey(AUtility.Crypto.DecryptDES(FixCat, Application.identifier));
        MaxSdkCallbacks.OnSdkInitializedEvent += (config) =>
        {
            Debug.Log("MAX SDK 初始化完成");
            // 初始化广告
            OutpouringMobilePay();
            OutpouringAccidentallyPay();
        };
        Debug.Log("MAX SDK 初始化中");
        MaxSdk.InitializeSdk();
    }
    // byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
    // string DecryptDES()
    // {
    //     byte[] rgbKey = Encoding.UTF8.GetBytes(Application.identifier.Substring(0, 8));
    //     byte[] rgbIV = Keys;
    //     byte[] inputByteArray = Convert.FromBase64String(SdkKey);
    //     DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
    //     MemoryStream mStream = new MemoryStream();
    //     CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
    //     cStream.Write(inputByteArray, 0, inputByteArray.Length);
    //     cStream.FlushFinalBlock();
    //     cStream.Close();
    //     return Encoding.UTF8.GetString(mStream.ToArray());
    // }

    #region Reward Ad Methods
    private void OutpouringMobilePay()
    {
        // 激励广告回调
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardAdLoaded;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardAdLoadFailed;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardAdHidden;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardAdReceived;

        LoadMobileMy();
    }

    private void LoadMobileMy()
    {
        MaxSdk.LoadRewardedAd(MAX_REWARD_ID);
    }

    private void OnRewardAdLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        BetMobileMyBefore = true;
        Debug.Log("激励视频加载完成");
    }

    private void OnRewardAdLoadFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        BetMobileMyBefore = false;
        Debug.Log($"激励视频加载失败: {errorInfo.Message}");
        // 重新尝试加载
        Invoke(nameof(LoadMobileMy), 5f);
    }

    public void TeamMobilePolar(Action<bool> OnRewardAdCompleted, string index)
    {
        this.AxMobileMyPollution = OnRewardAdCompleted;
        if (BetMobileMyBefore)
        {
            if (ByPlow)
            {
                OnRewardAdCompleted?.Invoke(true);
                return;
            }

            MaxSdk.ShowRewardedAd(MAX_REWARD_ID);
        }
        else
        {
            Debug.LogWarning("激励视频未加载");
            UITrickle.Religion.LeftChunk.SetActive(true);
            UITrickle.Religion.LeftChunk.GetComponent<LeftChunk>().CrowLeft("No ads right now, please try it later.");
            OnRewardAdCompleted?.Invoke(false);
            LoadMobileMy();
        }
    }

    private void OnRewardAdReceived(string adUnitId, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        ByMobileMyPollution = true;
    }

    private void OnRewardAdHidden(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        AxMobileMyPollution?.Invoke(ByMobileMyPollution);
        AxMobileMyPollution = null;
        BetMobileMyBefore = false;
        ByMobileMyPollution = false;
        LoadMobileMy();
    }
    #endregion

    #region Interstitial Ad Methods
    private void OutpouringAccidentallyPay()
    {
        // 插页广告回调
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoaded;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailed;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHidden;

        EmitAccidentallyMy();
    }

    private void EmitAccidentallyMy()
    {
        MaxSdk.LoadInterstitial(MAX_INTER_ID);
    }

    private void OnInterstitialLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        BetAccidentallyMyBefore = true;
        Debug.Log("插屏广告加载完成");
    }

    private void OnInterstitialLoadFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        BetAccidentallyMyBefore = false;
        Debug.Log($"插屏广告加载失败: {errorInfo.Message}");
        Invoke(nameof(EmitAccidentallyMy), 5f);
    }

    public void CrowAccidentallyMy()
    {
        if (BetAccidentallyMyBefore)
        {
            MaxSdk.ShowInterstitial(MAX_INTER_ID);
        }
        else
        {
            Debug.LogWarning("插屏广告未加载");
            EmitAccidentallyMy();
        }
    }

    private void OnInterstitialHidden(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        BetAccidentallyMyBefore = false;
        EmitAccidentallyMy();
    }
    #endregion
}