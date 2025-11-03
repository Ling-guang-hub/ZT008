// Project  BlockDropRush
// FileName  FlyPopManager.cs
// Author  AX
// Desc
// CreateAt  2025-10-21 11:10:30 
//


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FlyPopManager : MonoBehaviour
{
    [FormerlySerializedAs("baseFlyObj")]

    public GameObject baseFlyObj;

    [FormerlySerializedAs("curFlyObj")]


    public GameObject curFlyObj;

    private int _timeStep = 20;

  
    private void Start()
    {
     

        MessageCenterLogic.GetInstance().Register(CConfig.mg_FinishFlyBox, (md) => { StartFly(); });

        MessageCenterLogic.GetInstance().Register(CConfig.mg_PassAnim, (md) => { CloseBox(); });
        MessageCenterLogic.GetInstance().Register(CConfig.mg_ClosePanel, (md) => { StartFly(); });
    }


    private void CloseBox()
    {
        if (curFlyObj != null)
        {
            Destroy(curFlyObj);
            curFlyObj = null;
        }
    }

    
    public void StartFly()
    {
        if (!SaveDataManager.GetBool(CConfig.sv_FinishNewGuide)) return;

        if (curFlyObj != null)
        {
            Destroy(curFlyObj);
            curFlyObj = null;
        }

        if (IsInvoking(nameof(InitFlyBox)))
        {
            CancelInvoke(nameof(InitFlyBox));
        }

        Invoke(nameof(InitFlyBox), _timeStep);
    }

    public void InitFlyBox()
    {
        curFlyObj = Instantiate(baseFlyObj, transform);
        curFlyObj.gameObject.SetActive(true);
        // curFlyObj.GetComponent<FlyPopCtrl>().
    }


    public void PopPause()
    {
        if (curFlyObj == null) return;
        curFlyObj.GetComponent<FlyPopCtrl>().PopPause();
    }

    public void PopResume()
    {
        if (curFlyObj == null) return;
        curFlyObj.GetComponent<FlyPopCtrl>().PopResume();
    }
}