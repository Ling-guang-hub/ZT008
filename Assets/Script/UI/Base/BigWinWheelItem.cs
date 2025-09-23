// Project  ScratchCard
// FileName  BigWinWheelItem.cs
// Author  AX
// Desc
// CreateAt  2025-06-10 17:06:48 
//


using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;


public class BigWinWheelItem : MonoBehaviour
{
    [FormerlySerializedAs("wheelObj")]

    public GameObject wheelObj;

    [FormerlySerializedAs("adImg")]


    public GameObject adImg;

    [FormerlySerializedAs("getBtn")]


    public Button getBtn;

    [FormerlySerializedAs("getText")]


    public Text getText;

    [FormerlySerializedAs("wheelItems")]


    public List<GameObject> wheelItems;

    [FormerlySerializedAs("thisMulti")]


    public float thisMulti;

    [FormerlySerializedAs("numSpriteList")]


    public List<Sprite> numSpriteList;

    private List<int> _numIdxList;

    private List<float> _multiList;

    private int _curObjIdx;

    private Sequence _thisActSeq;

    private void Awake()
    {
        _multiList = new List<float>() { 1.5f, 2, 3, 4, 5 };
        _thisActSeq = DOTween.Sequence();
        _curObjIdx = 0;
        PostDeed();

    }


    public void StopAct(bool  isGetBtn = true)
    {
        if (_thisActSeq != null)
        {
            _thisActSeq.Pause();
        }

        if (!isGetBtn)
        {
            ShowLightImg();
        }
    }
    
    

    public void PostDeed()
    {
        wheelObj.gameObject.transform.rotation = Quaternion.Euler(0, 0, 22.5f);
        adImg.gameObject.SetActive(SaveDataManager.GetBool(CConfig.sv_FinishFirstBigWin));
        _numIdxList = !SaveDataManager.GetBool(CConfig.sv_FinishFirstBigWin)
            ? new List<int>() { 4, 4, 4, 4, 4, 4, 4, 4 }
            : new List<int>() { 2, 1, 0, 1, 3, 1, 0, 1 };
        
        thisMulti = _multiList[0];
        getText.text = "Get X " + thisMulti;
        
        for (int i = 0; i < wheelItems.Count; i++)
        {
            wheelItems[i].GetComponent<BigWinMultiItem>().bgImg.GetComponent<Image>().sprite =
                numSpriteList[_numIdxList[i]];
            wheelItems[i].GetComponent<BigWinMultiItem>().lightBg.gameObject.SetActive(false);
        }

        ItLog();
    }
    

    private void ItLog()
    {
        _thisActSeq = GetRotateAnimSeq();
        _thisActSeq.Play();
    }

    private void ShowLightImg()
    {
        wheelItems[_curObjIdx].gameObject.GetComponent<BigWinMultiItem>().lightBg.gameObject.SetActive(true);
    }


    private Sequence GetRotateAnimSeq()
    {
        DOTween.Kill("DoRotateAnim");
        Sequence s = DOTween.Sequence();
        for (int i = 0; i < _numIdxList.Count; i++)
        {
            int thisIdx = i;
            s.Append(
                wheelObj.transform.DOLocalRotate(new Vector3(0, 0, -45f * i - 22.5f), 0.5f).SetEase(Ease.Linear)
                    .OnComplete(
                        () => { ResetText(thisIdx); })
            );
        }

        s.SetId("DoRotateAnim");
        s.SetLoops(-1);
        return s;
    }

    private void ResetText(int idx)
    {
        idx++;
        if (idx >= _numIdxList.Count)
        {
            idx = 0;
        }

        _curObjIdx = idx;
        int thisIdx = _numIdxList[idx];

        thisMulti = _multiList[thisIdx];
        getText.text = "Get X " + thisMulti;
    }
}