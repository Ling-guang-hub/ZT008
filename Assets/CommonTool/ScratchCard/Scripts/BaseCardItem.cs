// Project  ScratchCard
// FileName  CardItem.cs
// Author  AX
// Desc
// CreateAt  2025-04-01 14:04:52 
//


using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BaseCardItem : CommonItem
{
    public GameObject fxObj;

    public Image topImg;

    public Image defaultImg;

    public Image goodsImg;

    public Image rewardImg;

    public Text rewardNumText;

    public Text bigText;

    public string itemName;

    public GameObject layoutGroup;


    private void RefreshObj()
    {
        if (layoutGroup == null) return;
        HorizontalLayoutGroup thisGroup = layoutGroup.GetComponent<HorizontalLayoutGroup>();
        if (thisGroup == null) return;

        thisGroup.CalculateLayoutInputHorizontal(); // 计算水平布局
        thisGroup.SetLayoutHorizontal(); // 应用水平布局
        // thisGroup.CalculateLayoutInputVertical();   // 计算垂直布局（如果需要）
        // thisGroup.SetLayoutVertical();

        ContentSizeFitter fitter = rewardNumText.gameObject.GetComponent<ContentSizeFitter>();
        if (fitter == null) return;
        fitter.SetLayoutHorizontal();
        // fitter.SetLayoutVertical();
    }

    private void OnDestroy()
    {
        DOTween.Kill(topImg.transform);
        DOTween.Kill(defaultImg.transform);
    }

    private void CloseObj()
    {
        DOTween.Kill(defaultImg.transform);
        DOTween.Kill(topImg.transform);
        if (fxObj != null)
        {
            fxObj.gameObject.SetActive(false);
        }

        topImg.gameObject.SetActive(false);
        defaultImg.gameObject.SetActive(false);
        goodsImg.gameObject.SetActive(false);
        rewardImg.gameObject.SetActive(false);
        rewardNumText.gameObject.SetActive(false);
        bigText.gameObject.SetActive(false);
    }


    public void SetBigText(string str)
    {
        CloseObj();
        bigText.text = str;
        bigText.gameObject.SetActive(true);
    }


    private void ShowFxObj()
    {
        if (fxObj == null) return;
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Win_prize);
        fxObj.gameObject.SetActive(true);
    }

    
    
     
    public  override void ShowTopImg(float durTime, float delay)
    {
        DOTween.Kill(topImg.transform);
        topImg.transform.localScale = defaultImg.transform.localScale;
        // Color color = topImg.color;
        // color.a = 0f;
        // topImg.color = color;
        topImg.gameObject.SetActive(true);
        // topImg.DOFade(1f).From()
        topImg.DOFade(1f, durTime).From(0f).SetDelay(delay).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
    
        });

        Invoke(nameof(ShowFxObj), delay);
    }

    public override void ShowLoopAct()
    {
        DOTween.Kill(topImg.transform, true);
        Color color = topImg.color;
        color.a = 1f;
        topImg.color = color;

        topImg.transform.localScale = defaultImg.transform.localScale;
        topImg.gameObject.SetActive(true);
        defaultImg.DOFade(0f, 0.2f).SetDelay(1f).SetEase(Ease.InOutQuad);

        topImg.transform.DOScale(1.1f, 1.2f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);
    }

    public void ShowLoopFadeAct()
    {
        DOTween.Kill(topImg.transform, true);
        Color color = topImg.color;
        color.a = 1f;
        topImg.color = color;

        topImg.transform.localScale = defaultImg.transform.localScale;
        topImg.gameObject.SetActive(true);

        topImg.DOFade(0.2f, 1.2f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);
    }


    // only  default
    public void ShowItem(Sprite defaultSprite, Sprite topSprite)
    {
        CloseObj();
        defaultImg.sprite = defaultSprite;
        topImg.sprite = topSprite;

        defaultImg.gameObject.SetActive(true);
    }

    public void ShowItem(Sprite defaultSprite)
    {
        CloseObj();
        defaultImg.sprite = defaultSprite;

        defaultImg.gameObject.SetActive(true);
    }


    //  only reward
    public void ShowItem(BaseRewardItemData rewardItemData)
    {
        CloseObj();

        if (rewardItemData.Type == CommonRewardType.Goods)
        {
            goodsImg.sprite = rewardItemData.RewardSprite;
            goodsImg.gameObject.SetActive(true);
        }
        else
        {
            rewardImg.sprite = rewardItemData.RewardSprite;
            rewardNumText.text = rewardItemData.Amount.ToString();
            rewardNumText.gameObject.SetActive(true);
            rewardImg.gameObject.SetActive(true);
        }

        RefreshObj();
    }

    public void ShowItem(Sprite defaultSprite, Sprite topSprite, BaseRewardItemData rewardItemData)
    {
        CloseObj();
        defaultImg.sprite = defaultSprite;
        topImg.sprite = topSprite;
        defaultImg.gameObject.SetActive(true);

        if (rewardItemData.Type == CommonRewardType.Goods)
        {
            goodsImg.sprite = rewardItemData.RewardSprite;
            goodsImg.gameObject.SetActive(true);
        }
        else
        {
            rewardImg.sprite = rewardItemData.RewardSprite;
            rewardNumText.text = rewardItemData.Amount.ToString();
            rewardNumText.gameObject.SetActive(true);
            rewardImg.gameObject.SetActive(true);
        }

        RefreshObj();
    }
}