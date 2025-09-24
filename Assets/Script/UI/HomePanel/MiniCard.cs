// Project  ScratchCard
// FileName  MiniCard.cs
// Author  AX
// Desc
// CreateAt  2025-04-03 18:04:22 
//


using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// public class MiniCard : MonoBehaviour, IPointerClickHandler
public class MiniCard : MonoBehaviour
{
    [FormerlySerializedAs("cardType")]

    public CardType cardType;

    [FormerlySerializedAs("cardId")]


    public int cardId;

    [FormerlySerializedAs("cardSpineObj")]


    public GameObject cardSpineObj;

    [FormerlySerializedAs("cardButton")]


    public Button cardButton;

    [FormerlySerializedAs("unlockLine")]


    public int unlockLine;

    // public GameObject lockImg;

    private SkeletonGraphic _cardSkeleton;

    private bool _isSelected;

    private bool _isLocked;

    private void Start()
    {
        cardButton.onClick.AddListener(() => { BeClick(); });
    }

    private void BeSelectedAct()
    {
        _cardSkeleton.AnimationState.SetEmptyAnimation(0, 0);
        _cardSkeleton.AnimationState.SetAnimation(0, "on", false);
        _cardSkeleton.AnimationState.SetAnimation(0, "idle_on", true);
    }

    private void BeDefaultAct()
    {
        _cardSkeleton.AnimationState.SetEmptyAnimation(0, 0);
        _cardSkeleton.AnimationState.SetAnimation(0, "off", false);
        _cardSkeleton.AnimationState.SetAnimation(0, "idle_off", true);
    }

    public void BeUnlockAct()
    {
        _cardSkeleton.AnimationState.SetEmptyAnimation(0, 0);
        _cardSkeleton.AnimationState.SetAnimation(0, "Lock", true);
    }

    public void PostDeed()
    {
        _isSelected = LocalCommonData.CurrentCardId == cardId;
        _cardSkeleton = cardSpineObj.GetComponent<SkeletonGraphic>();
        _isLocked = CheckUnlock();
        InitAnim();
    }

    private string GetActName()
    {
        if (CheckUnlock())
        {
            return "Lock";
        }

        return _isSelected ? "idle_on" : "idle_off";
    }

    private void InitAnim()
    {
        _cardSkeleton.Initialize(true);
        _cardSkeleton.Skeleton.SetSkin(cardType.ToString());
        _cardSkeleton.Skeleton.SetSlotsToSetupPose();
        _cardSkeleton.Skeleton.SetToSetupPose();
        _cardSkeleton.AnimationState.SetEmptyAnimation(0, 0);
        _cardSkeleton.AnimationState.SetAnimation(0, GetActName(), true);
    }


    private void BeClick()
    {
        if (_isSelected) return;
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
        BeSelect();
        TameMagic.Instance.ResetBigCard();
    }

    public void BeSelect()
    {
        if (_isSelected) return;
        _isSelected = true;
        if (!CheckUnlock())
        {
            BeSelectedAct();
        }
        else
        {
            transform.DOScale(1.1f, 0.1f);
        }

        LocalCommonData.CurrentCardId = cardId;
        TameMagic.Instance.SelectOneCard();
    }


    public void DoUnlock()
    {
        
        if (!_isLocked) return;

        if (!CheckUnlock())
        {
            _isLocked = false;
            BeDefaultAct();
        }
    }

    public void BeNotSelected()
    {
        
        if (!_isSelected) return;
        _isSelected = false;
        transform.localScale = Vector3.one;
        if (CheckUnlock())
        {
            BeUnlockAct();
        }
        else
        {
            BeDefaultAct();
        }
    }

    private bool CheckUnlock()
    {
        return CardManager.Instance.GetFinishCardNum() < unlockLine;
    }
}