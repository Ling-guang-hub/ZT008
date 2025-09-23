// /**
// * @Author  AX
// * @Desc
// * @Date
// * @ 2025 04 01
// */

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardUtil
{
    // private static readonly Dictionary<ItemGroup, string> ItemDict = new Dictionary<ItemGroup, string>
    // {
    //     { ItemGroup.Simple, "b" }
    // };


    public static T GetRandomItem<T>()
    {
        int idx = Random.Range(0, Enum.GetNames(typeof(T)).Length);
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(idx);
    }


    public static List<T> Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }

        return list;
    }
}


public class CardItemParam
{
    public CardType ItemSource { get; set; }

    public int Index { get; set; }
}

public class LocalCardWeight
{
    public CardRewardType Type;

    public double Weight;

    public double RewardNum;

    public int RewardMulti;

    public int GoalCount;
}

public class LocalCardParam
{
    public int Id;

    public CardType Type;

    public List<LocalCardWeight> RewardWeight;

    public int Limit;

    public int RefreshTime;

    public int UnlockLine;

    public int WheelValue;

    public string CardName;

    public string CardDesc;

}


public static class LocalCardData
{
    public static List<int> ActCardIds;

    public static Dictionary<int, CardType> CardTypeDict;

    public static Dictionary<int, LocalCardParam> CardParamDict;
    
    public static Dictionary<int, int> CardWheelValueDict;
}

public class CompleteData
{
    public bool HasCoin;
    public int CoinAmount;
    public bool HasCash;
    public bool IsSpecial;
    public decimal CashAmount;
    public List<KeyValuePair<int, Vector3>> CashPos;
    public List<KeyValuePair<int, Vector3>> CoinPos;
    public List<KeyValuePair<int, Vector3>> GoodsPos;
    public List<KeyValuePair<int, Vector3>> CollectsPos;
    // public List<Vector3> CollectsPos;

}


public static class LocalRewardData
{
    public static bool ShowRewardPanel;

    public static CompleteData CompleteData;

    public static bool ShowThankPanel;


    public static void ResetCompleteData()
    {
        ShowRewardPanel = false;
        ShowThankPanel = true;
        CompleteData = new CompleteData()
        {
            HasCoin = false,
            HasCash = false,
            IsSpecial = false,
            CoinAmount = 0,
            CashAmount = 0,
            GoodsPos = new List<KeyValuePair<int, Vector3>>(),
            // CollectsPos = new List<Vector3>(),
            CollectsPos = new List<KeyValuePair<int, Vector3>>(),
            CoinPos = new List<KeyValuePair<int, Vector3>>(),
            CashPos = new List<KeyValuePair<int, Vector3>>(),
        };
    }
}

public static class LocalWheelData
{
    public static CommonRewardType WheelType;

    public static double WheelAmount;

    public static int WheelCardId;

    public static Sprite WheelCardSprite;

}

public static class LocalPassportData
{
    public static int LastLevel;

    public static int CurrentLevel;

    public static int PassportActDay;
}



public static class LocalCommonData
{
    public static int CurrentCardId;

    public static List<string> PlayAnimList = new List<string>();

    public static bool IsGamePass;

    public static void ResetCommonData()
    {
        PlayAnimList = new List<string>();
        IsGamePass = false;
    }

    public static readonly float ItemDoFadeDuringTime = 0.2f;
    
    public static readonly float ItemDoFadeDelayTime = 0.2f;

    public static float ScreenRate = 0.5f;

    public static bool IsGamePanel;

    public static int NextRandomCardId;

    // public static CardItemType CardRewardTypeToCareItemType(CardRewardType rewardType)
    // {
    //     return rewardType switch
    //     {
    //         CardRewardType.Coin => CardItemType.Coin,
    //         CardRewardType.Cash => CardItemType.Cash,
    //         _ => CardItemType.Goods
    //     };
    // }


    // public static int Score { get; set; }      // 传递数值参数
    // public static object CustomData { get; set; } // 传递复杂对象
}


public enum CardRewardType
{
    Coin,
    Cash,
    Goods,
    Thanks
}

public enum CardType
{
    Simple,
    RowSame,
    SeekSame,
    SameThree,
    TicTacToe,
    Poker,
    Jackpot,
    Football,
    Diamond
}


public enum CardItemType
{
    Coin,
    Cash,
    Goods
}


public class BaseCardData
{
    public string SpriteName;
    public Sprite ItemSprite;
    public Sprite TopSprite;
    public BaseRewardItemData RewardItemData;
}

public class BaseRewardItemData
{
    public bool IsThanks;

    public CommonRewardType Type;

    public int Amount;

    public Sprite RewardSprite;

    public Vector2 ItemPosition;

    public int GoodsIdx;

    public int RewardMulti;

    public int GoalCount;

    // public BaseRewardItemData(bool isThanks)
    // {
    //     IsThanks = isThanks;
    // }
}