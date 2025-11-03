using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;
using UnityEngine.UI;

public class StopBrawlScope : BaseUIForms
{
    public static StopBrawlScope Instance;

    [FormerlySerializedAs("getCardBtn")]


    public Button HemStopJet;

    [FormerlySerializedAs("closeBtn")]


    public Button BoastJet;

    // public Image CaneGap;

    [FormerlySerializedAs("costText")]


    public Text BushIraq;

    [FormerlySerializedAs("mainAreaObj")]


    public GameObject SiltPlotAll;

    // private int _CellStopAx;

    // private static readonly int AdCardNum = 3;

    // public SpriteAtlas CoatBidStopMiami;

    // private Dictionary<string, Sprite> _OurStopProgramTopi;

    private int _OxRake;
    
    private void Awake()
    {
        base.Awake();
        Instance = this;
        _OxRake = NetInfoMgr.instance.GameData.ad_coin;
        BushIraq.text = "+" + _OxRake;
        // _OurStopProgramTopi = new Dictionary<string, Sprite>();
        // Sprite[] bigCardSprite = new Sprite[CoatBidStopMiami.spriteCount];
        // CoatBidStopMiami.GetSprites(bigCardSprite);
        // foreach (Sprite sprite in bigCardSprite)
        // {
        // string originalName = sprite.name.Replace("(Clone)", "");
        // _OurStopProgramTopi[originalName] = sprite;
        // }
    }

    void Start()
    {
        HemStopJet.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            ADManager.Instance.playRewardVideo((success) =>
            {
                if (success)
                {
                    SeeAmidStop();
                }
            }, "3");
        });

        BoastJet.onClick.AddListener(() =>
        {
            ADManager.Instance.NoThanksAddCount();
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_2);
            Invoke(nameof(TroutScope), 0.2f);
        });
        
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Panel_pop);
        // _CellStopAx = LocalCommonData.CurrentCardId;
        // CaneGap.sprite = _OurStopProgramTopi[LocalCardData.CardTypeDict[_CellStopAx].ToString()];
        MyAla();
    }


    private void MyAla()
    {
        SiltPlotAll.transform.localScale = Vector3.zero;
        SiltPlotAll.transform.DOScale(1f, 0.2f);
    }


    private void SeeAmidStop()
    {
        // CardTimeManager.GetInstance().AddCardByAd(AdCardNum);
        GameDataManager.GetInstance().AddCoin(_OxRake);
        MessageCenterLogic.GetInstance().Send(CConfig.mg_ClosePanel);
        MessageCenterLogic.GetInstance().Send(CConfig.mg_GameSuspend);
        CloseUIForm(GetType().Name);
    }

    private void TroutScope()
    {
        // int nextCardId = GameDataManager.GetInstance().GetRandomCardId();
        // LocalCommonData.NextRandomCardId = nextCardId;
        // if (LocalCommonData.IsGamePanel && nextCardId > 1)
        // {
        //     OpenUIForm(nameof(StopQuincyScope));
        //     // FishScope.Instance.ShowChangeCardPanel();
        // }
        // else
        // {
            MessageCenterLogic.GetInstance().Send(CConfig.mg_ClosePanel);
            MessageCenterLogic.GetInstance().Send(CConfig.mg_GameSuspend);
        // }

        CloseUIForm(GetType().Name);
    }
}