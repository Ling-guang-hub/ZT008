// Project  BlockDropRush
// FileName  CardManager.cs
// Author  AX
// Desc
// CreateAt  2025-09-11 16:09:14 
//


using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    private static readonly string SpecialCardStr = "m_SpecialCardStr";

    private static readonly string SpecialCardRateStr = "m_SpecialCardRateStr";

    private static readonly List<int> SpecialCardRate = new List<int>() { 10, 15, 20, 30, 40, 50, 60, 75, 90, 100 };

    private bool _isSuperCard;

    private List<CollectType> _collectTypes;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _isSuperCard = false;

        _collectTypes = new List<CollectType>()
        {
            CollectType.Flame,
            CollectType.Lightning,
            CollectType.KeyRing
        };
    }


    private CollectType GetRandomType()
    {
        return _collectTypes[Random.Range(0, 3)];
    }


    public int GetFinishCardNum()
    {
        return SaveDataManager.GetInt(CConfig.sv_FinishCard);
    }

    public KeyValuePair<int, float> GetCurLevel()
    {
        int levelStep = LocalCommonData.LevelStep;
        int cardNum = GetFinishCardNum();
        int level = 0;
        int nextStep = 5;
        while (nextStep <= cardNum)
        {
            level++;
            nextStep += ((level + 1) * levelStep);
        }

        float rate = 1f - 1f * (nextStep - cardNum) / ((level + 1) * levelStep);
        return new KeyValuePair<int, float>(level, rate);
    }


    public bool CheckIsSuperCard()
    {
        // return true;
        if (!PlayerPrefs.HasKey(SpecialCardStr))
        {
            _isSuperCard = false;
            SetSpecialCard(1);
        }
        else
        {
            _isSuperCard = GetSuperCardRate() >= Random.Range(0, 100);
            AddSpecialCardNum(_isSuperCard);
        }

        MessageCenterLogic.GetInstance().Send(CConfig.mg_ShowSuperCardRate);
        return _isSuperCard;
    }


    public int GetNextCardRate()
    {
        int cardNum = 0;
        if (!_isSuperCard)
        {
            cardNum = GetSpecialCardNum();
        }

        return SpecialCardRate[cardNum];
    }


    public bool TakeCard()
    {
        int cost = LocalCardData.CardParamDict[LocalCommonData.CurrentCardId].CardCost;
        if (cost > GameDataManager.GetInstance().GetCoin())
        {
            return false;
        }

        GameDataManager.GetInstance().SubCoin(cost);
        return true;
    }


    private void AddSpecialCardNum(bool isClear = false)
    {
        int num = isClear ? 1 : GetSpecialCardNum() + 1;
        SetSpecialCard(num);
    }


    private int GetSuperCardRate()
    {
        return SpecialCardRate[GetSpecialCardNum() - 1];
    }

    private void SetSpecialCard(int cardNum)
    {
        SaveDataManager.SetInt(SpecialCardStr, cardNum);
    }

    private int GetSpecialCardNum()
    {
        return SaveDataManager.GetInt(SpecialCardStr);
    }


    public BaseRewardItemData RewardToItemData(LocalCardWeight reward)
    {
        BaseRewardItemData itemData = new BaseRewardItemData
        {
            RewardMulti = reward.RewardMulti,
            GoalCount = reward.GoalCount,
        };
        if (reward.Type == CardRewardType.Coin)
        {
            itemData.Type = CommonRewardType.Coin;
            itemData.Amount = (int)reward.RewardNum;
            itemData.IsThanks = false;
            itemData.GoodsIdx = -1;
        }
        else if (reward.Type == CardRewardType.Cash)
        {
            itemData.Type = CommonRewardType.Cash;
            itemData.Amount = Convert.ToDecimal(reward.RewardNum);
            itemData.IsThanks = false;
            itemData.GoodsIdx = -1;
        }
        else if (reward.Type == CardRewardType.Goods)
        {
            itemData.Type = CommonRewardType.Goods;
            itemData.Amount = 1;
            itemData.IsThanks = false;
            itemData.GoodsIdx = -1;
            itemData.CollectType = reward.CollectType;
        }
        else
        {
            itemData = GetThanksRandomItem();
        }

        return itemData;
    }

    public BaseRewardItemData GetThanksRandomItem()
    {
        BaseRewardItemData itemData = new BaseRewardItemData
        {
            IsThanks = true,
            RewardMulti = 0,
            Type = CommonRewardType.Coin,
        };

        switch (itemData.Type)
        {
            case CommonRewardType.Coin:
                itemData.Amount = Random.Range(LocalCommonData.RandomCoinStart, LocalCommonData.RandomCoinEnd);
                itemData.GoodsIdx = -1;
                break;
            case CommonRewardType.Cash:
                itemData.Amount = Random.Range(LocalCommonData.RandomCashStart, LocalCommonData.RandomCashEnd);
                itemData.GoodsIdx = -1;
                break;
            default:
                itemData.Type = CommonRewardType.Goods;
                itemData.GoodsIdx = -1;
                itemData.Amount = 1;
                itemData.CollectType = GetRandomType();
                break;
        }

        return itemData;
    }


    public BaseRewardItemData GetOnceRewardData()
    {
        LocalCardWeight rewardData = GameUtil.GetLocalRewardWeight();
        return RewardToItemData(rewardData);
    }

    public BaseRewardItemData GetSureReward()
    {
        LocalCardWeight rewardData = GameUtil.GetLocalRewardWeight();
        while (rewardData.Type == CardRewardType.Thanks)
        {
            rewardData = GameUtil.GetLocalRewardWeight();
        }

        return RewardToItemData(rewardData);
    }

    public BaseRewardItemData GetSureRewardWithOutGoods()
    {
        LocalCardWeight rewardData = GameUtil.GetLocalRewardWeight();
        while (rewardData.Type == CardRewardType.Thanks || rewardData.Type == CardRewardType.Goods)
        {
            rewardData = GameUtil.GetLocalRewardWeight();
        }

        return RewardToItemData(rewardData);
    }
}