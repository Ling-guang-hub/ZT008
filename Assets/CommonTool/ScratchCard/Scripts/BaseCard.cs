// Project  ScratchCard
// FileName  BaseCard.cs
// Author  AX
// Desc
// CreateAt  2025-03-31 18:03:07 
//


using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ScratchCardAsset;
using ScratchCardAsset.Animation;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

public class BaseCard : MonoBehaviour
{
    public GameObject top;

    public ScratchCardManager mainCard;

    public ScratchAnimator cardAnimator;

    public float cardLessProgress;

    private bool _mainCardFlag;

    public int cardId;

    public CardType cardType;

    // card item on sprint 
    public SpriteAtlas baseItemOnAtlas;

    // card item off sprint 
    public SpriteAtlas baseItemOffAtlas;
    protected List<string> ItemSpriteOnName;
    protected List<string> ItemSpriteOffName;
    protected Dictionary<string, Sprite> ItemOnSpriteDict;
    protected Dictionary<string, Sprite> ItemOffSpriteDict;

    public SpriteAtlas baseRewardAtlas;
    protected Dictionary<string, Sprite> RewardSpriteDict;

    protected GameObject BaseCardItemObj;

    // anim list for card item
    protected List<GameObject> BaseAnimItemList;

    public List<GameObject> mainItemList;
    public List<GameObject> rewardItemList;

    public GameObject checkSpineObj;

    public GameObject superSpineObj;

    public GameObject boardSpineObj;

    private SkeletonGraphic _checkSpineSkeleton;

    private SkeletonGraphic _superSpineSkeleton;


    private SkeletonGraphic _boardSpineSkeleton;

    protected List<int> ActiveGoodIdx;
    protected List<int> PrepareGoodIdx;

    protected int PreCollectCount;


    protected List<BaseRewardItemData> BaseRewardDataList;

    protected bool IsSpecialCard;


    protected List<Sprite> RewardSpriteList;


    private static readonly string BaseCardItemPath = "ScratchCard/Prefabs/ScBaseCardItem";

    private static readonly int GoodsLimit = 10;


    private void Awake()
    {
        InitBaseData();
    }

    private void Update()
    {
        if (!_mainCardFlag && mainCard.Progress.GetProgress() > cardLessProgress)
        {
            FinishCard();
        }
    }


    // for ui  music
    public bool GetCardIsFinish()
    {
        return _mainCardFlag || mainCard.Progress.GetProgress() >= cardLessProgress;
    }


    public void FinishCard()
    {
        LocalCommonData.IsGamePass = true;

        SkinMagic.Instance.HoldTimerEnd();

        _mainCardFlag = true;
        mainCard.FillScratchCard();

        GameDataManager.GetInstance().AddFinishedCard();

        WheelBarManager.GetInstance().AddWheelValue();

        ShowComplete();
    }


    private void InitSprite()
    {
        ItemSpriteOnName = new List<string>();
        ItemSpriteOffName = new List<string>();
        ItemOnSpriteDict = new Dictionary<string, Sprite>();
        ItemOffSpriteDict = new Dictionary<string, Sprite>();
        // ActiveGoodIdx = new List<int>();
        // PrepareGoodIdx = new List<int>();
        PreCollectCount = 0;


        // on sprite
        Sprite[] itemOnSprites = new Sprite[baseItemOnAtlas.spriteCount];
        baseItemOnAtlas.GetSprites(itemOnSprites);

        foreach (Sprite sprite in itemOnSprites)
        {
            string originalName = sprite.name.Replace("(Clone)", "");
            ItemOnSpriteDict[originalName] = sprite;
            ItemSpriteOnName.Add(originalName);
        }


        //  off sprite
        Sprite[] itemOffSprites = new Sprite[baseItemOffAtlas.spriteCount];
        baseItemOffAtlas.GetSprites(itemOffSprites);

        foreach (Sprite sprite in itemOffSprites)
        {
            string originalName = sprite.name.Replace("(Clone)", "");
            ItemOffSpriteDict[originalName] = sprite;
            ItemSpriteOffName.Add(originalName);
        }


        // reward sprite
        RewardSpriteList = new List<Sprite>();
        RewardSpriteDict = new Dictionary<string, Sprite>();
        // 获取所有 Sprite
        Sprite[] rewardSprites = new Sprite[baseRewardAtlas.spriteCount];
        baseRewardAtlas.GetSprites(rewardSprites);

        foreach (Sprite sprite in rewardSprites)
        {
            string originalName = sprite.name.Replace("(Clone)", "");
            RewardSpriteDict[originalName] = sprite;
            RewardSpriteList.Add(sprite);
        }
    }


    // protected List<int> GetActiveGoodIdx()
    // {
    //     List<int> actList = new List<int>();
    //     List<int> usedIdx = GameDataManager.GetInstance().GetGoods();
    //
    //     for (int i = 0; i < GoodsLimit; i++)
    //     {
    //         if (!usedIdx.Contains(i))
    //         {
    //             actList.Add(i);
    //         }
    //     }
    //
    //     return actList;
    // }

    // private int GetTargetGoodIdx()
    // {
    //     if (ActiveGoodIdx.Count <= 0) return -1;
    //     int num = Random.Range(0, ActiveGoodIdx.Count);
    //     int idx = ActiveGoodIdx[num];
    //     PrepareGoodIdx.Add(idx);
    //     ActiveGoodIdx.Remove(idx);
    //     return idx;
    // }


    // private BaseRewardItemData GetOnceRewardData()
    // {
    //     LocalCardWeight rewardData = GameUtil.GetLocalRewardWeight();
    //
    //     while (rewardData.Type == CardRewardType.Goods && ActiveGoodIdx.Count == 0)
    //     {
    //         rewardData = GameUtil.GetLocalRewardWeight();
    //     }
    //
    //     return RewardToItemData(rewardData);
    // }


    //  set sprite to  reward
    private BaseRewardItemData RewardSetSprite(BaseRewardItemData rewardData)
    {
        // if (rewardData.Type == CommonRewardType.Goods)
        // {
        // rewardData.RewardSprite = RewardSpriteDict[];
        // }
        // else
        // {
        rewardData.RewardSprite = RewardSpriteDict[rewardData.Type.ToString()];
        // }

        return rewardData;
    }


    // get new user first reward
    // protected BaseRewardItemData GetNewUserReward()
    // {
    //     BaseRewardItemData rewardData = new BaseRewardItemData
    //     {
    //         IsThanks = false,
    //         Type = CommonRewardType.Cash,
    //         RewardSprite = RewardSpriteDict["Cash"],
    //         Amount = (int)Math.Ceiling(GameUtil.GetNewUserCashRewardNum())
    //     };
    //     return rewardData;
    // }

    protected BaseRewardItemData GetNewUserReward()
    {
        BaseRewardItemData rewardData = new BaseRewardItemData
        {
            IsThanks = false,
            Type = CommonRewardType.Coin,
            RewardSprite = RewardSpriteDict["Coin"],
            Amount = 10000,
        };
        return rewardData;
    }


    //  get one  effective  reward
    protected BaseRewardItemData GetEffectiveReward()
    {
        BaseRewardItemData rewardData = CardManager.Instance.GetSureReward();
        rewardData = SetCollectIdx(rewardData);
        return RewardSetSprite(rewardData);
    }

    protected BaseRewardItemData GetReward()
    {
        BaseRewardItemData rewardData = CardManager.Instance.GetOnceRewardData();
        rewardData = SetCollectIdx(rewardData);
        return RewardSetSprite(rewardData);
    }


    protected BaseRewardItemData GetThankReward()
    {
        return RewardSetSprite(CardManager.Instance.GetThanksRandomItem());
    }

    protected BaseRewardItemData RewardToBaseItem(LocalCardWeight reward)
    {
        return RewardSetSprite(CardManager.Instance.RewardToItemData(reward));
    }


    private BaseRewardItemData SetCollectIdx(BaseRewardItemData rewardData)
    {
        if (!rewardData.IsThanks && rewardData.Type == CommonRewardType.Goods)
        {
            if (PreCollectCount < CollectManager.Instance.GetActiveCollectCount())
            {
                rewardData.GoodsIdx = PreCollectCount;
                PreCollectCount++;
            }
            else
            {
                rewardData = CardManager.Instance.GetSureRewardWithOutGoods();
            }
        }

        return rewardData;
    }


    // protected BaseRewardItemData GetGoodsByIdx(int idx)
    // {
    //     BaseRewardItemData itemData = new BaseRewardItemData
    //     {
    //         IsThanks = false,
    //         RewardMulti = 0,
    //         Type = CommonRewardType.Goods,
    //         GoodsIdx = idx,
    //         Amount = 1
    //     };
    //
    //     return RewardSetSprite(itemData);
    // }


    // private BaseRewardItemData GetThanksRandomItem()
    // {
    //     BaseRewardItemData itemData = new BaseRewardItemData
    //     {
    //         IsThanks = true,
    //         RewardMulti = 0,
    //         Type = CommonRewardType.Coin,
    //     };
    //     int num = Random.Range(0, 3);
    //
    //     if (num == 1 && !CommonUtil.IsApple())
    //     {
    //         itemData.Type = CommonRewardType.Cash;
    //     }
    //
    //     List<int> tempGoodsIdx = GetActiveGoodIdx();
    //
    //     if (num == 2 && tempGoodsIdx.Count > 0)
    //     {
    //         itemData.Type = CommonRewardType.Goods;
    //     }
    //
    //     switch (itemData.Type)
    //     {
    //         case CommonRewardType.Coin:
    //             itemData.Amount = Random.Range(100, 10000);
    //             itemData.GoodsIdx = -1;
    //             break;
    //         case CommonRewardType.Cash:
    //             itemData.Amount = Random.Range(10, 10000);
    //             itemData.GoodsIdx = -1;
    //             break;
    //         default:
    //             int thisIdx = Random.Range(0, GoodsLimit);
    //             // int idx = tempGoodsIdx[thisIdx];
    //             itemData.Type = CommonRewardType.Goods;
    //             itemData.GoodsIdx = thisIdx;
    //             itemData.Amount = 1;
    //             break;
    //     }
    //
    //     return itemData;
    // }

    // private BaseRewardItemData GetThanksRandomItem()
    // {
    //     BaseRewardItemData itemData = new BaseRewardItemData
    //     {
    //         IsThanks = true,
    //         RewardMulti = 0,
    //         Type = CommonRewardType.Coin,
    //     };
    //
    //     switch (itemData.Type)
    //     {
    //         case CommonRewardType.Coin:
    //             itemData.Amount = Random.Range(100, 10000);
    //             itemData.GoodsIdx = -1;
    //             break;
    //         case CommonRewardType.Cash:
    //             itemData.Amount = Random.Range(10, 10000);
    //             itemData.GoodsIdx = -1;
    //             break;
    //         default:
    //             int thisIdx = Random.Range(0, GoodsLimit);
    //             itemData.Type = CommonRewardType.Goods;
    //             itemData.GoodsIdx = thisIdx;
    //             itemData.Amount = 1;
    //             break;
    //     }
    //
    //     return itemData;
    // }

    // private BaseRewardItemData RewardToItemData(LocalCardWeight reward)
    // {
    //     BaseRewardItemData itemData = new BaseRewardItemData
    //     {
    //         RewardMulti = reward.RewardMulti,
    //         GoalCount = reward.GoalCount,
    //     };
    //     if (reward.Type == CardRewardType.Coin)
    //     {
    //         itemData.Type = CommonRewardType.Coin;
    //         itemData.Amount = (int)reward.RewardNum;
    //         itemData.IsThanks = false;
    //         itemData.GoodsIdx = -1;
    //     }
    //     else if (reward.Type == CardRewardType.Cash)
    //     {
    //         itemData.Type = CommonRewardType.Cash;
    //         itemData.Amount = (int)reward.RewardNum;
    //         itemData.IsThanks = false;
    //         itemData.GoodsIdx = -1;
    //     }
    //     else if (reward.Type == CardRewardType.Goods)
    //     {
    //         int goodsIdx = GetTargetGoodIdx();
    //         if (goodsIdx < 0)
    //         {
    //             itemData = CardManager.Instance.GetThanksRandomItem();
    //         }
    //         else
    //         {
    //             itemData.Type = CommonRewardType.Goods;
    //             itemData.Amount = 1;
    //             itemData.IsThanks = false;
    //             itemData.GoodsIdx = goodsIdx;
    //         }
    //     }
    //     else
    //     {
    //         itemData = CardManager.Instance.GetThanksRandomItem();
    //     }
    //
    //     return itemData;
    // }


    // init data before start
    protected void InitBaseData()
    {
        BaseCardItemObj = Resources.Load<GameObject>(BaseCardItemPath);
        BaseAnimItemList = new List<GameObject>();
        BaseRewardDataList = new List<BaseRewardItemData>();
        InitSprite();
        checkSpineObj.gameObject.SetActive(false);
        _checkSpineSkeleton = checkSpineObj.GetComponent<SkeletonGraphic>();
        _boardSpineSkeleton = boardSpineObj.GetComponent<SkeletonGraphic>();
        _superSpineSkeleton = superSpineObj.GetComponent<SkeletonGraphic>();
    }


    private void PlayScrape()
    {
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Scrape);
    }


    public void ShowFinishSpine()
    {
        float dailyTime = 0;
        for (int i = 0; i < 3; i++)
        {
            Invoke(nameof(PlayScrape), dailyTime);
            dailyTime += 0.4f;
        }

        top.gameObject.SetActive(false);
        // cardAnimator.Play();

        _checkSpineSkeleton.Initialize(true);
        checkSpineObj.gameObject.SetActive(true);
        _checkSpineSkeleton.AnimationState.SetEmptyAnimation(0, 0);
        _checkSpineSkeleton.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "animation", false);
    }


    public virtual void InitCardData()
    {
        BaseAnimItemList = new List<GameObject>();
        // PrepareGoodIdx = new List<int>();
        PreCollectCount = 0;
        // ActiveGoodIdx = GetActiveGoodIdx();
    }


    public virtual float DoWinAnim()
    {
        if (BaseAnimItemList.Count < 1) return 0f;
        BaseAnimItemList.Sort((a, b) =>
            a.GetComponent<CommonItem>().baseIdx.CompareTo(b.GetComponent<CommonItem>().baseIdx));
        float delayTime = 0f;
        float durTime = LocalCommonData.ItemDoFadeDuringTime;
        foreach (var t in BaseAnimItemList)
        {
            delayTime += LocalCommonData.ItemDoFadeDelayTime;
            t.GetComponent<CommonItem>().ShowTopImg(durTime, delayTime);
        }

        return delayTime + durTime;
    }

    public virtual void DoLoopAnim()
    {
        if (BaseAnimItemList.Count < 1) return;
        foreach (var t in BaseAnimItemList)
        {
            t.GetComponent<CommonItem>().ShowLoopAct();
        }
    }


    public void ShowComplete()
    {
        SkinMagic.Instance.DoFinishAnimAndShowCompletePanel();
    }


    protected string GetRandomSpriteName()
    {
        int idx = Random.Range(0, ItemSpriteOffName.Count);
        return ItemSpriteOffName[idx];
    }


    protected string GetAntherSpriteName(string targetName)
    {
        string randomName = GetRandomSpriteName();

        while (randomName.Equals(targetName))
        {
            randomName = GetRandomSpriteName();
        }

        return randomName;
    }


    private void InitCardSpecial()
    {
        IsSpecialCard = CardManager.Instance.CheckIsSuperCard();

    }

    protected async void DoBoardAct()
    {
        _boardSpineSkeleton.Initialize(true);
        boardSpineObj.gameObject.SetActive(true);
        _boardSpineSkeleton.AnimationState.SetEmptyAnimation(0, 0);
        _boardSpineSkeleton.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "animation", true);

        await UniTask.Delay(400);

        superSpineObj.gameObject.SetActive(true);
        _superSpineSkeleton.Initialize(true);
        _superSpineSkeleton.AnimationState.Complete += ShowSuperFinish;
        _superSpineSkeleton.AnimationState.SetEmptyAnimation(0, 0);
        _superSpineSkeleton.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "animation", false);
    }


    protected void DealSpecialCard()
    {
        InitCardSpecial();
        IsSpecialCard = GetSpecialType();
        LocalRewardData.CompleteData.IsSpecial = IsSpecialCard;
        if (IsSpecialCard)
        {
            DoBoardAct();
        }
    }

    private void ShowSuperFinish(TrackEntry trackEntry)
    {
        _superSpineSkeleton.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "idle", true);
        _superSpineSkeleton.AnimationState.Complete -= ShowSuperFinish;
    }


    public void CloseSpineObj()
    {
        superSpineObj.gameObject.SetActive(false);
        boardSpineObj.gameObject.SetActive(false);
    }

    public bool GetSpecialType()
    {
        // return Random.Range(0, 100) > 50;
        return IsSpecialCard;
        // return true;
    }

    public void OpenSuperSpine()
    {
        if (!IsSpecialCard) return;
        if (superSpineObj.activeInHierarchy) return;
        superSpineObj.gameObject.SetActive(true);
    }

    public void CloseSuperSpine()
    {
        if (!IsSpecialCard) return;
        superSpineObj.gameObject.SetActive(false);
    }
}