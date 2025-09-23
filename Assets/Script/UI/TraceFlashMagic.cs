// Project  ScratchCard
// FileName  TraceFlashMagic.cs
// Author  AX
// Desc
// CreateAt  2025-04-16 14:04:44 
//


using System;
using System.Globalization;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TraceFlashMagic : BaseUIForms
{
    // public Button GutPartBuy;

    [FormerlySerializedAs("getNormalBtn")]


    public Button GutPotterBuy;

    [FormerlySerializedAs("cashImg")]


    public GameObject PulpBog;
    [FormerlySerializedAs("coinImg")]

    public GameObject GenuBog;
    [FormerlySerializedAs("cardImg")]

    public Image SoarBog;

    [FormerlySerializedAs("rewardText")]


    public Text CorpseBent;

    private double _CageBureau;

    private Sprite _SoarMormon;

    private static readonly int SunsetRelic = 2;

    private string _AxFlyway;

    void Start()
    {
        GutPotterBuy.onClick.AddListener(() =>
        {
            ADManager.Instance.NoThanksAddCount();
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_2);
            SowPotterSunset();
        });
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Reward);
        PostDeed();
    }


    // private void VoleSowBuy()
    // {
    //     GutPotterBuy.gameObject.SetActive(true);
    //     GutPotterBuy.enabled = true;
    //     // GutPartBuy.enabled = true;
    // }


    private void PostDeed()
    {
        PulpBog.gameObject.SetActive(false);
        GenuBog.gameObject.SetActive(false);
        SoarBog.gameObject.SetActive(false);


        GutPotterBuy.gameObject.SetActive(true);
        GutPotterBuy.enabled = true;
        _AxFlyway = "0";

        _CageBureau = LocalWheelData.WheelAmount;
        _SoarMormon = LocalWheelData.WheelCardSprite;
        CorpseBent.color = new Color32(255, 213, 31, 255);

        switch (LocalWheelData.WheelType)
        {
            case CommonRewardType.Coin:
                GenuBog.gameObject.SetActive(true);
                break;
            case CommonRewardType.Cash:
                CorpseBent.color = new Color32(150, 255, 63, 255);
                PulpBog.gameObject.SetActive(true);
                break;
            case CommonRewardType.Card:
                SoarBog.sprite = _SoarMormon;
                SoarBog.gameObject.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        CorpseBent.text = "" + _CageBureau;
    }

    // private void CarbonAstute()
    // {
    //     // MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Num_roll);
    //     AnimationController.CarbonAstute(_CageBureau, _CageBureau * SunsetRelic, 0.2f, CorpseBent,
    //         () => { });
    //     _CageBureau *= SunsetRelic;
    // }

    // private async void SowPartSunset()
    // {
    //     _AxFlyway = "1";
    //     // GutPartBuy.gameObject.SetActive(false);
    //     // GutPotterBuy.gameObject.SetActive(false);
    //     GutPotterBuy.enabled = false;
    //     // GutPartBuy.enabled = false;
    //     
    //     CarbonAstute();
    //     await UniTask.Delay(1500);
    //     SowSunsetCanRecurMagic();
    // }


    private  void SowPotterSunset()
    {
        GutPotterBuy.enabled = false;
        SowSunsetCanRecurMagic();
    }

    private void SowSunsetCanRecurMagic()
    {
        LocalWheelData.WheelAmount = _CageBureau;
        PostEventScript.GetInstance().SendEvent("1008", _AxFlyway);
        TraceMagic.Instance.CloseWheel();
        CloseUIForm(GetType().Name);
    }
}