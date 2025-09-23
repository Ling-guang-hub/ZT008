// Project  ScratchCard
// FileName  TraceMagic.cs
// Author  AX
// Desc
// CreateAt  2025-04-15 18:04:37 
//


using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;
using UnityEngine.UI;
using RotateMode = DG.Tweening.RotateMode;

public class TraceMagic : BaseUIForms
{
    public static TraceMagic Instance;

    [FormerlySerializedAs("mainAreaObj")]


    public GameObject DireFlapSum;

    [FormerlySerializedAs("bigWheelObj")]


    public GameObject bigWheelObj;

    [FormerlySerializedAs("baseMiniCardAtlas")]


    public SpriteAtlas baseMiniCardAtlas;

    [FormerlySerializedAs("bigWheelItemList")]


    public List<GameObject> bigWheelItemList;

    [FormerlySerializedAs("playBtn")]


    public Button playBtn;
    
    private List<GameObject> _smallWheelItemList;

    private List<WheelBigItemReward> _bigItemRewardList;

    private Dictionary<string, Sprite> _miniCardAtlas;

    [FormerlySerializedAs("bigLightObj")]


    public GameObject bigLightObj;

    private SkeletonGraphic _bigLightSkeleton;

    private int _bigWinLimit;

    private int _thisRewardIdx;

   
    // public WheelRewardData BigRewardInfo;
    // private SmallWheelWeight SmallRewardInfo;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        _bigItemRewardList = new List<WheelBigItemReward>();
        _thisRewardIdx = 0;
        _bigWinLimit = NetInfoMgr.instance.GameData.bigwin_limit; 
        InitSprite();
    }

    void Start()
    {
        _bigLightSkeleton = bigLightObj.GetComponent<SkeletonGraphic>();
        // _bigLightSkeleton.AnimationState.Complete += VoleVerify;
        
        playBtn.onClick.AddListener(() =>
        {
            PlayWheel();
        });
    }


    public void CloseWheel()
    {
        Invoke(nameof(DoClose), 0.3f);
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        ShowAct();
        PostEventScript.GetInstance().SendEvent("1003");

        WheelBarManager.GetInstance().AddWheelStep();
        
        if (!SaveDataManager.GetBool(CConfig.sv_FirstWheel))
        {
            PostEventScript.GetInstance().SendEvent("1017");
            SaveDataManager.SetBool(CConfig.sv_FirstWheel, true);

        }

        TaskManager.GetInstance().TakeTask(TaskType.Lucky);
        
        // SOHOShopManager.instance.AddTaskValue("Wheel", 1);
        
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Panel_pop);
        // bigLightObj.gameObject.SetActive(false);
        InitDataAndUI();
        InitBigItemData();
        playBtn.gameObject.SetActive(true);
    }


    public void PlayWheel()
    {
        SpinDoRota();
        playBtn.gameObject.SetActive(false);   
    }



    private void ShowAct()
    {
        DireFlapSum.transform.localScale = Vector3.zero;
        DireFlapSum.transform.DOScale(1f, 0.2f).OnComplete(() =>
        {
     
        });
    }

    private void InitSprite()
    {
        _miniCardAtlas = new Dictionary<string, Sprite>();
        Sprite[] miniCardSprite = new Sprite[baseMiniCardAtlas.spriteCount];
        baseMiniCardAtlas.GetSprites(miniCardSprite);

        foreach (Sprite sprite in miniCardSprite)
        {
            string originalName = sprite.name.Replace("(Clone)", "");
            _miniCardAtlas[originalName] = sprite;
        }
    }

    private void DoClose()
    {
        SkinMagic.Instance.AfterWheelBonus();
        CloseUIForm(GetType().Name);
    }


    private Sprite GetSpriteByCardId(int cardId)
    {
        CardType cardType = LocalCardData.CardTypeDict[cardId];
        return _miniCardAtlas[cardType.ToString()];
    }

    private void SetReward()
    {
        WheelBigItemReward thisReward = _bigItemRewardList[_thisRewardIdx];
        LocalWheelData.WheelType = thisReward.Type;
        LocalWheelData.WheelAmount = thisReward.Count;
        LocalWheelData.WheelCardId = thisReward.CardId;
        if (thisReward.Type == CommonRewardType.Card)
        {
            LocalWheelData.WheelCardSprite = GetSpriteByCardId(thisReward.CardId);
        }
    }

    private void InitDataAndUI()
    {
        _bigItemRewardList = GameUtil.GetWheelBigItemRewards();
        _thisRewardIdx = GameUtil.GetWheelRewardIdx();
        SetReward();

        bigWheelObj.transform.localRotation = new Quaternion();
    }


    private void InitBigItemData()
    {
        for (int i = 0; i < _bigItemRewardList.Count; i++)
        {
            WheelBigItemReward thisReward = _bigItemRewardList[i];
            if (thisReward.Type == CommonRewardType.Card)
            {
                bigWheelItemList[i].GetComponent<BigWheelItem>().cardIcon.sprite = GetSpriteByCardId(thisReward.CardId);
            }

            bigWheelItemList[i].GetComponent<BigWheelItem>().InitIcon(thisReward);
        }
    }


    private void ShowWinPanel()
    {
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Rotate_win);
        bigLightObj.GetComponent<SkeletonGraphic>().Initialize(true);
        _bigLightSkeleton.AnimationState.Complete += VoleVerify;
        bigLightObj.GetComponent<SkeletonGraphic>().AnimationState.ClearTrack(0);
        bigLightObj.SetActive(true);
        bigLightObj.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "animation", false);
    }


    private void ShowBonusPanel()
    {
        
        // if ( LocalWheelData.WheelAmount >= _bigWinLimit)
        // {
        //     OpenUIForm(nameof(TraceWebFlyMagic));
        // }
        // else
        // {
        //     OpenUIForm(nameof(TraceFlashMagic));
        // }
    }

    private void SpinDoRota()
    {

        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Rotate);
        bigWheelObj.transform.DOLocalRotate(new Vector3(0, 0, -360 * 3+180f - 30f * _thisRewardIdx), 3f,
                RotateMode.FastBeyond360).SetDelay(0.3f)
            .SetEase(Ease.OutQuart).OnComplete(() =>
            {
                // ShowBonusPanel();
                ShowWinPanel();
            });
    }
    
    
    private void VoleVerify(TrackEntry trackEntry)
    {
        OpenUIForm(LocalWheelData.WheelAmount >= _bigWinLimit ? nameof(TraceWebFlyMagic) : nameof(TraceFlashMagic));
        bigLightObj.gameObject.SetActive(false);
    }
    
}