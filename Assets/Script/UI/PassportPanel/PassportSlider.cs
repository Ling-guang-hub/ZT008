// Project  ScratchCard
// FileName  PassportSlider.cs
// Author  AX
// Desc
// CreateAt  2025-04-24 11:04:46 
//


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PassportSlider : MonoBehaviour
{
    // public Button passBtn;

    [FormerlySerializedAs("sliderImg")]


    public Image sliderImg;

    [FormerlySerializedAs("redPointImg")]


    public GameObject redPointImg;


    void Start()
    {
        // passBtn.onClick.AddListener(() =>
        // {
        //     if (LocalCommonData.IsGamePass) return;
        //     UIManager.GetInstance().ShowUIForms("EdgeBothMagic");
        // });
    }


    private PassportLevelData GetCurrentLevelData()
    {
        List<PassportLevelData> dataList = GameUtil.GetPassportData();
        PassportLevelData currentLevelData = dataList[0];
        for (int i = 0; i < dataList.Count; i++)
        {
            KeyValuePair<int, int> pair = GameDataManager.GetInstance().GetPassportIdxReward(i);
            if (pair.Key == 0)
            {
                currentLevelData = dataList[i];
                break;
            }
        }

        return currentLevelData;
    }


    public void ShowSlider()
    {
        int currentCard =  CardManager.Instance.GetFinishCardNum();
        PassportLevelData currentData = GetCurrentLevelData();

        redPointImg.gameObject.SetActive(currentCard >= currentData.LeastCard);
        sliderImg.fillAmount = currentCard >= currentData.LeastCard
            ? 1
            : 1 - ((float)currentData.LeastCard - currentCard) / currentData.NeedCard;
    }
}