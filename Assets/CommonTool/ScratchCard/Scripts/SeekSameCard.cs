// Project  ScratchCard
// FileName  SimpleCard.cs
// Author  AX
// Desc
// CreateAt  2025-03-31 18:03:49 
//


using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using DG.Tweening;
using Spine;
using Spine.Unity;
using Random = UnityEngine.Random;
using Sequence = DG.Tweening.Sequence;

/**
 *   Find reward symbol
 */
public class SeekSameCard : BaseCard
{
    public GameObject targetObj;

    private static readonly int ItemRowLength = 5;

    public GameObject mainArea;

    private List<Vector3> rewardPosList;


    void Awake()
    {
        InitBaseData();
        cardType = CardType.SeekSame;
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


    private void SetItemImg(string targetSpriteName, int idx, BaseRewardItemData reward)
    {
        GameObject rewardObj = mainItemList[idx];

        if (reward.IsThanks)
        {
            string otherName = GetAntherSpriteName(targetSpriteName);

            rewardObj.GetComponent<BaseCardItem>()
                .ShowItem(ItemOffSpriteDict[otherName], ItemOnSpriteDict[otherName], reward);
        }
        else
        {
            // DealSpecial(reward);

            LocalRewardData.ShowThankPanel = false;

            rewardObj.GetComponent<BaseCardItem>().ShowItem(ItemOffSpriteDict[targetSpriteName],
                ItemOnSpriteDict[targetSpriteName], reward);

            Vector3 rewardPos = rewardObj.GetComponent<BaseCardItem>().goodsImg.transform.position;

            switch (reward.Type)
            {
                case CommonRewardType.Coin:
                    LocalRewardData.CompleteData.HasCoin = true;
                    LocalRewardData.CompleteData.CoinAmount += (int)reward.Amount;
                    LocalRewardData.ShowRewardPanel = true;
                    LocalRewardData.CompleteData.CoinPos.Add(new KeyValuePair<int, Vector3>(reward.Amount, rewardPos));
                    break;
                case CommonRewardType.Cash:
                    LocalRewardData.CompleteData.HasCash = true;
                    LocalRewardData.CompleteData.CashAmount += (decimal)reward.Amount;
                    LocalRewardData.ShowRewardPanel = true;
                    LocalRewardData.CompleteData.CashPos.Add(new KeyValuePair<int, Vector3>(reward.Amount, rewardPos));
                    break;
                case CommonRewardType.Goods:
                    // LocalRewardData.CompleteData.CollectsPos.Add(rewardPos);
                    LocalRewardData.CompleteData.CollectsPos.Add(
                        new KeyValuePair<int, Vector3>(reward.GoodsIdx, rewardPos));
                    break;
            }
        }
    }


    private void SetItemGroupImg()
    {
        // set  target obj
        string targetSpriteName = GetRandomSpriteName();
        targetObj.GetComponent<BaseCardItem>()
            .ShowItem(ItemOnSpriteDict[targetSpriteName]);

        int randIdx = Random.Range(0, mainItemList.Count);

        for (int i = 0; i < mainItemList.Count; i++)
        {
            BaseRewardItemData reward = GetReward();
            if (i == randIdx && IsSpecialCard)
            {
                reward = GetEffectiveReward();
            }

            if (!reward.IsThanks)
            {
                BaseAnimItemList.Add(mainItemList[i]);
            }

            SetItemImg(targetSpriteName, i, reward);
        }
    }


    private void InitCardItems()
    {
        // if (mainArea.transform.childCount == 0)
        // {
        //     CreateItem();
        // }

        LocalRewardData.ResetCompleteData();

        DealSpecialCard();

        SetItemGroupImg();
    }
}