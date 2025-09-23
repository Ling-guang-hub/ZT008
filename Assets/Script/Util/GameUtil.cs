using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameUtil
{
    public static List<LocalCardWeight> GetLocalRewardAfterMultiWeightList()
    {
        List<LocalCardWeight> list = LocalCardData.CardParamDict[LocalCommonData.CurrentCardId].RewardWeight;

        List<LocalCardWeight> multiList = new List<LocalCardWeight>();


        foreach (var netData in list)
        {
            LocalCardWeight target = new LocalCardWeight()
            {
                RewardMulti = netData.RewardMulti,
                Weight = netData.Weight,
                GoalCount = netData.GoalCount,
            };

            if (netData.Type == CardRewardType.Coin)
            {
                double num = netData.RewardNum * GetGoldMulti();
                target.RewardNum = (int)Math.Ceiling(num);
                target.Type = CardRewardType.Coin;
            }
            else if (netData.Type == CardRewardType.Cash)
            {
                double num = netData.RewardNum * GetCashMulti();
                target.RewardNum = Math.Ceiling(num);
                target.Type = CardRewardType.Cash;
            }
            else
            {
                target.RewardNum = netData.RewardNum;
                target.Type = CardRewardType.Goods;
            }

            multiList.Add(target);
        }

        return multiList;
    }


    public static LocalCardWeight GetLocalRewardWeight()
    {
        List<LocalCardWeight> list = LocalCardData.CardParamDict[LocalCommonData.CurrentCardId].RewardWeight;

        float maxWeight = 0;
        foreach (LocalCardWeight obj in list)
        {
            maxWeight += (float)obj.Weight;
        }

        float randomWeight = Random.Range(0, maxWeight);
        double tempWeight = 0;

        LocalCardWeight target = new LocalCardWeight()
        {
            Type = CardRewardType.Thanks,
            RewardNum = 1,
            RewardMulti = 1,
        };

        foreach (var t in list)
        {
            tempWeight += t.Weight;
            if (tempWeight >= randomWeight)
            {
                target.Type = t.Type;
                target.RewardNum = t.RewardNum;
                target.RewardMulti = t.RewardMulti;
                target.GoalCount = t.GoalCount;
                break;
            }
        }

        if (target.Type == CardRewardType.Coin)
        {
            double num = target.RewardNum * GetGoldMulti();
            target.RewardNum = (int)Math.Ceiling(num);
        }
        else if (target.Type == CardRewardType.Cash)
        {
            double num = target.RewardNum * GetCashMulti();
            target.RewardNum = Math.Ceiling(num);
        }

        return target;
    }

    public static List<WheelBigItemReward> GetWheelBigItemRewards()
    {
        List<WheelBigItemReward> list = new List<WheelBigItemReward>();
        List<NetWeightData> sourceList = NetInfoMgr.instance.GameData.wheel_weight_group;
        foreach (NetWeightData item in sourceList)
        {
            WheelBigItemReward newReward = new WheelBigItemReward
            {
                Weight = item.weight
            };
            switch (item.type)
            {
                case "Coin":
                {
                    newReward.Type = CommonRewardType.Coin;
                    double netParam = item.count * GetGoldMulti();
                    newReward.Count = Math.Ceiling(netParam);
                    break;
                }
                case "Cash":
                {
                    // newReward.Type = CommonRewardType.Cash;
                    // if (CommonUtil.IsApple())
                    // {
                    newReward.Type = CommonRewardType.Coin;
                    // }

                    // double netParam = item.count * GetCashMulti();
                    double netParam = item.count * GetGoldMulti();
                    newReward.Count = Math.Ceiling(netParam);
                    break;
                }
                default:
                    newReward.Type = CommonRewardType.Card;
                    newReward.Count = item.count;
                    newReward.CardId = int.Parse(item.type);
                    break;
            }

            list.Add(newReward);
        }

        return list;
    }


    public static int GetWheelRewardIdx()
    {
        List<WheelBigItemReward> list = GetWheelBigItemRewards();
        float maxWeight = 0;
        foreach (WheelBigItemReward reward in list)
        {
            maxWeight += reward.Weight;
        }

        float thisWeight = Random.Range(0, maxWeight);
        int idx = 0;
        float tempWeight = 0;
        for (int i = 0; i < list.Count; i++)
        {
            WheelBigItemReward reward = list[i];
            tempWeight += reward.Weight;
            if (tempWeight >= thisWeight)
            {
                idx = i;
                break;
            }
        }

        return idx;
    }


    public static List<PassportLevelData> GetPassportData()
    {
        List<PassportLevelData> localList = GameDataManager.GetInstance().GetPassportNetData();
        if (localList is { Count: > 0 })
        {
            return localList;
        }

        List<PassportLevelData> list = new List<PassportLevelData>();

        List<NetPassportData> sourceList = NetInfoMgr.instance.GameData.passport_data_group;

        int tempCard = 0;

        foreach (NetPassportData item in sourceList)
        {
            tempCard += item.card;
            PassportLevelData newReward = new PassportLevelData
            {
                NeedCard = item.card,
                LeastCard = tempCard,
            };

            newReward.CashCount = (int)Math.Ceiling(item.cash * GetGoldMulti());
            // newReward.Type = item.type == "Cash" ? CommonRewardType.Cash : CommonRewardType.Coin;
            // newReward.RewardNum = item.type == "Cash"
            //     ? (int)Math.Ceiling(item.count * GetCashMulti())
            //     : (int)Math.Ceiling(item.count * GetGoldMulti());

            newReward.Type = CommonRewardType.Coin;
            newReward.RewardNum = (int)Math.Ceiling(item.count * GetGoldMulti());
            list.Add(newReward);
        }

        for (int i = 0; i < list.Count - 1; i++)
        {
            list[i].NextCard = list[i + 1].NeedCard;
        }

        GameDataManager.GetInstance().SetPassportNetData(list);

        return list;
    }


    public static List<WheelBigItemReward> GetCollectRewardDataList()
    {
        List<WheelBigItemReward> list = new List<WheelBigItemReward>();
        List<NetWeightData> sourceList = NetInfoMgr.instance.GameData.collect_weight_group;
        foreach (NetWeightData item in sourceList)
        {
            WheelBigItemReward newReward = new WheelBigItemReward
            {
                Weight = item.weight
            };
            switch (item.type)
            {
                case "Coin":
                {
                    newReward.Type = CommonRewardType.Coin;
                    double netParam = item.count * GetGoldMulti();
                    newReward.Count = Math.Ceiling(netParam);
                    break;
                }
                // case "Cash":
                // {
                //     newReward.Type = CommonRewardType.Coin;
                //     double netParam = item.count * GetGoldMulti();
                //     newReward.Count = Math.Ceiling(netParam);
                //     break;
                // }
                // default:
                //     newReward.Type = CommonRewardType.Card;
                //     newReward.Count = item.count;
                //     newReward.CardId = int.Parse(item.type);
                //     break;
            }

            list.Add(newReward);
        }

        return list;
    }


    public static int GetCollectReward()
    {
        List<WheelBigItemReward> list = GetCollectRewardDataList();
        float maxWeight = 0;
        foreach (WheelBigItemReward reward in list)
        {
            maxWeight += reward.Weight;
        }

        float thisWeight = Random.Range(0, maxWeight);
        double count = 0;
        float tempWeight = 0;
        for (int i = 0; i < list.Count; i++)
        {
            WheelBigItemReward reward = list[i];
            tempWeight += reward.Weight;
            if (tempWeight >= thisWeight)
            {
                count = reward.Count;
                break;
            }
        }
        return Mathf.CeilToInt(float.Parse(count.ToString()) );
    }


    public static double GetNewUserCashRewardNum()
    {
        // float random = Random.Range((float)NetInfoMgr.instance.InitData.cash_random[0],
        // (float)NetInfoMgr.instance.InitData.cash_random[1]);
        // return 20f * (1 + random);
        return 20f;
    }


    /// <summary>
    /// 获取multi系数
    /// </summary>
    /// <returns></returns>
    private static double GetMulti(RewardType type, double cumulative, MultiGroup[] multiGroup)
    {
        foreach (MultiGroup item in multiGroup)
        {
            if (item.max > cumulative)
            {
                if (type == RewardType.Gold)
                {
                    float random = Random.Range((float)NetInfoMgr.instance.InitData.cash_random[0],
                        (float)NetInfoMgr.instance.InitData.cash_random[1]);
                    return item.multi * (1 + random);
                }
                else
                {
                    return item.multi;
                }
            }
        }

        return 1;
    }

    public static double GetGoldMulti()
    {
        return GetMulti(RewardType.Gold, SaveDataManager.GetDouble(CConfig.sv_CumulativeGoldCoin),
            NetInfoMgr.instance.InitData.gold_group);
    }

    public static double GetCashMulti()
    {
        return GetMulti(RewardType.Cash, SaveDataManager.GetDouble(CConfig.sv_CumulativeCash),
            NetInfoMgr.instance.InitData.cash_group);
    }


    // public static double GetAmazonMulti()
    // {
    //     return GetMulti(RewardType.Amazon, SaveDataManager.GetDouble(CConfig.sv_CumulativeAmazon), NetInfoMgr.instance.InitData.amazon_group);
    // }
}


/// <summary>
/// 奖励类型
/// </summary>
public enum RewardType
{
    Gold,
    Cash,
    // Amazon
}

public enum CommonRewardType
{
    Coin,
    Cash,
    Card,
    Goods,
}


public class WheelBigItemReward
{
    public CommonRewardType Type;

    public int CardId;

    public float Weight;

    public double Count;
}


public class PassportLevelData
{
    public CommonRewardType Type;

    public int RewardNum;

    public int CashCount;

    public int NeedCard;

    public int LeastCard;

    public int NextCard;
}

public class WinPanelData
{
    public int CoinAmount;

    public bool IsCard;
}