// Project  BlockDropRush
// FileName  FlyPopCtrl.cs
// Author  AX
// Desc
// CreateAt  2025-10-21 13:10:58 
//


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

public class FlyPopCtrl : MonoBehaviour
{
    [FormerlySerializedAs("icon")]

    public GameObject icon;
    public Text text;
    private Sequence _seq1;
    private Sequence _seq2;

    [FormerlySerializedAs("clickBtn")]


    public Button clickBtn;

    [FormerlySerializedAs("imageList")]


    public List<Sprite> imageList;

    [FormerlySerializedAs("Speen")]


    public float Speen;

    [FormerlySerializedAs("bgImg")]


    public Image bgImg;

    private decimal _thisBonus;

    void Start()
    {
        clickBtn.onClick.AddListener(ButtonAction);

        // MessageCenterLogic.GetInstance().Register(CConfig.mg_PassAnim, (md) =>
        // {
        // });

        // MessageCenterLogic.GetInstance().Register(CConfig.mg_ReStartAnim, (md) => { PopResume(); });

        Init();
    }


    private void OnDestroy()
    {
        transform.DOKill();
    }

    public void Init()
    {
        _thisBonus = decimal.Round(GameUtil.GetFlyBoxReward(), 2);
        text.text = _thisBonus + "";
        icon.SetActive(true);
        bgImg.gameObject.SetActive(false);
        FlyMoveAction();
    }

    public void PopPause()
    {
        transform.DOPause();
        clickBtn.enabled = false;
        _seq1.Pause();
        _seq2.Pause();
    }

    public void PopResume()
    {
        transform.DOPlay();
        clickBtn.enabled = true;
        _seq1.Play();
        _seq2.Play();
    }

    private void ButtonAction()
    {
        icon.SetActive(false);
        text.text = "";
        bgImg.gameObject.SetActive(true);
        StartPopAct();
        StartCoroutine(ShowCrashRewardPanel());
    }


    IEnumerator ShowCrashRewardPanel()
    {
        yield return new WaitForSeconds(0.4f);
        transform.DOKill();
        GetComponent<RectTransform>().DOKill();
        MessageCenterLogic.GetInstance().Send(CConfig.mg_PassAnim);
        WinPanelData thisData = new WinPanelData()
        {
            CashAmount = _thisBonus,
            PanelType = PanelType.FlyBox
        };
        UIManager.GetInstance().ShowUIForms(nameof(MooseAbuseScope), thisData);
        Destroy(gameObject);
    }

    public void FlyMoveAction()
    {
        _seq1 = DOTween.Sequence();
        _seq2 = DOTween.Sequence();

        int dict = Random.Range(0, 2) == 0 ? 1 : -1;
        transform.localPosition = new Vector3(-450f * dict, 0, 0);

        _seq1.Append(transform.DOLocalMoveY(150f + Random.Range(-50f, 50f), 2.5f).SetEase(Ease.InSine));
        _seq1.Append(transform.DOLocalMoveY(0, 2.5f).SetEase(Ease.InSine));
        _seq1.SetLoops(-1);
        _seq1.Play();

        _seq2.Append(transform.DOScale(1.1f, 0.5f).SetEase(Ease.Linear));
        _seq2.Append(transform.DOScale(1f, 0.5f).SetEase(Ease.Linear));
        _seq2.SetLoops(-1);
        _seq2.Play();
        transform.DOLocalMoveX(450f * dict, 10f).SetEase(Ease.Linear).OnComplete(() =>
        {
            _seq1.Kill();
            _seq2.Kill();
            transform.DOKill();
            GetComponent<RectTransform>().DOKill();
            Destroy(gameObject);
        });
    }


    IEnumerator PlayAction()
    {
        foreach (Sprite sprite in imageList)
        {
            bgImg.sprite = sprite;
            yield return new WaitForSeconds(Speen);
        }
        // StartCoroutine(nameof(PlayAction));
    }

    private void StartPopAct()
    {
        StartCoroutine(nameof(PlayAction));
    }

    private void ClosePopAct()
    {
        StopCoroutine(nameof(PlayAction));
    }

    // private void OnDisable()
    // {
    //     StopCoroutine(nameof(PlayAction));
    // }
}