// Project  BlockDropRush
// FileName  CardTimeManager.cs
// Author  AX
// Desc
// CreateAt  2025-07-02 10:07:54 
//


using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class CardTimeManager : MonoSingleton<CardTimeManager>
{
    private int _cardLimit;

    private int _refreshTime;

    private int _cardNum;

    private float _countDownTime;

    private bool _timeFlag;


    public void PostDeed()
    {
    }

    // use card
    public bool TakeCard()
    {
        _cardNum = GameDataManager.GetInstance().GetCard();
        _cardLimit = LocalCardData.CardParamDict[LocalCommonData.CurrentCardId].Limit;

        if (_cardNum < 1) return false;
        if (_cardNum == _cardLimit)
        {
            GameDataManager.GetInstance().SetLastCardTime();
        }
   
        GameDataManager.GetInstance().TakeCard();
        ShowCardNum();

        return true;
    }

    public void AddCard(int num)
    {
        GameDataManager.GetInstance().AddCard(num);
    }

    private void SendMaxStr()
    {
        MessageCenterLogic.GetInstance().Send(CConfig.mg_ShowCardMaxStr);
    }

    public void FirstShowCardNum()
    {
        ShowCardNum();
        RefreshNumAndTime();
    }

    public void RefreshNumAndTime()
    {
        // ResetIcon();
        _cardNum = GameDataManager.GetInstance().GetCard();
        _cardLimit = LocalCardData.CardParamDict[LocalCommonData.CurrentCardId].Limit;
        _refreshTime = LocalCardData.CardParamDict[LocalCommonData.CurrentCardId].RefreshTime;


        if (_timeFlag)
        {
            OnTimeEnd();
        }

        if (_cardNum >= _cardLimit)
        {
            SendMaxStr();
            return;
        }
        
        if (CalculateCard())
        {
            SendMaxStr();
            return;
        }

        ShowCardNum();
        OnTimeStart();
    }


    public float GetCurTime()
    {
        return _countDownTime;
    }

    private void ShowCardNum()
    {
        MessageCenterLogic.GetInstance().Send(CConfig.mg_GetCardByAd);
    }
    
    private bool AddCardByTime()
    {
        AddCard(1);
        GameDataManager.GetInstance().SetLastCardTime();
        _cardNum = GameDataManager.GetInstance().GetCard();
        if (_cardNum >= _cardLimit)
        {
            SendMaxStr();
            // timeText.text = _maxStr;
            return true;
        }

        _countDownTime = _refreshTime;
        return false;
    }
    
    
    private bool CalculateCard()
    {
        long ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long subTime = ts - GameDataManager.GetInstance().GetLastCardTime();
        int addCard = (int)subTime / _refreshTime;
        if (addCard == 0)
        {
            _countDownTime = _refreshTime - subTime;
            return false;
        }

        if (addCard + _cardNum >= _cardLimit)
        {
            GameDataManager.GetInstance().AddCard(_cardLimit - _cardNum);
            return true;
        }

        GameDataManager.GetInstance().AddCard(addCard);

        _countDownTime = _refreshTime - (float)subTime % _refreshTime;
        GameDataManager.GetInstance().SetLastCardTime((long)_countDownTime);
        return false;
    }
    
    
    private void OnTimeStart()
    {
        _timeFlag = true;
        StartCoroutine(nameof(CountdownCoroutine));
    }

    private void OnTimeEnd()
    {
        StopCoroutine(nameof(CountdownCoroutine));
        _timeFlag = false;
    }
    
    

    private void SendShowTime()
    {
        MessageCenterLogic.GetInstance().Send(CConfig.mg_ShowCardTime);
    }

    IEnumerator CountdownCoroutine()
    {
        while (_countDownTime > 0 && _timeFlag)
        {
            _timeFlag = true;
            _countDownTime--;
            SendShowTime();
            yield return new WaitForSeconds(1f);
            if (_countDownTime != 0) continue;
            if (AddCardByTime())
            {
                break;
            }
        }

        OnTimeEnd();
    }
    
}