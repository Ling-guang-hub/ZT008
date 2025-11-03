using Cysharp.Threading.Tasks;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UtahScope : BaseUIForms
{
    [FormerlySerializedAs("getNormalBtn")]

    public Button HemJungleJet;

    [FormerlySerializedAs("titleSpineObj")]


    public GameObject BatheGreekAll;
    private SkeletonGraphic BatheContrast;

    void Start()
    {
        BatheContrast = BatheGreekAll.GetComponent<SkeletonGraphic>();
        BatheContrast.AnimationState.Complete += BoreMottle;
        HemJungleJet.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            TroutScope();
        });
    }


    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.GameLose);
        HemJungleJet.enabled = false;
        MyGreekSake();
        Invoke(nameof(HatJetAla), 1f);
    }

    private void HatJetAla()
    {
        HemJungleJet.enabled = true;
    }

    private void LeakyPlutoSake()
    {
        BatheGreekAll.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "animation", false);
    }

    private async void TroutScope()
    {
        await UniTask.Delay(200);
        BatheGreekAll.SetActive(false);
        FishScope.Instance.AfterLosePanel();
        CloseUIForm(GetType().Name);
    }

    private void MyGreekSake()
    {
        BatheGreekAll.SetActive(true);
        LeakyPlutoSake();
    }

    private void BoreMottle(TrackEntry trackEntry)
    {
        BatheGreekAll.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "idle", true);
    }
}