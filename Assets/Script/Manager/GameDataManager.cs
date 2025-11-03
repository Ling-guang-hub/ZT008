using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameDataManager : MonoSingleton<GameDataManager>
{
    // Start is called before the first frame update
    void Start()
    {
        // DataManager.Instance.Init();
    }

    public void InitGameData()
    {
    }


    public int GetCoin()
    {
        return SaveDataManager.GetInt(CConfig.sv_GoldCoin);
    }


    public void SubCoin(int coin)
    {
        int oldCoin = GetCoin();
        if (oldCoin - coin < 0) return;
        PostEventScript.GetInstance().SendEvent("108" + LocalCommonData.CurrentCardId);

        SaveDataManager.SetInt(CConfig.sv_GoldCoin,
            oldCoin - coin);
        MessageCenterLogic.GetInstance().Send(CConfig.mg_SubCoin, new MessageData(oldCoin));
    }


    public void AddCoin(int coin)
    {
        if(coin < 0) return;
        SaveDataManager.SetInt(CConfig.sv_GoldCoin, GetCoin() + coin);
        SaveDataManager.SetInt(CConfig.sv_CumulativeGoldCoin, SaveDataManager.GetInt(CConfig.sv_CumulativeGoldCoin) + coin);
    }

    public void AddMoney(decimal money)
    {
        if(money < 0) return;
        CashOutManager.GetInstance().AddMoney((float)money);
    }


    public decimal GetMoney()
    {
        float money = CashOutManager.GetInstance().Money;
        return Convert.ToDecimal(money);
    }


    // public void TakeCard()
    // {
    //     TakeCard(LocalCommonData.CurrentCardId);
    // }

    // public void TakeCard(int cardId)
    // {
    //     PostEventScript.GetInstance().SendEvent("108" + cardId);
    //     // SaveDataManager.SetInt(CConfig.sv_CardNum + cardId, GetCard(cardId) - 1);
    // }

    // public void AddCard(int num)
    // {
    //     AddCard(num, LocalCommonData.CurrentCardId);
    // }

    // public void AddCard(int num, int cardId)
    // {
    //     SaveDataManager.SetInt(CConfig.sv_CardNum + cardId, num + GetCard(cardId));
    // }

    // public int GetCard()
    // {
    //     return GetCard(LocalCommonData.CurrentCardId);
    // }


    // public int GetCard(int carId)
    // {
    //     return SaveDataManager.GetInt(CConfig.sv_CardNum + carId);
    // }

    // public void SetLastCardTime(long time = 0)
    // {
    //     SetLastCardTime(LocalCommonData.CurrentCardId, time);
    // }

    // public void SetLastCardTime(int cardId, long time = 0)
    // {
    //     long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - time;
    //     SaveDataManager.SetLong(CConfig.sv_LastCardTime + cardId, timestamp);
    // }

    // public long GetLastCardTime()
    // {
    //     return GetLastCardTime(LocalCommonData.CurrentCardId);
    // }

    // public long GetLastCardTime(int cardId)
    // {
    //     return SaveDataManager.GetLong(CConfig.sv_LastCardTime + cardId);
    // }

    private int GetFinishedCard()
    {
        return SaveDataManager.GetInt(CConfig.sv_FinishCard);
    }

    public void AddFinishedCard()
    {
        CardType cardTaskType = LocalCardData.CardTypeDict[LocalCommonData.CurrentCardId];

        SaveDataManager.SetInt(CConfig.sv_FinishCard, GetFinishedCard() + 1);
        SaveDataManager.SetInt(CConfig.sv_CumulativeFinishCard,
            SaveDataManager.GetInt(CConfig.sv_CumulativeFinishCard) + 1);

        TaskManager.GetInstance().TakeTask(TaskType.Total);
        TaskManager.GetInstance().TakeTask(TaskType.Card, cardTaskType);
        ADManager.Instance.UpdateTrialNum(GetFinishedCard());
        
        MessageCenterLogic.GetInstance().Send(CConfig.mg_ShowLevelBar);
    }

    public KeyValuePair<long, long> GetPassportLifeTime()
    {
        return new KeyValuePair<long, long>(SaveDataManager.GetLong(CConfig.sv_PassportStartTime),
            SaveDataManager.GetLong(CConfig.sv_PassportEndTime));
    }

    public void CheckPassportTime()
    {
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        if (SaveDataManager.GetLong(CConfig.sv_PassportEndTime) <= timestamp)
        {
            ResetPassportTime();
        }
    }


    public void ResetPassportTime()
    {
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        SaveDataManager.SetInt(CConfig.sv_FinishCard, 0);
        SaveDataManager.SetLong(CConfig.sv_PassportStartTime, timestamp);
        long endTime = timestamp + LocalPassportData.PassportActDay * 24 * 60 * 60;
        SaveDataManager.SetLong(CConfig.sv_PassportEndTime, endTime);
    }

    public void SetPassportLeftReward(int idx, int rewardNum)
    {
        PostEventScript.GetInstance().SendEvent("1005", (idx + 1).ToString());
        long startTIme = GetPassportLifeTime().Key;
        KeyValuePair<int, int> reward = GetPassportIdxReward(idx);
        string leftKey = CConfig.sv_PassportLeftReward + startTIme + idx;
        SaveDataManager.SetInt(leftKey, rewardNum);
    }

    public void SetPassportRightReward(int idx, int rewardNum)
    {
        PostEventScript.GetInstance().SendEvent("1006", (idx + 1).ToString());
        long startTIme = GetPassportLifeTime().Key;
        KeyValuePair<int, int> reward = GetPassportIdxReward(idx);
        string rightKey = CConfig.sv_PassportRightReward + startTIme + idx;
        SaveDataManager.SetInt(rightKey, rewardNum);
    }

    public KeyValuePair<int, int> GetPassportIdxReward(int idx)
    {
        long startTIme = GetPassportLifeTime().Key;
        string leftKey = CConfig.sv_PassportLeftReward + startTIme + idx;
        string rightKey = CConfig.sv_PassportRightReward + startTIme + idx;
        if (!SaveDataManager.HasKey(leftKey))
        {
            SaveDataManager.SetInt(leftKey, 0);
            SaveDataManager.SetInt(rightKey, 0);
        }

        return new KeyValuePair<int, int>(SaveDataManager.GetInt(leftKey), SaveDataManager.GetInt(rightKey));
    }


    public void SetPassportNetData(List<PassportLevelData> list)
    {
        long startTIme = GetPassportLifeTime().Key;
        string dataKey = CConfig.sv_PassportNetData + startTIme;
        SaveDataManager.SetString(dataKey, JsonMapper.ToJson(list));
    }

    public List<PassportLevelData> GetPassportNetData()
    {
        long startTIme = GetPassportLifeTime().Key;
        string dataKey = CConfig.sv_PassportNetData + startTIme;
        List<PassportLevelData> newList =
            JsonMapper.ToObject<List<PassportLevelData>>(SaveDataManager.GetString(dataKey));

        return newList;
    }


    public void SetPassportMinLevel(int idx)
    {
        SaveDataManager.SetInt(CConfig.sv_PassportLevel, idx);
    }


    private bool CheckIsLock(int cardId)
    {
        return CardManager.Instance.GetFinishCardNum() >= LocalCardData.CardParamDict[cardId].UnlockLine;
    }


    // public int GetRandomCardId()
    // {
        // List<int> copyList = new List<int>();
        //
        // LocalCardData.ActCardIds.ForEach(item => copyList.Add(item));
        //
        // CardUtil.Shuffle(copyList);
        // int randomCardId = -1;
        // for (int i = 0; i < copyList.Count; i++)
        // {
        //     if (copyList[i] != LocalCommonData.CurrentCardId && GetCard(copyList[i]) > 0 && CheckIsLock(copyList[i]))
        //     {
        //         randomCardId = copyList[i];
        //         break;
        //     }
        // }
        //
        // return randomCardId;
    // }
}