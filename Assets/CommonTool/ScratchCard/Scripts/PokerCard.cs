// Project  ScratchCard
// FileName  SimpleCard.cs
// Author  AX
// Desc
// CreateAt  2025-03-31 18:03:49 
//


using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/**
 *   Find reward symbol
 */
public class PokerCard : BaseCard
{
    public static PokerCard Instance;

    private static readonly int ItemRowLength = 2;

    private static readonly int ItemColLength = 4;

    public GameObject targetObj;


    public GameObject itemGroupArea;


    private List<Vector3> rewardPosList;

    public List<GameObject> userPokers;

    public List<GameObject> dealerPokers;

    private List<List<GameObject>> _itemGroupList;


    void Awake()
    {
        Instance = this;
        InitBaseData();
        cardType = CardType.Poker;
    }

    void Start()
    {
        // SetSpine();
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

    private void SetRewardItem(GameObject rewardObj, BaseRewardItemData reward)
    {
        LocalRewardData.ShowThankPanel = false;

        Vector3 rewardPos = rewardObj.GetComponent<BaseCardItem>().goodsImg.transform.position;

        switch (reward.Type)
        {
            case CommonRewardType.Coin:
                LocalRewardData.CompleteData.HasCoin = true;
                LocalRewardData.CompleteData.CoinAmount += (int)reward.Amount;
                LocalRewardData.ShowRewardPanel = true;
                LocalRewardData.CompleteData.CoinPos.Add(new KeyValuePair<decimal, Vector3>(reward.Amount, rewardPos));
                break;
            case CommonRewardType.Cash:
                LocalRewardData.CompleteData.HasCash = true;
                LocalRewardData.CompleteData.CashAmount += (decimal)reward.Amount;
                LocalRewardData.ShowRewardPanel = true;
                LocalRewardData.CompleteData.CashPos.Add(new KeyValuePair<decimal, Vector3>(reward.Amount, rewardPos));
                break;
            case CommonRewardType.Goods:
                LocalRewardData.CompleteData.CollectsPos.Add(
                    new KeyValuePair<int, Vector3>(reward.GoodsIdx, rewardPos));
                // LocalRewardData.CompleteData.CollectsPos.Add(rewardPos);
                CollectManager.Instance.needFly = true;
                CollectManager.Instance.curType = reward.CollectType;
                break;
        }
    }


    private void SetPoker(int idx, bool isThanks)
    {
        int firstValue = Random.Range(0, 13);
        int secondValue = Random.Range(0, 13);
        while (firstValue == secondValue)
        {
            secondValue = Random.Range(0, 13);
        }

        int userValue = !isThanks ? Math.Max(firstValue, secondValue) : Math.Min(firstValue, secondValue);
        int dealerValue = isThanks ? Math.Max(firstValue, secondValue) : Math.Min(firstValue, secondValue);
        userPokers[idx].GetComponent<PokerItem>().InitPoker(userValue);
        dealerPokers[idx].GetComponent<PokerItem>().InitPoker(dealerValue);
    }


    private void SetItemGroupImg()
    {
        
        List<BaseRewardItemData> rewardDataList = GetRewardList(userPokers.Count);

        for (int i = 0; i < userPokers.Count; i++)
        {
            BaseRewardItemData reward = rewardDataList[i];
            // if (i == randIdx && IsSpecialCard)
            // {
            //     reward = GetEffectiveReward();
            // }

            // DealSpecial(reward);
            rewardItemList[i].GetComponent<BaseCardItem>().ShowItem(reward);
            SetPoker(i, reward.IsThanks);

            if (!reward.IsThanks)
            {
                SetRewardItem(rewardItemList[i], reward);
                BaseAnimItemList.Add(userPokers[i]);
                BaseAnimItemList.Add(dealerPokers[i]);
            }
        }
    }

    private List<BaseRewardItemData> GetRewardList(int lenght)
    {
        int randIdx = Random.Range(0, lenght);
        bool hasGoods = false;
        List<BaseRewardItemData> rewardList = new List<BaseRewardItemData>();
        for (int i = 0; i < lenght; i++)
        {
            BaseRewardItemData rewardData = GetReward();
            if (i == randIdx && IsSpecialCard)
            {
                rewardData = GetEffectiveReward();
            }
            if (!rewardData.IsThanks && rewardData.Type == CommonRewardType.Goods)
            {
                if (hasGoods)
                {
                    while (rewardData.Type == CommonRewardType.Goods)
                    {
                        rewardData = GetEffectiveReward(); 
                    }
                }
                else
                {
                    hasGoods = true;
                }
            }
            rewardList.Add(rewardData);
        }

        return rewardList;
    }
    
    public override float DoWinAnim()
    {
        if (BaseAnimItemList.Count < 1) return 0f;

        BaseAnimItemList.Sort((a, b) =>
            a.GetComponent<CommonItem>().baseIdx.CompareTo(b.GetComponent<CommonItem>().baseIdx));
        float delayTime = 0f;
        float durTime = LocalCommonData.ItemDoFadeDuringTime;
        for (int i = 0; i < BaseAnimItemList.Count; i++)
        {
            if (i % 2 == 0)
            {
                delayTime += LocalCommonData.ItemDoFadeDelayTime;
            }

            GameObject item = BaseAnimItemList[i];
            item.GetComponent<CommonItem>().ShowTopImg(durTime, delayTime);
        }

        return delayTime + durTime;
    }


    private void InitCardItems()
    {
        // CreateItem();

        LocalRewardData.ResetCompleteData();

        DealSpecialCard();

        SetItemGroupImg();
    }
}