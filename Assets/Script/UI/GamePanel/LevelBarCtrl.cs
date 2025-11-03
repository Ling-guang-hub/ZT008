// Project  BlockDropRush
// FileName  LevelBarCtrl.cs
// Author  AX
// Desc
// CreateAt  2025-10-23 14:10:01 
//


using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelBarCtrl : MonoBehaviour
{
    [FormerlySerializedAs("testText")]

    public Text testText;

    [FormerlySerializedAs("levelText")]


    public Text levelText;

    [FormerlySerializedAs("sliderImg")]


    public Image sliderImg;

    private float _slderMin = 0.2f;

    private float _sliderMax = 0.5f;

    private float _sliderStep;

    private float _currentValue;

    private void Start()
    {
        ShowLevel();
        MessageCenterLogic.GetInstance().Register(CConfig.mg_ShowLevelBar, (md) => { DoSliderUIAct(); });
    }


    private float GetCurrentValue()
    {
        return 0.2f + CardManager.Instance.GetCurLevel().Value * (_sliderMax - _slderMin);
    }

    public void ShowLevel()
    {
        _currentValue = GetCurrentValue();
        // testText.text = CardManager.Instance.GetFinishCardNum() + "";
        testText.text = "LV.";
        levelText.text = CardManager.Instance.GetCurLevel().Key + "";
        sliderImg.fillAmount = _currentValue;
    }


    public void DoSliderUIAct()
    {
        if (!gameObject.activeInHierarchy) return;

        if (GetCurrentValue() > _currentValue)
        {
            _currentValue = GetCurrentValue();
            StartCoroutine(nameof(fullSlider));
        }
        else
        {
            ShowLevel();
        }
    }

    IEnumerator fullSlider()
    {
        while (sliderImg.fillAmount < _currentValue)
        {
            sliderImg.fillAmount += 0.5f * Time.deltaTime;
            yield return null;
        }

        ShowLevel();

    }
}