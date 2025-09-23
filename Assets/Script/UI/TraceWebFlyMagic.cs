// Project  ScratchCard
// FileName  TraceFlashMagic.cs
// Author  AX
// Desc
// CreateAt  2025-04-16 14:04:44 
//


using System;
using System.Globalization;
using Cysharp.Threading.Tasks;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TraceWebFlyMagic : BaseUIForms
{
    [FormerlySerializedAs("getMoreBtn")]

    public Button GutPartBuy;

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

    [FormerlySerializedAs("titleSpineObj")]


    public GameObject CrushWrongSum;

    [FormerlySerializedAs("wheelItemObj")]


    public GameObject FlintBombSum;
    
    private SkeletonGraphic CrushGenerous;

    private double _CageBureau;

    private CommonRewardType _CageSpan;

    private int _SoarSo;

    private Sprite _SoarMormon;

    private static  float _CorpseRelic = 2;

    private string _AxFlyway;

    void Start()
    {
        CrushGenerous = CrushWrongSum.GetComponent<SkeletonGraphic>();
        CrushGenerous.AnimationState.Complete += VoleVerify;

        GutPartBuy.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);

            ADManager.Instance.playRewardVideo((success) =>
            {
                if (success)
                {
                    SowPartSunset();
                }
            }, "1");
        });

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
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Bigwin);
        PostDeed();
        ItMite();
        
        FlintBombSum.GetComponent<BigWinWheelItem>().PostDeed();
        
        Invoke(nameof(VoleSowBuy), 1.5f);
        
    }

    private void HillyAppleYarn()
    {
        CrushWrongSum.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "animation", false);
    }

    private void ItMite()
    {
        CrushWrongSum.SetActive(true);
        HillyAppleYarn();
    }

    private void VoleSowBuy()
    {
        GutPotterBuy.gameObject.SetActive(true);
        GutPotterBuy.enabled = true;
    }


    private void PostDeed()
    {
        _CorpseRelic = 1.5f;
        PulpBog.gameObject.SetActive(false);
        GenuBog.gameObject.SetActive(false);
        SoarBog.gameObject.SetActive(false);

        FlintBombSum.gameObject.SetActive(true);
        
        GutPartBuy.enabled = true;
        
        GutPotterBuy.enabled = false;
        GutPotterBuy.gameObject.SetActive(false);
        _AxFlyway = "0";

        _CageSpan = LocalWheelData.WheelType;
        _CageBureau = LocalWheelData.WheelAmount;
        _SoarSo = LocalWheelData.WheelCardId;
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

    private void CarbonAstute()
    {
        AnimationController.ChangeNumber(_CageBureau, _CageBureau * _CorpseRelic, 0.2f, CorpseBent,
            () => { });
        _CageBureau *= _CorpseRelic;
    }

    private async void SowPartSunset()
    {
        FlintBombSum.GetComponent<BigWinWheelItem>().StopAct(false);
        _CorpseRelic =   FlintBombSum.GetComponent<BigWinWheelItem>().thisMulti;
        
        if (IsInvoking(nameof(VoleSowBuy)))
        {
            CancelInvoke(nameof(VoleSowBuy));
        }
        
        GutPartBuy.enabled = false;
        GutPotterBuy.enabled = false;
        _AxFlyway = "1";
        CarbonAstute();
        await UniTask.Delay(1500);
        SowSunsetCanRecur();
    }

    private async void SowPotterSunset()
    {
        FlintBombSum.GetComponent<BigWinWheelItem>().StopAct();
        GutPartBuy.enabled = false;
        GutPotterBuy.enabled = false;
        await UniTask.Delay(100);
        SowSunsetCanRecur();
    }

    private void SowSunsetCanRecur()
    {
        LocalWheelData.WheelAmount = _CageBureau;
        PostEventScript.GetInstance().SendEvent("1007", _AxFlyway);
        TraceMagic.Instance.CloseWheel();
        CloseUIForm(GetType().Name);
    }

    private void VoleVerify(TrackEntry trackEntry)
    {
        CrushWrongSum.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "idel", true);
    }
}