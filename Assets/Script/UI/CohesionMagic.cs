using Cysharp.Threading.Tasks;
using DG.Tweening;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CohesionMagic : BaseUIForms
{
    // [Header("按钮")]
    // public Button GutPartBuy;

    [FormerlySerializedAs("getNormalBtn")]


    public Button GutPotterBuy;

    [FormerlySerializedAs("mainContentObj")]


    public GameObject DireBequestSum;

    [FormerlySerializedAs("coinText")]


    public Text GenuBent;

    [FormerlySerializedAs("boardSpineObj")]


    public GameObject FlareWrongSum;

    private WinPanelData _PulMagicDeed;

    private int _GenuBureau;

    private SkeletonGraphic _FlareGenerous;


    // Start is called before the first frame update
    void Start()
    {
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
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Reward);
        PostDeed();
        Invoke(nameof(VoleSowBuy), 0.1f);
    }

    private void VoleSowBuy()
    {
        GutPotterBuy.gameObject.SetActive(true);
        GutPotterBuy.enabled = true;
    }

    private void PostDeed()
    {
        _GenuBureau = 0;
        GutPotterBuy.gameObject.SetActive(false);
        GutPotterBuy.enabled = false;

        FlareWrongSum.gameObject.SetActive(false);
        GenuBent.text = "" + _PulMagicDeed.CoinAmount;
        _GenuBureau = _PulMagicDeed.CoinAmount;
    }


    private void SowPotterSunset()
    {
        GutPotterBuy.gameObject.SetActive(false);
        GutPotterBuy.enabled = false;
        SowSunsetCanRecur();
    }


    private void SowSunsetCanRecur()
    {
        PostEventScript.GetInstance().SendEvent("1008", "0");
        if (_PulMagicDeed.IsCard)
        {
            SkinMagic.Instance.AfterCardBonus(_GenuBureau, 0);
        }
        else
        {
            SkinMagic.Instance.AfterCollectBonus(_GenuBureau);
        }

        CloseUIForm(GetType().Name);
    }

    private void HillySpillYarn()
    {
        FlareWrongSum.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "animation", false);
    }


    private void VoleVerify(TrackEntry trackEntry)
    {
        // FlareWrongSum.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "idel", true);
    }
}