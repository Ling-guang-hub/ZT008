/***
 * 
 * 
 * 网络信息控制
 * 
 * **/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Sirenix.OdinInspector;

//using MoreMountains.NiceVibrations;

public class A_RodDumpBee : MonoBehaviour
{

    public static A_RodDumpBee instance;
    //请求超时时间
    private static float TIMEOUT= 3f;
    //base
    [ShowInInspector]
[UnityEngine.Serialization.FormerlySerializedAs("BaseUrl")]    public string MintGun;
    //登录url
    [ShowInInspector]
[UnityEngine.Serialization.FormerlySerializedAs("BaseLoginUrl")]    public string MintKarstGun;
    //配置url
    [ShowInInspector]
[UnityEngine.Serialization.FormerlySerializedAs("BaseConfigUrl")]    public string MintExciteGun;
    //时间戳url
    [ShowInInspector]
[UnityEngine.Serialization.FormerlySerializedAs("BaseTimeUrl")]    public string MintDustGun;
    //更新AdjustId url
    [ShowInInspector]
[UnityEngine.Serialization.FormerlySerializedAs("BaseAdjustUrl")]    public string MintRatherGun;
    //后台gamecode
    [ShowInInspector]
[UnityEngine.Serialization.FormerlySerializedAs("GameCode")]    public string AeroItem= "20000";

    //channel渠道平台
#if UNITY_IOS
    [ShowInInspector]
    public string Flannel= "iOS";
#elif UNITY_ANDROID
    [ShowInInspector]
    public string Channel = "Android";
#else
    [ShowInInspector]
[UnityEngine.Serialization.FormerlySerializedAs("Channel")]    public string Channel = "Other";
#endif
    //工程包名
    private string ImageryFire{ get { return Application.identifier; } }
    //登录url
    private string KarstGun= "";
    //配置url
    private string ExciteGun= "";
    //更新AdjustId url
    private string RatherGun= "";
[UnityEngine.Serialization.FormerlySerializedAs("country")]    //国家
    public string Leather= "";
[UnityEngine.Serialization.FormerlySerializedAs("ConfigData")]    //服务器Config数据
    public ServerData ExciteDale;
[UnityEngine.Serialization.FormerlySerializedAs("InitData")]    //游戏内数据
    public Init LadeDale;
[UnityEngine.Serialization.FormerlySerializedAs("CashOut_Data")]    //提现相关后台数据
    public CashOutData SnapWho_Dale;
    
    [HideInInspector]
[UnityEngine.Serialization.FormerlySerializedAs("gaid")]    public string Horn;
    [HideInInspector]
[UnityEngine.Serialization.FormerlySerializedAs("aid")]    public string Ute;
    [HideInInspector]
[UnityEngine.Serialization.FormerlySerializedAs("idfa")]    public string Step;
    int Fatal_Actor= 0;
[UnityEngine.Serialization.FormerlySerializedAs("ready")]    public bool Fatal= false;
[UnityEngine.Serialization.FormerlySerializedAs("BlockRule")]    //ios 获取idfa函数声明
public BlockRuleData HalveYuan;
#if UNITY_IOS
    [DllImport("__Internal")]
    internal extern static void getIDFA();
#endif
    
    [HideInInspector] [UnityEngine.Serialization.FormerlySerializedAs("DataFrom")]public string DaleScar; //数据来源 打点用


    void Awake()
    {
        instance = this;
        var aBuildConfig = AUtility.Config.GetTargetBuildConfig();
#if UNITY_ANDROID
        Channel = "Android";
#endif
#if UNITY_IOS
        Flannel = "iOS";
#endif
        if (aBuildConfig == null || string.IsNullOrEmpty(aBuildConfig.BaseUrl))
        {
            Debug.LogError("没有配置ABuildConfig");
            return;
        }
        MintGun = aBuildConfig.BaseUrl;
        MintKarstGun = aBuildConfig.BaseUrl + BConsumer.ArchiveKey.KarstGun;
        MintExciteGun = aBuildConfig.BaseUrl + BConsumer.ArchiveKey.ExciteGun;
        MintDustGun = aBuildConfig.BaseUrl + BConsumer.ArchiveKey.DustGun;
        MintRatherGun = aBuildConfig.BaseUrl + BConsumer.ArchiveKey.DustGun;
        AeroItem = aBuildConfig.GameCode;
        
        KarstGun = MintKarstGun + AeroItem + "&channel=" + Flannel + "&version=" + Application.version;
        ExciteGun = MintExciteGun + AeroItem + "&channel=" + Flannel + "&version=" + Application.version;
        RatherGun = MintRatherGun + AeroItem;
    }
    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass aj = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject p = aj.GetStatic<AndroidJavaObject>("currentActivity");
            p.Call("getGaid");
            p.Call("getAid");
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS
            getIDFA();
            string idfv = UnityEngine.iOS.Device.vendorIdentifier;
            PlayerPrefs.SetString("idfv", idfv);
#endif
        }
        else
        {
            Karst();           //编辑器登录
        }
        //获取config数据
        AgeExciteDale();

        //提现登录
        // CashOutManager.GetInstance().Login();
    }

    /// <summary>
    /// 获取gaid回调
    /// </summary>
    /// <param name="gaid_str"></param>
    public void gaidAction(string gaid_str)
    {
        Debug.Log("unity收到gaid：" + gaid_str);
        Horn = gaid_str;
        if (Horn == null || Horn == "")
        {
            Horn = PlayerPrefs.GetString("gaid");
        }
        else
        {
            PlayerPrefs.SetString("gaid", Horn);
        }
        Fatal_Actor++;
        if (Fatal_Actor == 2)
        {
            Karst();
        }
    }
    /// <summary>
    /// 获取aid回调
    /// </summary>
    /// <param name="aid_str"></param>
    public void aidAction(string aid_str)
    {
        Debug.Log("unity收到aid：" + aid_str);
        Ute = aid_str;
        if (Ute == null || Ute == "")
        {
            Ute = PlayerPrefs.GetString("aid");
        }
        else
        {
            PlayerPrefs.SetString("aid", Ute);
        }
        Fatal_Actor++;
        if (Fatal_Actor == 2)
        {
            Karst();
        }
    }
    /// <summary>
    /// 获取idfa成功
    /// </summary>
    /// <param name="message"></param>
    public void idfaSuccess(string message)
    {
        Debug.Log("idfa success:" + message);
        Step = message;
        PlayerPrefs.SetString("idfa", Step);
        Karst();
    }
    /// <summary>
    /// 获取idfa失败
    /// </summary>
    /// <param name="message"></param>
    public void idfaFail(string message)
    {
        Debug.Log("idfa fail");
        Step = PlayerPrefs.GetString("idfa");
        Karst();
    }
    /// <summary>
    /// 登录
    /// </summary>
    public void Karst()
    {
        //获取本地缓存的Local用户ID
        string localId = PlayerPrefs.GetString(BConsumer.ArchiveKey.It_GuessGoldSo);

        //没有用户ID，视为新用户，生成用户ID
        if (localId == "" || localId.Length == 0)
        {
            //生成用户随机id
            TimeSpan st = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            string timeStr = Convert.ToInt64(st.TotalSeconds).ToString() + UnityEngine.Random.Range(0, 10).ToString() + UnityEngine.Random.Range(1, 10).ToString() + UnityEngine.Random.Range(1, 10).ToString() + UnityEngine.Random.Range(1, 10).ToString();
            localId = timeStr;
            PlayerPrefs.SetString(BConsumer.ArchiveKey.It_GuessGoldSo, localId);
        }

        //拼接登录接口参数
        string url = "";
        if (Application.platform == RuntimePlatform.IPhonePlayer)       //一个参数 - iOS
        {
            url = KarstGun + "&" + "randomKey" + "=" + localId + "&idfa=" + Step + "&packageName=" + ImageryFire;
        }
        else if (Application.platform == RuntimePlatform.Android)  //两个参数 - Android
        {
            url = KarstGun + "&" + "randomKey" + "=" + localId + "&gaid=" + Horn + "&androidId=" + Ute + "&packageName=" + ImageryFire;
        }
        else //编辑器
        {
            url = KarstGun + "&" + "randomKey" + "=" + localId + "&packageName=" + ImageryFire;
        }

        //获取国家信息
        getImpinge(() =>
        {
            url += "&country=" + Leather;
            //登录请求
            A_RodMoveTrickle.Instance.ArmyAge(url,
                (data) =>
                {
                    Debug.Log("Login 成功" + data.downloadHandler.text);
                    PlayerPrefs.SetString("init_time", DateTime.Now.ToString());
                    ServerUserData serverUserData = JsonConvert.DeserializeObject<ServerUserData>(data.downloadHandler.text);
                    PlayerPrefs.SetString(BConsumer.ArchiveKey.It_GuessMexicoSo, serverUserData.data.ToString());

                    AtomRatherWhen();

                    if (PlayerPrefs.GetInt("SendedEvent") != 1 && !String.IsNullOrEmpty(A_DrenchDoom.PeckTie))
                        A_DrenchDoom.AtomVenus();
                },
                () =>
                {
                    Debug.Log("Login 失败");
                });
        });
    }
    /// <summary>
    /// 获取国家
    /// </summary>
    /// <param name="cb"></param>
    private void getImpinge(Action cb)
    {
        bool callBackReady = false;
        if (String.IsNullOrEmpty(Leather))
        {
            A_RodMoveTrickle.Instance.ArmyAge("https://a.mafiagameglobal.com/event/country/", (data) =>
            {
                Leather = JsonConvert.DeserializeObject<Dictionary<string, string>>(data.downloadHandler.text)["country"];
                Debug.Log("获取国家 成功:" + Leather);
                if (!callBackReady)
                {
                    callBackReady = true;
                    cb?.Invoke();
                }
            },
            () =>
            {
                Debug.Log("获取国家 失败");
                if (!callBackReady)
                {
                    Leather = "";
                    callBackReady = true;
                    cb?.Invoke();
                }
            });
        }
        else
        {
            if (!callBackReady)
            {
                callBackReady = true;
                cb?.Invoke();
            }
        }
    }

    /// <summary>
    /// 获取服务器Config数据
    /// </summary>
    private void AgeExciteDale()
    {
        Debug.Log("GetConfigData:" + ExciteGun);
        //获取并存入Config
        A_RodMoveTrickle.Instance.ArmyAge(ExciteGun,
        (data) =>
        {
            DaleScar = "OnlineData";
            Debug.Log("ConfigData 成功" + data.downloadHandler.text);
            PlayerPrefs.SetString("OnlineData", data.downloadHandler.text);
            GetExciteDale(data.downloadHandler.text);
        },
        () =>
        {
            Debug.Log("ConfigData 失败");
            AgeLoactionDale();
        });
    }

    /// <summary>
    /// 获取本地Config数据
    /// </summary>
    private void AgeLoactionDale()
    {
        //是否有缓存
        if (PlayerPrefs.GetString("OnlineData") == "" || PlayerPrefs.GetString("OnlineData").Length == 0)
        {
            DaleScar = "LocalData_Updated"; //已联网更新过的数据
            Debug.Log("本地数据");
            TextAsset json = Resources.Load<TextAsset>("LocationJson/LocationData");
            GetExciteDale(json.text);
        }
        else
        {
            DaleScar = "LocalData_Original"; //原始数据
            Debug.Log("服务器缓存数据");
            GetExciteDale(PlayerPrefs.GetString("OnlineData"));
        }
    }

    /// <summary>
    /// 解析config数据
    /// </summary>
    /// <param name="configJson"></param>
    void GetExciteDale(string configJson)
    {
        //如果已经获得了数据则不再处理
        if (ExciteDale == null)
        {
            RootData rootData = JsonConvert.DeserializeObject<RootData>(configJson);
            ExciteDale = rootData.data;
            //InitData = JsonMapper.ToObject<Init>(ConfigData.init);

            if (!string.IsNullOrEmpty(ExciteDale.BlockRule))
                HalveYuan = JsonConvert.DeserializeObject<BlockRuleData>(ExciteDale.BlockRule);
            // if (!string.IsNullOrEmpty(ExciteDale.CashOut_Data))
                // SnapWho_Dale = JsonConvert.DeserializeObject<CashOutData>(ExciteDale.CashOut_Data);

            //GameReady();
            AgeGoldDump();
        }
    }
    /// <summary>
    /// 进入游戏
    /// </summary>
    void AeroMount()
    {
        //打开admanager
        // adManager.SetActive(true);
        //进度条可以继续
        Fatal = true;
    }

    /// <summary>
    /// 向后台发送adjustId
    /// </summary>
    public void AtomRatherWhen()
    {
        string serverId = PlayerPrefs.GetString(BConsumer.ArchiveKey.It_GuessMexicoSo);
        string adjustId = A_RatherLadeTrickle.Instance.AgeRatherWhen();
        if (string.IsNullOrEmpty(serverId) || string.IsNullOrEmpty(adjustId))
        {
            // Debug.LogError("SendAdjustAdid 失败");
            return;
        }
        Debug.Log("发送adid  serverId:" + serverId + " adjustId:" + adjustId);
        string url = RatherGun + "&serverId=" + serverId + "&adid=" + adjustId;
        A_RodMoveTrickle.Instance.ArmyAge(url,
            (data) =>
            {
                Debug.Log("服务器更新adjust adid 成功" + data.downloadHandler.text);
            },
            () =>
            {
                Debug.Log("服务器更新adjust adid 失败");
            });
    }
[UnityEngine.Serialization.FormerlySerializedAs("UserDataStr")]

    //获取用户信息
    public string GoldDaleOwn= "";
[UnityEngine.Serialization.FormerlySerializedAs("UserData")]    public UserInfoData GoldDale;
    int AgeGoldDumpJoint= 0;
    void AgeGoldDump()
    {
        //还有进入正常模式的可能
        if (PlayerPrefs.HasKey("OtherChance") && PlayerPrefs.GetString("OtherChance") == "YES")
            PlayerPrefs.DeleteKey("Save_AP");
        //已经记录过用户信息 跳过检查
        if (PlayerPrefs.HasKey("OtherChance") && PlayerPrefs.GetString("OtherChance") == "NO")
        {
            AeroMount();
            return;
        }

        //检查归因渠道信息
        //CheckAdjustNetwork();
        //获取用户信息
        string CheckUrl = MintGun + "/api/client/user/checkUser";
        A_RodMoveTrickle.Instance.ArmyAge(CheckUrl,
        (data) =>
        {
            GoldDaleOwn = data.downloadHandler.text;
            print("+++++ 获取用户数据 成功" + GoldDaleOwn);
            UserRootData rootData = JsonConvert.DeserializeObject<UserRootData>(GoldDaleOwn);
            GoldDale = JsonConvert.DeserializeObject<UserInfoData>(rootData.data);
            if (GoldDaleOwn.Contains("apple")
            || GoldDaleOwn.Contains("Apple")
            || GoldDaleOwn.Contains("APPLE"))
                GoldDale.IsHaveApple = true;
            AeroMount();
        }, () => { });
        Invoke(nameof(DyAgeGoldDump), 1);
    }
    void DyAgeGoldDump()
    {
        if (!Fatal)
        {
            AgeGoldDumpJoint++;
            if (AgeGoldDumpJoint < 10)
            {
                print("+++++ 获取用户数据失败 重试： " + AgeGoldDumpJoint);
                AgeGoldDump();
            }
            else
            {
                print("+++++ 获取用户数据 失败次数过多，放弃");
                AeroMount();
            }
        }
    }
}
