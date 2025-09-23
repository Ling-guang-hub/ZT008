// Project  ScratchCard
// FileName  TaskBtnController.cs
// Author  AX
// Desc
// CreateAt  2025-06-12 16:06:55 
//


using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class TaskBtnController : MonoBehaviour
{
    [FormerlySerializedAs("redPointObj")]

    public GameObject redPointObj;


    private void Start()
    {
        MessageCenterLogic.GetInstance().Register(CConfig.mg_TakeTask, (md) => { ShowPoint(); });
    }


    public void ShowPoint()
    {
        bool flag = TaskManager.GetInstance().CheckTaskDoneAndNotGet();
        if (gameObject.activeInHierarchy)
        {
            redPointObj.SetActive(flag);
        }
    }
    
    
    
}