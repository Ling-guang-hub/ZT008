// Project  ScratchCard
// FileName  SimpleCard.cs
// Author  AX
// Desc
// CreateAt  2025-03-31 18:03:49 
//


using System.Collections.Generic;
using UnityEngine;

/**
 *   find 3  identical  in a row
 */
public class RowSameCard : BaseCard
{
    public static RowSameCard Instance;

    private List<List<GameObject>> _itemGroupList;

    private static readonly int ItemRowLength = 3;
    private static readonly int ItemColLength = 4;

    public GameObject rewardArea;

    public GameObject itemGroupArea;

    void Awake()
    {
        Instance = this;
        InitBaseData();
        cardType = CardType.RowSame;
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

    private void CreateItem()
    {
        _itemGroupList = new List<List<GameObject>>();
        int idx = 0;
        for (int i = 0; i < ItemColLength; i++)
        {
            List<GameObject> rowList = new List<GameObject>();
            float yPos = 285 - 190 * i;

            for (int j = 0; j < ItemRowLength; j++)
            {
                GameObject item = Instantiate(BaseCardItemObj, itemGroupArea.transform, false);
                item.transform.localScale = itemGroupArea.transform.localScale;
                item.transform.localPosition = new Vector3(190 * j - 190, yPos, 1);
                item.GetComponent<BaseCardItem>().baseIdx = idx;
                item.gameObject.SetActive(true);
                rowList.Add(item);
                idx++;
            }

            _itemGroupList.Add(rowList);
        }
    }


    private List<string> GetRowSpriteNameList(bool isReward)
    {
        List<string> list = new List<string>();
        bool hasSame = false;

        string randomName = GetRandomSpriteName();
        list.Add(randomName);

        while (list.Count < ItemRowLength)
        {
            if (!isReward)
            {
                randomName = GetRandomSpriteName();
                if (list.Contains(randomName))
                {
                    if (hasSame)
                    {
                        continue;
                    }

                    hasSame = true;
                }
            }

            list.Add(randomName);
        }

        return list;
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
            if (i % 3 == 0)
            {
                delayTime += LocalCommonData.ItemDoFadeDelayTime;
            }

            GameObject item = BaseAnimItemList[i];
            item.GetComponent<CommonItem>().ShowTopImg(durTime, delayTime);
        }

        return delayTime + durTime;
    }


    //  
    private void SetRewardItem(int col, BaseRewardItemData rewardData)
    {
        GameObject rewardObj = rewardItemList[col];

        rewardObj.GetComponent<BaseCardItem>().ShowItem(rewardData);

        if (rewardData.IsThanks) return;

        LocalRewardData.ShowThankPanel = false;

        Vector3 rewardPos = rewardObj.GetComponent<BaseCardItem>().goodsImg.transform.position;
        switch (rewardData.Type)
        {
            case CommonRewardType.Coin:
                LocalRewardData.CompleteData.HasCoin = true;
                LocalRewardData.CompleteData.CoinAmount += (int)rewardData.Amount;
                LocalRewardData.ShowRewardPanel = true;
                LocalRewardData.CompleteData.CoinPos.Add(new KeyValuePair<int, Vector3>(rewardData.Amount, rewardPos));
                break;
            case CommonRewardType.Cash:
                LocalRewardData.CompleteData.HasCash = true;
                LocalRewardData.CompleteData.CashAmount += (decimal)rewardData.Amount;
                LocalRewardData.ShowRewardPanel = true;
                LocalRewardData.CompleteData.CashPos.Add(new KeyValuePair<int, Vector3>(rewardData.Amount, rewardPos));
                break;
            case CommonRewardType.Goods:
                LocalRewardData.CompleteData.CollectsPos.Add(  new KeyValuePair<int, Vector3>(rewardData.GoodsIdx, rewardPos));
                // LocalRewardData.CompleteData.GoodsPos.Add(
                //     new KeyValuePair<int, Vector3>(rewardData.GoodsIdx, rewardPos));
                break;
        }
    }

    private void SetItemImg(List<GameObject> itemList, int idx, BaseRewardItemData reward)
    {
        // DealSpecial(reward);
        SetRewardItem(idx, reward);
        List<string> nameList = GetRowSpriteNameList(!reward.IsThanks);

        for (int i = 0; i < itemList.Count; i++)
        {
            GameObject item = itemList[i];
            if (reward.IsThanks)
            {
                item.GetComponent<BaseCardItem>()
                    .ShowItem(ItemOffSpriteDict[nameList[i]]);
            }
            else
            {
                item.GetComponent<BaseCardItem>()
                    .ShowItem(ItemOffSpriteDict[nameList[i]], ItemOnSpriteDict[nameList[i]]);
                BaseAnimItemList.Add(item);
            }
        }
    }


    private void SetItemGroupImg()
    {
        List<BaseRewardItemData> rewardDataList = new List<BaseRewardItemData>();


        int randIdx = Random.Range(0, _itemGroupList.Count);

        for (int i = 0; i < _itemGroupList.Count; i++)
        {
            BaseRewardItemData reward = GetReward();
            if (i == randIdx && IsSpecialCard)
            {
                reward = GetEffectiveReward();
            }

            rewardDataList.Add(reward);
        }

        
        // for new user
        if (!SaveDataManager.GetBool(CConfig.sv_FinishFirstBigWin))
        {
            for (int i = 0; i < rewardDataList.Count; i++)
            {
                if (i == 0)
                {
                    rewardDataList[i] = GetNewUserReward();
                }
                else
                {
                    rewardDataList[i].IsThanks = true;
                }
            }

            CardUtil.Shuffle(rewardDataList);
        }

        if (SaveDataManager.GetBool(CConfig.sv_FinishFirstBigWin) &&
            !SaveDataManager.GetBool(CConfig.sv_InitSecondCard))
        {
            SaveDataManager.SetBool(CConfig.sv_InitSecondCard, true); 
            for (int i = 0; i < rewardDataList.Count; i++)
            {
                rewardDataList[i] = GetEffectiveReward();
                while (rewardDataList[i].Type == CommonRewardType.Goods)
                {
                    rewardDataList[i] = GetEffectiveReward();
                }
            }

            CardUtil.Shuffle(rewardDataList);
        }

        for (int i = 0; i < _itemGroupList.Count; i++)
        {
            SetItemImg(_itemGroupList[i], i, rewardDataList[i]);
        }
    }


    private void RowSameSpecial()
    {
        if (!SaveDataManager.GetBool(CConfig.sv_InitFirstCard))
        {
            SaveDataManager.SetBool(CConfig.sv_InitFirstCard, true);
            IsSpecialCard = false;
            LocalRewardData.CompleteData.IsSpecial = false;
            return;
        }
        if (SaveDataManager.GetBool(CConfig.sv_InitFirstCard) &&
            !SaveDataManager.GetBool(CConfig.sv_InitSecondCard))
        {
            IsSpecialCard = true;
            LocalRewardData.CompleteData.IsSpecial = true;
            DoBoardAct();
            return;
        }

        DealSpecialCard();
    }


    private void InitCardItems()
    {
        if (itemGroupArea.transform.childCount == 0)
        {
            CreateItem();
        }

        LocalRewardData.ResetCompleteData();

        RowSameSpecial();

        SetItemGroupImg();
    }
}