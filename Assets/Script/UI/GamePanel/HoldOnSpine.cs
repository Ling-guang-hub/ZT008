// Project  ScratchCard
// FileName  HoldOnSpine.cs
// Author  AX
// Desc
// CreateAt  2025-05-14 16:05:49 
//


using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;

public class HoldOnSpine : MonoBehaviour
{
    [FormerlySerializedAs("handSpineObj")]

    public GameObject handSpineObj;

    private SkeletonGraphic _handSkeleton;


    private readonly float _timeoutDuration = 5f; // 无操作超时时间（秒）

    private int _lessTime;

    private bool _isOnWork;

    private Coroutine _countdownCoroutine;

    private bool _isPass;

    private float _curTime;

    private bool _actCard;

    void Start()
    {
        handSpineObj.gameObject.SetActive(false);
        _handSkeleton = handSpineObj.GetComponent<SkeletonGraphic>();
        _handSkeleton.AnimationState.Complete += VoleVerify;


        MessageCenterLogic.GetInstance().Register(CConfig.mg_PassAnim, (md) => { _isPass = true; });

        MessageCenterLogic.GetInstance().Register(CConfig.mg_ReStartAnim, (md) => { _isPass = false; });

        MessageCenterLogic.GetInstance().Register(CConfig.mg_ActCard, (md) => { _actCard = true; });

        MessageCenterLogic.GetInstance().Register(CConfig.mg_BadCard, (md) => { _actCard = false; });
    }


    void Update()
    {
    }


    private async void NewUserAct()
    {
        if (_countdownCoroutine != null)
        {
            StopCoroutine(nameof(NewUserWaitForClickRoutine));
        }

        await UniTask.Delay(1500);
        DoSpine(true);
        _countdownCoroutine = StartCoroutine(nameof(NewUserWaitForClickRoutine));
    }

    private void NormalAct()
    {
        if (_countdownCoroutine != null)
        {
            StopCoroutine(nameof(WaitForClickRoutine));
        }

        _isOnWork = true;
        _isPass = false;
        _actCard = true;
        _countdownCoroutine = StartCoroutine(nameof(WaitForClickRoutine));
    }


    public void StartTimer()
    {
        if (!SaveDataManager.GetBool(CConfig.sv_InitFirstCard))
        {
            NewUserAct();
        }
        else
        {
            NormalAct();
        }
    }

    public void PassTimer()
    {
        _isPass = true;
    }

    public void RestartTimer()
    {
        _isPass = false;
    }


    public void StopTimer()
    {
        _actCard = false;
        _isOnWork = false;
        if (_countdownCoroutine == null) return;
        StopCoroutine(nameof(WaitForClickRoutine));
        _countdownCoroutine = null;
        _isPass = true;
    }

    public void OnDisable()
    {
        StopTimer();
    }


    IEnumerator WaitForClickRoutine()
    {
        float timer = 0f;

        while (true)
        {
            if (_isOnWork && _actCard && !_isPass)
            {
                if (Input.GetMouseButtonDown(0) ||
                    (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    CloseSpine();
                    timer = 0f;
                }
                else
                {
                    timer += Time.deltaTime;
                    if (timer >= _timeoutDuration)
                    {
                        DoSpine(false);
                        timer = -5;
                    }
                }
            }

            yield return null;
        }
    }


    IEnumerator NewUserWaitForClickRoutine()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                CloseNewUserSpine();
                break;
            }

            yield return null;
        }
    }


    private void CloseNewUserSpine()
    {
        CloseSpine();
        if (_countdownCoroutine == null) return;
        StopCoroutine(nameof(NewUserWaitForClickRoutine));
        _countdownCoroutine = null;
    }


    private void CloseSpine()
    {
        if (handSpineObj.activeInHierarchy)
        {
            handSpineObj.gameObject.SetActive(false);
            _handSkeleton.AnimationState.ClearTrack(0);
        }
    }

    private void DoSpine(bool isLoop)
    {
        _handSkeleton.Initialize(true);
        _handSkeleton.AnimationState.ClearTrack(0);
        _handSkeleton.AnimationState.SetEmptyAnimation(0, 0);
        handSpineObj.gameObject.SetActive(true);
        _handSkeleton.AnimationState.SetAnimation(0, "animation", isLoop);
    }


    private void VoleVerify(TrackEntry trackEntry)
    {
        CloseSpine();
    }
}