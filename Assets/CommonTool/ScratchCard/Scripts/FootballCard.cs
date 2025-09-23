// Project  ScratchCard
// FileName  SimpleCard.cs
// Author  AX
// Desc
// CreateAt  2025-03-31 18:03:49 
//


using System.Collections.Generic;
using UnityEngine;

/**
 *   More goal, more cash
 */
public class FootballCard : BaseCard
{
    public static FootballCard Instance;

    private List<List<GameObject>> _itemGroupList;

    private static readonly int ItemRowLength = 6;
    private static readonly int ItemColLength = 5;

    public GameObject rewardArea;

    public GameObject itemGroupArea;

    private List<LocalCardWeight> _cardWeightList;

    private string _targetOnName;

    private int _thisGoalCount;

    void Awake()
    {
        Instance = this;
        InitBaseData();
        cardType = CardType.Football;
        _targetOnName = ItemSpriteOnName[0];
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


    public override float DoWinAnim()
    {
        float delayTime = 0f;
        float durTime = LocalCommonData.ItemDoFadeDuringTime;

        for (int i = 0; i < rewardItemList.Count; i++)
        {
            if (i < _thisGoalCount)
            {
                delayTime += LocalCommonData.ItemDoFadeDelayTime;
                rewardItemList[i].GetComponent<CommonItem>().ShowTopImg(durTime, delayTime);
            }
        }

        return base.DoWinAnim();
    }

    private void SetTopData()
    {
        _cardWeightList = GameUtil.GetLocalRewardAfterMultiWeightList();
        int idx = 0;

        BaseRewardItemData thisReward = GetReward();
        // DealSpecial(thisReward);
        _thisGoalCount = thisReward.GoalCount;
        if (IsSpecialCard && _thisGoalCount < 1)
        {
            _thisGoalCount = 1;
        }

        foreach (LocalCardWeight weight in _cardWeightList)
        {
            if (weight.GoalCount == 0) continue;

            BaseRewardItemData rewardData = RewardToBaseItem(weight);
            GameObject item = rewardItemList[idx];
            item.gameObject.GetComponent<BaseCardItem>().ShowItem(rewardData);

            if (weight.GoalCount <= _thisGoalCount)
            {
                // BaseAnimItemList.Add(item);
                SetRewardItem(item, rewardData);
            }

            idx++;
        }
    }


    private void CreateItem()
    {
        _itemGroupList = new List<List<GameObject>>();
        int idx = 0;
        for (int i = 0; i < ItemColLength; i++)
        {
            List<GameObject> rowList = new List<GameObject>();
            float yPos = 240 - 120 * i;

            for (int j = 0; j < ItemRowLength; j++)
            {
                GameObject item = Instantiate(BaseCardItemObj, itemGroupArea.transform, false);
                item.transform.localScale = itemGroupArea.transform.localScale;
                item.transform.localPosition = new Vector3(140 * j - 350, yPos, 1);
                item.GetComponent<BaseCardItem>().baseIdx = idx;
                item.gameObject.SetActive(true);
                rowList.Add(item);
                idx++;
            }

            _itemGroupList.Add(rowList);

            // GameObject rewardObj = Instantiate(BaseCardItemObj, rewardArea.transform, false);
            // rewardObj.transform.localScale = rewardArea.transform.localScale;
            // rewardObj.transform.localPosition = new Vector3(0, yPos, 1);
            // rewardObj.gameObject.SetActive(true);
            // rewardGroupList.Add(rewardObj);
        }
    }


    private void SetRewardItem(GameObject rewardObj, BaseRewardItemData rewardData)
    {
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
                // LocalRewardData.CompleteData.CollectsPos.Add(rewardPos);
                LocalRewardData.CompleteData.CollectsPos.Add(
                    new KeyValuePair<int, Vector3>(rewardData.GoodsIdx, rewardPos));
                break;
        }
    }


    private void SetItemGroupImg()
    {
        List<string> itemNames = new List<string>();
        while (itemNames.Count < _thisGoalCount)
        {
            itemNames.Add(_targetOnName);
        }

        while (itemNames.Count < ItemRowLength * ItemColLength)
        {
            string otherName = GetAntherSpriteName(_targetOnName);
            itemNames.Add(otherName);
        }

        CardUtil.Shuffle(itemNames);

        int idx = 0;

        for (int i = 0; i < _itemGroupList.Count; i++)
        {
            for (int j = 0; j < _itemGroupList[i].Count; j++)
            {
                GameObject item = _itemGroupList[i][j];
                string thisName = itemNames[idx];

                if (thisName == _targetOnName)
                {
                    item.GetComponent<BaseCardItem>()
                        .ShowItem(ItemOffSpriteDict[thisName], ItemOnSpriteDict[thisName]);
                    BaseAnimItemList.Add(item);
                }
                else
                {
                    item.GetComponent<BaseCardItem>()
                        .ShowItem(ItemOffSpriteDict[thisName]);
                }

                idx++;
            }
        }
    }


    private void InitCardItems()
    {
        if (itemGroupArea.transform.childCount == 0)
        {
            CreateItem();
        }

        LocalRewardData.ResetCompleteData();

        DealSpecialCard();

        SetTopData();
        SetItemGroupImg();
    }
}