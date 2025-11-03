// Project  ScratchCard
// FileName  BigWhellItem.cs
// Author  AX
// Desc
// CreateAt  2025-04-15 18:04:30 
//


using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BigWheelItem : MonoBehaviour
{
    public Text text;

    [FormerlySerializedAs("rewardImg")]


    public Image RevereGap;
    // public Image coinIcon;
    // public Image cardIcon;

    [FormerlySerializedAs("cash01")]


    public Sprite cash01;
    [FormerlySerializedAs("cash02")]

    public Sprite cash02;
    [FormerlySerializedAs("cash03")]

    public Sprite cash03;
    [FormerlySerializedAs("coin01")]

    public Sprite coin01;
    [FormerlySerializedAs("coin02")]

    public Sprite coin02;
    [FormerlySerializedAs("coin03")]

    public Sprite coin03;


    private WheelBigItemReward _wheelBigItemReward;


    public void InitIcon(WheelBigItemReward reward)
    {
        _wheelBigItemReward = reward;
        int amount = (int)Math.Ceiling(reward.Count);
        RevereGap.sprite = PanelManager.Instance.GetRewardSprite(reward.Type.ToString(), amount);
        // if (_wheelBigItemReward.Type == CommonRewardType.Cash)
        // {
        //     if (reward.Count < LocalCommonData.CashStep[0])
        //     {
        //         RevereGap.sprite = cash01;
        //     }
        //     else if (reward.Count >= LocalCommonData.CashStep[0] && reward.Count < LocalCommonData.CashStep[1])
        //     {
        //         RevereGap.sprite = cash02;
        //     }
        //     else
        //     {
        //         RevereGap.sprite = cash03;
        //     }
        // }
        // else
        // {
        //     
        //     if (reward.Count < LocalCommonData.CoinStep[0])
        //     {
        //         RevereGap.sprite = cash01;
        //     }
        //     else if (reward.Count >= LocalCommonData.CoinStep[0] && reward.Count < LocalCommonData.CoinStep[1])
        //     {
        //         RevereGap.sprite = cash02;
        //     }
        //     else
        //     {
        //         RevereGap.sprite = cash03;
        //     }
        //     
        // }

        text.text = amount + "";
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}