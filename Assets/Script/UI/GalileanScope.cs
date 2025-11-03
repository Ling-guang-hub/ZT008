using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GalileanScope : BaseUIForms
{
    // [Header("按钮")]
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

    private decimal _RevereFaint = 2;

    private string _HeAdmire;


    // Start is called before the first frame update
    void Start()
    {
        HemAmidJet.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            ADManager.Instance.playRewardVideo((success) =>
            {
                if (success)
                {
                    SeeAdmireWisdom();
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
        HemJungleJet.enabled = true;
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
        HemJungleJet.enabled = false;


        _LikeArmory = decimal.Round(_BowScopeIraq.CashAmount, 2);
        FoulIraq.text = "" + _LikeArmory;
        RevereGap.sprite = _BowScopeIraq.PanelType == PanelType.Slot
            ? _BowScopeIraq.RewardSprite
            : PanelManager.Instance.GetRewardSprite("Cash", _LikeArmory);
    }

    private void QuincyLegacy()
    {

        decimal endNum = Decimal.Multiply(_LikeArmory, _RevereFaint);
        AnimationController.ChangeNumber(_LikeArmory, endNum, 0.2f, FoulIraq,
            () => { });
        _LikeArmory = endNum;
    }


    private async void SeeAdmireWisdom()
    {
        if (IsInvoking(nameof(BoreSeeJet)))
        {
            CancelInvoke(nameof(BoreSeeJet));
        }

        TroutJet();

        _HeAdmire = "1";
        QuincyLegacy();
        await UniTask.Delay(1500);
        SeeWisdomFinTrout();
    }

    private async void SeeJungleWisdom()
    {
        TroutJet();
        await UniTask.Delay(200);
        SeeWisdomFinTrout();
    }


    private void SeeWisdomFinTrout()
    {
        // PostEventScript.GetInstance().SendEvent("1008", "0");

        switch (_BowScopeIraq.PanelType)
        {
            case PanelType.Card:
                FishScope.Instance.AfterCardBonus(0, _LikeArmory);
                break;
            case PanelType.Slot:
                TaleScope.Instance.DoClosePanel();
                break;
            case PanelType.Wheel:
                LocalWheelData.WheelAmount = decimal.ToDouble(_LikeArmory);
                // PostEventScript.GetInstance().SendEvent("1008", _HeAdmire);
                TrendScope.Instance.CloseWheel();
                break;
            default:
                FishScope.Instance.AfterCollectBonus(0, _LikeArmory);
                break;
        }

        CloseUIForm(GetType().Name);
    }
}