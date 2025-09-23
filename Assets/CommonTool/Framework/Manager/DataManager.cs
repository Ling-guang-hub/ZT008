using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zeta_framework;

/// <summary>
/// 数据管理器
/// </summary>

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public GameSettingCtrl gameSetting; // 游戏配置
    public ResourceCtrl resource;   // 资源

    private void Start()
    {
        // 初始化游戏配置和存档
        Init();
    }

    public void Init()
    {
        Instance = this;

        // 初始化配置
        TextAsset text = Resources.Load<TextAsset>("LocationJson/GameSetting");
        JsonData setting = JsonMapper.ToObject(text.text);
        gameSetting = new GameSettingCtrl(setting["GameSetting"]);
        resource = JsonMapper.ToObject<ResourceCtrl>(setting["Item"].ToJson());

        // 读取存档
        string keepin = SaveDataManager.GetString("sv_framework_data");
        JsonData savedData = string.IsNullOrEmpty(keepin) ? new JsonData() : JsonMapper.ToObject(keepin);
        resource.Init(savedData.ContainsKey("resource") ? savedData["resource"] : null);

        // 展示初始数据
        SaveData();
        Debug.Log("数据初始化完成");

        InvokeRepeating(nameof(HandleInterval), 3, 1);
    }

    /// <summary>
    /// 存档
    /// </summary>
    public void SaveData()
    {
        //Debug.Log("Before data save: " + SaveDataManager.GetString("sv_framework_data"));
        Dictionary<string, Dictionary<string, object>> data = new()
        {
            { "resource", resource.GetSerializeData() },
        };

        string saveDataStr = JsonMapper.ToJson(data);
        if (!saveDataStr.Equals(SaveDataManager.GetString("sv_framework_data")))
        {
            SaveDataManager.SetString("sv_framework_data", saveDataStr);
        }
        //Debug.Log("After data save:" + JsonMapper.ToJson(data));
    }

    /// <summary>
    /// 每秒执行的函数，处理例如更新活动状态等
    /// </summary>
    private void HandleInterval()
    {
        
    }

}