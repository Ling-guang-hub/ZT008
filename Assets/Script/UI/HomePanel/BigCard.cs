// Project  ScratchCard
// FileName  BigCard.cs
// Author  AX
// Desc
// CreateAt  2025-04-14 16:04:36 
//


using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;

public class BigCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
// public class BigCard : MonoBehaviour,  IPointerClickHandler
{
    [FormerlySerializedAs("cardImg")]

    public Image CaneGap;

    [FormerlySerializedAs("baseBigCardAtlas")]


    public SpriteAtlas CoatBidStopMiami;
    private Dictionary<string, Sprite> _OurStopProgramTopi;

    [FormerlySerializedAs("cardType")]


    public CardType cardType;

    [FormerlySerializedAs("cardId")]


    public int cardId;

    [FormerlySerializedAs("costText")]


    public Text BushIraq;

    private Vector3 _offset;

    private float _zCoord;

    [FormerlySerializedAs("clickFlag")]


    public bool clickFlag;

    private float _startX;

    private float _offsetX;

    private float _endPosX;

    private float _beginX;
   

    private void OnDestroy()
    {
        DOTween.Kill(transform);
    }

    private void ObjOnDrag()
    {
        if (!clickFlag) return;

        _endPosX = GetMouseWorldPos().x;

        _offsetX = _endPosX - _startX;

        ParkScope.Instance.BigCardDoMove(_offsetX);

        _startX = _endPosX;
    }


    private Vector3 GetMouseWorldPos()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = _zCoord;
        return Camera.main.ScreenToWorldPoint(screenPos);
    }

    private void DoSelect(bool isLeft)
    {
        clickFlag = false;
        ParkScope.Instance.ClickNextBtn(!isLeft);
    }


    public void LintIraq()
    {
        clickFlag = true;

        _OurStopProgramTopi = new Dictionary<string, Sprite>();
        Sprite[] bigCardSprite = new Sprite[CoatBidStopMiami.spriteCount];
        CoatBidStopMiami.GetSprites(bigCardSprite);
        foreach (Sprite sprite in bigCardSprite)
        {
            string originalName = sprite.name.Replace("(Clone)", "");
            _OurStopProgramTopi[originalName] = sprite;
        }
        CaneGap.sprite = _OurStopProgramTopi[cardType.ToString()];
        BushIraq.text = LocalCardData.CardParamDict[cardId].CardCost + "";

    }


    public void OnDrag(PointerEventData eventData)
    {
        ObjOnDrag();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        clickFlag = false;

        // switch (GetMouseWorldPos().x - _beginX)
        switch (transform.position.x)
        {
            case < -1.2f:
                DoSelect(true);
                return;
            case > 1.2f:
                DoSelect(false);
                return;
            default:
                ParkScope.Instance.BigCardDoHome();
                break;
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        _startX = GetMouseWorldPos().x;
        _beginX = GetMouseWorldPos().x;
    }
}