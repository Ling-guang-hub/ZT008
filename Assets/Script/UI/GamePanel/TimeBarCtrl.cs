// Project  BlockDropRush
// FileName  TimeBarCtrl.cs
// Author  AX
// Desc
// CreateAt  2025-09-12 10:09:07 
//


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;
using UnityEngine.UI;

public class TimeBarCtrl: MonoBehaviour
{

    [FormerlySerializedAs("iconImg")]


    public Image iconImg;

    [FormerlySerializedAs("cardNumText")]


    public Text cardNumText;

    [FormerlySerializedAs("baseCardIconAtlas")]


    public SpriteAtlas baseCardIconAtlas;

    [FormerlySerializedAs("superRateText")]


    public Text superRateText;

    private Dictionary<string, Sprite> _cardIconSpritesDict;

    private void Awake()
    {
        PostDeed();
    }


    private void PostDeed()
    {
        _cardIconSpritesDict = new Dictionary<string, Sprite>();

        Sprite[] cardIconSprite = new Sprite[baseCardIconAtlas.spriteCount];
        baseCardIconAtlas.GetSprites(cardIconSprite);
        foreach (Sprite sprite in cardIconSprite)
        {
            string originalName = sprite.name.Replace("(Clone)", "");
            _cardIconSpritesDict[originalName] = sprite;
        }

        ResetIcon();
    }


    public void Start()
    {
        MessageCenterLogic.GetInstance().Register(CConfig.mg_GetCardByAd, (md) => { ShowCardNum(); });
    }

    private void ResetIcon()
    {
        iconImg.sprite = _cardIconSpritesDict[LocalCardData.CardTypeDict[LocalCommonData.CurrentCardId].ToString()];
    }


    private void ShowCardNum()
    {
        ResetIcon();
        int cardNum = GameDataManager.GetInstance().GetCard();
        cardNumText.text = cardNum + "";
        superRateText.text = CardManager.Instance.GetNextCardRate() + "%";
    }

}
