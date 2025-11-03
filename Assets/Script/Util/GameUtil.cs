using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameUtil
{
    // use for footballcard
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
                double num = netData.RewardNum * GetCoinMultiWithRandom();
                target.RewardNum = (int)Math.Ceiling(num);
                target.Type = CardRewardType.Coin;
            }
            else if (netData.Type == CardRewardType.Cash)
            {
                double num = netData.RewardNum * GetCashMultiWithRandom();
                // target.RewardNum =(int) Math.Ceiling(num);
                target.RewardNum = Math.Round(num,2);
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
                target.CollectType = t.CollectType;
                break;
            }
        }

        if (target.Type == CardRewardType.Coin)
        {
            double num = target.RewardNum * GetCoinMultiWithRandom();
            target.RewardNum = (int)Math.Ceiling(num);
        }
        else if (target.Type == CardRewardType.Cash)
        {
            double num = target.RewardNum * GetCashMultiWithRandom();
            target.RewardNum = Math.Round(num,2);
            // target.RewardNum = (int)Math.Ceiling(num);
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
                    double gold = item.count * GetCoinMultiWithRandom();
                    newReward.Count = (int)Math.Ceiling(gold);
                    break;
                }
                case "Cash":
                {
                    newReward.Type = CommonRewardType.Cash;
                    double cash = item.count * GetCashMultiWithRandom();
                    // newReward.Count = (int)Math.Ceiling(cash);
                    newReward.Count = Math.Round(cash,2);;
                    break;
                }
                default:
                    newReward.Type = CommonRewardType.Coin;
                    double other = item.count * GetCoinMultiWithRandom();
                    newReward.Count = (int)Math.Ceiling(other);
                    // newReward.Type = CommonRewardType.Card;
                    // newReward.Count = item.count;
                    // newReward.CardId = int.Parse(item.type);
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
            newReward.Type = item.type == "Cash" ? CommonRewardType.Cash : CommonRewardType.Coin;
            newReward.RewardNum = item.type == "Cash"
                ? (int)Math.Ceiling(item.count * GetCashMulti())
                : (int)Math.Ceiling(item.count * GetGoldMulti());

            // newReward.Type = CommonRewardType.Coin;
            // newReward.RewardNum = (int)Math.Ceiling(item.count * GetGoldMulti());
            list.Add(newReward);
        }

        for (int i = 0; i < list.Count - 1; i++)
        {
            list[i].NextCard = list[i + 1].NeedCard;
        }

        GameDataManager.GetInstance().SetPassportNetData(list);

        return list;
    }


    public static List<SlotRewardData> GetCollectRewardDataList()
    {
        List<SlotRewardData> list = new List<SlotRewardData>();
        List<NetWeightData> sourceList = NetInfoMgr.instance.GameData.collect_weight_group;
        foreach (NetWeightData item in sourceList)
        {
            SlotType slotType = StringUtil.ToEnum<SlotType>(item.type);

            SlotRewardData newReward = new SlotRewardData
            {
                Weight = item.weight,
                Type = slotType
            };

            if (item.type.Contains("Coin"))
            {
                double netParam = item.count * GetCoinMultiWithRandom();
                newReward.Count = (int)Math.Ceiling(netParam);
            }
            else
            {
                decimal netParam =Convert.ToDecimal( item.count * GetCashMultiWithRandom());
                // newReward.Count = (int)Math.Ceiling(netParam);
                newReward.Count = decimal.Round(netParam,2);
            }

            list.Add(newReward);
        }

        return list;
    }


    public static SlotRewardData GetCollectReward()
    {
        List<SlotRewardData> list = GetCollectRewardDataList();
        float maxWeight = 0;
        foreach (SlotRewardData reward in list)
        {
            maxWeight += reward.Weight;
        }

        SlotRewardData targetReward = new SlotRewardData();

        float thisWeight = Random.Range(0, maxWeight);
        float tempWeight = 0;
        foreach (var reward in list)
        {
            tempWeight += reward.Weight;
            if (tempWeight >= thisWeight)
            {
                targetReward.Type = reward.Type;
                targetReward.Count = reward.Count;
                break;
            }
        }

        return targetReward;
    }

    
    public static decimal GetFlyBoxReward()
    {
        int flyBox =  NetInfoMgr.instance.GameData.fly_pop;
        return Convert.ToDecimal(flyBox*GetCashMultiWithRandom());
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
        return NetInfoMgr.instance.InitData.gold_group[0].multi;
        // return GetMulti(RewardType.Gold, SaveDataManager.GetDouble(CConfig.sv_CumulativeGoldCoin),
        //     NetInfoMgr.instance.InitData.gold_group);
    }
    
    public static double GetCoinMultiWithRandom()
    {
        float random = Random.Range((float)NetInfoMgr.instance.InitData.cash_random[0],
            (float)NetInfoMgr.instance.InitData.cash_random[1]);
        return GetGoldMulti() * (1 + random);
    }

    public static double GetCashMulti()
    {
        return NetInfoMgr.instance.InitData.cash_group[0].multi;
        // return GetMulti(RewardType.Cash, SaveDataManager.GetDouble(CConfig.sv_CumulativeCash),
        //     NetInfoMgr.instance.InitData.cash_group);
    }
    
    public static double GetCashMultiWithRandom()
    {
        float random = Random.Range((float)NetInfoMgr.instance.InitData.cash_random[0],
            (float)NetInfoMgr.instance.InitData.cash_random[1]);
        return GetCashMulti() * (1 + random);
    }
    
    
    
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


public enum SlotType
{
    Cash01,
    Cash02,
    Cash03,
    Coin01,
    Coin02,
    Coin03,
    Big777
}

public enum PanelType
{
    Card,
    Wheel,
    Slot,
    FlyBox,
    Default
    
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

    public decimal CashAmount;

    public PanelType PanelType;
    
    public Sprite RewardSprite;
        
}

public class SlotRewardData
{
    public SlotType Type;

    public float Weight;

    public decimal Count;
}

