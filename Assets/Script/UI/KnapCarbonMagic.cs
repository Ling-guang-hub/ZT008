// Project  BlockDropRush
// FileName  KnapCarbonMagic.cs
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

public class KnapCarbonMagic: BaseUIForms
{
    
    public static KnapCarbonMagic Instance;

    [FormerlySerializedAs("changeCardBtn")]


    public Button PermitKnapBuy;

    [FormerlySerializedAs("closeBtn")]


    public Button AlikeBuy;

    [FormerlySerializedAs("cardImg")]


    public Image SoarBog;

    [FormerlySerializedAs("mainAreaObj")]


    public GameObject DireFlapSum;
    
    private int _CageKnapSo;

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
        PermitKnapBuy.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            CarbonKnap();
        });

        AlikeBuy.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_2);
            Invoke(nameof(RecurMagic), 0.2f);
        });
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Panel_pop);
        _CageKnapSo = LocalCommonData.NextRandomCardId;
        
        SoarBog.sprite = _SetKnapTorontoPray[LocalCardData.CardTypeDict[_CageKnapSo].ToString()];
        ItLog();
    }


    private void ItLog()
    {
        DireFlapSum.transform.localScale = Vector3.zero;
        DireFlapSum.transform.DOScale(1f, 0.2f);
    }

    private void CarbonKnap()
    {
        MessageCenterLogic.GetInstance().Send(CConfig.mg_GetCardByAd);
        Invoke(nameof(RecurMagic), 0.2f);
        SkinMagic.Instance.AfterChangeCardPanel(_CageKnapSo);
    }

    private void RecurMagic()
    {
        MessageCenterLogic.GetInstance().Send(CConfig.mg_ClosePanel);
        MessageCenterLogic.GetInstance().Send(CConfig.mg_GameSuspend);
        CloseUIForm(GetType().Name);
    }
    
    
}
