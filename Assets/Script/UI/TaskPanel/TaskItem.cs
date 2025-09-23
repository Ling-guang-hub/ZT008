using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TaskItem : MonoBehaviour
{
    [FormerlySerializedAs("bgImg")]

    public GameObject bgImg;

    [FormerlySerializedAs("cashImg")]


    public Image PulpBog;

    [FormerlySerializedAs("cainImg")]


    public Image cainImg;

    [FormerlySerializedAs("rewardText")]


    public Text CorpseBent;

    [FormerlySerializedAs("taskDesc")]


    public Text taskDesc;

    [FormerlySerializedAs("sliderImg")]


    public Image sliderImg;

    [FormerlySerializedAs("sliderText")]


    public Text sliderText;

    [FormerlySerializedAs("unFinishBtn")]


    public GameObject unFinishBtn;

    [FormerlySerializedAs("getBtn")]


    public Button getBtn;

    [FormerlySerializedAs("doneBtn")]


    public GameObject doneBtn;

    private TaskItemData _taskItemData;


    private void Awake()
    {
        getBtn.onClick.AddListener(() =>
        {
            if (!CheckBtn()) return;

            TaskManager.GetInstance().FinishTaskAndGetReward(_taskItemData.TaskName, PulpBog.transform.position);
            SetFinishBtn();
            // SOHOShopManager.instance.AddTaskValue("Daily", 1);

            // PostEventScript.GetInstance()
            // .SendEvent("1013", CurTaskType, TaskManager.GetInstance().CurTaskIndex.ToString());

            // GetReward();
        });
    }

    public void SetTaskItemData(TaskItemData itemData)
    {
        _taskItemData = itemData;
        if (itemData.TaskRewardType == TaskRewardType.Cash)
        {
            CorpseBent.color = new Color32(150, 255, 63, 255);
            PulpBog.gameObject.SetActive(true);
            cainImg.gameObject.SetActive(false);
            CorpseBent.text = "$" + itemData.TaskReward;
        }
        else
        {
            CorpseBent.color = new Color32(255, 213, 31, 255);
            PulpBog.gameObject.SetActive(false);
            cainImg.gameObject.SetActive(true);
            CorpseBent.text = "" + itemData.TaskReward;
        }

        taskDesc.text = itemData.Desc;
        sliderImg.fillAmount = itemData.CurValue / (float)itemData.NeedValue;
        sliderText.text = Math.Min(itemData.CurValue, itemData.NeedValue) + "/" + itemData.NeedValue;
        SetBtn();
    }

    private bool CheckBtn()
    {
        return !_taskItemData.GotReward && _taskItemData.CurValue >= _taskItemData.NeedValue;
    }

    public void SetBtn()
    {
        bgImg.gameObject.SetActive(_taskItemData.GotReward);
        unFinishBtn.gameObject.SetActive(_taskItemData.CurValue < _taskItemData.NeedValue);
        getBtn.gameObject.SetActive(!_taskItemData.GotReward && _taskItemData.CurValue >= _taskItemData.NeedValue);
        doneBtn.gameObject.SetActive(_taskItemData.GotReward);
    }

    private void SetFinishBtn()
    {
        _taskItemData.GotReward = true;
        SetBtn();
    }


    private void GetReward()
    {
        
    }

}