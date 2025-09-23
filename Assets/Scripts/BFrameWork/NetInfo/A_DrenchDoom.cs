using UnityEngine;

public class A_DrenchDoom
{
    static string Beef_AP; //ApplePie的本地存档 存储第一次进入状态 未来不再受ApplePie开关影响
    static string Emphasize; //距离黑名单位置的距离 打点用
    static string Sister; //进审理由 打点用
    [HideInInspector] public static string PeckTie= ""; //判断流程 打点用

    public static void AtomVenus()
    {
        //打点
        if (A_RodDumpBee.instance.GoldDale != null)
        {
            string Info1 = "[" + (Beef_AP == "A" ? "审" : "正常") + "] [" + Sister + "]";
            string Info2 = "[" + A_RodDumpBee.instance.GoldDale.lat + "," + A_RodDumpBee.instance.GoldDale.lon + "] [" + A_RodDumpBee.instance.GoldDale.regionName + "] [" + Emphasize + "]";
            string Info3 = "[" + A_RodDumpBee.instance.GoldDale.query + "] [Null]";  // [" + Adjust_TrackerName + "]";
            A_GangVenusElliot.Instance.AtomVenus("3000", Info1, Info2, Info3);
            Debug.Log($"3000点位，{Info1}，{Info2}，{Info3}");
        }
        else
            A_GangVenusElliot.Instance.AtomVenus("3000", "No UserData");
        A_GangVenusElliot.Instance.AtomVenus("3001", (Beef_AP == "A" ? "审" : "正常"), PeckTie, A_RodDumpBee.instance.DaleScar);
        PlayerPrefs.SetInt("SendedEvent", 1);
    }
}
