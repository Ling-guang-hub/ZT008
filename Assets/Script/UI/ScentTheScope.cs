using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ScentTheScope : BaseUIForms
{
    // [Header("按钮")]
    [FormerlySerializedAs("getMoreBtn")]

    public Button HemAmidJet;

    [FormerlySerializedAs("getNormalBtn")]


    public Button HemJungleJet;

    [FormerlySerializedAs("wheelItemObj")]


    public GameObject ShaleBurnAll;

    [FormerlySerializedAs("mainContentObj")]


    public GameObject SiltReshapeAll;

    [FormerlySerializedAs("onlyCashText")]


    public Text ToilDivaIraq;

    [FormerlySerializedAs("boardSpineObj")]


    public GameObject LewisGreekAll;

    private WinPanelData _BowScopeIraq;
    
    private decimal _LikeArmory;

    private static decimal _RevereFaint;

    private string _HeAdmire;


    // Start is called before the first frame update
    void Start()
    {

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

        _RevereFaint = 2;
        Invoke(nameof(BoreDespiteWisdom), 2f);
        Invoke(nameof(BoreAmidJet), 2.7f);
        Invoke(nameof(BoreSeeJet), 3f);
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
        // ShaleBurnAll.gameObject.SetActive(true);
    }

    private void BoreDespiteWisdom()
    {
        QuincyLegacy();
    }

    private void LeakyComerSake()
    {
        LewisGreekAll.GetComponent<SkeletonGraphic>().Initialize(true);
        LewisGreekAll.gameObject.SetActive(true);
        LewisGreekAll.GetComponent<SkeletonGraphic>().AnimationState.Complete += BoreComerGreekMottle;

        LewisGreekAll.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "1open", false);
    }

    private async void MyGreekSake()
    {
        await UniTask.Delay(1000);
        LeakyComerSake();
    }


    private void BoreSeeJet()
    {
        HemJungleJet.gameObject.SetActive(SaveDataManager.GetBool(CConfig.sv_FinishFirstBigWin));
        HemJungleJet.enabled = true;
    }

    private void LintIraq()
    {
        // _completeData = LocalRewardData.CompleteData;
        _RevereFaint = 1;
        _LikeArmory = 0;
        _HeAdmire = "0";

        HemJungleJet.gameObject.SetActive(false);

        ShaleBurnAll.gameObject.SetActive(false);

        HemAmidJet.enabled = false;
        HemJungleJet.enabled = false;
        _LikeArmory =decimal.Round( _BowScopeIraq.CashAmount,2) ;
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
        FishScope.Instance.AfterCardBonus(0, _LikeArmory);
        LewisGreekAll.GetComponent<SkeletonGraphic>().AnimationState.SetEmptyAnimation(0, 0);
        LewisGreekAll.gameObject.SetActive(false);
        CloseUIForm(GetType().Name);
    }




    private void BoreComerGreekMottle(TrackEntry trackEntry)
    {
        LewisGreekAll.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "2loop", true);
    }
}