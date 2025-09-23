// Project  ScratchCard
// FileName  EdgeBothMagic.cs
// Author  AX
// Desc
// CreateAt  2025-04-18 11:04:32 
//


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EdgeBothMagic : BaseUIForms
{
    public static EdgeBothMagic Instance;

    [FormerlySerializedAs("topBar")]


    public GameObject topBar;

    [FormerlySerializedAs("mainAreaObj")]


    public GameObject DireFlapSum;

    [FormerlySerializedAs("itemArea")]


    public GameObject itemArea;

    [FormerlySerializedAs("baseItemObj")]


    public GameObject baseItemObj;

    [FormerlySerializedAs("timeText")]


    public Text timeText;

    [FormerlySerializedAs("levelText")]


    public Text levelText;

    [FormerlySerializedAs("closeBtn")]


    public Button AlikeBuy;

    [FormerlySerializedAs("passportDragAct")]


    public PassportDragAct passportDragAct;

    private static List<PassportLevelData> _passportLevelData;

    private static KeyValuePair<long, long> _passportLifeTime;

    private List<GameObject> _itemList;

    private int _currentLevel;

    private float _offsetY;

    private float _beginPosy;

    private float _endPosy;

    private float _timeStr;

    private float _countDownTime;

    private Coroutine _countdownCoroutine;

    private bool _isCoinFly;


    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        _offsetY = 450f;
        _beginPosy = -300f;
        _passportLifeTime = GameDataManager.GetInstance().GetPassportLifeTime();

        _passportLevelData = GameUtil.GetPassportData();
        _itemList = new List<GameObject>();
        LocalPassportData.LastLevel = _passportLevelData.Count - 1;
        passportDragAct.topPosY = _beginPosy;
        _endPosy = -400f - (_passportLevelData.Count - 3) * -400;
        passportDragAct.downPosY = _endPosy;
        passportDragAct.itemCount = _passportLevelData.Count;
    }


    private void Start()
    {
        AlikeBuy.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_2);
            MessageCenterLogic.GetInstance().Send(CConfig.mg_ClosePanel);
            MessageCenterLogic.GetInstance().Send(CConfig.mg_GameSuspend);

            CloseUIForm(GetType().Name);
        });
    }


    private int GetMinIdx()
    {
        int idx = 0;
        for (idx = 0; idx < _passportLevelData.Count; idx++)
        {
            KeyValuePair<int, int> pair = GameDataManager.GetInstance().GetPassportIdxReward(idx);
            if (pair.Key == 0)
            {
                break;
            }
        }

        return idx;
    }

    private float GetCurrentItemPos()
    {
        if (_passportLevelData.Count < 4 || _currentLevel < 2)
        {
            return _beginPosy;
        }

        if (_passportLevelData.Count - _currentLevel < 2)
        {
            return _endPosy;
        }


        return 400f - (_currentLevel - 2) * -400;
    }


    private void CloseTopBar()
    {
        topBar.gameObject.SetActive(false);
        _isCoinFly = false;
    }

    public async void AddCoinAndCash(int coinAmount, Vector3 coinPis, decimal cashAmount, Vector3 cashPos)
    {
        if (coinAmount <= 0 && cashAmount <= 0) return;


        // if (topBar.gameObject.activeInHierarchy) return;

        topBar.gameObject.SetActive(true);
        _isCoinFly = true;
        if (coinAmount > 0)
        {
            await topBar.GetComponent<TopBar>().AddCoinAndDoAnima(coinAmount, coinPis, 0, Vector2.zero);
        }

        if (cashAmount > 0)
        {
            await topBar.GetComponent<TopBar>().AddCoinAndDoAnima(0, Vector2.zero, cashAmount, cashPos);
        }

        Invoke(nameof(CloseTopBar), 1.5f);
    }

    public bool CheckCoinFly()
    {
        return _isCoinFly;
    }


    private void ItLog()
    {
        DireFlapSum.transform.localScale = Vector3.zero;
        DireFlapSum.transform.DOScale(1f, 0.2f);
    }

    public override void Display(object uiFormParams)
    {
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Panel_pop);

        PostEventScript.GetInstance().SendEvent("1004");
        topBar.gameObject.SetActive(false);
        levelText.text = CardManager.Instance.GetFinishCardNum() + "";
        ResetMinLevel();
        passportDragAct.SetCurrentItem(GetCurrentItemPos());
        _isCoinFly = false;
        base.Display(uiFormParams);
        if (_itemList.Count == 0)
        {
            //初始化UI
            for (int i = 0; i < _passportLevelData.Count; i++)
            {
                GameObject item = Instantiate(baseItemObj, itemArea.transform);
                item.transform.localPosition = new Vector3(0, _offsetY + i * -400, 0);
                item.gameObject.SetActive(true);
                _itemList.Add(item);
            }
        }

        for (int i = 0; i < _itemList.Count; i++)
        {
            _itemList[i].GetComponent<PassportItem>().PostDeed(i, _passportLevelData[i]);
        }

        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Flip);

        for (int i = 0; i < _itemList.Count; i++)
        {
            int idx = i;
            Transform itemObj = _itemList[i].transform;
            itemObj.DOKill();
            itemObj.localScale = new Vector2(1, 0);
            itemObj.DOScaleY(1, 0.2f).SetDelay(0.02f * idx).OnComplete(() => { });
        }

        if (_countdownCoroutine != null)
        {
            StopCoroutine(nameof(CountdownCoroutine));
        }

        long endTime = _passportLifeTime.Value;
        _countDownTime = endTime - DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        _countdownCoroutine = StartCoroutine(nameof(CountdownCoroutine));
    }


    public void ResetMinLevel()
    {
        _currentLevel = GetMinIdx();
        GameDataManager.GetInstance().SetPassportMinLevel(_currentLevel);
        LocalPassportData.CurrentLevel = _currentLevel;
    }

    private void Update()
    {
        // 遍历所有触摸点
    }

    private void ShowTimeData()
    {
        TimeSpan t = TimeSpan.FromSeconds(_countDownTime);
        string str = $"{t.Hours:00}:{t.Minutes:00}:{t.Seconds:00}";
        timeText.text = t.Days + " day " + str + " LEFT ";
    }


    IEnumerator CountdownCoroutine()
    {
        while (_countDownTime > 0)
        {
            _countDownTime--;
            ShowTimeData();
            yield return new WaitForSeconds(1f);
        }
    }
}

public class LevelPassData //通行证数据
{
    [FormerlySerializedAs("ID")]

    public int ID;
    [FormerlySerializedAs("Description")]

    public string Description;
    [FormerlySerializedAs("NeedValue")]

    public int NeedValue;
    [FormerlySerializedAs("AdReward")]

    public int AdReward;
    [FormerlySerializedAs("BaseReward")]

    public int BaseReward;
    [FormerlySerializedAs("CollectState")]

    public int CollectState; //0未领取 1已领取基础 2已领取广告 3基础和广告都领取
    [FormerlySerializedAs("RewardType")]

    public RewardType RewardType;
}