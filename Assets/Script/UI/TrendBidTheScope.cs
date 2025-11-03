// Project  ScratchCard
// FileName  TrendAbuseScope.cs
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

public class TrendBidTheScope : BaseUIForms
{
    [FormerlySerializedAs("getMoreBtn")]

    public Button HemAmidJet;

    [FormerlySerializedAs("getNormalBtn")]


    public Button HemJungleJet;

    [FormerlySerializedAs("cashImg")]


    public GameObject LikeGap;
    [FormerlySerializedAs("coinImg")]

    public GameObject FoulGap;
    [FormerlySerializedAs("cardImg")]

    public Image CaneGap;

    [FormerlySerializedAs("rewardText")]


    public Text RevereIraq;

    [FormerlySerializedAs("titleSpineObj")]


    public GameObject BatheGreekAll;

    [FormerlySerializedAs("wheelItemObj")]


    public GameObject ShaleBurnAll;
    
    private SkeletonGraphic BatheContrast;

    private double _CellArmory;

    private CommonRewardType _CellFlop;

    private int _CaneAx;

    private Sprite _CaneBeside;

    private static  float _RevereFaint = 2;

    private string _HeAdmire;

    void Start()
    {
        BatheContrast = BatheGreekAll.GetComponent<SkeletonGraphic>();
        BatheContrast.AnimationState.Complete += BoreMottle;

        HemAmidJet.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);

            ADManager.Instance.playRewardVideo((success) =>
            {
                if (success)
                {
                    SeeAmidWisdom();
                }
            }, "1");
        });

        HemJungleJet.onClick.AddListener(() =>
        {
            ADManager.Instance.NoThanksAddCount();
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_2);
            SeeJungleWisdom();
        });
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Bigwin);
        LintIraq();
        MyPile();
        
        ShaleBurnAll.GetComponent<BigWinWheelItem>().LintIraq();
        
        Invoke(nameof(BoreSeeJet), 1.5f);
        
    }

    private void LeakyPlutoSake()
    {
        BatheGreekAll.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "animation", false);
    }

    private void MyPile()
    {
        BatheGreekAll.SetActive(true);
        LeakyPlutoSake();
    }

    private void BoreSeeJet()
    {
        HemJungleJet.gameObject.SetActive(true);
        HemJungleJet.enabled = true;
    }


    private void LintIraq()
    {
        _RevereFaint = 1.5f;
        LikeGap.gameObject.SetActive(false);
        FoulGap.gameObject.SetActive(false);
        CaneGap.gameObject.SetActive(false);

        ShaleBurnAll.gameObject.SetActive(true);
        
        HemAmidJet.enabled = true;
        
        HemJungleJet.enabled = false;
        HemJungleJet.gameObject.SetActive(false);
        _HeAdmire = "0";

        _CellFlop = LocalWheelData.WheelType;
        _CellArmory = LocalWheelData.WheelAmount;
        _CaneAx = LocalWheelData.WheelCardId;
        _CaneBeside = LocalWheelData.WheelCardSprite;

        RevereIraq.color = new Color32(255, 213, 31, 255);
        switch (LocalWheelData.WheelType)
        {
            case CommonRewardType.Coin:
                FoulGap.gameObject.SetActive(true);
                break;
            case CommonRewardType.Cash:
                RevereIraq.color = new Color32(150, 255, 63, 255);
                LikeGap.gameObject.SetActive(true);
                break;
            case CommonRewardType.Card:
                CaneGap.sprite = _CaneBeside;
                CaneGap.gameObject.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        RevereIraq.text = "" + _CellArmory;
    }

    private void QuincyLegacy()
    {
        AnimationController.ChangeNumber(_CellArmory, _CellArmory * _RevereFaint, 0.2f, RevereIraq,
            () => { });
        _CellArmory *= _RevereFaint;
    }

    private async void SeeAmidWisdom()
    {
        ShaleBurnAll.GetComponent<BigWinWheelItem>().StopAct(false);
        _RevereFaint =   ShaleBurnAll.GetComponent<BigWinWheelItem>().thisMulti;
        
        if (IsInvoking(nameof(BoreSeeJet)))
        {
            CancelInvoke(nameof(BoreSeeJet));
        }
        
        HemAmidJet.enabled = false;
        HemJungleJet.enabled = false;
        _HeAdmire = "1";
        QuincyLegacy();
        await UniTask.Delay(1500);
        SeeWisdomFinTrout();
    }

    private async void SeeJungleWisdom()
    {
        ShaleBurnAll.GetComponent<BigWinWheelItem>().StopAct();
        HemAmidJet.enabled = false;
        HemJungleJet.enabled = false;
        await UniTask.Delay(100);
        SeeWisdomFinTrout();
    }

    private void SeeWisdomFinTrout()
    {
        LocalWheelData.WheelAmount = _CellArmory;
        PostEventScript.GetInstance().SendEvent("1007", _HeAdmire);
        TrendScope.Instance.CloseWheel();
        CloseUIForm(GetType().Name);
    }

    private void BoreMottle(TrackEntry trackEntry)
    {
        BatheGreekAll.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "idel", true);
    }
}