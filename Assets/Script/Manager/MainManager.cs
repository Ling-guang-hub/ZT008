using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    private bool ready = false;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }


    //切前后台也需要检测屏蔽 防止游戏中途更改手机状态
    private void OnApplicationFocus(bool focusStatus)
    {
        if (focusStatus)
        {
#if UNITY_ANDROID
            CommonUtil.AndroidBlockCheck();
#endif
        }
    }

    private void MarkNewUserAndInitData()
    {
        // 新用户
        SaveDataManager.SetBool(CConfig.sv_IsNewPlayer, false);
        SaveDataManager.SetBool(CConfig.sv_ShowRateUs, CommonUtil.IsApple());
        SaveDataManager.SetBool(CConfig.sv_FinishFirstBigWin, CommonUtil.IsApple()); //  first  show  bigwin 
        // SaveDataManager.SetBool(CConfig.sv_FinishFirstShowShop, CommonUtil.IsApple());  //  

        SaveDataManager.SetBool(CConfig.sv_InitFirstCard, CommonUtil.IsApple());
        SaveDataManager.SetBool(CConfig.sv_InitSecondCard, CommonUtil.IsApple());

        SaveDataManager.SetBool(CConfig.sv_FinishFirstCheckCard, CommonUtil.IsApple());
        SaveDataManager.SetBool(CConfig.sv_FinishFirstNewCard, CommonUtil.IsApple());
        SaveDataManager.SetBool(CConfig.sv_FinishNewGuide, CommonUtil.IsApple());
        SaveDataManager.SetBool(CConfig.sv_FinishShopGuide, CommonUtil.IsApple());


        SaveDataManager.SetBool(CConfig.sv_FirstWheel, false);
        SaveDataManager.SetBool(CConfig.sv_FirstTaskPanel, false);
        SaveDataManager.SetBool(CConfig.sv_HasShowRatePanel, false);
        SaveDataManager.SetBool(CConfig.sv_WheelDataInit, false);


        SaveDataManager.SetInt(CConfig.sv_BackHomeCount, 0);

        SaveDataManager.SetInt(CConfig.sv_GoldCoin, 0);
        SaveDataManager.SetInt(CConfig.sv_CumulativeGoldCoin, 0);
        SaveDataManager.SetDecimal(CConfig.sv_Cash, decimal.Zero);
        SaveDataManager.SetDecimal(CConfig.sv_CumulativeCash, decimal.Zero);
        // GameDataManager.GetInstance().SetNewUserGoods();

        SaveDataManager.SetInt(CConfig.sv_FinishCard, 0);
        SaveDataManager.SetInt(CConfig.sv_CumulativeFinishCard, 0);
        SaveDataManager.SetInt(CConfig.sv_PassportStartTime, 0);
        SaveDataManager.SetInt(CConfig.sv_PassportEndTime, 0);
        SaveDataManager.SetInt(CConfig.sv_PassportLevel, 0);

        GameDataManager.GetInstance().ResetPassportTime();
    }


    private static void NetDataToLocal()
    {
        // set  first card
        LocalCommonData.CurrentCardId = NetInfoMgr.instance.GameData.focus_card;
        LocalCommonData.RandomCoinStart = NetInfoMgr.instance.GameData.random_coin[0];
        LocalCommonData.RandomCoinEnd = NetInfoMgr.instance.GameData.random_coin[1];

        // get active cards  
        LocalCardData.ActCardIds = NetInfoMgr.instance.GameData.active_card;

        //  make  active  card  param  to  dict  by id
        List<NetCardParam> paramList = NetInfoMgr.instance.GameData.card_reward;
        LocalCardData.CardParamDict = new Dictionary<int, LocalCardParam>();
        LocalCardData.CardTypeDict = new Dictionary<int, CardType>();

        LocalCardData.CardWheelValueDict = new Dictionary<int, int>();


        foreach (NetCardParam param in paramList)
        {
            int id = param.id;
            CardType cardType = StringUtil.ToEnum<CardType>(param.type);
            LocalCardParam localParam = new LocalCardParam
            {
                Type = cardType,
                Limit = param.limit,
                RefreshTime = param.refresh_time,
                UnlockLine = param.unlock,
                WheelValue = param.wheel_value,
                CardName = param.name,
                CardDesc = param.desc,
                RewardWeight = new List<LocalCardWeight>()
            };

            foreach (NetWeightData weightData in param.card_weight)
            {
                // if (weightData.type == "Goods"||weightData.type.Contains("Cash") ) continue;
                if (weightData.type.Contains("Cash")) continue;
                LocalCardWeight localWeight = NetWeightToLocal(weightData);
                localParam.RewardWeight.Add(localWeight);
            }

            LocalCardData.CardWheelValueDict.TryAdd(id, localParam.WheelValue);
            LocalCardData.CardTypeDict.TryAdd(id, cardType);
            LocalCardData.CardParamDict.TryAdd(id, localParam);
        }

        LocalPassportData.PassportActDay = NetInfoMgr.instance.GameData.passport_day;
    }

    private void InitUserData()
    {
        foreach (int cardId in LocalCardData.ActCardIds)
        {
            string key = CConfig.sv_CardNum + cardId;
            if (!SaveDataManager.HasKey(key))
            {
                int limit = LocalCardData.CardParamDict[cardId].Limit;
                SaveDataManager.SetInt(key, limit);
            }
        }

        LocalRewardData.ResetCompleteData();
    }


    private static LocalCardWeight NetWeightToLocal(NetWeightData netData)
    {
        LocalCardWeight localWeight = new LocalCardWeight
        {
            Weight = netData.weight,
            RewardNum = netData.count,
            RewardMulti = 1,
            GoalCount = netData.goal,
        };
        switch (netData.type)
        {
            case "Coin":
                localWeight.Type = CardRewardType.Coin;
                break;
            case "Cash":
                localWeight.Type = CardRewardType.Cash;
                if (CommonUtil.IsApple())
                {
                    localWeight.Type = CardRewardType.Coin;
                }

                break;
            case "Goods":
                localWeight.Type = CardRewardType.Goods;
                break;
            case "Thanks":
                localWeight.Type = CardRewardType.Thanks;
                localWeight.RewardMulti = 0;
                break;
            case "DoubleCash":
                localWeight.Type = CardRewardType.Cash;
                if (CommonUtil.IsApple())
                {
                    localWeight.Type = CardRewardType.Coin;
                }

                localWeight.RewardMulti = 2;
                break;
            case "DoubleCoin":
                localWeight.Type = CardRewardType.Coin;
                localWeight.RewardMulti = 2;
                break;
        }

        return localWeight;
    }


    public void GameInit()
    {
        bool isNewPlayer = !PlayerPrefs.HasKey(CConfig.sv_IsNewPlayer + "Bool") ||
                           SaveDataManager.GetBool(CConfig.sv_IsNewPlayer);
        AdjustInitManager.Instance.InitAdjustData(isNewPlayer);

        NetDataToLocal();

        InitUserData();

        if (isNewPlayer)
        {
            MarkNewUserAndInitData();
        }
        else
        {
            GameDataManager.GetInstance().CheckPassportTime();
        }


        MusicMgr.GetInstance().PlayBg(MusicType.SceneMusic.BGM);

        GameDataManager.GetInstance().InitGameData();

        WheelBarManager.GetInstance().InitData();
        TaskManager.GetInstance().TaskDataInit();

        UIManager.GetInstance().ShowUIForms(nameof(TameMagic));

        ready = true;

// #if SOHOShop
//         SOHOShopManager.instance.InitSOHOShop();
// #endif
    }
}