using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class KeepMagic : BaseUIForms
{
    
    public static KeepMagic Instance;
    
    [FormerlySerializedAs("closeButton")]

    
    public Button closeButton;

    [FormerlySerializedAs("mainObj")]


    public GameObject mainObj;

    [FormerlySerializedAs("baseTaskItem")]


    public GameObject baseTaskItem;

    [FormerlySerializedAs("taskGroup")]


    public Transform taskGroup;

    [FormerlySerializedAs("taskTimeText")]


    public Text taskTimeText;

    // public Text taskADTimeText;

    [FormerlySerializedAs("taskADNumberText")]


    public Text taskADNumberText;

    [FormerlySerializedAs("taskADPanel")]


    public GameObject taskADPanel;

    [FormerlySerializedAs("adBtn")]


    public Button adBtn;

    [FormerlySerializedAs("topBar")]


    public GameObject topBar;

    private ObjectPool _taskPool;

    private LocalTaskData _localTaskData;

    private Dictionary<int, TaskItem> _taskItemDic;

    private List<TaskItem> _taskItemList;

    private string _resetInStr;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        _taskItemDic = new Dictionary<int, TaskItem>();
        _taskItemList = new List<TaskItem>();
        _resetInStr = "Reset In ";
        TaskPoolInit();
    }


    private void Start()
    {
        MessageCenter.AddMsgListener(TaskConstant.SendCountDownTaskTimeUpdate, OnCountDownTaskTimeUpdate);

        MessageCenter.AddMsgListener(TaskConstant.SendTaskDataUpdate, OnTaskDataUpdate);

        // MessageCenter.AddMsgListener(TaskConstant.GetRewardAndDoAnim, OnGetTaskReward);


        closeButton.onClick.AddListener(() =>
        {
            ADManager.Instance.NoThanksAddCount();
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_2);
            // MessageCenterLogic.GetInstance().Send(CConfig.mg_ClosePanel);
            MessageCenterLogic.GetInstance().Send(CConfig.mg_ClosePanel);
            MessageCenterLogic.GetInstance().Send(CConfig.mg_GameSuspend);
            CloseUIForm(GetType().Name);
        });

        adBtn.onClick.AddListener(() =>
        {
            ADManager.Instance.playRewardVideo((success) =>
            {
                if (success)
                {
                    TaskManager.GetInstance().SetHaveADPlayerNumber();
                }
            }, "10");
        });
    }

    private void OnTaskDataUpdate(KeyValuesUpdate kv)
    {
        TaskItemUpdate();
    }

    private void OnCountDownTaskTimeUpdate(KeyValuesUpdate kv)
    {
        ShowTimeBar();
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);

        if (!SaveDataManager.GetBool(CConfig.sv_FirstTaskPanel))
        {
            PostEventScript.GetInstance().SendEvent("1018");
            SaveDataManager.SetBool(CConfig.sv_FirstTaskPanel, true);
        }

        topBar.gameObject.SetActive(false);
        TaskItemUpdate();
        ShowAct();
    }

    private void OnDisable()
    {
        // BubbleGamePanel.instance.TopPanel.GetComponent<Canvas>().sortingOrder = 299;
        // BubbleManager.GetInstance().IsOpenPanel = false;
        // BubbleManager.GetInstance().SendGamePause(false);
    }


    private void ShowAct()
    {
        mainObj.transform.localScale = Vector3.zero;
        mainObj.transform.DOScale(1f, 0.2f).OnComplete(() =>
        {
            // TaskItemUpdate();
        });
    }

    private void TaskItemDataInit()
    {
        _localTaskData = TaskManager.GetInstance().GetCurLevelData();

        if (_taskItemList.Count < 1)
        {
            _taskItemDic = new Dictionary<int, TaskItem>();
            // foreach (var itemData in _localTaskData.TaskList)
            for (int i = 0; i < _localTaskData.TaskList.Count; i++)
            {
                TaskItemData itemData = _localTaskData.TaskList[i];
                GameObject obj = _taskPool.Get();
                var taskItem = obj.GetComponent<TaskItem>();
                taskItem.transform.localScale = Vector3.one;
                taskItem.SetTaskItemData(itemData);
                _taskItemList.Add(taskItem);
                _taskItemDic.Add(i, taskItem);
            }
        }
        else
        {
            for (int i = 0; i < _localTaskData.TaskList.Count; i++)
            {
                _taskItemList[i].SetTaskItemData(_localTaskData.TaskList[i]);
                _taskItemDic[i] = _taskItemList[i];
            }
        }
    }

    // private void OnGetTaskReward(KeyValuesUpdate kv)
    // {
    //     string taskName = kv.Values.ToString();
    //     TaskItemData data = TaskManager.GetInstance().GetTaskData(taskName);
    //     if (data.TaskReward <= 0) return;
    //
    //     topBar.gameObject.SetActive(true);
    //     if (data.TaskRewardType == TaskRewardType.Cash)
    //     {
    //         topBar.GetComponent<TopBar>().AddCoinAndDoAnima(coinAmount, coinPis, 0, Vector2.zero);
    //     }
    //     else
    //     {
    //         topBar.GetComponent<TopBar>().AddCoinAndDoAnima(0, Vector2.zero, cashAmount, cashPos);
    //     }
    //
    //     Debug.Log("OnGetTaskReward");
    // }


    public  void AddCoinAndCash(TaskRewardType type, double  amount   , Vector3 startPos)
    {
        if (amount <= 0) return;

        topBar.gameObject.SetActive(true);
        
        if (type == TaskRewardType.Coin)
        {
             topBar.GetComponent<TopBar>().AddCoinAndDoAnima((int)amount, startPos, 0, Vector2.zero);
        }
        else
        {
            topBar.GetComponent<TopBar>().AddCoinAndDoAnima(0, Vector2.zero, (decimal)amount, startPos);  
        }

    }

    private void TaskItemUpdate()
    {
        if (TaskManager.GetInstance().CheckTaskAllFinish())
        {
            taskADPanel.SetActive(true);
            ShowAdNum();
        }
        else
        {
            taskADPanel.SetActive(false);
        }

        TaskItemDataInit();
        ShowTimeBar();
    }


    private void ShowTimeBar()
    {
        TimeSpan t = TimeSpan.FromSeconds(TaskManager.GetInstance().CountDownTaskTime);

        if (taskTimeText.isActiveAndEnabled)
        {
            taskTimeText.text = _resetInStr + $"{t.Hours:00}:{t.Minutes:00}:{t.Seconds:00}";
        }

        // if (taskADTimeText.isActiveAndEnabled)
        // {
        // taskADTimeText.text = _resetInStr + $"{t.Hours:00}:{t.Minutes:00}:{t.Seconds:00}";
        // }
    }

    private void ShowAdNum()
    {
        taskADNumberText.text = "Watch" + "(" + TaskManager.GetInstance().GetHaveAdNum() + "/" +
                                TaskManager.GetInstance().GetNeedAdNum() + ")" + "AD resets now";
    }


    private void TaskPoolInit()
    {
        _taskPool = new ObjectPool();
        _taskPool.Init("TaskItem", taskGroup.transform);
        _taskPool.Prefab = baseTaskItem;
    }
}