// Project  ScratchCard
// FileName  PassportItem.cs
// Author  AX
// Desc
// CreateAt  2025-04-18 16:04:35 
//


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PassportItem : MonoBehaviour
{
    [FormerlySerializedAs("finishObj")]

    public GameObject finishObj;

    [FormerlySerializedAs("leftIcon")]


    public Image leftIcon;

    [FormerlySerializedAs("leftMask")]


    public GameObject leftMask;

    [FormerlySerializedAs("leftFutureMask")]


    public GameObject leftFutureMask;


    [FormerlySerializedAs("rightIcon")]



    public Image rightIcon;

    [FormerlySerializedAs("rightMask")]


    public GameObject rightMask;

    [FormerlySerializedAs("rightFutureMask")]


    public GameObject rightFutureMask;

    [FormerlySerializedAs("firstItem")]


    public GameObject firstItem;

    [FormerlySerializedAs("firstSlider")]


    public Image firstSlider;

    [FormerlySerializedAs("lastItem")]


    public GameObject lastItem;

    [FormerlySerializedAs("fullItem")]


    public GameObject fullItem;

    [FormerlySerializedAs("fullSliderImg")]


    public Image fullSliderImg;


    [FormerlySerializedAs("cashSprite")]



    public Sprite cashSprite;

    [FormerlySerializedAs("coinSprite")]


    public Sprite coinSprite;

    [FormerlySerializedAs("levelNumText")]


    public Text levelNumText;

    [FormerlySerializedAs("rightNumText")]


    public Text rightNumText;

    [FormerlySerializedAs("leftNumText")]


    public Text leftNumText;

    [FormerlySerializedAs("leftGetBtn")]


    public Button leftGetBtn;
    [FormerlySerializedAs("rightGetBtn")]

    public Button rightGetBtn;

    // public GameObject leftIncompleteObj;
    // public GameObject rightIncompleteObj;

    [FormerlySerializedAs("adImg")]


    public GameObject adImg;


    private int _levelIdx;

    private PassportLevelData _levelData;

    private int _lastLevel;

    private int _finishedCardNum;


    private void Start()
    {
        leftGetBtn.onClick.AddListener(() =>
        {
            // if (EdgeBothMagic.Instance.topBar.gameObject.activeInHierarchy) return;
            if (EdgeBothMagic.Instance.CheckCoinFly()) return;
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            GetLeftReward();
        });

        rightGetBtn.onClick.AddListener(() =>
        {
            // if (EdgeBothMagic.Instance.topBar.gameObject.activeInHierarchy) return;
            if (EdgeBothMagic.Instance.CheckCoinFly()) return;
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            ADManager.Instance.playRewardVideo((success) =>
            {
                if (success)
                {
                    GetRightReward();
                }
            }, "101");
        });
    }

    public void PostDeed(int thisIdx, PassportLevelData levelData)
    {
        _levelIdx = thisIdx;
        _levelData = levelData;
        _lastLevel = LocalPassportData.LastLevel;
        _finishedCardNum = CardManager.Instance.GetFinishCardNum();
        InitReward();
        InitUI();
        SetSliderUI();
    }


    private void InitButton()
    {
        KeyValuePair<int, int> pair = GameDataManager.GetInstance().GetPassportIdxReward(_levelIdx);
        int leftNum = pair.Key;
        int rightNum = pair.Value;
        if (leftNum > 0)
        {
            leftGetBtn.gameObject.SetActive(false);
            leftFutureMask.SetActive(false);
            leftMask.SetActive(true);
        }

        if (rightNum > 0)
        {
            rightGetBtn.gameObject.SetActive(false);
            rightFutureMask.SetActive(false);
            rightMask.SetActive(true);
        }
    }

    private void InitReward()
    {
        levelNumText.text = _levelData.LeastCard.ToString();
        if (_levelData.Type == CommonRewardType.Cash)
        {
            leftNumText.text = "$ " + _levelData.RewardNum;
            leftIcon.sprite = cashSprite;
        }
        else
        {
            leftNumText.text = _levelData.RewardNum.ToString();
            leftIcon.sprite = coinSprite;
        }

        leftNumText.text = _levelData.RewardNum.ToString();
        rightNumText.text = " " + _levelData.CashCount;
    }

    private void InitUI()
    {
        if (_finishedCardNum >= _levelData.LeastCard)
        {
            finishObj.gameObject.SetActive(true);
            leftFutureMask.SetActive(false);
            rightFutureMask.SetActive(false);
            leftGetBtn.gameObject.SetActive(true);
            rightGetBtn.gameObject.SetActive(true);
        }
        else
        {
            leftGetBtn.gameObject.SetActive(false);
            rightGetBtn.gameObject.SetActive(false);
            leftFutureMask.gameObject.SetActive(true);
            rightFutureMask.gameObject.SetActive(true);
        }

        InitButton();
    }

    private void SetSliderUI()
    {
        // firstSlider.gameObject.SetActive(_finishedCardNum >= _levelData.LeastCard);
        firstItem.gameObject.SetActive(_levelIdx == 0);
        fullItem.gameObject.SetActive(_levelIdx != _lastLevel);
        lastItem.gameObject.SetActive(false);
        if (_levelIdx == 0)
        {
            if (_finishedCardNum > _levelData.LeastCard)
            {
                firstSlider.fillAmount = 1f;
                fullSliderImg.fillAmount = (float)(_finishedCardNum - _levelData.LeastCard) / _levelData.NextCard;
            }
            else
            {
                firstSlider.fillAmount = (float)_finishedCardNum / _levelData.LeastCard;
                fullSliderImg.fillAmount = 0f;
            }
        }
        else
        {
            if (_finishedCardNum <= _levelData.LeastCard)
            {
                fullSliderImg.fillAmount = 0f;
            }
            else
            {
                fullSliderImg.fillAmount = (float)(_finishedCardNum - _levelData.LeastCard) / _levelData.NextCard;
                // firstSlider.fillAmount = (float)_finishedCardNum/_levelData.LeastCard;
            }
        }
    }


    private void GetLeftReward()
    {
        if (_levelData.Type == CommonRewardType.Cash)
        {
            EdgeBothMagic.Instance.AddCoinAndCash(0, Vector2.zero, _levelData.RewardNum, leftIcon.transform.position);
        }
        else
        {
            EdgeBothMagic.Instance.AddCoinAndCash(_levelData.RewardNum, leftIcon.transform.position, 0, Vector2.zero);
        }

        GameDataManager.GetInstance().SetPassportLeftReward(_levelIdx, _levelData.RewardNum);
        leftGetBtn.gameObject.SetActive(false);
        leftMask.gameObject.SetActive(true);
        EdgeBothMagic.Instance.ResetMinLevel();
    }

    private void GetRightReward()
    {
        EdgeBothMagic.Instance.AddCoinAndCash(_levelData.CashCount, rightIcon.transform.position, 0, Vector2.zero);
        GameDataManager.GetInstance().SetPassportRightReward(_levelIdx, _levelData.CashCount);
        rightGetBtn.gameObject.SetActive(false);
        rightMask.gameObject.SetActive(true);
    }

    private bool CheckCard()
    {
        if (CardManager.Instance.GetFinishCardNum() < _levelData.LeastCard) return false;

        return true;
    }
}