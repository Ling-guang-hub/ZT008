// Project  ScratchCard
// FileName  MainBtnTimer.cs
// Author  AX
// Desc
// CreateAt  2025-06-12 20:06:50 
//


using System;
using System.Collections;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;

public class MainBtnTimer : MonoBehaviour
{
    [FormerlySerializedAs("handSpineObj")]

    public GameObject handSpineObj;

    private SkeletonGraphic _handSkeleton;

    private Coroutine _countdownCoroutine;

    private bool _isOnWork;

    private bool _isPass;

    private int _holdOnTime;

    private int _curTime;

    private void Awake()
    {
        handSpineObj.SetActive(false);
        _handSkeleton = handSpineObj.GetComponent<SkeletonGraphic>();
        _holdOnTime = 3;
    }


    private void DoSpine()
    {
        _handSkeleton.Initialize(true);
        _handSkeleton.AnimationState.SetEmptyAnimation(0, 0);
        handSpineObj.SetActive(true);
        _handSkeleton.AnimationState.SetAnimation(0, "animation", true);
    }


    public void StopSpine()
    {
        if (IsInvoking(nameof(DoSpine)))
        {
            CancelInvoke(nameof(DoSpine));
        }

        if (handSpineObj.gameObject.activeInHierarchy)
        {
            handSpineObj.SetActive(false);
        }
    }


    public void StartSpine()
    {
        if (IsInvoking(nameof(DoSpine)))
        {
            CancelInvoke(nameof(DoSpine));
        }

        Invoke(nameof(DoSpine), _holdOnTime);
    }


    // public void StartTime()
    // {
    //     if (_countdownCoroutine != null)
    //     {
    //         StopCoroutine("WaitForClick");
    //         _countdownCoroutine = null;
    //     }
    //
    //     _isOnWork = true;
    //     _curTime = _holdOnTime;
    //     _countdownCoroutine = StartCoroutine("WaitForClick");
    // }

    // public void PassTimer()
    // {
    //     _isPass = true;
    // }


    // private void CloseSpine()
    // {
    //     if (!handSpineObj.activeInHierarchy) return;
    //     handSpineObj.gameObject.SetActive(false);
    //     
    // }
    //
    //
    // private void OpenSpine()
    // {
    //     _curTime = _holdOnTime;
    //     if (handSpineObj.activeInHierarchy) return;
    //     _isPass = true;
    //     DoSpine();
    // }


    // IEnumerator WaitForClick()
    // {
    //     while (_isOnWork)
    //     {
    //         if (!_isPass)
    //         {
    //
    //             _curTime--;
    //             if (_curTime < 0)
    //             {
    //                 OpenSpine();
    //             }
    //         }
    //         yield return new WaitForSeconds(1);
    //     }
    // }
}