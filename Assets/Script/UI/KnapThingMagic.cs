using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;
using UnityEngine.UI;

public class KnapThingMagic : BaseUIForms
{
    public static KnapThingMagic Instance;

    [FormerlySerializedAs("getCardBtn")]


    public Button GutKnapBuy;

    [FormerlySerializedAs("closeBtn")]


    public Button AlikeBuy;

    [FormerlySerializedAs("cardImg")]


    public Image SoarBog;

    [FormerlySerializedAs("mainAreaObj")]


    public GameObject DireFlapSum;

    private int _CageKnapSo;

    private static readonly int DyKnapRoe = 3;

    [FormerlySerializedAs("baseBigCardAtlas")]


    public SpriteAtlas KierWebKnapChimp;

    private Dictionary<string, Sprite> _SetKnapTorontoPray;

    private void Awake()
    {
        base.Awake();
        Instance = this;
        _SetKnapTorontoPray = new Dictionary<string, Sprite>();
        Sprite[] bigCardSprite = new Sprite[KierWebKnapChimp.spriteCount];
        KierWebKnapChimp.GetSprites(bigCardSprite);
        foreach (Sprite sprite in bigCardSprite)
        {
            string originalName = sprite.name.Replace("(Clone)", "");
            _SetKnapTorontoPray[originalName] = sprite;
        }
    }

    void Start()
    {
        GutKnapBuy.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            ADManager.Instance.playRewardVideo((success) =>
            {
                if (success)
                {
                    SowPartKnap();
                }
            }, "3");
        });

        AlikeBuy.onClick.AddListener(() =>
        {
            ADManager.Instance.NoThanksAddCount();
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_2);
            Invoke(nameof(RecurMagic), 0.2f);
        });
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Panel_pop);
        _CageKnapSo = LocalCommonData.CurrentCardId;
        SoarBog.sprite = _SetKnapTorontoPray[LocalCardData.CardTypeDict[_CageKnapSo].ToString()];
        ItLog();
    }


    private void ItLog()
    {
        DireFlapSum.transform.localScale = Vector3.zero;
        DireFlapSum.transform.DOScale(1f, 0.2f);
    }


    private void SowPartKnap()
    {
        GameDataManager.GetInstance().AddCard(DyKnapRoe);
        MessageCenterLogic.GetInstance().Send(CConfig.mg_GetCardByAd);
        // Invoke("ClosePanel", 0.2f);
        MessageCenterLogic.GetInstance().Send(CConfig.mg_ClosePanel);
        MessageCenterLogic.GetInstance().Send(CConfig.mg_GameSuspend);
        CloseUIForm(GetType().Name);
    }

    private void RecurMagic()
    {
        int nextCardId = GameDataManager.GetInstance().GetRandomCardId();
        LocalCommonData.NextRandomCardId = nextCardId;
        if (LocalCommonData.IsGamePanel && nextCardId > 1)
        {
            OpenUIForm(nameof(KnapCarbonMagic));
            // SkinMagic.Instance.ShowChangeCardPanel();
        }
        else
        {
            MessageCenterLogic.GetInstance().Send(CConfig.mg_ClosePanel);
            MessageCenterLogic.GetInstance().Send(CConfig.mg_GameSuspend);
        }

        CloseUIForm(GetType().Name);
    }
}