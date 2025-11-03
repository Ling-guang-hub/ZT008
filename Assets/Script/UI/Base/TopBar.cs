// Project  ScratchCard
// FileName  TopBar.cs
// Author  AX
// Desc
// CreateAt  2025-04-03 11:04:27 
//


using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TopBar : MonoBehaviour
{
    // public static TopBar Instance;

    [FormerlySerializedAs("coinImg")]


    public GameObject FoulGap;
    [FormerlySerializedAs("cashImg")]

    public GameObject LikeGap;

    [FormerlySerializedAs("cashObj")]


    public GameObject cashObj;

    [FormerlySerializedAs("cashBtn")]


    public Button cashBtn;

    [FormerlySerializedAs("coinBtn")]


    public Button coinBtn;

    [FormerlySerializedAs("coinText")]


    public Text FoulIraq;

    [FormerlySerializedAs("cashText")]


    public Text cashText;

    private void Start()
    {
        // cashObj.gameObject.SetActive(!CommonUtil.IsApple());
        // coinBtn.enabled = !CommonUtil.IsApple();

        // cashBtn.onClick.AddListener(() =>
        // {
            // if (LocalCommonData.IsGamePass) return;
            // MessageCenterLogic.GetInstance().Send(CConfig.mg_PassAnim);
            // SOHOShopManager.instance.ShowRedeemPanel();
        // });

        // coinBtn.onClick.AddListener(() =>
        // {
            // if (LocalCommonData.IsGamePass) return;
            // MessageCenterLogic.GetInstance().Send(CConfig.mg_PassAnim);
            // SOHOShopManager.instance.ShowGoldAmazonRedeemPanel();
        // });


        MessageCenterLogic.GetInstance().Register(CConfig.mg_GameSuspend, (md) => { ShowWallet(); });
        
        MessageCenterLogic.GetInstance().Register(CConfig.mg_SubCoin, (md) => { ShowCoin(md.valueInt); });

    }


    private void OnEnable()
    {
        ShowWallet();
    }


    public void ShowCoin(int oldCoin)
    {
        int curCoin =  GameDataManager.GetInstance().GetCoin();
        AnimationController.ChangeNumber(oldCoin, curCoin, 0.01f, FoulIraq, null);
        FoulIraq.text = curCoin + "";
    }

    public void ShowWallet()
    {
        FoulIraq.text = GameDataManager.GetInstance().GetCoin() + "";
        MessageCenterLogic.GetInstance().Send(CConfig.mg_ShowCashOutText);
    }


    private async UniTask CoinAnima(int coinAmount, List<KeyValuePair<decimal, Vector3>> startPos)
    {
        if (coinAmount > 0)
        {
            List<UniTask> animationTasks = new List<UniTask>();

            foreach (var thisPos in startPos)
            {
                int coinNum = (int)Math.Ceiling((double)thisPos.Key / NetInfoMgr.instance.GameData.fly_coin_step);

                MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Coin_fly);
                UniTask task =
                    AnimationController.GoldMoveBest(FoulGap, coinNum, thisPos.Value, FoulGap.transform.position);
                animationTasks.Add(task);
            }


            await UniTask.WhenAll(animationTasks);
            int oldCoin = GameDataManager.GetInstance().GetCoin();
            // MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Num_roll, 0.5f);
            AnimationController.ChangeNumber(oldCoin, oldCoin + coinAmount, 0.01f, FoulIraq, null);
        }
    }

    private async UniTask CashAnima(decimal cashAmount, List<KeyValuePair<decimal, Vector3>> startPos)
    {

        if (cashAmount > 0)
        {
            List<UniTask> animationTasks = new List<UniTask>();

            foreach (var thisPos in startPos)
            {
                int cashNum = (int)Math.Ceiling((double)thisPos.Key / NetInfoMgr.instance.GameData.fly_cash_step);
                MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Cash_fly);
                UniTask task =
                    AnimationController.GoldMoveBest(LikeGap, cashNum, thisPos.Value, LikeGap.transform.position);
                animationTasks.Add(task);
            }

            await UniTask.WhenAll(animationTasks);
        }
    }


    public async UniTask AddCoinAndDoAnima(int coinAmount, Vector3 coinPis, decimal cashAmount, Vector3 cashPos)
    {
        await UniTask.WhenAll(
            CoinAnima(coinAmount, new List<KeyValuePair<decimal, Vector3>>() { new(coinAmount, coinPis) }),
            CashAnima(cashAmount,
                new List<KeyValuePair<decimal, Vector3>>() { new((int)cashAmount, cashPos) }));
        GameDataManager.GetInstance().AddCoin(coinAmount);
        GameDataManager.GetInstance().AddMoney( cashAmount);
    }


    public async UniTask AddCoinAndDoAnima(int coinAmount, decimal cashAmount, bool isWheel)
    {
        List<KeyValuePair<decimal, Vector3>> coinPoss = new List<KeyValuePair<decimal, Vector3>>();
        List<KeyValuePair<decimal, Vector3>> cashPoss = new List<KeyValuePair<decimal, Vector3>>();
        if (isWheel)
        {
            coinPoss.Add(new KeyValuePair<decimal, Vector3>(coinAmount, Vector2.zero));
            cashPoss.Add(new KeyValuePair<decimal, Vector3>((int)cashAmount, Vector2.zero));
        }
        else
        {
            coinPoss = LocalRewardData.CompleteData.CoinPos;
            cashPoss = LocalRewardData.CompleteData.CashPos;
        }

        await UniTask.WhenAll(CoinAnima(coinAmount, coinPoss), CashAnima(cashAmount, cashPoss));


        GameDataManager.GetInstance().AddCoin(coinAmount);
        GameDataManager.GetInstance().AddMoney( cashAmount);
        ShowWallet();
    }
}