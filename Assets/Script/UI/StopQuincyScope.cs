// Project  BlockDropRush
// FileName  StopQuincyScope.cs
// Author  AX
// Desc
// CreateAt  2025-07-01 17:07:15 
//


using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;
using UnityEngine.UI;

public class StopQuincyScope: BaseUIForms
{
    
    public static StopQuincyScope Instance;

    [FormerlySerializedAs("changeCardBtn")]


    public Button MentalStopJet;

    [FormerlySerializedAs("closeBtn")]


    public Button BoastJet;

    [FormerlySerializedAs("cardImg")]


    public Image CaneGap;

    [FormerlySerializedAs("mainAreaObj")]


    public GameObject SiltPlotAll;
    
    private int _CellStopAx;

    [FormerlySerializedAs("baseBigCardAtlas")]


    public SpriteAtlas CoatBidStopMiami;

    private Dictionary<string, Sprite> _OurStopProgramTopi;
    
    
    private void Awake()
    {
        base.Awake();
        Instance = this;
        _OurStopProgramTopi = new Dictionary<string, Sprite>();
        Sprite[] bigCardSprite = new Sprite[CoatBidStopMiami.spriteCount];
        CoatBidStopMiami.GetSprites(bigCardSprite);
        foreach (Sprite sprite in bigCardSprite)
        {
            string originalName = sprite.name.Replace("(Clone)", "");
            _OurStopProgramTopi[originalName] = sprite;
        }
    }
    
    
    void Start()
    {
        MentalStopJet.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            QuincyStop();
        });

        BoastJet.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_2);
            Invoke(nameof(TroutScope), 0.2f);
        });
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Panel_pop);
        _CellStopAx = LocalCommonData.NextRandomCardId;
        
        CaneGap.sprite = _OurStopProgramTopi[LocalCardData.CardTypeDict[_CellStopAx].ToString()];
        MyAla();
    }


    private void MyAla()
    {
        SiltPlotAll.transform.localScale = Vector3.zero;
        SiltPlotAll.transform.DOScale(1f, 0.2f);
    }

    private void QuincyStop()
    {
        MessageCenterLogic.GetInstance().Send(CConfig.mg_ShowSuperCardRate);
        Invoke(nameof(TroutScope), 0.2f);
        FishScope.Instance.AfterChangeCardPanel(_CellStopAx);
    }

    private void TroutScope()
    {
        MessageCenterLogic.GetInstance().Send(CConfig.mg_ClosePanel);
        MessageCenterLogic.GetInstance().Send(CConfig.mg_GameSuspend);
        CloseUIForm(GetType().Name);
    }
    
    
}
