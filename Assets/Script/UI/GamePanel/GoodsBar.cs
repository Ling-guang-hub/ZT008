// Project  ScratchCard
// FileName  GoodsBar.cs
// Author  AX
// Desc
// CreateAt  2025-04-09 17:04:30 
//


using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GoodsBar : MonoBehaviour
{
    [FormerlySerializedAs("smallWheelObj")]

    public GameObject smallWheelObj;

    [FormerlySerializedAs("btnObj")]


    public GameObject btnObj;

    [FormerlySerializedAs("bgImg")]


    public Image bgImg;

    [FormerlySerializedAs("goods")]


    public List<GameObject> goods;

    [FormerlySerializedAs("finishSpineObj")]


    public GameObject finishSpineObj;


    private SkeletonGraphic _finishSkeleton;


    private List<int> _currentIdx;

    private bool _isFinishFlag;

    private void Start()
    {
        finishSpineObj.SetActive(false);
        _finishSkeleton = finishSpineObj.GetComponent<SkeletonGraphic>();
        _finishSkeleton.AnimationState.Complete += VoleVerify;
    }

    public void ShowGoods()
    {
        // _currentIdx = GameDataManager.GetInstance().GetGoods();
        for (int i = 0; i < goods.Count; i++)
        {
            goods[i].GetComponent<GoodsItem>().topImg.gameObject.SetActive(_currentIdx.Contains(i));
        }
    }


    public void AddGoodsAndPlayAnim()
    {
        // _currentIdx = GameDataManager.GetInstance().GetGoods();

        List<Vector3> thisPos = new List<Vector3>();
        List<int> idxs = new List<int>();

        List<KeyValuePair<int, Vector3>> tarPos = LocalRewardData.CompleteData.GoodsPos;

        foreach (var t in tarPos)
        {
            int thisIdx = t.Key;
            Vector3 pos = t.Value;

            thisPos.Add(pos);
            idxs.Add(thisIdx);
            if (!_currentIdx.Contains(thisIdx))
            {
                // GameDataManager.GetInstance().AddGoods(thisIdx);
            }
        }

        Sequence s = GetFlyAnimSeq(idxs, thisPos);

        DOTween.Play(s);


        LocalRewardData.CompleteData.GoodsPos = new List<KeyValuePair<int, Vector3>>();
    }


    private Sequence GetFlyAnimSeq(List<int> targetIdx, List<Vector3> thisPos)
    {
        DOTween.Kill("GoodsFlyAnim");
        Sequence s = DOTween.Sequence();
        for (int i = 0; i < targetIdx.Count; i++)
        {
            int idx = targetIdx[i];
            GameObject sourceObj = goods[idx];
            GameObject obj = Instantiate(sourceObj, sourceObj.transform.parent);
            obj.transform.localScale *= 0.7f;
            obj.transform.position = thisPos[i];
            obj.GetComponent<GoodsItem>().topImg.gameObject.SetActive(true);
            
            s.Join(obj.transform.DOScale(sourceObj.transform.localScale, 0.6f).SetEase(Ease.InCubic).SetDelay(0.1f));
            // s.Join(obj.transform.DOMove(sourceObj.transform.position, 0.6f).SetDelay(0.1f).OnComplete(() =>
            // {
            //     MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Word_fly);
            //     sourceObj.GetComponent<GoodsItem>().topImg.gameObject.SetActive(true);
            //     Destroy(obj);
            // }));
            // float midX = obj.transform.position.x > 0 ? obj.transform.position.x   ; 
            float midX = obj.transform.position.x * 1.2f;
            float midY = obj.transform.position.y + (sourceObj.transform.position.y - obj.transform.position.y) / 3;

            Vector3 midPoint = new Vector3(midX, midY, sourceObj.transform.position.z);

            Vector3[] path = new Vector3[] { obj.transform.position, midPoint, sourceObj.transform.position };

            s.Join(obj.transform.DOPath(path, 0.8f).SetEase(Ease.InCubic).SetDelay(0.1f).OnComplete(() =>
            {
                MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Word_fly);
                sourceObj.GetComponent<GoodsItem>().topImg.gameObject.SetActive(true);
                Destroy(obj);
            }));
        }

        s.OnComplete(AfterFlyGoods);
        s.SetId("GoodsFlyAnim");
        return s;
    }

    private void ClearGoods()
    {
        // GameDataManager.GetInstance().CleanGoods();
        ShowGoods();
    }

    private void DealGoods(bool isActive)
    {
        for (int i = 0; i < goods.Count; i++)
        {
            goods[i].gameObject.SetActive(isActive);
        }
    }


    private void DoSpineAimi()
    {
        finishSpineObj.GetComponent<SkeletonGraphic>().AnimationState.ClearTrack(0);
        finishSpineObj.SetActive(true);

        finishSpineObj.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "animation", false);
    }


    private void ShowLuckPanel()
    {
        GameObject copyObj = Instantiate(smallWheelObj.gameObject, btnObj.transform, false);
        copyObj.transform.localPosition = smallWheelObj.transform.localPosition;
        copyObj.transform.localScale = smallWheelObj.transform.localScale * 0.5f;
        copyObj.gameObject.SetActive(true);
        copyObj.transform.DOMove(new Vector3(0f, 0f, 0f), 1f).SetEase(Ease.Linear).OnComplete(() => { });

        copyObj.transform.DOScale(2.2f, 1.5f).SetEase(Ease.OutQuart);

        copyObj.transform.DOScale(2.2f, 0.3f).SetDelay(1.5f).OnComplete(() =>
        {
            finishSpineObj.SetActive(false);
            DealGoods(true);
            copyObj.gameObject.SetActive(false);

            MessageCenterLogic.GetInstance().Send(CConfig.mg_PassAnim);
            UIManager.GetInstance().ShowUIForms(nameof(TraceMagic));
            Destroy(copyObj);
        });
    }


    private void AfterFlyGoods()
    {
        // if (GameDataManager.GetInstance().GetGoods().Count >= goods.Count)
        // {
        //     ClearGoods();
        //     LocalRewardData.ResetCompleteData();
        //     MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Collect_word);
        //     DealGoods(false);
        //     DoSpineAimi();
        // }
        // else
        // {
        //     SkinMagic.Instance.AfterFlyGoods();
        // }
    }

    private void VoleVerify(TrackEntry trackEntry)
    {
        ShowLuckPanel();
    }
}