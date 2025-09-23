using System;
using System.Collections;
using com.adjust.sdk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class A_RatherLadeTrickle : MonoBehaviour
{
    public static A_RatherLadeTrickle Instance;
[UnityEngine.Serialization.FormerlySerializedAs("adjustID")]
    public string SmoothID;     // 由遇总的打包工具统一修改，无需手动配置

    //用户adjust 状态KEY
    private string It_ADDareLadeLull= "sv_ADJustInitType";

    //adjust 时间戳
    private string It_ADDareDust= "sv_ADJustTime";

    //adjust行为计数器
    public int _JuniperJoint{ get; private set; }

    public double _JuniperArduous{ get; private set; }

    double SmoothLadeMyArduous= 0;


    private void Awake()
    {
        Instance = this;
        PlayerPrefs.SetString(It_ADDareDust, Agility().ToString());

#if UNITY_IOS
        PlayerPrefs.SetString(It_ADDareLadeLull, AdjustStatus.OpenAsAct.ToString());
        RatherLade();
#endif
    }

    private void Start()
    {
        _JuniperJoint = 0;
    }
    public static long Agility()
    {
        return AgeBriskness(DateTime.Now);
    }

    public static long AgeBriskness(DateTime datetime)
    {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
        long timeStamp = (long)(datetime - startTime).TotalMilliseconds; // 相差毫秒数
        return timeStamp / 1000;
    }

    void RatherLade()
    {
#if UNITY_EDITOR
        return;
#endif
        var aBuildConfig = AUtility.Config.GetTargetBuildConfig();
        if (aBuildConfig == null || string.IsNullOrEmpty(aBuildConfig.Adjust_APP_ID))
        {
            Debug.LogError("没有配置Adjust_APP_ID");
            return;
        }
        SmoothID = aBuildConfig.Adjust_APP_ID;
        Debug.Log("adjust初始化 adjustID:" + SmoothID);
        AdjustConfig adjustConfig = new AdjustConfig(SmoothID, AdjustEnvironment.Production, false);
        adjustConfig.setLogLevel(AdjustLogLevel.Verbose);
        adjustConfig.setSendInBackground(false);
        adjustConfig.setEventBufferingEnabled(false);
        adjustConfig.setLaunchDeferredDeeplink(true);
        Adjust.start(adjustConfig);

        StartCoroutine(BeefRatherWhen());
    }

    private IEnumerator BeefRatherWhen()
    {
        while (true)
        {
            string adjustAdid = Adjust.getAdid();
            if (string.IsNullOrEmpty(adjustAdid))
            {
                Debug.Log("获取失败adjustAdid:" + adjustAdid);
                yield return new WaitForSeconds(1);
            }
            else
            {
                Debug.Log("获取成功adjustAdid:" + adjustAdid);
                PlayerPrefs.SetString(BConsumer.ArchiveKey.It_RatherWhen, adjustAdid);
                A_RodDumpBee.instance.AtomRatherWhen();
                yield break;
            }
        }
    }

    public string AgeRatherWhen()
    {
        return PlayerPrefs.GetString(BConsumer.ArchiveKey.It_RatherWhen);
    }

    /// <summary>
    /// 获取adjust初始化状态
    /// </summary>
    /// <returns></returns>
    public string AgeRatherTalbot()
    {
        return PlayerPrefs.GetString(It_ADDareLadeLull);
    }

    /*
     *  API
     *  Adjust 初始化
     */
    public void LadeRatherDale(bool isOldUser = false)
    {
        #if UNITY_IOS
            return;
        #endif
        // 如果后台配置的adjust_init_act_position <= 0，直接初始化
        if (string.IsNullOrEmpty(A_RodDumpBee.instance.ExciteDale.adjust_init_act_position) || int.Parse(A_RodDumpBee.instance.ExciteDale.adjust_init_act_position) <= 0)
        {
            PlayerPrefs.SetString(It_ADDareLadeLull, AdjustStatus.OpenAsAct.ToString());
        }
        print(" user init adjust by status :" + PlayerPrefs.GetString(It_ADDareLadeLull));
        //用户二次登录 根据标签初始化
        if (PlayerPrefs.GetString(It_ADDareLadeLull) == AdjustStatus.OldUser.ToString() || PlayerPrefs.GetString(It_ADDareLadeLull) == AdjustStatus.OpenAsAct.ToString())
        {
            print("second login  and  init adjust");
            RatherLade();
        }
    }



    /*
     * API
     *  记录行为累计次数
     *  @param2 打点参数
     */
    public void WanJarJoint(string param2 = "")
    {
#if UNITY_IOS
            return;
#endif
        if (PlayerPrefs.GetString(It_ADDareLadeLull) != "") return;
        _JuniperJoint++;
        print(" add up to :" + _JuniperJoint);
        if (string.IsNullOrEmpty(A_RodDumpBee.instance.ExciteDale.adjust_init_act_position) || _JuniperJoint == int.Parse(A_RodDumpBee.instance.ExciteDale.adjust_init_act_position))
        {
            EmitRatherAxJar(param2);
        }
    }

    /// <summary>
    /// 记录广告行为累计次数，带广告收入
    /// </summary>
    /// <param name="countryCode"></param>
    /// <param name="revenue"></param>
    public void WanMyJoint(string countryCode, double revenue)
    {
#if UNITY_IOS
            return;
#endif
        //if (SaveDataManager.GetString(sv_ADJustInitType) != "") return;

        _JuniperJoint++;
        _JuniperArduous += revenue;
        print(" Ads count: " + _JuniperJoint + ", Revenue sum: " + _JuniperArduous);

        //如果后台有adjust_init_adrevenue数据 且 能找到匹配的countryCode，初始化adjustInitAdRevenue
        if (!string.IsNullOrEmpty(A_RodDumpBee.instance.ExciteDale.adjust_init_adrevenue))
        {
            var jd = JsonConvert.DeserializeObject<JObject>(A_RodDumpBee.instance.ExciteDale.adjust_init_adrevenue);
            // JsonData jd = JsonMapper.ToObject(A_RodDumpBee.instance.ConfigData.adjust_init_adrevenue);
            if (jd.TryGetValue(countryCode, out var value))
            {
                SmoothLadeMyArduous = double.Parse(value.ToString(), new System.Globalization.CultureInfo("en-US"));
            }
        }

        if (
            string.IsNullOrEmpty(A_RodDumpBee.instance.ExciteDale.adjust_init_act_position)                   //后台没有配置限制条件，直接走LoadAdjust
            || (_JuniperJoint == int.Parse(A_RodDumpBee.instance.ExciteDale.adjust_init_act_position)         //累计广告次数满足adjust_init_act_position条件，且累计广告收入满足adjust_init_adrevenue条件，走LoadAdjust
                && _JuniperArduous >= SmoothLadeMyArduous)
        )
        {
            EmitRatherAxJar();
        }
    }

    /*
     * API
     * 根据行为 初始化 adjust
     *  @param2 打点参数 
     */
    public void EmitRatherAxJar(string param2 = "")
    {
        if (PlayerPrefs.GetString(It_ADDareLadeLull) != "") return;

        // 根据比例分流   adjust_init_rate_act  行为比例
        if (string.IsNullOrEmpty(A_RodDumpBee.instance.ExciteDale.adjust_init_rate_act) || int.Parse(A_RodDumpBee.instance.ExciteDale.adjust_init_rate_act) > Random.Range(0, 100))
        {
            print("user finish  act  and  init adjust");
            PlayerPrefs.SetString(It_ADDareLadeLull, AdjustStatus.OpenAsAct.ToString());
            RatherLade();

            // 上报点位 新用户达成 且 初始化
            A_GangVenusElliot.Instance.AtomVenus("1091", AgeRatherDust(), param2);
        }
        else
        {
            print("user finish  act  and  not init adjust");
            PlayerPrefs.SetString(It_ADDareLadeLull, AdjustStatus.CloseAsAct.ToString());
            // 上报点位 新用户达成 且  不初始化
            A_GangVenusElliot.Instance.AtomVenus("1092", AgeRatherDust(), param2);
        }
    }

    
    /*
     * API
     *  重置当前次数
     */
    public void TableJarJoint()
    {
        print("clear current ");
        _JuniperJoint = 0;
    }


    // 获取启动时间
    private string AgeRatherDust()
    {
        return Agility() - long.Parse(PlayerPrefs.GetString(It_ADDareDust)) + "";
    }
}
