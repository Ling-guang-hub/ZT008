using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TaskManager : MonoSingleton<TaskManager>
{
    [FormerlySerializedAs("CurLevelTaskData")]

    public LocalTaskData CurLevelTaskData;

    [FormerlySerializedAs("CurItemList")]


    public List<TaskItemData> CurItemList;

    private List<LocalTaskData> _baseTaskDataList;

    private Dictionary<string, TaskItemData> _curTaskItemDataDic;

    private int _finishLevelTotal;

    private int _resetLevelTotal;

    private int _curTaskIndex;

    private bool _isOpenTaskPanel;

    private Coroutine _taskCoroutine;

    [FormerlySerializedAs("CurTaskIndex")]


    public int CurTaskIndex;
    // {
    // get
    // {
    //     // _curTaskIndex = SaveDataManager.GetInt("TaskIndex");
    //     // return _curTaskIndex;
    // }
    // set
    // {
    //     // _curTaskIndex = value > List_TaskTime.Count - 1 ? List_TaskTime.Count - 1 : value;
    //     // SaveDataManager.SetInt("TaskIndex", _curTaskIndex);
    // }
    // }

    private int _countDownTaskTime;

    public int CountDownTaskTime
    {
        get
        {
            _countDownTaskTime = SaveDataManager.GetInt(TaskConstant.CountDownTaskTime);
            return _countDownTaskTime;
        }
        set
        {
            _countDownTaskTime = value;
            SaveDataManager.SetInt(TaskConstant.CountDownTaskTime, value);
        }
    }

    // protected override void Awake()
    // {
    // }

    private void OnApplicationQuit()
    {
        SetQuitTime();
    }

    private List<TaskItemData> GetItemList(List<NetTaskItemData> netItems)
    {
        List<TaskItemData> list = new List<TaskItemData>();
        foreach (var t in netItems)
        {
            TaskItemData newItem = new TaskItemData(t);
            newItem.TaskName = "" + newItem.TaskType + newItem.CardType;
            list.Add(newItem);
        }

        return list;
    }

    //  net config  trans to  local  config
    private void NetTaskToLocalData()
    {
        NetTaskData netList = NetInfoMgr.instance.NetTaskData;

        _baseTaskDataList = new List<LocalTaskData>();

        for (int i = 0; i < netList.task_list.Count; i++)
        {
            LocalTaskData data = new LocalTaskData
            {
                ResetTime = netList.reset_time_list[i],
                ResetAdNum = netList.reset_now_ad_list[i],
                TaskList = GetItemList(netList.task_list[i])
            };
            _baseTaskDataList.Add(data);
        }
    }


    public void TaskDataInit()
    {
        NetTaskToLocalData();

        if (!PlayerPrefs.HasKey(TaskConstant.CurTaskLevel))
        {
            NewUserInit();
        }
        else
        {
            CurTaskIndex = SaveDataManager.GetInt(TaskConstant.CurTaskLevel);
            _finishLevelTotal = SaveDataManager.GetInt(TaskConstant.FinishLevelTotal);
            _resetLevelTotal = SaveDataManager.GetInt(TaskConstant.ResetLevelTotal);
            InitUserValue();
            CountDownTaskTimeInit();
        }
        //
        // if (_taskCoroutine != null)
        // {
        //     StopCoroutine(_taskCoroutine);
        //     _taskCoroutine = null;
        // }

        _taskCoroutine = StartCoroutine(nameof(TaskTimeUpdate));
    }


    public void OpenTaskPanel()
    {
        // if (CheckTaskRewardNotClaimed() && !CommonUtil.IsApple() && NetInfoMgr.instance.GameData.bonustask_open == "open")
        // {

        // UIManager.GetInstance().ShowUIForms(StringConst.KeepMagic);
        // }
    }


    public LocalTaskData GetCurLevelData()
    {
        InitUserValue();
        return CurLevelTaskData;
    }


    public void FinishTaskAndGetReward(string taskName, Vector3 iconPos)
    {
        if (CheckTaskStatus(taskName)) return;
        TaskItemData itemData = _curTaskItemDataDic[taskName];
        itemData.GotReward = true;
        if (itemData.TaskType == TaskType.Task)
        {
            SetNeedADPlayerNumber();
        }
        else
        {
            TakeTask(TaskType.Task);
        }

        FinishTask(taskName);
        GetReward(itemData.TaskRewardType, itemData.TaskReward, iconPos);
        TaskDataUpdate();
    }


    public void TaskDataUpdate()
    {
        KeyValuesUpdate key = new KeyValuesUpdate(TaskConstant.SendTaskDataUpdate, CurItemList);
        MessageCenter.SendMessage(TaskConstant.SendTaskDataUpdate, key);
    }


    private void GetReward(TaskRewardType type, double amount, Vector3 iconPos)
    {
        KeepMagic.Instance.AddCoinAndCash(type ,amount, iconPos);

        // if (rewardType == TaskRewardType.Cash)
        // {
        //     GameDataManager.GetInstance().AddCash(rewardAmount);
        // }
        // else
        // {
        //     GameDataManager.GetInstance().AddCoin((int)rewardAmount);
        // }
    }

    public TaskItemData GetTaskData(string taskName)
    {
        return _curTaskItemDataDic[taskName];
    }

    private void InitUserValue()
    {
        CurLevelTaskData = _baseTaskDataList[CurTaskIndex];

        _curTaskItemDataDic = new Dictionary<string, TaskItemData>();

        foreach (var itemData in CurLevelTaskData.TaskList)
        {
            KeyValuePair<int, bool> pair = GetTaskValue(itemData.TaskName);
            itemData.CurValue = pair.Key;
            itemData.GotReward = pair.Value;
            _curTaskItemDataDic.Add(itemData.TaskName, itemData);
        }
    }

    private void CountDownTaskTimeInit()
    {
        long subSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds() -
                          SaveDataManager.GetLong(TaskConstant.QuitGameTime);

        if (CountDownTaskTime <= subSeconds)
        {
            if (CheckTaskHaveFinish() && CheckSohoTaskOpen())
            {
                UpgradeUserTaskLevel();
            }
        }
        else
        {
            CountDownTaskTime -= (int)subSeconds;
            GetADPlayerNumberCache();
        }
    }

    private void CountDownTaskTimeUpdate()
    {
        if (CountDownTaskTime <= 0)
        {
            UpgradeOrResetLevel();
        }

        KeyValuesUpdate key = new KeyValuesUpdate(TaskConstant.SendCountDownTaskTimeUpdate, CountDownTaskTime);
        MessageCenter.SendMessage(TaskConstant.SendCountDownTaskTimeUpdate, key);
    }

    IEnumerator TaskTimeUpdate()
    {
        while (CountDownTaskTime > 0)
        {
            CountDownTaskTime--;
            CountDownTaskTimeUpdate();
            yield return new WaitForSeconds(1f);
        }
    }


    public bool CheckTaskAllFinish()
    {
        foreach (var t in CurLevelTaskData.TaskList)
        {
            if (!t.GotReward)
            {
                return false;
            }
        }

        return true;
    }


    public bool CheckTaskDoneAndNotGet()
    {
        foreach (var t in CurLevelTaskData.TaskList)
        {
            if (!t.GotReward && t.CurValue >= t.NeedValue)
            {
                return true;
            }
        }

        return false;
    }


    public bool CheckShouldOpenPanel()
    {
        List<string> nameList = new List<string>();

        foreach (var t in CurLevelTaskData.TaskList)
        {
            if (!t.GotReward && t.CurValue >= t.NeedValue)
            {
                string name = GetShowTaskKey(t.TaskName);

                if (!SaveDataManager.GetBool(name))
                {
                    nameList.Add(t.TaskName);
                    SaveDataManager.SetBool(name, true);
                }
            }
        }

        return nameList.Count > 0;
    }


    private void SetNeedADPlayerNumber()
    {
        SaveDataManager.SetInt(TaskConstant.NeedPlayADNumber, GetCurNeedAdNum());
    }

    public void SetHaveADPlayerNumber()
    {
        int newAdNum = SaveDataManager.GetInt(TaskConstant.HavePlayADNumber) + 1;
        SaveDataManager.SetInt(TaskConstant.HavePlayADNumber, newAdNum);

        if (newAdNum >= SaveDataManager.GetInt(TaskConstant.NeedPlayADNumber))
        {
            if (CheckSohoTaskOpen())
            {
                UpgradeUserTaskLevel();
            }
            else
            {
                ResetUserTaskLevel();
            }
        }
        else
        {
            TaskDataUpdate();
        }
    }

    public int GetNeedAdNum()
    {
        return SaveDataManager.GetInt(TaskConstant.NeedPlayADNumber);
    }

    public int GetHaveAdNum()
    {
        return SaveDataManager.GetInt(TaskConstant.HavePlayADNumber);
    }


    public void CheckADPlayerFinish()
    {
    }


    private void NewUserInit()
    {
        CurTaskIndex = 0;
        _resetLevelTotal = 0;
        _finishLevelTotal = 0;
        SaveDataManager.SetInt(TaskConstant.CurTaskLevel, 0);
        SaveDataManager.SetInt(TaskConstant.ResetLevelTotal, 0);
        SaveDataManager.SetInt(TaskConstant.FinishLevelTotal, 0);

        SaveDataManager.SetInt(TaskConstant.CountDownTaskTime, _baseTaskDataList[0].ResetTime);
        SaveDataManager.SetInt(TaskConstant.NeedPlayADNumber, _baseTaskDataList[0].ResetAdNum);
        SaveDataManager.SetInt(TaskConstant.HavePlayADNumber, 0);
        ResetItemData();
    }


    private int GetCurNeedAdNum()
    {
        float num = CountDownTaskTime / (float)CurLevelTaskData.ResetTime
                    * CurLevelTaskData.ResetAdNum;
        return Mathf.CeilToInt(num) <= 0 ? 1 : Mathf.CeilToInt(num);
    }


    private void UpgradeOrResetLevel()
    {
        if (CheckTaskHaveFinish() && CheckSohoTaskOpen())
        {
            UpgradeUserTaskLevel();
        }
        else
        {
            ResetUserTaskLevel();
        }
    }

    private void UpgradeUserTaskLevel()
    {
        if (_finishLevelTotal >= _baseTaskDataList.Count)
        {
            CurTaskIndex = Random.Range(Math.Max(0, _baseTaskDataList.Count - 2), _baseTaskDataList.Count);
        }
        else
        {
            CurTaskIndex++;
        }

        SaveDataManager.SetInt(TaskConstant.CurTaskLevel, CurTaskIndex);
        _finishLevelTotal++;
        SaveDataManager.SetInt(TaskConstant.FinishLevelTotal, _finishLevelTotal);
        ResetUserTaskLevel();
    }


    private void ResetUserTaskLevel()
    {
        _resetLevelTotal = SaveDataManager.GetInt(TaskConstant.ResetLevelTotal) + 1;
        SaveDataManager.SetInt(TaskConstant.ResetLevelTotal, _resetLevelTotal);
        ResetItemData();
    }

    private void ResetItemData()
    {
        CurLevelTaskData = _baseTaskDataList[CurTaskIndex];

        _curTaskItemDataDic = new Dictionary<string, TaskItemData>();

        foreach (var itemData in CurLevelTaskData.TaskList)
        {
            itemData.CurValue = 0;
            itemData.GotReward = false;
            InitTaskValue(itemData.TaskName);
            _curTaskItemDataDic.Add(itemData.TaskName, itemData);
        }

        CountDownTaskTime = CurLevelTaskData.ResetTime;
        TaskDataUpdate();
        ADPlayerNumberInit();
    }


    private void SetQuitTime()
    {
        long ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        SaveDataManager.SetLong(TaskConstant.QuitGameTime, ts);
        CountDownTaskTime--;
    }


    private bool CheckTaskHaveFinish()
    {
        foreach (var itemData in CurLevelTaskData.TaskList)
        {
            KeyValuePair<int, bool> pair = GetTaskValue(itemData.TaskName);
            if (!pair.Value)
            {
                return false;
            }
        }

        return true;
    }

    public bool CheckSohoTaskOpen()
    {
        return true;
        // foreach (var item in CashRedeemManager.instance.CashRedeemList)
        // {
        //     if (item.currentTask != null)
        //     {
        //         /*for (int i = 0; i < item.preConditions.Count; i++)
        //         {*/
        //         if (item.currentTask.type == "Daily")
        //         {
        //             if (item.state == Redeem.RedeemState.Checked)
        //             {
        //                 return true;
        //             }
        //         }
        //         //}
        //     }
        // }

        return false;
    }


    private KeyValuePair<int, bool> GetTaskValue(string taskName)
    {
        KeyValuePair<string, string> taskKeyPair = GetValueKey(taskName);
        // if (!PlayerPrefs.HasKey(taskKeyPair.Key))
        // {
        //     SaveDataManager.SetInt(taskKeyPair.Key, 0);
        // }
        // if (!PlayerPrefs.HasKey(taskKeyPair.Value))
        // {
        //     SaveDataManager.SetBool(taskKeyPair.Key, false);
        // }

        return new KeyValuePair<int, bool>(SaveDataManager.GetInt(taskKeyPair.Key),
            SaveDataManager.GetBool(taskKeyPair.Value));
    }


    public void TakeTask(TaskType taskType, CardType cardType = CardType.Simple)
    {
        string taskName = "" + taskType + cardType;
        if (!_curTaskItemDataDic.ContainsKey(taskName)) return;
        TakeTask(taskName, 1);
    }

    public void TakeTask(string taskName, int num)
    {
        MessageCenterLogic.GetInstance().Send(CConfig.mg_TakeTask);
        string taskKey = GetValueKey(taskName).Key;
        int oldValue = SaveDataManager.GetInt(taskKey);
        SaveDataManager.SetInt(taskKey, oldValue + num);
        UpdateTakeTask(taskName);
    }

    private void UpdateTakeTask(string taskName)
    {
        KeyValuePair<int, bool> pair = GetTaskValue(taskName);
        _curTaskItemDataDic[taskName].CurValue = pair.Key;
        _curTaskItemDataDic[taskName].GotReward = pair.Value;
    }


    private void InitTaskValue(string taskName)
    {
        KeyValuePair<string, string> taskPair = GetValueKey(taskName);
        SaveDataManager.SetInt(taskPair.Key, 0);
        SaveDataManager.SetBool(taskPair.Value, false);

        SaveDataManager.SetBool(GetShowTaskKey(taskName), false);
    }

    private void FinishTask(string taskName)
    {
        // SOHOShopManager.instance.AddCashoutPreCondition
        // SOHOShopManager.instance.AddTaskValue("DailyTask", 1);
        string taskStatus = GetValueKey(taskName).Value;
        SaveDataManager.SetBool(taskStatus, true);
    }

    private bool CheckTaskStatus(string taskName)
    {
        string taskStatus = GetValueKey(taskName).Value;
        return SaveDataManager.GetBool(taskStatus);
    }


    private KeyValuePair<string, string> GetValueKey(string taskName)
    {
        string key = TaskConstant.UserValue + taskName + _resetLevelTotal;
        string status = TaskConstant.UserTaskStatus + taskName + _resetLevelTotal;
        return new KeyValuePair<string, string>(key, status);
    }

    private void ADPlayerNumberInit()
    {
        SaveDataManager.SetInt(TaskConstant.NeedPlayADNumber, CurLevelTaskData.ResetAdNum);
        SaveDataManager.SetInt(TaskConstant.HavePlayADNumber, 0);
    }

    private string GetShowTaskKey(string taskName)
    {
        return TaskConstant.AutoShowTaskPanel + taskName + _resetLevelTotal;
    }

    private void GetADPlayerNumberCache()
    {
        int newNum = GetCurNeedAdNum();
        if (newNum < SaveDataManager.GetInt(TaskConstant.HavePlayADNumber))
        {
            SaveDataManager.SetInt(TaskConstant.HavePlayADNumber, 0);
            SaveDataManager.SetInt(TaskConstant.NeedPlayADNumber, newNum);
        }
    }
}

public class LocalTaskData
{
    [FormerlySerializedAs("TaskList")]

    public List<TaskItemData> TaskList;

    [FormerlySerializedAs("ResetTime")]


    public int ResetTime;

    [FormerlySerializedAs("ResetAdNum")]


    public int ResetAdNum;
}

public class TaskItemData
{
    [FormerlySerializedAs("TaskName")]

    public string TaskName;

    [FormerlySerializedAs("TaskType")]


    public TaskType TaskType;

    [FormerlySerializedAs("CardType")]


    public CardType CardType;

    [FormerlySerializedAs("TaskRewardType")]


    public TaskRewardType TaskRewardType;

    [FormerlySerializedAs("TaskReward")]


    public double TaskReward;

    [FormerlySerializedAs("NeedValue")]


    public int NeedValue;

    [FormerlySerializedAs("CurValue")]


    public int CurValue;

    [FormerlySerializedAs("Desc")]


    public string Desc;

    [FormerlySerializedAs("GotReward")]


    public bool GotReward;

    public TaskItemData(NetTaskItemData netItem)
    {
        TaskReward = netItem.rewad_num;
        // TaskRewardType = netItem.reward_type == "Cash" && !CommonUtil.IsApple()
        //     ? TaskRewardType.Cash
        //     : TaskRewardType.Coin;
        TaskRewardType =TaskRewardType.Coin;
        NeedValue = netItem.num;
        Desc = netItem.des;
        CurValue = 0;
        GotReward = false;
        CardType = CardType.Simple;
        switch (netItem.type)
        {
            case "Finish":
                TaskType = TaskType.Total;
                break;
            case "Lucky":
                TaskType = TaskType.Lucky;
                break;
            case "Task":
                TaskType = TaskType.Task;
                break;
            default:
                TaskType = TaskType.Card;
                CardType = (CardType)Enum.Parse(typeof(CardType), netItem.type);
                break;
        }
    }
}


public enum TaskType
{
    Card,
    Lucky, //  转盘
    Total, //  完成总数
    Task, //   完成任务数量
}

public enum TaskRewardType
{
    Coin,
    Cash,
}

public class TaskConstant
{
    public static readonly string CurTaskLevel = "t_CurTaskLevel";

    public static readonly string ResetLevelTotal = "t_ResetLevelTotal";

    public static readonly string FinishLevelTotal = "t_FinishLevelTotal";

    public static readonly string QuitGameTime = "t_QuitGameTime";

    public static readonly string HavePlayADNumber = "t_HavePlayADNumber";

    public static readonly string NeedPlayADNumber = "t_NeedPlayADNumber";

    public static readonly string UserValue = "t_UserValue";

    public static readonly string UserTaskStatus = "t_UserTaskStatus";

    public static readonly string AutoShowTaskPanel = "t_AutoShowTaskPanel";

    public static readonly string CountDownTaskTime = "t_CountDownTaskTime";

    public static readonly string SendCountDownTaskTimeUpdate = "t_SendCountDownTaskTimeUpdate";

    public static readonly string SendTaskDataUpdate = "t_SendTaskDataUpdate";

    public static readonly string GetRewardAndDoAnim = "t_GetRewardAndDoAnim";


    // public static 
}