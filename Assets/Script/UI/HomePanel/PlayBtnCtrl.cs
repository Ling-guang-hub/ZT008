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


    public void ShowUI()
    {
        int lockLevel = LocalCardData.CardParamDict[LocalCommonData.CurrentCardId].UnlockLine;
        int needNum = lockLevel - CardManager.Instance.GetCurLevel().Key;
        unlockText.text = "Lv." + lockLevel + " card unlock";
        unlockText.gameObject.SetActive(needNum > 0);
        lockBg.gameObject.SetActive(needNum > 0);
        lockMask.gameObject.SetActive(needNum > 0);
        GetComponent<Button>().enabled = needNum <= 0;
    }
}