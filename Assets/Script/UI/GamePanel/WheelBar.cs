// Project  ScratchCard
// FileName  WheelBar.cs
// Author  AX
// Desc
// CreateAt  2025-06-23 17:06:46 
//


using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WheelBar : MonoBehaviour
{
    [FormerlySerializedAs("smallWheelObj")]

    public GameObject smallWheelObj;

    [FormerlySerializedAs("btnObj")]


    public GameObject btnObj;

    // public Image bgImg;

    [FormerlySerializedAs("fillImg")]


    public Image fillImg;

    [FormerlySerializedAs("curRate")]


    public Text curRate;

    [FormerlySerializedAs("fxObj")]


    public GameObject fxObj;

    private float _currentValue;

    private bool _isFinishFlag;


    private void Start()
    {
        _isFinishFlag = false;
        fxObj.gameObject.SetActive(false);
        // finishSpineObj.SetActive(false);
        // _finishSkeleton = finishSpineObj.GetComponent<SkeletonGraphic>();
        // _finishSkeleton.AnimationState.Complete += VoleVerify;

        MessageCenterLogic.GetInstance().Register(CConfig.mg_ShowWheelBar, (md) => { DoSliderUIAct(); });
    }

    public void ShowSliderUI()
    {
        _currentValue = WheelBarManager.GetInstance().GetCurRate();
        fillImg.fillAmount = _currentValue;
        string str = new decimal(fillImg.fillAmount * 100).ToString("#");
        str = str.Length > 0 ? str : "0";
        curRate.text = str + "%";
    }

    public void DoSliderUIAct()
    {
        if (!gameObject.activeInHierarchy) return;

        if (WheelBarManager.GetInstance().GetCurRate() > _currentValue)
        {
            _currentValue = WheelBarManager.GetInstance().GetCurRate();
            StartCoroutine(nameof(fullSlider));
        }
        else
        {
            ShowSliderUI();
        }
    }

    IEnumerator fullSlider()
    {
        while (fillImg.fillAmount < _currentValue)
        {
            fillImg.fillAmount += 0.5f * Time.deltaTime;
            curRate.text = Mathf.FloorToInt(fillImg.fillAmount * 100) + "%";
            yield return null;
        }

        if (_currentValue >= 1)
        {
            fxObj.SetActive(true);
            Invoke(nameof(CloseFx),0.2f);
        }
    }

    private void CloseFx()
    {
        fxObj.SetActive(false);
    }

    private void DoSpineAimi()
    {
        // finishSpineObj.GetComponent<SkeletonGraphic>().AnimationState.ClearTrack(0);
        // finishSpineObj.SetActive(true);

        // finishSpineObj.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "animation", false);
    }


    public void ShowLuckPanel()
    {
        if (_isFinishFlag) return;

        _isFinishFlag = true;


        GameObject copyObj = Instantiate(smallWheelObj.gameObject, btnObj.transform, false);
        copyObj.transform.localPosition = smallWheelObj.transform.localPosition;
        copyObj.transform.localScale = smallWheelObj.transform.localScale * 0.5f;
        copyObj.gameObject.SetActive(true);
        // copyObj.transform.DOMove(new Vector3(0f, 0f, 0f), 1f).SetEase(Ease.Linear).OnComplete(() => { });
        copyObj.transform.DOMove(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.Linear).OnComplete(() => { });

        // copyObj.transform.DOScale(2.2f, 1.5f).SetEase(Ease.OutQuart);
        copyObj.transform.DOScale(2.2f, 1f).SetEase(Ease.OutQuart);

        // copyObj.transform.DOScale(2.2f, 0.3f).SetDelay(1.5f).OnComplete(() =>
        copyObj.transform.DOScale(2.2f, 0.1f).SetDelay(1f).OnComplete(() =>
        {
            // finishSpineObj.SetActive(false);
            copyObj.gameObject.SetActive(false);
            _isFinishFlag = false;
            MessageCenterLogic.GetInstance().Send(CConfig.mg_PassAnim);
            UIManager.GetInstance().ShowUIForms(nameof(TraceMagic));
            Destroy(copyObj);
        });
    }


    // private void VoleVerify(TrackEntry trackEntry)
    // {
    //     ShowLuckPanel();
    // }
}