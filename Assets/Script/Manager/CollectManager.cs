// Project  BlockDropRush
// FileName  CollectManager.cs
// Author  AX
// Desc
// CreateAt  2025-09-11 17:09:45 
//


using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectManager : MonoBehaviour
{
    public static CollectManager Instance;

    public readonly int CollectLimit = 7;

    private readonly string CurCollectKeyStr = "Coll_CurCollectKey";

    public Dictionary<string, int> LimitDict;
    public Dictionary<CollectType, int> RewardDict;

    public bool needFly;

    public CollectType curType;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void InitData()
    {
        LimitDict = new Dictionary<string, int>();
        RewardDict = new Dictionary<CollectType, int>
        {
            [CollectType.KeyRing] = 100,
            [CollectType.Flame] = NetInfoMgr.instance.GameData.flame_cash,
            [CollectType.Lightning] = NetInfoMgr.instance.GameData.lightning_cash
        };

    }


    public int GetCollectReward(CollectType collectType)
    {
        return RewardDict[collectType];
    }

    public string GetCurrentCollectKey(CollectType idx)
    {
        return CurCollectKeyStr + idx;
    }


    public int GetCurCollectCount(string keyStr)
    {
        return PlayerPrefs.GetInt(keyStr);
    }


    public int GetActiveCollectCount(string keyStr)
    {
        return CollectLimit - PlayerPrefs.GetInt(keyStr);
    }

    public void AddCollectCount(int num, string keyStr)
    {
        PlayerPrefs.SetInt(keyStr, num + GetCurCollectCount(keyStr));
    }

    public void ClearCollectCount(string keyStr)
    {
        PlayerPrefs.SetInt(keyStr, 0);
    }

    public void SetCollectCount(int num, string keyStr)
    {
        PlayerPrefs.SetInt(keyStr, num);
    }

    public bool CheckGetReward()
    {
        foreach (string strKey in LimitDict.Keys)
        {
            if (GetCurCollectCount(strKey) >= LimitDict[strKey])
            {
                return true;
            }
        }

        return false;
    }
}