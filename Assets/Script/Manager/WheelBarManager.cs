// Project  ScratchCard
// FileName  WheelBarManager.cs
// Author  AX
// Desc
// CreateAt  2025-06-23 15:06:16 
//


using System.Collections.Generic;
using UnityEngine;

public class WheelBarManager : MonoSingleton<WheelBarManager>
{
    private List<int> _wheelStep;

    private int _curValue;
    private int _curStep;

    private int _totalWheelCount;

    private Dictionary<int, int> _cardWheelValueMap;

    public void InitData()
    {
        _wheelStep = NetInfoMgr.instance.GameData.wheel_step;
        _cardWheelValueMap = LocalCardData.CardWheelValueDict;

        if (SaveDataManager.GetBool(CConfig.sv_WheelDataInit))
        {
            _curValue = SaveDataManager.GetInt(CConfig.sv_CurWheelValue);
            _curStep = SaveDataManager.GetInt(CConfig.sv_CurWheelStep);
            _totalWheelCount = SaveDataManager.GetInt(CConfig.sv_TotalWheelCount);
        }
        else
        {
            SaveDataManager.SetBool(CConfig.sv_WheelDataInit, true);

            _curStep = _wheelStep[0];
            _curValue = 0;
            _totalWheelCount = 0;

            SaveDataManager.SetInt(CConfig.sv_CurWheelValue, _curValue);
            SaveDataManager.SetInt(CConfig.sv_CurWheelStep, _curStep);
            SaveDataManager.SetInt(CConfig.sv_TotalWheelCount, _totalWheelCount);
        }
    }

    private void ShowWheel()
    {
    }

    public void AddWheelStep()
    {
        _curValue = 0;
        _totalWheelCount++;
        _curStep = _totalWheelCount > _wheelStep.Count - 1 ? _wheelStep[^1] : _wheelStep[_totalWheelCount];

        SaveDataManager.SetInt(CConfig.sv_CurWheelValue, 0);
        SaveDataManager.SetInt(CConfig.sv_CurWheelStep, _curStep);
        SaveDataManager.SetInt(CConfig.sv_TotalWheelCount, _totalWheelCount);
        MessageCenterLogic.GetInstance().Send(CConfig.mg_ShowWheelBar);
    }

    public void AddWheelValue()
    {
        int cardId = LocalCommonData.CurrentCardId;
        int wheelValue = _cardWheelValueMap[cardId];
        _curValue = SaveDataManager.GetInt(CConfig.sv_CurWheelValue) + wheelValue;

        SaveDataManager.SetInt(CConfig.sv_CurWheelValue, _curValue);

        MessageCenterLogic.GetInstance().Send(CConfig.mg_ShowWheelBar);
    }

    public float GetCurRate()
    {
        float rate = 1f * _curValue / _curStep;
        return rate > 1f ? 1f : rate;
    }
}