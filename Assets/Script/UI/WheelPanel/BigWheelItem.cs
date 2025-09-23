// Project  ScratchCard
// FileName  BigWhellItem.cs
// Author  AX
// Desc
// CreateAt  2025-04-15 18:04:30 
//


using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BigWheelItem : MonoBehaviour
{
    public Text text;
    [FormerlySerializedAs("cashIcon")]

    public Image cashIcon;
    [FormerlySerializedAs("coinIcon")]

    public Image coinIcon;
    [FormerlySerializedAs("cardIcon")]

    public Image cardIcon;

    private WheelBigItemReward _wheelBigItemReward;


    public void InitIcon(WheelBigItemReward reward)
    {
        _wheelBigItemReward = reward;

        cashIcon.gameObject.SetActive(false);
        coinIcon.gameObject.SetActive(false);
        cardIcon.gameObject.SetActive(false);
        switch (_wheelBigItemReward.Type)
        {
            case CommonRewardType.Cash:
                cashIcon.gameObject.SetActive(true);
                break;
            case CommonRewardType.Coin:
                coinIcon.gameObject.SetActive(true);
                break;
            default:
                cardIcon.gameObject.SetActive(true);
                break;
        }

        text.text = reward.Count.ToString();
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