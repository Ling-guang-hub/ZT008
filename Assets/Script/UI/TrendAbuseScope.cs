// Project  ScratchCard
// FileName  TrendAbuseScope.cs
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

public class TrendAbuseScope : BaseUIForms
{
    // public Button HemAmidJet;

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

    private double _CellArmory;

    private Sprite _CaneBeside;

    private static readonly int WisdomFaint = 2;

    private string _HeAdmire;

    void Start()
    {
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
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Reward);
        LintIraq();
    }


    // private void BoreSeeJet()
    // {
    //     HemJungleJet.gameObject.SetActive(true);
    //     HemJungleJet.enabled = true;
    //     // HemAmidJet.enabled = true;
    // }


    private void LintIraq()
    {
        LikeGap.gameObject.SetActive(false);
        FoulGap.gameObject.SetActive(false);
        CaneGap.gameObject.SetActive(false);


        HemJungleJet.gameObject.SetActive(true);
        HemJungleJet.enabled = true;
        _HeAdmire = "0";

        _CellArmory = LocalWheelData.WheelAmount;
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

    // private void QuincyLegacy()
    // {
    //     // MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Num_roll);
    //     AnimationController.QuincyLegacy(_CellArmory, _CellArmory * WisdomFaint, 0.2f, RevereIraq,
    //         () => { });
    //     _CellArmory *= WisdomFaint;
    // }

    // private async void SeeAmidWisdom()
    // {
    //     _HeAdmire = "1";
    //     // HemAmidJet.gameObject.SetActive(false);
    //     // HemJungleJet.gameObject.SetActive(false);
    //     HemJungleJet.enabled = false;
    //     // HemAmidJet.enabled = false;
    //     
    //     QuincyLegacy();
    //     await UniTask.Delay(1500);
    //     SeeWisdomFinTroutScope();
    // }


    private  void SeeJungleWisdom()
    {
        HemJungleJet.enabled = false;
        SeeWisdomFinTroutScope();
    }

    private void SeeWisdomFinTroutScope()
    {
        LocalWheelData.WheelAmount = _CellArmory;
        PostEventScript.GetInstance().SendEvent("1008", _HeAdmire);
        TrendScope.Instance.CloseWheel();
        CloseUIForm(GetType().Name);
    }
}