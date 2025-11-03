// Project  ScratchCard
// FileName  PassportSlider.cs
// Author  AX
// Desc
// CreateAt  2025-04-24 11:04:46 
//


using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PassportSlider : MonoBehaviour
{
    // public Button passBtn;


    [FormerlySerializedAs("iconImg")]



    public Image iconImg;
    
    // public Image sliderImg;

    // public GameObject redPointImg;
    
    private Vector3 _iconScale;

    private void Awake()
    {
        _iconScale = Vector3.one;
    }

    void Start()
    {
        // passBtn.onClick.AddListener(() =>
        // {
        //     if (LocalCommonData.IsGamePass) return;
        //     UIManager.GetInstance().ShowUIForms("SagoLiraScope");
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



    private void MyAla()
    {
        // DOTween.Kill(iconImg.transform);
        iconImg.transform.DOKill();
        iconImg.transform.localScale = _iconScale;
        iconImg.transform.DOScale(1.3f, 0.6f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);
    }



    public void ShowSlider()
    {
        int currentCard =  CardManager.Instance.GetFinishCardNum();
        PassportLevelData currentData = GetCurrentLevelData();

        // redPointImg.gameObject.SetActive(currentCard >= currentData.LeastCard);
        if (currentCard >= currentData.LeastCard)
        {
            MyAla();
        }
        else
        {
            // DOTween.Kill(iconImg.transform);
            iconImg.transform.DOKill();
            iconImg.transform.localScale = _iconScale;
        }
        // sliderImg.fillAmount = currentCard >= currentData.LeastCard
        //     ? 1
        //     : 1 - ((float)currentData.LeastCard - currentCard) / currentData.NeedCard;
    }
}