using Cysharp.Threading.Tasks;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PuffMagic : BaseUIForms
{
    [FormerlySerializedAs("getNormalBtn")]

    public Button GutPotterBuy;

    [FormerlySerializedAs("titleSpineObj")]


    public GameObject CrushWrongSum;
    private SkeletonGraphic CrushGenerous;

    void Start()
    {
        CrushGenerous = CrushWrongSum.GetComponent<SkeletonGraphic>();
        CrushGenerous.AnimationState.Complete += VoleVerify;
        GutPotterBuy.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            RecurMagic();
        });
    }


    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.GameLose);
        GutPotterBuy.enabled = false;
        ItWrongYarn();
        Invoke(nameof(SowBuyLog), 1f);
    }

    private void SowBuyLog()
    {
        GutPotterBuy.enabled = true;
    }

    private void HillyAppleYarn()
    {
        CrushWrongSum.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "animation", false);
    }

    private async void RecurMagic()
    {
        await UniTask.Delay(200);
        CrushWrongSum.SetActive(false);
        SkinMagic.Instance.AfterLosePanel();
        CloseUIForm(GetType().Name);
    }

    private void ItWrongYarn()
    {
        CrushWrongSum.SetActive(true);
        HillyAppleYarn();
    }

    private void VoleVerify(TrackEntry trackEntry)
    {
        CrushWrongSum.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "idle", true);
    }
}