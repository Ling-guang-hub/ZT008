// Project  BlockDropRush
// FileName  PlayBtnCtrl.cs
// Author  AX
// Desc
// CreateAt  2025-09-15 15:09:48 
//


using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayBtnCtrl : MonoBehaviour
{
    [FormerlySerializedAs("unlockText")]

    public Text unlockText;

    [FormerlySerializedAs("lockBg")]


    public GameObject lockBg;

    [FormerlySerializedAs("lockMask")]


    public GameObject lockMask;

    private int GetNeedNum()
    {
        return LocalCardData.CardParamDict[LocalCommonData.CurrentCardId].UnlockLine -
               CardManager.Instance.GetFinishCardNum();
    }

    public void ShowUI()
    {
        int num = GetNeedNum();
        unlockText.text = "PLAY  " + num + " CARD UNLOCK";
        unlockText.gameObject.SetActive(num > 0);
        lockBg.gameObject.SetActive(num > 0);
        lockMask.gameObject.SetActive(num > 0);
        GetComponent<Button>().enabled = num <= 0;
    }
}