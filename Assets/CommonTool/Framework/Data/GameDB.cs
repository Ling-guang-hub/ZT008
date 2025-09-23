//
// Auto Generated Code By excel2json
// https://neil3d.gitee.io/coding/excel2json.html
// 1. 每个 Sheet 形成一个 Struct 定义, Sheet 的名称作为 Struct 的名称
// 2. 表格约定：第一行是变量名称，第二行是变量类型

// Generate From GameSetting.xlsx

using System;
using System.Collections.Generic;

namespace zeta_framework
{

public class GameSettingDB
{
	public string id; // 配置名称
	public string value; // 配置的值
	public string value_type; // 属性类型
	public string comment; // 注释
}

public class ItemDB
{
	public string id; // 资源ID(名称)
	public string value_type; // 属性类型
	public string comment; // 注释
	public string icon; // 图标
	public int defaultValue; // 默认值
	public int minValue; // 最小值
	public int maxValue; // 最大值
	public int type; // 资源类型(1、消耗类、2、经验类)
}


/// <summary>
/// 1. 资源类为名为'Item'的Sheet中的配置
/// 2. 表格约定：id为属性名称，value_type为属性类型，comment为注释
/// Generate From GameSetting.xlsx -> Sheet[Item]
/// </summary>
public class ResourceDB
{
	public Item gold { get; set; } // 金币
	public Item diamond { get; set; } // 钻石
	public Item amazon { get; set; } // 亚马逊卡
	public Item exp { get; set; } // 经验
}

/// <summary>
/// 1. 资源类为名为'GameSetting'的Sheet中的配置
/// 2. 表格约定：id为属性名称，value_type为属性类型，comment为注释
/// Generate From GameSetting.xlsx -> Sheet[GameSetting]
/// </summary>
public class SettingDB
{
}

}
// End of Auto Generated Code
