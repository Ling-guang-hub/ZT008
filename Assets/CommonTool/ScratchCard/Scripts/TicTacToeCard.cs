// Project  ScratchCard
// FileName  SimpleCard.cs
// Author  AX
// Desc
// CreateAt  2025-03-31 18:03:49 
//


using System;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;


/**
 *  There are 3 “O”in a line
 */
public class TicTacToeCard : BaseCard
{
    public static TicTacToeCard Instance;

    public List<GameObject> boardList;

    private List<GameObject> _rewardBoardList;

    void Awake()
    {
        Instance = this;
        InitBaseData();
        cardType = CardType.TicTacToe;
        _rewardBoardList = new List<GameObject>();
    }


    // first  init  card data
    public override void InitCardData()
    {
        base.InitCardData();
        InitCardItems();
    }


    // TODO
    private void InitReward()
    {
    }

    private void SetRewardItem(BaseRewardItemData rewardData, GameObject boardObj)
    {
        if (rewardData.IsThanks) return;

        _rewardBoardList.Add(boardObj);
        LocalRewardData.ShowThankPanel = false;

        Vector3 rewardPos = boardObj.GetComponent<TicTacToeBoard>().goodsImg.transform.position;
        // boardList[i].GetComponent<TicTacToeBoard>().InitData(reward);

        switch (rewardData.Type)
        {
            case CommonRewardType.Coin:
                LocalRewardData.CompleteData.HasCoin = true;
                LocalRewardData.CompleteData.CoinAmount += (int)(rewardData.Amount * rewardData.RewardMulti);
                LocalRewardData.ShowRewardPanel = true;
                LocalRewardData.CompleteData.CoinPos.Add(new KeyValuePair<int, Vector3>(rewardData.Amount, rewardPos));
                break;
            case CommonRewardType.Cash:
                LocalRewardData.CompleteData.HasCash = true;
                LocalRewardData.CompleteData.CashAmount += (decimal)(rewardData.Amount * rewardData.RewardMulti);
                LocalRewardData.ShowRewardPanel = true;
                LocalRewardData.CompleteData.CashPos.Add(new KeyValuePair<int, Vector3>(rewardData.Amount, rewardPos));
                break;
            case CommonRewardType.Goods:
                // LocalRewardData.CompleteData.CollectsPos.Add(rewardPos);
                LocalRewardData.CompleteData.CollectsPos.Add(
                    new KeyValuePair<int, Vector3>(rewardData.GoodsIdx, rewardPos));
                break;
        }
    }


    private void SetBoardRewardList()
    {
        _rewardBoardList = new List<GameObject>();

        int randIdx = Random.Range(0, boardList.Count);

        for (int i = 0; i < boardList.Count; i++)
        {
            BaseRewardItemData reward = GetReward();
            if (i == randIdx && IsSpecialCard)
            {
                reward = GetEffectiveReward();
            }

            // DealSpecial(reward);
            SetRewardItem(reward, boardList[i]);
            boardList[i].GetComponent<TicTacToeBoard>().InitData(reward);
            // InitBoardList(reward, boardList[i]);
        }
    }


    public override void DoLoopAnim()
    {
        if (_rewardBoardList.Count == 0) return;

        for (int i = 0; i < _rewardBoardList.Count; i++)
        {
            _rewardBoardList[i].GetComponent<TicTacToeBoard>().DoLoopAnimInBoard();
        }
    }


    public override float DoWinAnim()
    {
        if (_rewardBoardList.Count == 0) return 0;

        float num = 0f;

        foreach (var t in _rewardBoardList)
        {
            float temp = t.GetComponent<TicTacToeBoard>().DoWinAnimInBoard();
            num = Math.Max(num, temp);
        }

        return num;
    }


    private void InitCardItems()
    {
        LocalRewardData.ResetCompleteData();

        DealSpecialCard();

        SetBoardRewardList();
    }
}