// Project  ScratchCard
// FileName  TicTacToeBoard.cs
// Author  AX
// Desc
// CreateAt  2025-04-16 15:04:20 
//


using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TicTacToeBoard : MonoBehaviour
{
    public GameObject itemArea;

    public GameObject chessItem;

    [SerializeField] private int rowLine = 3;

    [SerializeField] private int colLine = 3;

    private List<List<GameObject>> _itemList;

    public Image goodsImg;
    public Image rewardImg;
    public Text rewardText;

    public Sprite isOOnSprite;
    public Sprite isOOffSprite;
    public Sprite isXOffSprite;

    public GameObject layoutGroup;
    
    private BaseRewardItemData _thisReward;

    private List<GameObject> _rewardItemList;

    private List<List<int>> _rewardIdList;

    private int _rewardRow;

    private int _rewardCol;

    public void InitData(BaseRewardItemData reward)
    {
        _thisReward = reward;
        if (itemArea.transform.childCount == 0)
        {
            CreateItem();
        }

        SetItem(reward.RewardMulti);
        SetRewardSprite(reward);
    }


    public void DoLoopAnimInBoard()
    {
        foreach (var t in _rewardItemList)
        {
            t.GetComponent<BaseCardItem>().ShowLoopAct();
        }
    }

    public float DoWinAnimInBoard()
    {
        if (_rewardItemList.Count < 1) return 0f;

        _rewardItemList.Sort((a, b) =>
            a.GetComponent<BaseCardItem>().baseIdx.CompareTo(b.GetComponent<BaseCardItem>().baseIdx));

        float delayTime = 0f;
        float durTime = LocalCommonData.ItemDoFadeDuringTime;
        foreach (var t in _rewardItemList)
        {
            delayTime += LocalCommonData.ItemDoFadeDelayTime;
            t.GetComponent<BaseCardItem>().ShowTopImg(durTime, delayTime);
        }

        return delayTime + durTime;
    }


    private void CreateItem()
    {
        _itemList = new List<List<GameObject>>();
        int idx = 0;
        for (int row = 0; row < rowLine; row++)
        {
            List<GameObject> colList = new List<GameObject>();
            float yPos = 105f - row * 105f;
            for (int col = 0; col < colLine; col++)
            {
                GameObject item = Instantiate(chessItem, itemArea.transform, false);
                item.transform.localScale = itemArea.transform.localScale * 0.65f;
                item.transform.localPosition = new Vector3(-105f + 105f * col, yPos, 1);
                item.GetComponent<BaseCardItem>().baseIdx = idx;
                item.gameObject.SetActive(true);
                colList.Add(item);
                idx++;
            }

            _itemList.Add(colList);
        }
    }

    private List<int> GetRandomCol()
    {
        List<int> list = new List<int>();
        for (int row = 0; row < rowLine; row++)
        {
            list.Add(row);
        }

        return CardUtil.Shuffle(list);
    }

    private void SetThanks()
    {
        List<int> colIdx = GetRandomCol();
        List<List<int>> targetList = new List<List<int>>();
        foreach (int t in colIdx)
        {
            List<int> tempList = new List<int> { t };
            targetList.Add(tempList);
        }

        int lastRow = Random.Range(0, rowLine);
        int lastCol = Random.Range(0, colLine);
        while (targetList[lastRow].Contains(lastCol))
        {
            lastCol = Random.Range(0, colLine);
        }

        targetList[lastRow].Add(lastCol);


        List<List<int>> newList = new List<List<int>>();
        for (int i = 0; i < rowLine; i++)
        {
            List<int> tempList = new List<int>();

            for (int j = 0; j < colLine; j++)
            {
                if (!targetList[i].Contains(j))
                {
                    tempList.Add(j);
                }
            }

            newList.Add(tempList);
        }

        SetSprite(newList);
    }


    private List<List<int>> GetBaseDoubleReward(bool isDouble)
    {
        List<List<int>> list = new List<List<int>>();

        int rewardRow = Random.Range(0, rowLine);
        int rewardCol = Random.Range(0, colLine);
        _rewardRow = rewardRow;
        _rewardCol = rewardCol;
        for (int row = 0; row < rowLine; row++)
        {
            List<int> tempCol = new List<int>();
            for (int col = 0; col < colLine; col++)
            {
                if (col == rewardCol || row == rewardRow)
                {
                    tempCol.Add(col);
                }
            }

            _rewardIdList.Add(new List<int>(tempCol));
            list.Add(tempCol);
        }

        if (!isDouble)
        {
            bool isRow = Random.Range(0, 100) < 50;
            int unRewardCol = Random.Range(0, colLine);
            while (unRewardCol == rewardCol)
            {
                unRewardCol = Random.Range(0, colLine);
            }

            int unRewardRow = Random.Range(0, rowLine);
            while (unRewardRow == rewardRow)
            {
                unRewardRow = Random.Range(0, colLine);
            }

            if (isRow)
            {
                list[rewardRow].Remove(unRewardCol);
                list[unRewardRow].Add(unRewardCol);
                _rewardRow = -1;
            }
            else
            {
                list[unRewardRow].Remove(rewardCol);
                list[unRewardRow].Add(unRewardCol);
                _rewardCol = -1;
            }
        }

        return list;
    }


    private void SetDoubleReward()
    {
        List<List<int>> list = GetBaseDoubleReward(true);
        SetSprite(list);
    }


    private void SetOneReward()
    {
        List<List<int>> list = GetBaseDoubleReward(false);
        SetSprite(list);
    }


    private void SetSprite(List<List<int>> targetList)
    {
        for (int row = 0; row < rowLine; row++)
        {
            for (int col = 0; col < colLine; col++)
            {
                if (targetList[row].Contains(col))
                {
                    _itemList[row][col].GetComponent<BaseCardItem>()
                        .ShowItem(isOOffSprite, isOOnSprite);
                    if (row == _rewardRow || col == _rewardCol)
                    {
                        _rewardItemList.Add(_itemList[row][col]);
                    }
                }
                else
                {
                    _itemList[row][col].GetComponent<BaseCardItem>()
                        .ShowItem(isXOffSprite);
                }
            }
        }
    }

    
    private void RefreshObj()
    {
        if (layoutGroup == null) return;
        HorizontalLayoutGroup thisGroup = layoutGroup.GetComponent<HorizontalLayoutGroup>();
        if (thisGroup == null) return;

        thisGroup.CalculateLayoutInputHorizontal(); // 计算水平布局
        thisGroup.SetLayoutHorizontal(); // 应用水平布局
        // thisGroup.CalculateLayoutInputVertical();   // 计算垂直布局（如果需要）
        // thisGroup.SetLayoutVertical();

        ContentSizeFitter fitter = rewardText.gameObject.GetComponent<ContentSizeFitter>();
        if (fitter == null) return;
        fitter.SetLayoutHorizontal();
        // fitter.SetLayoutVertical();
    }
    

    private void SetRewardSprite(BaseRewardItemData rewardItemData)
    {
        if (rewardItemData.Type == CommonRewardType.Goods)
        {
            goodsImg.sprite = rewardItemData.RewardSprite;
        }
        else
        {
            rewardImg.sprite = rewardItemData.RewardSprite;
            rewardText.text = rewardItemData.Amount.ToString();
        }

        goodsImg.gameObject.SetActive(rewardItemData.Type == CommonRewardType.Goods);
        rewardImg.gameObject.SetActive(rewardItemData.Type != CommonRewardType.Goods);
        rewardText.gameObject.SetActive(rewardItemData.Type != CommonRewardType.Goods);
        RefreshObj();
    }

    private void SetItem(int rewardCount)
    {
        _rewardItemList = new List<GameObject>();
        _rewardIdList = new List<List<int>>();
        _rewardRow = -1;
        _rewardCol = -1;
        switch (rewardCount)
        {
            case 1:
                SetOneReward();
                break;
            case 2:
                SetDoubleReward();
                break;
            default:
                SetThanks();
                break;
        }
    }
}