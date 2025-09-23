// Project  BlockDropRush
// FileName  CollectBarCtrl.cs
// Author  AX
// Desc
// CreateAt  2025-09-11 17:09:10 
//


using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CollectBarCtrl : MonoBehaviour
{
    [FormerlySerializedAs("collectItems")]

    public List<GameObject> collectItems;

    [FormerlySerializedAs("flyObj")]


    public GameObject flyObj;

    private readonly string CollectsSeqStr = "CollectsSeq";

    [FormerlySerializedAs("fxObj")]


    public GameObject fxObj;

    private int _curCollectCount;

    
    private Vector3 _lastPos;
    
    private void Start()
    {
        ShowCollects();
        _lastPos = new Vector3(0, 10, 0);
    }


    public void ShowCollects()
    {
        _curCollectCount = CollectManager.Instance.GetCurCollectCount();
        for (int i = 0; i < collectItems.Count; i++)
        {
            collectItems[i].gameObject.SetActive(i < _curCollectCount);
        }
    }


    public void AddCollectAndPlayAct()
    {
        // List<Vector3> thisPos = new List<Vector3>();

        // List<Vector3> thisPos = LocalRewardData.CompleteData.CollectsPos;

        List<KeyValuePair<int, Vector3>> thisPos = LocalRewardData.CompleteData.CollectsPos;
        Dictionary<int, List<Vector3>> collectPos = new Dictionary<int, List<Vector3>>();
        List<int> keyList = new List<int>();

        foreach (var t in thisPos)
        {
            int idx = t.Key;
            Vector3 pos = t.Value;

            if (!keyList.Contains(idx))
            {
                collectPos[idx] = new List<Vector3>();
                keyList.Add(idx);
            }

            collectPos[idx].Add(pos);
        }

        Sequence s = GetFlyAnimSeq(keyList, collectPos);

        DOTween.Play(s);

        LocalRewardData.CompleteData.GoodsPos = new List<KeyValuePair<int, Vector3>>();
    }

    private void DoFx(Vector3 thisPos)
    {
        if (thisPos == _lastPos) return;
        _lastPos = thisPos;
        GameObject item = Instantiate(fxObj, fxObj.transform.parent, false);
        item.transform.localPosition = thisPos;
        item.transform.gameObject.SetActive(true);
        Destroy(item, 0.2f);
    }

    
    

    private Sequence GetFlyAnimSeq(List<int> keys, Dictionary<int, List<Vector3>> collectPosDic)
    {
        _curCollectCount = CollectManager.Instance.GetCurCollectCount();
        DOTween.Kill(CollectsSeqStr);
        Sequence s = DOTween.Sequence();

        float deTime = 0.1f;

        for (int i = 0; i < keys.Count; i++)
        {
            int thisKey = keys[i];
            int targetObjIdx = _curCollectCount + i;
            List<Vector3> collectPos = collectPosDic[thisKey];
            if (collectPos.Count > 1)
            {
                for (int j = 0; j < collectPos.Count; j++)
                {
                    GameObject obj = Instantiate(flyObj, flyObj.transform.parent);
                    obj.transform.localScale *= 0.7f;
                    obj.transform.position = collectPos[j];
                    obj.gameObject.SetActive(true);

                    s.Insert(deTime,
                        obj.transform.DOScale(flyObj.transform.localScale, 0.6f).SetEase(Ease.InCubic).SetDelay(0.1f));

                    float midX = obj.transform.position.x * 1.2f;
                    float midY = obj.transform.position.y +
                                 (flyObj.transform.position.y - obj.transform.position.y) / 3;

                    Vector3 midPoint = new Vector3(midX, midY, flyObj.transform.position.z);
                    Vector3 targetPos = collectItems[targetObjIdx].transform.position;
                    Vector3[] path = new Vector3[] { obj.transform.position, midPoint, targetPos };

                    s.Insert(deTime + 0.1f, obj.transform.DOPath(path, 0.8f).SetEase(Ease.InCubic).SetDelay(0.1f)
                        .OnComplete(() =>
                        {
                            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Word_fly);
                            collectItems[targetObjIdx].gameObject.SetActive(true);
                            DoFx(collectItems[targetObjIdx].transform.localPosition);
                            Destroy(obj);
                        }));
                }

                deTime += 0.1f;
            }
            else
            {
                GameObject obj = Instantiate(flyObj, flyObj.transform.parent);
                obj.transform.localScale *= 0.7f;
                obj.transform.position = collectPos[0];
                obj.gameObject.SetActive(true);

                s.Join(obj.transform.DOScale(flyObj.transform.localScale, 0.6f).SetEase(Ease.InCubic).SetDelay(0.1f));

                float midX = obj.transform.position.x * 1.2f;
                float midY = obj.transform.position.y + (flyObj.transform.position.y - obj.transform.position.y) / 3;

                Vector3 midPoint = new Vector3(midX, midY, flyObj.transform.position.z);

                Vector3 targetPos = collectItems[targetObjIdx].transform.position;
                Vector3[] path = new Vector3[] { obj.transform.position, midPoint, targetPos };

                s.Join(obj.transform.DOPath(path, 0.8f).SetEase(Ease.InCubic).SetDelay(0.1f).OnComplete(() =>
                {
                    MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Word_fly);
                    collectItems[targetObjIdx].gameObject.SetActive(true);
                    DoFx(collectItems[targetObjIdx].transform.localPosition);
                    Destroy(obj);
                }));
                // }
            }
        }

        s.OnComplete(() => { AfterFly(keys.Count); });
        s.SetId(CollectsSeqStr);
        return s;
    }


    
    
    private void AfterFly(int count)
    {
        CollectManager.Instance.AddCollectCount(count);
        ShowCollects();
        LocalRewardData.CompleteData.CollectsPos.Clear();
        SkinMagic.Instance.CheckShowCollectBonus();
    }


    public void DoScaleAct()
    {
        Vector3 startScale = collectItems[0].transform.localScale;
        
        foreach (var item in collectItems)
        {
            item.transform.DOScale(startScale * 1.2f, 0.3f)
                .SetEase(Ease.InOutQuad).OnComplete(() =>
                {
                    item.transform.DOScale(startScale, 0.3f);
                });
        }
        
    }

}