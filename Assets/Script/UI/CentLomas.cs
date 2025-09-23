using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CentLomas : MonoBehaviour
{
    [FormerlySerializedAs("InitGroup")]

    public GameObject PostLomas;

    private GameObject OnlookerRelicRarity;
    private float SectFully = 120f; // 两个item的position.x之差

    // Start is called before the first frame update
    void Start()
    {
        OnlookerRelicRarity = PostLomas.transform.Find("SlotCard_1").gameObject;
        float x = SectFully * 3;
        int multiCount = NetInfoMgr.instance.InitData.slot_group.Count;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < multiCount; j++)
            {
                GameObject fangkuai = Instantiate(OnlookerRelicRarity, PostLomas.transform);
                fangkuai.transform.localPosition = new Vector3(x + SectFully * multiCount * i + SectFully * j, OnlookerRelicRarity.transform.localPosition.y, 0);
                fangkuai.transform.Find("Text").GetComponent<Text>().text = "×" + NetInfoMgr.instance.InitData.slot_group[j].multi;
            }
        }
    }

    public void PainRelic()
    {
        PostLomas.GetComponent<RectTransform>().localPosition = new Vector3(0, -10, 0);
    }

    public async UniTask<int> Week(int index)
    {
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Sound_OneArmBandit);
        await AnimationController.HorizontalScroll(PostLomas, -(SectFully * 2 + SectFully * NetInfoMgr.instance.InitData.slot_group.Count * 3 + SectFully * (index + 1)));
        return NetInfoMgr.instance.InitData.slot_group[index].multi;
    }
}
