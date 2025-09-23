﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AdjustTestPanel : BaseUIForms
{
    [FormerlySerializedAs("CloseButton")]

    public Button CloseButton;
    [FormerlySerializedAs("AdjustAdidText")]

    public Text AdjustAdidText;
    [FormerlySerializedAs("ServerIdText")]

    public Text ServerIdText;
    [FormerlySerializedAs("ActCounterText")]

    public Text ActCounterText;
    [FormerlySerializedAs("AdjustTypeText")]

    public Text AdjustTypeText;
    [FormerlySerializedAs("ResetActCountButton")]

    public Button ResetActCountButton;
    [FormerlySerializedAs("AddActCountButton")]

    public Button AddActCountButton;

    // Start is called before the first frame update
    void Start()
    {
        CloseButton.onClick.AddListener(() => {
            CloseUIForm(GetType().Name);
        });

        ResetActCountButton.onClick.AddListener(() => {
            AdjustInitManager.Instance.ResetActCount();
        });

        AddActCountButton.onClick.AddListener(() => {
            AdjustInitManager.Instance.AddActCount("test");
        });
    }

    private void ShowCounterText()
    {
        AdjustAdidText.text = AdjustInitManager.Instance.GetAdjustAdid();
        ServerIdText.text = SaveDataManager.GetString(CConfig.sv_LocalServerId);
        ActCounterText.text = AdjustInitManager.Instance._currentCount.ToString();
        AdjustTypeText.text = SaveDataManager.GetString("sv_ADJustInitType");
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        InvokeRepeating(nameof(ShowCounterText), 0, 0.5f);
    }

    public override void Hidding()
    {
        base.Hidding();
        CancelInvoke(nameof(ShowCounterText));
    }
}
