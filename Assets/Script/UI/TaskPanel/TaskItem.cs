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


    public Image LikeGap;

    [FormerlySerializedAs("cainImg")]


    public Image cainImg;

    [FormerlySerializedAs("rewardText")]


    public Text RevereIraq;

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

            TaskManager.GetInstance().FinishTaskAndGetReward(_taskItemData.TaskName, LikeGap.transform.position);
            SetFinishBtn();

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
            RevereIraq.color = new Color32(150, 255, 63, 255);
            LikeGap.gameObject.SetActive(true);
            cainImg.gameObject.SetActive(false);
            RevereIraq.text = "" + itemData.TaskReward;
        }
        else
        {
            RevereIraq.color = new Color32(255, 213, 31, 255);
            LikeGap.gameObject.SetActive(false);
            cainImg.gameObject.SetActive(true);
            RevereIraq.text = "" + itemData.TaskReward;
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