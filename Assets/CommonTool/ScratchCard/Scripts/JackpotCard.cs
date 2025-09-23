// Project  ScratchCard
// FileName  SimpleCard.cs
// Author  AX
// Desc
// CreateAt  2025-03-31 18:03:49 
//


using System.Collections.Generic;
using UnityEngine;

/**
 *  Find 3 identical numbers
 */
public class JackpotCard : BaseCard
{
    public static JackpotCard Instance;

    public GameObject itemGroupArea;

    private static readonly int ItemRowLength = 4;
    private static readonly int ItemColLength = 5;

    private static readonly int RewardLimit = 3;

    private static readonly int MaxRewardCount = 3;

    private List<string> _usedNameList;
    private List<string> _cannotUsedList;
    private List<string> _rewardNameList;
    private List<BaseCardData> _baseDataList;

    void Awake()
    {
        Instance = this;
        InitBaseData();
        cardType = CardType.Jackpot;
        _usedNameList = new List<string>();
        _cannotUsedList = new List<string>();
        _baseDataList = new List<BaseCardData>();
    }


    // first  init  card data
    public override void InitCardData()
    {
        base.InitCardData();
        InitCardItems();
    }


    // TODO
    private void InitReward()
    {
    }


    //  
    private void MergeAllReward(BaseCardData cardData, GameObject itemObj)
    {
        BaseRewardItemData rewardData = cardData.RewardItemData;

        if (rewardData.IsThanks)
        {
            itemObj.GetComponent<BaseCardItem>().ShowItem(cardData.ItemSprite, cardData.TopSprite, rewardData);
        }
        else
        {
            LocalRewardData.ShowThankPanel = false;

            itemObj.GetComponent<BaseCardItem>().ShowItem(cardData.ItemSprite, cardData.TopSprite, rewardData);

            Vector3 rewardPos = itemObj.GetComponent<BaseCardItem>().goodsImg.transform.position;

            switch (rewardData.Type)
            {
                case CommonRewardType.Coin:
                    LocalRewardData.CompleteData.HasCoin = true;
                    LocalRewardData.CompleteData.CoinAmount += (int)rewardData.Amount;
                    LocalRewardData.ShowRewardPanel = true;
                    LocalRewardData.CompleteData.CoinPos.Add(
                        new KeyValuePair<int, Vector3>(rewardData.Amount, rewardPos));
                    break;
                case CommonRewardType.Cash:
                    LocalRewardData.CompleteData.HasCash = true;
                    LocalRewardData.CompleteData.CashAmount += (decimal)rewardData.Amount;
                    LocalRewardData.ShowRewardPanel = true;
                    LocalRewardData.CompleteData.CashPos.Add(
                        new KeyValuePair<int, Vector3>(rewardData.Amount, rewardPos));
                    break;
                case CommonRewardType.Goods:
                    // LocalRewardData.CompleteData.CollectsPos.Add(rewardPos);
                    LocalRewardData.CompleteData.CollectsPos.Add(
                        new KeyValuePair<int, Vector3>(rewardData.GoodsIdx, rewardPos));
                    break;
            }
        }
    }


    private string GetActiveItemName()
    {
        string spriteName = GetRandomSpriteName();
        while (_cannotUsedList.Contains(spriteName))
        {
            spriteName = GetRandomSpriteName();
        }

        return spriteName;
    }


    private void MakeItemBaseData()
    {
        BaseRewardDataList = new List<BaseRewardItemData>();
        _usedNameList = new List<string>();
        _cannotUsedList = new List<string>();
        _baseDataList = new List<BaseCardData>();
        _rewardNameList = new List<string>();

        int randIdx = Random.Range(0, MaxRewardCount);

        for (int i = 0; i < MaxRewardCount; i++)
        {
            BaseRewardItemData reward = GetReward();
            if (i == randIdx && IsSpecialCard)
            {
                reward = GetEffectiveReward();
            }

            if (reward.IsThanks) continue;
            string spriteName = GetActiveItemName();
            _rewardNameList.Add(spriteName);

            if (reward.Type != CommonRewardType.Goods)
            {
                reward.Amount /= 3;
            }

            for (int j = 0; j < RewardLimit; j++)
            {
                BaseCardData baseData = new BaseCardData()
                {
                    SpriteName = spriteName,
                    ItemSprite = ItemOffSpriteDict[spriteName],
                    TopSprite = ItemOnSpriteDict[spriteName],
                    RewardItemData = reward,
                };
                _baseDataList.Add(baseData);
            }

            _cannotUsedList.Add(spriteName);
            _usedNameList.Add(spriteName);
        }

        int temp = 0;
        while (_baseDataList.Count < ItemColLength * ItemRowLength && temp < 100)
        {
            temp++;
            string spriteName = GetActiveItemName();
            BaseRewardItemData reward = GetReward();
            reward.IsThanks = true;
            if (reward.Type != CommonRewardType.Goods)
            {
                reward.Amount /= 3;
            }

            BaseCardData baseData = new BaseCardData()
            {
                SpriteName = spriteName,
                ItemSprite = ItemOffSpriteDict[spriteName],
                TopSprite = ItemOnSpriteDict[spriteName],
                RewardItemData = reward,
            };
            _baseDataList.Add(baseData);
            if (_usedNameList.Contains(spriteName))
            {
                _cannotUsedList.Add(spriteName);
            }
            else
            {
                _usedNameList.Add(spriteName);
            }
        }
    }

    private void SetItemGroupImg()
    {
        List<BaseCardData> newDataList = CardUtil.Shuffle(_baseDataList);
        for (int i = 0; i < newDataList.Count; i++)
        {
            BaseCardData baseData = newDataList[i];
            GameObject itemObj = mainItemList[i];
            MergeAllReward(baseData, itemObj);
            if (_rewardNameList.Contains(baseData.SpriteName))
            {
                BaseAnimItemList.Add(itemObj);
            }
        }
    }

  

    private void InitCardItems()
    {
        // if (itemGroupArea.transform.childCount == 0)
        // {
        //     CreateItem();
        // }

        LocalRewardData.ResetCompleteData();

        DealSpecialCard();
        MakeItemBaseData();
        SetItemGroupImg();
    }
}