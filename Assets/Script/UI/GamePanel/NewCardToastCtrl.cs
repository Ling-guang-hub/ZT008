// Project  BlockDropRush
// FileName  NewCardToastCtrl.cs
// Author  AX
// Desc
// CreateAt  2025-10-29 16:10:41 
//


using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;
using UnityEngine.UI;

public class NewCardToastCtrl : MonoBehaviour
{
    [FormerlySerializedAs("baseNewCardAtlas")]

    public SpriteAtlas baseNewCardAtlas;

    private Dictionary<string, Sprite> _newCardSpritesDict;

    [FormerlySerializedAs("iconImg")]


    public GameObject iconImg;

    [FormerlySerializedAs("changeBtn")]


    public Button changeBtn;

    private CardType _cardType;

    private int _lastLevel;

    private int _curCardId;
    
    private void Awake()
    {
        _cardType = CardType.RowSame;
        _curCardId = 0;
        _newCardSpritesDict = new Dictionary<string, Sprite>();
        Sprite[] newCardSprite = new Sprite[baseNewCardAtlas.spriteCount];
        baseNewCardAtlas.GetSprites(newCardSprite);
        foreach (Sprite sprite in newCardSprite)
        {
            string originalName = sprite.name.Replace("(Clone)", "");
            _newCardSpritesDict[originalName] = sprite;
        }
    }


    private void Start()
    {
        _lastLevel = CardManager.Instance.GetCurLevel().Key;
        
        MessageCenterLogic.GetInstance().Register(CConfig.mg_ShowNewCardToast, (md) => { MyAla(); });
        
        changeBtn.onClick.AddListener(() =>
        {
            DoChangeCard();
        });
        
        
    }


    private void DoChangeCard()
    {
        LocalCommonData.CurrentCardId = _curCardId;
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_2);
        FishScope.Instance.SetMainBtn(true);
        ParkScope.Instance.ShowHomeArea();
        iconImg.transform.DOLocalMoveX(0, 0.1f).SetDelay(0.1f);
        UIManager.GetInstance().CloseOrReturnUIForms(nameof(FishScope));
    }


    private bool CheckIsFinish()
    {
        
        int newLevel = CardManager.Instance.GetCurLevel().Key;
        if (newLevel == _lastLevel) return false;

        _lastLevel = newLevel;
        foreach (int cardId in LocalCardData.ActCardIds)
        {
            if (LocalCardData.CardParamDict[cardId].UnlockLine == _lastLevel)
            {
                _curCardId = cardId;
                iconImg.GetComponent<Image>().sprite =
                    _newCardSpritesDict[LocalCardData.CardTypeDict[cardId].ToString()];
                return true;
            }
        }

        return false;
    }


    private void MyAla()
    {
        if (CheckIsFinish())
        {
            DoMove();
        }
    }


    private void DoMove()
    {
        iconImg.transform.DOLocalMoveX(350f, 0.5f).OnComplete(() =>
        {
            iconImg.transform.DOLocalMoveX(0, 0.5f).SetDelay(5f);
        });
    }
}