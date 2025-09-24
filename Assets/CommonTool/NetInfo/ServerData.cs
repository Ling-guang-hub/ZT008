using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//登录服务器返回数据
public class RootData
{
    public int code { get; set; }
    public string msg { get; set; }
    public ServerData data { get; set; }
}

//用户登录信息
public class ServerUserData
{
    public int code { get; set; }
    public string msg { get; set; }
    public int data { get; set; }
}

//服务器的数据
public class ServerData
{
    public string init { get; set; }
    public string version { get; set; }

    public string apple_pie { get; set; }
    public string inter_b2f_count { get; set; }
    public string inter_freq { get; set; }

    public string inter_delay { get; set; }
    public string relax_interval { get; set; }
    public string trial_MaxNum { get; set; }

    public string ad_fail_interval { get; set; }
    public string nextlevel_interval { get; set; }
    public string adjust_init_rate_act { get; set; }
    public string adjust_init_act_position { get; set; }
    public string adjust_init_adrevenue { get; set; }
    public string soho_shop { get; set; }

    public string game_data { get; set; }

    public string task_data { get; set; }

    public string rate_us_limit { get; set; }

    public string fall_down { get; set; }
    // public string HeiNameList { get; set; } //IP黑名单列表
    // public string LocationList { get; set; } //黑位置列表
    // public string HeiCity { get; set; } //城市黑名单列表
    
    // public string CashOut_MoneyName { get; set; } //货币名称
    // public string CashOut_Description { get; set; } //玩法描述
    // public string convert_goal { get; set; } //兑换目标
    
    public string BlockRule { get; set; } //屏蔽规则
    
    public string CashOut_Data { get; set; } //真提现数据
    
}

public class Init
{
    public List<SlotItem> slot_group { get; set; }

    public double[] cash_random { get; set; }
    public MultiGroup[] cash_group { get; set; }
    public MultiGroup[] gold_group { get; set; }
    public MultiGroup[] amazon_group { get; set; }
}

public class SlotItem
{
    public int multi { get; set; }
    public int weight { get; set; }
}

public class MultiGroup
{
    public int max { get; set; }
    public int multi { get; set; }
}





public class GameData
{

    public int first_coin { get; set; }
    
    public List<int> random_coin { get; set; }

    public int bigwin_limit { get; set; }

    public List<int> wheel_step { get; set; }
    
    public int fly_coin_step { get; set; }

    public int fly_cash_step { get; set; }

    public int focus_card { get; set; }

    public int new_user_focus { get; set; }

    public List<int> active_card { get; set; }

    public List<NetCardParam> card_reward { get; set; }

    public List<NetWeightData> wheel_weight_group { get; set; }

    public List<NetPassportData> passport_data_group { get; set; }
    
    public List<NetWeightData> collect_weight_group { get; set; }

    public int passport_day { get; set; }
}


public class NetPassportData
{
    public string type { get; set; }

    public int count { set; get; }

    public int cash { set; get; }
    public int card { set; get; }
}


public class NetWeightData
{
    public string type { set; get; }
    public int weight { set; get; }

    public int count { set; get; }

    public int goal { set; get; }
}

public class NetCardParam
{
    public int id { get; set; }
    public string type { get; set; }

    public List<NetWeightData> card_weight { get; set; }

    public int limit { get; set; }

    public int refresh_time { get; set; }

    public int unlock { get; set; }
    
    public int wheel_value { get; set; }
    
    public string name { get; set; }
    
    public string desc { get; set; }
    
}

public class UserRootData
{
    public int code { get; set; }
    public string msg { get; set; }
    public string data { get; set; }
}

public class LocationData
{
    public double X;
    public double Y;
    public double Radius;
}

public class UserInfoData
{
    public double lat;
    public double lon;
    public string query; //ip地址
    public string regionName; //地区名称
    public string city; //城市名称
    public bool IsHaveApple; //是否有苹果
}

public class BlockRuleData //屏蔽规则
{
    public LocationData[] LocationList; //屏蔽位置列表
    public string[] CityList; //屏蔽城市列表
    public string[] IPList; //屏蔽IP列表
    public string fall_down; //自然量
    public bool BlockVPN; //屏蔽VPN
    public bool BlockSimulator; //屏蔽模拟器
    public bool BlockRoot; //屏蔽root
    public bool BlockDeveloper; //屏蔽开发者模式
    public bool BlockUsb; //屏蔽USB调试
    public bool BlockSimCard; //屏蔽SIM卡
}

public class CashOutData //提现
{
    public string MoneyName; //货币名称
    public string Description; //玩法描述
    public string convert_goal; //兑换目标
    public List<CashOut_TaskData> TaskList; //任务列表
}

public class CashOut_TaskData
{
    public string Name; //任务名称
    public float NowValue; //当前值
    public double Target; //目标值
    public string Description; //任务描述
    public bool IsDefault; //是否默认（循环）任务
}



public class NetTaskData
{
    public List<List<NetTaskItemData>> task_list { get; set; }
    public List<int> reset_time_list { get; set; }
    public List<int> reset_now_ad_list { get; set; }
}

public class NetTaskItemData
{
    public string type { get; set; }
    public int num { get; set; }
    public string des { get; set; }
    public string reward_type { get; set; }
    public double rewad_num { get; set; }
}
