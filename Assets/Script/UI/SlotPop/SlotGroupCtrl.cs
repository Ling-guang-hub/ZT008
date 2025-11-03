// Project  BlockDropRush
// FileName  SlotGroupCtrl.cs
// Author  AX
// Desc
// CreateAt  2025-10-21 17:10:00 
//


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SlotGroupCtrl: MonoBehaviour
{
    
    [FormerlySerializedAs("baseSlotObj")]

    
    public GameObject baseSlotObj;
    [FormerlySerializedAs("slotObjList")]

    public List<GameObject> slotObjList;


    // public List<SlotObjData> rewardObjList;
    [FormerlySerializedAs("rewardObjList")]

    public List<GameObject> rewardObjList;
    
    private int maxIndex;
    private int rewardIndex;

    
    public void LintIraq()
    {
        for (int i = 0; i < maxIndex; i++)
        {
            GameObject objItem = Instantiate(baseSlotObj, transform);
            Vector3 pos = new Vector3();
            pos.y = i * 80f - 160f;
            objItem.transform.localPosition = pos;
            // objItem.GetComponent<SlotObjController>().InitDataRandom();
            slotObjList.Add(objItem);
        }
    }

    // public void CreateRewardObj(SlotObjData rewardData)
    // {
    //
    //     
    //     for (int i = rewardIndex; i < maxIndex; i++)
    //     {
    //         GameObject objItem = slotObjList[i];
    //
    //         if (i == rewardIndex)
    //         {
    //             objItem.GetComponent<SlotObjController>().InitDataByData(rewardData);
    //         }
    //         else
    //         {
    //             objItem.GetComponent<SlotObjController>().InitDataRandom();
    //         }
    //     }
    //     
    //     rewardObjList = new List<SlotObjData>();
    //     for (int i = rewardIndex-2; i < maxIndex; i++)
    //     {
    //         GameObject objItem = slotObjList[i];
    //         SlotObjData tempData = objItem.GetComponent<SlotObjController>().slotObjData;
    //         rewardObjList.Add(tempData);
    //     }
    //     
    // }
    //
    // public void RefreshData()
    // {
    //     // ClearData();
    //     ReInit();
    // }
    //
    // private void ReInit()
    // {
    //     for (int i = 0; i < maxIndex; i++)
    //     {
    //         GameObject objItem = slotObjList[i];
    //         if (i < 5)
    //         {
    //             SlotObjData tarItem = rewardObjList[i];
    //             objItem.GetComponent<SlotObjController>().InitDataByData(tarItem);
    //         }
    //         else
    //         {
    //             objItem.GetComponent<SlotObjController>().InitDataRandom();
    //         }
    //     }
    // }
    
    
}
