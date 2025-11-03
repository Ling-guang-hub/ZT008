using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BidTheScope : BaseUIForms
{
    // [Header("按钮")]
    [FormerlySerializedAs("getMoreBtn")]

    public Button HemAmidJet;

    [FormerlySerializedAs("getNormalBtn")]


    public Button HemJungleJet;

    [FormerlySerializedAs("wheelItemObj")]


    public GameObject ShaleBurnAll;

    [FormerlySerializedAs("onlyCashImg")]


    public GameObject ToilDivaGap;

    [FormerlySerializedAs("mainContentObj")]


    public GameObject SiltReshapeAll;

    [FormerlySerializedAs("onlyCashText")]


    public Text ToilDivaIraq;

    [FormerlySerializedAs("rewardImg")]


    public Image RevereGap;

    [FormerlySerializedAs("titleSpineObj")]


    public GameObject BatheGreekAll;

    [FormerlySerializedAs("slotSprite")]


    public Sprite WiseBeside;

    private WinPanelData _BowScopeIraq;

    private decimal _LikeArmory;

    private static decimal _RevereFaint;

    private SkeletonGraphic _BatheContrast;

    private string _HeAdmire;


    // Start is called before the first frame update
    void Start()
    {
        _BatheContrast = BatheGreekAll.GetComponent<SkeletonGraphic>();
        _BatheContrast.AnimationState.Complete += BoreMottle;

        HemAmidJet.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            if (!SaveDataManager.GetBool(CConfig.sv_FinishFirstBigWin))
            {
                PostEventScript.GetInstance().SendEvent("1013");
                SeeAmidWisdom();
                return;
            }

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
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_2);
            ADManager.Instance.NoThanksAddCount();
            SeeJungleWisdom();
        });
    }


    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        _BowScopeIraq = (WinPanelData)uiFormParams;
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Bigwin);
        LintIraq();
        MyGreekSake();
        ShaleBurnAll.GetComponent<BigWinWheelItem>().LintIraq();
        if (!SaveDataManager.GetBool(CConfig.sv_FinishFirstBigWin))
        {
            PostEventScript.GetInstance().SendEvent("1012");
        }

        Invoke(nameof(BoreAmidJet), 0.2f);
        Invoke(nameof(BoreSeeJet), 2f);
    }

    private void BoreAmidJet()
    {
        ShaleBurnAll.transform.DOScale(0.1f, 0f).OnComplete(() =>
            {
                ShaleBurnAll.gameObject.SetActive(true);
                HemAmidJet.enabled = true;
                ShaleBurnAll.transform.DOScale(1f, 0.2f);
            }
        );
    }

    private void LeakyPlutoSake()
    {
        BatheGreekAll.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "animation", false);
    }


    private async void MyGreekSake()
    {
        BatheGreekAll.SetActive(true);
        LeakyPlutoSake();
    }


    private void BoreSeeJet()
    {
        HemJungleJet.gameObject.SetActive(SaveDataManager.GetBool(CConfig.sv_FinishFirstBigWin));
        HemJungleJet.enabled = true;
    }

    private void LintIraq()
    {
        _RevereFaint = 1;
        _LikeArmory = 0;
        _HeAdmire = "0";
        ShaleBurnAll.gameObject.SetActive(false);
        HemJungleJet.gameObject.SetActive(false);

        HemJungleJet.enabled = false;

        _LikeArmory = decimal.Round(_BowScopeIraq.CashAmount,2);
        RevereGap.sprite = _BowScopeIraq.PanelType == PanelType.Slot
            ? WiseBeside
            : PanelManager.Instance.GetRewardSprite("Cash", _LikeArmory);

        ToilDivaGap.gameObject.SetActive(true);
        ToilDivaIraq.gameObject.SetActive(true);
        ToilDivaIraq.text = "" + _LikeArmory;
    }

    private void QuincyLegacy()
    {
        decimal endNum = _LikeArmory * _RevereFaint;
        AnimationController.ChangeNumber(_LikeArmory, endNum, 0.2f, ToilDivaIraq,
            () => { });
        _LikeArmory = endNum;
    }

    private async void SeeAmidWisdom()
    {
        if (IsInvoking(nameof(BoreSeeJet)))
        {
            CancelInvoke(nameof(BoreSeeJet));
        }

        ShaleBurnAll.GetComponent<BigWinWheelItem>().StopAct(false);
        _RevereFaint = (decimal)ShaleBurnAll.GetComponent<BigWinWheelItem>().thisMulti;
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
        if (!SaveDataManager.GetBool(CConfig.sv_FinishFirstBigWin))
        {
            SaveDataManager.SetBool(CConfig.sv_FinishFirstBigWin, true);
        }

        PostEventScript.GetInstance().SendEvent("1007", _HeAdmire);

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
                // PostEventScript.GetInstance().SendEvent("1007", _HeAdmire);
                TrendScope.Instance.CloseWheel();
                break;
            default:
                FishScope.Instance.AfterCollectBonus(0, _LikeArmory);
                break;
        }

        CloseUIForm(GetType().Name);
    }


    private void BoreMottle(TrackEntry trackEntry)
    {
        BatheGreekAll.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "idel", true);
    }


    private void BoreComerGreekMottle(TrackEntry trackEntry)
    {
        // BatheGreekAll.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "idel", true);
    }
}