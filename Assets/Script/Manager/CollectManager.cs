// Project  BlockDropRush
// FileName  CollectManager.cs
// Author  AX
// Desc
// CreateAt  2025-09-11 17:09:45 
//


using System;
using UnityEngine;

public class CollectManager : MonoBehaviour
{
    public static CollectManager Instance;

    public readonly int CollectLimit = 7;

    private readonly string CurCollectKey = "Coll_CurCollectKey";


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

    private void Start()
    {
        if (!PlayerPrefs.HasKey(CurCollectKey))
        {
            PlayerPrefs.SetInt(CurCollectKey, 0);
        }
    }


 

    public int GetCurCollectCount()
    {
        return PlayerPrefs.GetInt(CurCollectKey);
    }


    public int GetActiveCollectCount()
    {
        return CollectLimit - PlayerPrefs.GetInt(CurCollectKey);
    }

    public void AddCollectCount(int num)
    {
        PlayerPrefs.SetInt(CurCollectKey, num + GetCurCollectCount());
    }

    public void ClearCollectCount()
    {
        PlayerPrefs.SetInt(CurCollectKey, 0);
    }

    public bool CheckGetReward()
    {
        return GetCurCollectCount() >= CollectLimit;
    }
}