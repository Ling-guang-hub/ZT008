// Project  BlockDropRush
// FileName  MooseAbuseScope.cs
// Author  AX
// Desc
// CreateAt  2025-10-29 09:10:48 
//


using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MooseAbuseScope : BaseUIForms
{
    [FormerlySerializedAs("getMoreBtn")]

    public Button HemAmidJet;

    [FormerlySerializedAs("getNormalBtn")]


    public Button HemJungleJet;

    [FormerlySerializedAs("mainContentObj")]


    public GameObject SiltReshapeAll;

    [FormerlySerializedAs("coinText")]


    public Text FoulIraq;

    [FormerlySerializedAs("rewardImg")]


    public Image RevereGap;

    private WinPanelData _BowScopeIraq;

    private decimal _LikeArmory;

    private decimal _RevereFaint = 0.1m;

    private string _HeAdmire;


    private void Start()
    {
        HemAmidJet.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            ADManager.Instance.playRewardVideo((success) =>
            {
                if (success)
                {
                    SeeAmidAbuse();
                }
            }, "11");
        });

        HemJungleJet.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_2);
            ADManager.Instance.NoThanksAddCount();
            
            SeeJungleWisdom();
        });
    }


    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        _BowScopeIraq = (WinPanelData)uiFormParams;
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Reward);
        LintIraq();
        Invoke(nameof(BoreSeeJet), 0.3f);
    }

    private void BoreSeeJet()
    {
        HemJungleJet.gameObject.SetActive(true);
    }

    private void TroutJet()
    {
        HemJungleJet.gameObject.SetActive(false);
        HemAmidJet.gameObject.SetActive(false);
    }

    private void LintIraq()
    {
        _LikeArmory = 0;
        _HeAdmire = "0";
        HemAmidJet.gameObject.SetActive(true);
        HemJungleJet.gameObject.SetActive(false);

        _LikeArmory =decimal.Round(_BowScopeIraq.CashAmount,2);
        FoulIraq.text = "" + _LikeArmory;
        RevereGap.sprite = PanelManager.Instance.GetRewardSprite("Cash", _LikeArmory);
    }



    private async void SeeAmidAbuse()
    {
        if (IsInvoking(nameof(BoreSeeJet)))
        {
            CancelInvoke(nameof(BoreSeeJet));
        }

        TroutJet();
        // _HeAdmire = "1";
        
        // QuincyLegacy();
        await UniTask.Delay(200);
        SeeWisdomFinTrout();
    }

    private async void SeeJungleWisdom()
    {
        TroutJet();
        _LikeArmory =decimal.Round(_LikeArmory * _RevereFaint,2);
        await UniTask.Delay(200);
        SeeWisdomFinTrout();
    }


    private void SeeWisdomFinTrout()
    {
        // PostEventScript.GetInstance().SendEvent("1008", "0");
        FishScope.Instance.AfterCollectBonus(0, _LikeArmory);
        MessageCenterLogic.GetInstance().Send(CConfig.mg_ClosePanel);
        MessageCenterLogic.GetInstance().Send(CConfig.mg_GameSuspend);
        CloseUIForm(GetType().Name);
    }
}