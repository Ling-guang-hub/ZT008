// Project  BlockDropRush
// FileName  PanelManager.cs
// Author  AX
// Desc
// CreateAt  2025-09-12 10:09:14 
//


using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance;

    public Sprite cash01;
    public Sprite cash02;
    public Sprite cash03;
    public Sprite coin01;
    public Sprite coin02;
    public Sprite coin03;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void ShowCardStore()
    {
        MessageCenterLogic.GetInstance().Send(CConfig.mg_PassAnim);
        UIManager.GetInstance().ShowUIForms(nameof(StopBrawlScope));
    }


    public Sprite GetRewardSprite(string rewardName, decimal amount)
    {
        if (rewardName.Contains("Cash"))
        {
            if (amount < LocalCommonData.CashStep[0])
            {
                return cash01;
            }

            if (amount >= LocalCommonData.CashStep[0] && amount < LocalCommonData.CashStep[1])
            {
                return cash02;
            }

            return cash03;
        }

        if (amount < LocalCommonData.CoinStep[0])
        {
            return coin01;
        }

        if (amount >= LocalCommonData.CoinStep[0] && amount < LocalCommonData.CoinStep[1])
        {
            return coin02;
        }

        return coin03;
    }
}