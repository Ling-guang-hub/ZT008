// Project  ScratchCard
// FileName  PokerItem.cs
// Author  AX
// Desc
// CreateAt  2025-05-11 12:05:41 
//


using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PokerItem : CommonItem
{

    public GameObject fxObj;
    
    public GameObject pokerObj;
    
    public Image pokerBgImg;

    public Image pokerBgImgOn;

    public Image valueImg;

    public Image valueImgOn;
    
    public Image colourImg;

    public Image colourImgOn;
    
    public List<Sprite> pokerValueOnList;
    public List<Sprite> pokerValueOffList;

    public List<Sprite> pokerColourOnList;
    public List<Sprite> pokerColourOffList;

    private int _pokerValue;

    private int _pokerValueIdx;

    private int _pokerColourIdx;

    private bool _isTarget;


    public void InitPoker(int thisIdx)
    {
        
        fxObj.gameObject.SetActive(false);
        
        _pokerValueIdx = thisIdx;
        _pokerValue = thisIdx + 2;
        _pokerColourIdx = Random.Range(0, 4);

        pokerBgImg.gameObject.SetActive(true);
        pokerBgImgOn.gameObject.SetActive(false);
        
        valueImg.sprite = pokerValueOffList[_pokerValueIdx];
        valueImg.gameObject.SetActive(true);
        
        valueImgOn.gameObject.SetActive(false);
        valueImgOn.sprite = pokerValueOnList[_pokerValueIdx];
        
        colourImg.sprite = pokerColourOffList[_pokerColourIdx];
        colourImg.gameObject.SetActive(true);
        
        colourImgOn.gameObject.SetActive(false);
        colourImgOn.sprite = pokerColourOnList[_pokerColourIdx];
        
    }


    private void SetRedImg()
    {
        pokerBgImg.gameObject.SetActive(false);
        // pokerBgImgOn.gameObject.SetActive(true);
        valueImg.sprite = pokerValueOnList[_pokerValueIdx];
        colourImg.sprite = pokerColourOnList[_pokerColourIdx];
    }


    private void ShowPokerBgImg(Image targetImg,  float durTime, float delay)
    {
        DOTween.Kill(pokerObj.transform);
        
        pokerObj.transform.localScale = pokerBgImg.transform.localScale;
        
        Color color = targetImg.color;
        color.a = 0f;
        targetImg.color = color;
        targetImg.gameObject.SetActive(true);
        
        targetImg.DOFade(1f, durTime).SetDelay(delay).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
         
        });
    }

    private void ShowFxObj()
    {
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Win_prize);
        fxObj.gameObject.SetActive(true);
    }

    public override void ShowTopImg(float durTime, float delay)
    {

        ShowPokerBgImg(pokerBgImgOn, durTime, delay);
        ShowPokerBgImg(valueImgOn, durTime, delay);
        ShowPokerBgImg(colourImgOn, durTime, delay);
        Invoke(nameof(ShowFxObj), delay);


    }


    private void CloseDefaultImg()
    {
        // defaultImg.gameObject.SetActive(false);
    }

    public override void ShowLoopAct()
    {
        DOTween.Kill(pokerBgImgOn.transform, true);
        Color color = pokerBgImgOn.color;
        color.a = 1f;
        pokerBgImgOn.color = color;
        
        pokerObj.transform.localScale = pokerObj.transform.localScale;
        // topImg.gameObject.SetActive(true);
        // Invoke("CloseDefaultImg",0.2f);
        
        pokerObj.transform.DOScale(1.1f, 1.2f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);
    }
}