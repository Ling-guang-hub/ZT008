using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlainFlyMagic : BaseUIForms
{
    // [Header("按钮")]
    [FormerlySerializedAs("getMoreBtn")]

    public Button GutPartBuy;

    [FormerlySerializedAs("getNormalBtn")]


    public Button GutPotterBuy;

    [FormerlySerializedAs("wheelItemObj")]


    public GameObject FlintBombSum;

    [FormerlySerializedAs("mainContentObj")]


    public GameObject DireBequestSum;

    [FormerlySerializedAs("onlyCashText")]


    public Text LifeHeroBent;

    [FormerlySerializedAs("titleSpineObj")]


    public GameObject CrushWrongSum;

    [FormerlySerializedAs("boardSpineObj")]


    public GameObject FlareWrongSum;

    // private CompleteData _completeData;

    private WinPanelData _PulMagicDeed;

    private int _GenuBureau;

    private static decimal _CorpseRelic;

    private SkeletonGraphic _CrushGenerous;

    private SkeletonGraphic _FlareGenerous;


    private string _AxFlyway;


    // Start is called before the first frame update
    void Start()
    {
        // _CrushGenerous = CrushWrongSum.GetComponent<SkeletonGraphic>();
        // _CrushGenerous.AnimationState.Complete += VoleVerify;


        _FlareGenerous = FlareWrongSum.GetComponent<SkeletonGraphic>();



        GutPartBuy.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            if (!SaveDataManager.GetBool(CConfig.sv_FinishFirstBigWin))
            {
                PostEventScript.GetInstance().SendEvent("1013");
                SowPartSunset();
                return;
            }

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
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_2);
            ADManager.Instance.NoThanksAddCount();
            SowPotterSunset();
        });
    }


    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        _PulMagicDeed = (WinPanelData)uiFormParams;
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Bigwin);
        PostDeed();
        ItWrongYarn();
        FlintBombSum.GetComponent<BigWinWheelItem>().PostDeed();

        if (!SaveDataManager.GetBool(CConfig.sv_FinishFirstBigWin))
        {
            PostEventScript.GetInstance().SendEvent("1012");
        }

        _CorpseRelic = 2;
        Invoke(nameof(VoleBeneathSunset), 2f);
        Invoke(nameof(VolePartBuy), 2.7f);
        Invoke(nameof(VoleSowBuy), 3f);
    }


    private void VolePartBuy()
    {
        FlintBombSum.transform.DOScale(0.1f, 0f).OnComplete(() =>
            {
                FlintBombSum.gameObject.SetActive(true);
                GutPartBuy.enabled = true;
                FlintBombSum.transform.DOScale(1f, 0.2f);
            }
        );
        // FlintBombSum.gameObject.SetActive(true);
    }

    private void VoleBeneathSunset()
    {
        CarbonAstute();
    }

    private void HillyAppleYarn()
    {
        CrushWrongSum.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "animation", false);
    }

    private void HillySpillYarn()
    {
        FlareWrongSum.GetComponent<SkeletonGraphic>().Initialize(true);
        FlareWrongSum.gameObject.SetActive(true);
        FlareWrongSum.GetComponent<SkeletonGraphic>().AnimationState.Complete += VoleSpillWrongVerify;

        FlareWrongSum.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "1open", false);
    }

    private async void ItWrongYarn()
    {
        await UniTask.Delay(1000);
        HillySpillYarn();
    }


    private void VoleSowBuy()
    {
        GutPotterBuy.gameObject.SetActive(SaveDataManager.GetBool(CConfig.sv_FinishFirstBigWin));
        GutPotterBuy.enabled = true;
    }

    private void PostDeed()
    {
        // _completeData = LocalRewardData.CompleteData;
        _CorpseRelic = 1;
        _GenuBureau = 0;
        _AxFlyway = "0";

        GutPotterBuy.gameObject.SetActive(false);

        FlintBombSum.gameObject.SetActive(false);

        GutPartBuy.enabled = false;
        GutPotterBuy.enabled = false;
        _GenuBureau = _PulMagicDeed.CoinAmount;
        LifeHeroBent.text = "" + _GenuBureau;
    }

    private void CarbonAstute()
    {
        AnimationController.ChangeNumber(_GenuBureau, _GenuBureau * _CorpseRelic, 0.2f, LifeHeroBent,
            () => { });
        _GenuBureau = (int)Math.Ceiling(_GenuBureau * _CorpseRelic);
    }

    private async void SowPartSunset()
    {
        if (IsInvoking(nameof(VoleSowBuy)))
        {
            CancelInvoke(nameof(VoleSowBuy));
        }

        FlintBombSum.GetComponent<BigWinWheelItem>().StopAct(false);
        _CorpseRelic = (decimal)FlintBombSum.GetComponent<BigWinWheelItem>().thisMulti;
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
        if (!SaveDataManager.GetBool(CConfig.sv_FinishFirstBigWin))
        {
            SaveDataManager.SetBool(CConfig.sv_FinishFirstBigWin, true);
        }

        PostEventScript.GetInstance().SendEvent("1007", _AxFlyway);
        SkinMagic.Instance.AfterCardBonus(_GenuBureau, 0);
        FlareWrongSum.GetComponent<SkeletonGraphic>().AnimationState.SetEmptyAnimation(0, 0);
        FlareWrongSum.gameObject.SetActive(false);
        CloseUIForm(GetType().Name);
    }


    private void VoleVerify(TrackEntry trackEntry)
    {
        CrushWrongSum.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "idel", true);
    }


    private void VoleSpillWrongVerify(TrackEntry trackEntry)
    {
        FlareWrongSum.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "2loop", true);
    }
}