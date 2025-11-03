// Project  BlockDropRush
// FileName  TaleScope.cs
// Author  AX
// Desc
// CreateAt  2025-10-22 14:10:14 
//


using System;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TaleScope : BaseUIForms
{
    public static TaleScope Instance;

    [FormerlySerializedAs("slotGroup01")]


    public GameObject slotGroup01;

    [FormerlySerializedAs("slotGroup02")]


    public GameObject slotGroup02;

    [FormerlySerializedAs("slotGroup03")]


    public GameObject slotGroup03;

    private SlotRewardData _thisReward;

    [FormerlySerializedAs("baseSlotObj")]


    public GameObject baseSlotObj;

    [FormerlySerializedAs("baseSlotObjSpine")]


    public GameObject baseSlotObjSpine;

    [FormerlySerializedAs("coin01")]


    public Sprite coin01;
    [FormerlySerializedAs("coin02")]

    public Sprite coin02;
    [FormerlySerializedAs("coin03")]

    public Sprite coin03;
    [FormerlySerializedAs("cash01")]

    public Sprite cash01;
    [FormerlySerializedAs("cash02")]

    public Sprite cash02;
    [FormerlySerializedAs("cash03")]

    public Sprite cash03;
    [FormerlySerializedAs("big777")]

    public Sprite big777;


    [FormerlySerializedAs("playBtn")]



    public Button playBtn;

    [FormerlySerializedAs("slotTopSpine")]


    public GameObject slotTopSpine;

    private Dictionary<SlotType, Sprite> _spriteDict;

    private List<GameObject> _rewardList;

    private int _maxIndex;
    private int _rewardIdx;

    private Vector3 startPos;

    private readonly float _objStep = 160f;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        _spriteDict = new Dictionary<SlotType, Sprite>
        {
            { SlotType.Cash01, cash01 },
            { SlotType.Cash02, cash02 },
            { SlotType.Cash03, cash03 },
            { SlotType.Coin01, coin01 },
            { SlotType.Coin02, coin02 },
            { SlotType.Coin03, coin03 },
            { SlotType.Big777, big777 }
        };

        _maxIndex = 18;
        _rewardIdx = 16;
        _rewardList = new List<GameObject>();
        startPos = slotGroup01.transform.localPosition;
    }

    private void Start()
    {
        playBtn.onClick.AddListener(() =>
        {
            playBtn.gameObject.SetActive(false);
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            DoSpine();
            Invoke(nameof(MoveAct), 0.3f);
        });
    }

    private void DoSpine()
    {
        slotTopSpine.GetComponent<SkeletonGraphic>().Initialize(true);
        slotTopSpine.GetComponent<SkeletonGraphic>().AnimationState.SetEmptyAnimation(0, 0);
        slotTopSpine.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "animation", false);
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        ShowAct();
        LintIraq();
    }


    private void LintIraq()
    {
        playBtn.gameObject.SetActive(true);
        _thisReward = GameUtil.GetCollectReward();
        InitSlotObj(slotGroup01);
        InitSlotObj(slotGroup02);
        InitSlotObj(slotGroup03);
    }


    private Sprite GetThisSprite()
    {
        return _spriteDict[CardUtil.GetRandomItem<SlotType>()];
    }


    private void InitSlotObj(GameObject slotGroup)
    {
        Vector3 thisPos = slotGroup.transform.localPosition;
        slotGroup.transform.localPosition = new Vector3(thisPos.x, startPos.y, thisPos.z);
        if (slotGroup.transform.childCount > 0)
        {
            for (int i = 0; i < slotGroup.transform.childCount; i++)
            {
                GameObject objItem = slotGroup.transform.GetChild(i).gameObject;
                if (i != _rewardIdx)
                {
                    objItem.GetComponent<Image>().sprite = GetThisSprite();
                }
                else
                {
                    objItem.GetComponent<SlotSpineCtrl>().Init(_thisReward.Type);
                }
            }
        }
        else
        {
            for (int i = 0; i < _maxIndex; i++)
            {
                GameObject objItem;
                if (i != _rewardIdx)
                {
                    objItem = Instantiate(baseSlotObj, slotGroup.transform);
                    objItem.GetComponent<Image>().sprite = GetThisSprite();
                }
                else
                {
                    objItem = Instantiate(baseSlotObjSpine, slotGroup.transform);
                    objItem.GetComponent<SlotSpineCtrl>().Init(_thisReward.Type);
                    _rewardList.Add(objItem);
                }

                Vector3 pos = new Vector3
                {
                    y = i * _objStep - _objStep
                };
                objItem.transform.localPosition = pos;
                objItem.gameObject.SetActive(true);
            }
        }
    }


    private void MoveAct()
    {
        // musicFlag = true;
        // StartCoroutine(nameof(PlaySlotMusic));
        slotGroup01.transform.DOLocalMoveY(-_objStep * _rewardIdx + _objStep, 2f).OnComplete(() =>
        {
            // MusicMgr.GetInstance().PlayEffect(MusicType.SceneMusic.sound_slotwheel_stop);
        });
        slotGroup02.transform.DOLocalMoveY(-_objStep * _rewardIdx + _objStep, 2f).SetDelay(0.3f).OnComplete(() =>
        {
            // MusicMgr.GetInstance().PlayEffect(MusicType.SceneMusic.sound_slotwheel_stop);
        });
        slotGroup03.transform.DOLocalMoveY(-_objStep * _rewardIdx + _objStep, 2f).SetDelay(0.6f).OnComplete(() =>
        {
            Invoke(nameof(DoRewardListAct), 0.4f);
        });
    }

    public void ShowAct()
    {
    }

    public void DoRewardListAct()
    {
        foreach (GameObject objItem in _rewardList)
        {
            objItem.GetComponent<SlotSpineCtrl>().MyAla();
        }
        
        Invoke(nameof(GetSlotReward), 0.6f);
    }
    
    private void GetSlotReward()
    {
        if (_thisReward.Type.ToString().Contains("Coin"))
        {
            DoClosePanel();
        }
        else
        {
            WinPanelData thisData = new WinPanelData()
            {
                CashAmount =  _thisReward.Count,
                PanelType = PanelType.Slot,
                RewardSprite = _spriteDict[_thisReward.Type]
            };
            UIManager.GetInstance().ShowUIForms(_thisReward.Type == SlotType.Big777
                ? nameof(BidTheScope)
                : nameof(GalileanScope), thisData);
        }
    }

    public void DoClosePanel()
    {
        Invoke(nameof(DoClose), 0.3f);
    }

    private void DoClose()
    {
        if (_thisReward.Type.ToString().Contains("Coin"))
        {
            int coinAmount = (int) Math.Ceiling( _thisReward.Count);
            FishScope.Instance.AfterCollectBonus(coinAmount, 0);
        }
        else
        {
            FishScope.Instance.AfterCollectBonus(0, _thisReward.Count);
        }

        CloseUIForm(GetType().Name);
    }


 
}