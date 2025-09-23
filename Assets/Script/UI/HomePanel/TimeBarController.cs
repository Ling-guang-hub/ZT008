// Project  ScratchCard
// FileName  TimeBarController.cs
// Author  AX
// Desc
// CreateAt  2025-04-07 16:04:50 
//


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;
using UnityEngine.UI;

public class TimeBarController : MonoBehaviour
{
    public static TimeBarController Instance;

    [FormerlySerializedAs("iconImg")]


    public Image iconImg;

    [FormerlySerializedAs("cardNumText")]


    public Text cardNumText;

    [FormerlySerializedAs("timeText")]


    public Text timeText;

    [FormerlySerializedAs("addCardBtn")]


    public Button addCardBtn;

    private static readonly string _maxStr = "Max !!!";

    [FormerlySerializedAs("baseCardIconAtlas")]


    public SpriteAtlas baseCardIconAtlas;

    private Dictionary<string, Sprite> _cardIconSpritesDict;


    private void Awake()
    {
        Instance = this;
        PostDeed();
    }


    public void Update()
    {
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
        addCardBtn.onClick.AddListener(SowPartKnap);


        MessageCenterLogic.GetInstance().Register(CConfig.mg_GetCardByAd, (md) => { ShowCardNum(); });

        MessageCenterLogic.GetInstance().Register(CConfig.mg_ShowCardMaxStr, (md) => { ShowMaxData(); });

        MessageCenterLogic.GetInstance().Register(CConfig.mg_ShowCardTime, (md) => { ShowTimeData(); });
    }

    public void ResetIcon()
    {
        iconImg.sprite = _cardIconSpritesDict[LocalCardData.CardTypeDict[LocalCommonData.CurrentCardId].ToString()];
    }


    private void SowPartKnap()
    {
        PanelManager.Instance.ShowCardStore();
    }


    private void ShowCardNum()
    {
        ResetIcon();
        int cardNum = GameDataManager.GetInstance().GetCard();
        cardNumText.text = cardNum + "";
    }

    private void ShowTimeData()
    {
        if (!addCardBtn.gameObject.activeInHierarchy) return;
        TimeSpan t = TimeSpan.FromSeconds(CardTimeManager.GetInstance().GetCurTime());
        timeText.text = $"{t.Hours:00}:{t.Minutes:00}:{t.Seconds:00}";
    }

    private void ShowMaxData()
    {
        timeText.text = _maxStr;
        ShowCardNum();
    }
}