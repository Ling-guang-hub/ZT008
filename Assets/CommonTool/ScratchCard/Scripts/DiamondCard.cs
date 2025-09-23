// Project  ScratchCard
// FileName  SimpleCard.cs
// Author  AX
// Desc
// CreateAt  2025-03-31 18:03:49 
//


using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/**
 *  Win the reward of the winning grid
 */
public class DiamondCard : BaseCard
{
    public static DiamondCard Instance;

    private List<List<GameObject>> _itemGroupList;

    private static readonly int ItemRowLength = 5;
    private static readonly int ItemColLength = 5;

    public GameObject rewardArea;

    public GameObject itemGroupArea;

    public GameObject newBaseItemObj;

    public List<Image> rewardLightList;

    // private List<Image> _animLightList;

    private readonly List<string> _cardColName = new List<string>() { "A", "B", "C", "D", "E" };
    private readonly List<string> _cardRowName = new List<string>() { "1", "2", "3", "4", "5" };

    private List<string> _itemNameList;
    private List<string> _targetNameList;

    private string _specialName;

    void Awake()
    {
        Instance = this;
        InitBaseData();
        cardType = CardType.Diamond;
    }


    // first  init  card data
    public override void InitCardData()
    {
        base.InitCardData();
        InitCardItems();
    }


    public override float DoWinAnim()
    {
        float delayTime = 0f;
        float durTime = LocalCommonData.ItemDoFadeDuringTime;

        for (int i = 0; i < BaseAnimItemList.Count; i++)
        {
            delayTime += LocalCommonData.ItemDoFadeDelayTime;

            Image thisImage = rewardLightList[i];

            Color color = thisImage.color;
            color.a = 0f;
            thisImage.color = color;
            thisImage.gameObject.SetActive(true);
            thisImage.DOFade(1f, durTime).SetDelay(delayTime).SetEase(Ease.InOutQuad).OnComplete(() => { });

            delayTime += LocalCommonData.ItemDoFadeDelayTime;
            BaseAnimItemList[i].GetComponent<CommonItem>().ShowTopImg(durTime, delayTime);
        }

        return delayTime + durTime;
    }


    public override void DoLoopAnim()
    {
        if (BaseAnimItemList.Count < 1) return;
        foreach (var t in BaseAnimItemList)
        {
            t.GetComponent<BaseCardItem>().ShowLoopFadeAct();
        }
    }


    private void CreateItem()
    {
        _itemGroupList = new List<List<GameObject>>();
        _itemNameList = new List<string>();
        int idx = 0;
        for (int i = 0; i < ItemColLength; i++)
        {
            List<GameObject> rowList = new List<GameObject>();
            float yPos = 270 - 135 * i;

            for (int j = 0; j < ItemRowLength; j++)
            {
                GameObject item = Instantiate(newBaseItemObj, itemGroupArea.transform, false);
                item.transform.localScale = itemGroupArea.transform.localScale * 0.9f;
                item.transform.localPosition = new Vector3(155 * j - 310, yPos, 1);
                item.GetComponent<BaseCardItem>().baseIdx = idx;
                string thisName = _cardColName[i] + _cardRowName[j];
                item.GetComponent<BaseCardItem>().itemName = thisName;
                item.gameObject.SetActive(true);
                rowList.Add(item);
                _itemNameList.Add(thisName);
                idx++;
            }

            _itemGroupList.Add(rowList);
        }
    }


    //  
    private void SetRewardItem(GameObject item, bool isSpecial)
    {
        BaseRewardItemData rewardData = GetReward();
        if (isSpecial && IsSpecialCard)
        {
            rewardData = GetEffectiveReward();
        }

        if (rewardData.IsThanks)
        {
            string spriteName = GetRandomSpriteName();
            item.GetComponent<BaseCardItem>().ShowItem(ItemOffSpriteDict[spriteName]);
        }
        else
        {
            LocalRewardData.ShowThankPanel = false;

            item.GetComponent<BaseCardItem>().ShowItem(rewardData);

            Vector3 rewardPos = item.GetComponent<BaseCardItem>().goodsImg.transform.position;
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


    private void SetItemImg(List<GameObject> itemList)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            GameObject item = itemList[i];
            string thisName = item.GetComponent<BaseCardItem>().itemName;
            if (_targetNameList.Contains(thisName))
            {
                BaseAnimItemList.Add(item);
                SetRewardItem(item, thisName == _specialName);
            }
            else
            {
                if (Random.Range(0, 3) > 1)
                {
                    string spriteName = GetRandomSpriteName();
                    item.GetComponent<BaseCardItem>().ShowItem(ItemOffSpriteDict[spriteName]);
                }
                else
                {
                    BaseRewardItemData rewardData = GetThankReward();
                    item.GetComponent<BaseCardItem>().ShowItem(rewardData);
                }
            }
        }
    }


    private void InitRewardItem()
    {
        foreach (var t in rewardLightList)
        {
            t.gameObject.SetActive(false);
        }

        _specialName = "";
        _targetNameList = new List<string>();
        while (_targetNameList.Count < rewardItemList.Count)
        {
            int idx = Random.Range(0, _itemNameList.Count);
            string thisName = _itemNameList[idx];
            if (!_targetNameList.Contains(thisName))
            {
                _targetNameList.Add(thisName);
            }
        }

        _specialName = _targetNameList[0];

        _targetNameList.Sort();

        for (int i = 0; i < rewardItemList.Count; i++)
        {
            GameObject item = rewardItemList[i];
            item.GetComponent<BaseCardItem>().SetBigText(_targetNameList[i]);
        }
    }


    private void SetItemGroupImg()
    {
        InitRewardItem();

        for (int i = 0; i < _itemGroupList.Count; i++)
        {
            SetItemImg(_itemGroupList[i]);
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

        SetItemGroupImg();
    }
}