// Project  ScratchCard
// FileName  BigCardController.cs
// Author  AX
// Desc
// CreateAt  2025-05-21 16:05:01 
//


using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class BigCardController : MonoBehaviour
{
    [FormerlySerializedAs("baseBigCardObj")]

    public GameObject baseBigCardObj;

    [FormerlySerializedAs("mainCardArea")]


    public GameObject mainCardArea;

    private GameObject _middleMainObj;

    private GameObject _leftMainObj;

    private GameObject _rightMainObj;

    private float _bigCardRate;

    private static readonly float BigCardOffSet = 1000f;

    private void Awake()
    {
        _bigCardRate = LocalCommonData.ScreenRate > 0.5f ? 1f : 1.15f;
    }

    public void CreateFirstMainCard()
    {
        int leftCardId = GetNextCardId();
        _leftMainObj = Instantiate(baseBigCardObj, mainCardArea.transform, false);
        _leftMainObj.transform.localScale = mainCardArea.transform.localScale * _bigCardRate;
        _leftMainObj.transform.localPosition = new Vector3(-BigCardOffSet, 0, 1);
        _leftMainObj.transform.GetComponent<BigCard>().cardId = leftCardId;
        _leftMainObj.transform.GetComponent<BigCard>().cardType = LocalCardData.CardTypeDict[leftCardId];
        _leftMainObj.transform.GetComponent<BigCard>().PostDeed();
        _leftMainObj.gameObject.SetActive(true);


        _middleMainObj = Instantiate(baseBigCardObj, mainCardArea.transform, false);
        _middleMainObj.transform.localScale = mainCardArea.transform.localScale * _bigCardRate;
        _middleMainObj.transform.localPosition = new Vector3(0, 0, 1);
        _middleMainObj.transform.GetComponent<BigCard>().cardId = LocalCommonData.CurrentCardId;
        _middleMainObj.transform.GetComponent<BigCard>().cardType =
            LocalCardData.CardTypeDict[LocalCommonData.CurrentCardId];
        _middleMainObj.transform.GetComponent<BigCard>().PostDeed();
        _middleMainObj.gameObject.SetActive(true);


        int rightId = GetNextCardId(false);
        _rightMainObj = Instantiate(baseBigCardObj, mainCardArea.transform, false);
        _rightMainObj.transform.localScale = mainCardArea.transform.localScale * _bigCardRate;
        _rightMainObj.transform.localPosition = new Vector3(BigCardOffSet, 0, 1);
        _rightMainObj.transform.GetComponent<BigCard>().cardId = rightId;
        _rightMainObj.transform.GetComponent<BigCard>().cardType = LocalCardData.CardTypeDict[rightId];
        _rightMainObj.transform.GetComponent<BigCard>().PostDeed();
        _rightMainObj.gameObject.SetActive(true);

        DoScaleAct();
    }


    public async void NextMainCard(bool isLeft)
    {
        // left is -1  ; right  is 1
        int direct = isLeft ? -1 : 1;

        GameObject newMainCard = Instantiate(baseBigCardObj, mainCardArea.transform, false);
        newMainCard.transform.localScale = mainCardArea.transform.localScale * _bigCardRate;
        newMainCard.transform.localPosition = new Vector3(direct * BigCardOffSet, 0, 1);

        int nextId = GetNextCardId(isLeft);

        newMainCard.transform.GetComponent<BigCard>().cardId = nextId;
        newMainCard.transform.GetComponent<BigCard>().cardType = LocalCardData.CardTypeDict[nextId];
        newMainCard.transform.GetComponent<BigCard>().PostDeed();

        newMainCard.gameObject.SetActive(true);

        List<UniTask> animationTasks = new List<UniTask>();

        var middleTask = _middleMainObj.transform.DOLocalMoveX(-direct * BigCardOffSet, 0.2f).SetEase(Ease.Linear)
            .ToUniTask();
        animationTasks.Add(middleTask);

        if (isLeft)
        {
            var leftTask = _leftMainObj.transform
                .DOLocalMoveX(-BigCardOffSet - direct * BigCardOffSet, 0.2f)
                .SetEase(Ease.Linear).ToUniTask();
            animationTasks.Add(leftTask);
        }
        else
        {
            var rightTask = _rightMainObj.transform
                .DOLocalMoveX(BigCardOffSet - direct * BigCardOffSet, 0.2f)
                .SetEase(Ease.Linear).OnComplete(() => { }).ToUniTask();
            animationTasks.Add(rightTask);
        }

        await UniTask.WhenAll(animationTasks);

        if (isLeft)
        {
            _rightMainObj.gameObject.SetActive(false);
            Destroy(_rightMainObj);
            _rightMainObj = _middleMainObj;
            _middleMainObj = _leftMainObj;
            _leftMainObj = newMainCard;
        }
        else
        {
            _leftMainObj.gameObject.SetActive(false);
            Destroy(_leftMainObj);
            _leftMainObj = _middleMainObj;
            _middleMainObj = _rightMainObj;
            _rightMainObj = newMainCard;
        }

        SetBigFlag();
    }

    public void ResetBigCardList()
    {
        int leftCardId = GetNextCardId();

        _leftMainObj.transform.GetComponent<BigCard>().cardId = leftCardId;
        _leftMainObj.transform.GetComponent<BigCard>().cardType = LocalCardData.CardTypeDict[leftCardId];
        _leftMainObj.transform.GetComponent<BigCard>().PostDeed();

        _middleMainObj.transform.GetComponent<BigCard>().cardId = LocalCommonData.CurrentCardId;
        _middleMainObj.transform.GetComponent<BigCard>().cardType =
            LocalCardData.CardTypeDict[LocalCommonData.CurrentCardId];
        _middleMainObj.transform.GetComponent<BigCard>().PostDeed();

        int rightId = GetNextCardId(false);
        _rightMainObj.transform.GetComponent<BigCard>().cardId = rightId;
        _rightMainObj.transform.GetComponent<BigCard>().cardType = LocalCardData.CardTypeDict[rightId];
        _rightMainObj.transform.GetComponent<BigCard>().PostDeed();
    }

    public void CardsDoMove(float offsetX)
    {
        DOTween.Kill(_leftMainObj.transform);
        DOTween.Kill(_middleMainObj.transform);
        DOTween.Kill(_rightMainObj.transform);
        
        _leftMainObj.transform.position = new Vector3(_leftMainObj.transform.position.x + offsetX,
            _leftMainObj.transform.position.y, _leftMainObj.transform.position.z);
        _middleMainObj.transform.position = new Vector3(_middleMainObj.transform.position.x + offsetX,
            _middleMainObj.transform.position.y, _middleMainObj.transform.position.z);
        _rightMainObj.transform.position = new Vector3(_rightMainObj.transform.position.x + offsetX,
            _rightMainObj.transform.position.y, _rightMainObj.transform.position.z);
    }

    public async void CardsResetPos()
    {

        var leftTask = _leftMainObj.transform
            .DOLocalMoveX(-BigCardOffSet, 0.1f)
            .SetEase(Ease.Linear).ToUniTask();

        var middleTask = _middleMainObj.transform.DOLocalMoveX(0, 0.1f).SetEase(Ease.Linear)
            .ToUniTask();

        var rightTask = _rightMainObj.transform
            .DOLocalMoveX(BigCardOffSet, 0.1f)
            .SetEase(Ease.Linear).OnComplete(() => { }).ToUniTask();

        await UniTask.WhenAll(leftTask, middleTask, rightTask);

        SetBigFlag();
    }


    private void DoScaleAct()
    {
        DOTween.Kill(_leftMainObj.transform);
        DOTween.Kill(_middleMainObj.transform);
        DOTween.Kill(_rightMainObj.transform);
        
        _middleMainObj.transform.DOScale(_bigCardRate-0.05f, 1.2f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);
        
    }


    private void SetBigFlag()
    {
        _leftMainObj.GetComponent<BigCard>().clickFlag = true;
        _middleMainObj.GetComponent<BigCard>().clickFlag = true;
        _rightMainObj.GetComponent<BigCard>().clickFlag = true;
        _leftMainObj.transform.localScale = mainCardArea.transform.localScale * _bigCardRate;
        _middleMainObj.transform.localScale = mainCardArea.transform.localScale * _bigCardRate;
        _rightMainObj.transform.localScale = mainCardArea.transform.localScale * _bigCardRate;
        DoScaleAct();
    }


    private int GetNextCardId(bool isLeft = true)
    {
        int nextIdx = TameMagic.Instance.GetNextCardIdx(isLeft);
        int nextId = LocalCardData.ActCardIds[nextIdx];
        return nextId;
    }
}