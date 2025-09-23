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


    public GameObject GenuBog;
    [FormerlySerializedAs("cashImg")]

    public GameObject PulpBog;

    [FormerlySerializedAs("cashObj")]


    public GameObject cashObj;

    [FormerlySerializedAs("cashBtn")]


    public Button cashBtn;

    [FormerlySerializedAs("coinBtn")]


    public Button coinBtn;

    [FormerlySerializedAs("coinText")]


    public Text GenuBent;

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


        MessageCenterLogic.GetInstance().Register(CConfig.mg_GameSuspend, (md) =>
        {
            ShowWallet();
        });

    }


    private void OnEnable()
    {
        ShowWallet();
    }

    public void ShowWallet()
    {
        MessageCenterLogic.GetInstance().Send(CConfig.mg_ShowCashOutText);
        // GenuBent.text = GameDataManager.GetInstance().GetCoin().ToString(CultureInfo.CurrentCulture);
        // cashText.text = GameDataManager.GetInstance().GetCash().ToString(CultureInfo.CurrentCulture);
    }


    private async UniTask CoinAnima(int coinAmount, List<KeyValuePair<int, Vector3>> startPos)
    {
        if (coinAmount > 0)
        {
            List<UniTask> animationTasks = new List<UniTask>();

            foreach (var thisPos in startPos)
            {
                int coinNum = (int)Math.Ceiling((double)thisPos.Key / NetInfoMgr.instance.GameData.fly_coin_step);

                MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Coin_fly);
                UniTask task =
                    AnimationController.GoldMoveBest(GenuBog, coinNum, thisPos.Value, GenuBog.transform.position);
                animationTasks.Add(task);
            }


            await UniTask.WhenAll(animationTasks);
            int oldCoin = GameDataManager.GetInstance().GetCoin();
            // MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Num_roll, 0.5f);
            AnimationController.ChangeNumber(oldCoin, oldCoin + coinAmount, 0.01f, GenuBent, null);
        }
    }

    private async UniTask CashAnima(decimal cashAmount, List<KeyValuePair<int, Vector3>> startPos)
    {
        if (cashAmount > 0)
        {
            List<UniTask> animationTasks = new List<UniTask>();

            foreach (var thisPos in startPos)
            {
                int cashNum = (int)Math.Ceiling((double)thisPos.Key / NetInfoMgr.instance.GameData.fly_cash_step);
                MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Cash_fly);
                UniTask task =
                    AnimationController.GoldMoveBest(PulpBog, cashNum, thisPos.Value, PulpBog.transform.position);
                animationTasks.Add(task);
            }

            await UniTask.WhenAll(animationTasks);
        }
    }


    public async UniTask AddCoinAndDoAnima(int coinAmount, Vector3 coinPis, decimal cashAmount, Vector3 cashPos)
    {
        await UniTask.WhenAll(
            CoinAnima(coinAmount, new List<KeyValuePair<int, Vector3>>() { new(coinAmount, coinPis) }),
            CashAnima(cashAmount,
                new List<KeyValuePair<int, Vector3>>() { new((int)cashAmount, cashPos) }));
        GameDataManager.GetInstance().AddCoin(coinAmount);

    }


    public async UniTask AddCoinAndDoAnima(int coinAmount, decimal cashAmount, bool isWheel)
    {
        List<KeyValuePair<int, Vector3>> coinPoss = new List<KeyValuePair<int, Vector3>>();
        List<KeyValuePair<int, Vector3>> cashPoss = new List<KeyValuePair<int, Vector3>>();
        if (isWheel)
        {
            coinPoss.Add(new KeyValuePair<int, Vector3>(coinAmount, Vector2.zero));
            cashPoss.Add(new KeyValuePair<int, Vector3>((int)cashAmount, Vector2.zero));
        }
        else
        {
            coinPoss = LocalRewardData.CompleteData.CoinPos;
            cashPoss = LocalRewardData.CompleteData.CashPos;
        }

        await UniTask.WhenAll(CoinAnima(coinAmount, coinPoss), CashAnima(cashAmount, cashPoss));

        GameDataManager.GetInstance().AddCoin(coinAmount);
        ShowWallet();
    }
    
    
    
    
}