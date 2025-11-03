using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TaleFlock : MonoBehaviour
{
    [FormerlySerializedAs("InitGroup")]

    public GameObject LintFlock;

    private GameObject CreditorFaintGentle;
    private float ReadSmelt = 120f; // 两个item的position.x之差

    // Start is called before the first frame update
    void Start()
    {
        CreditorFaintGentle = LintFlock.transform.Find("SlotCard_1").gameObject;
        float x = ReadSmelt * 3;
        int multiCount = NetInfoMgr.instance.InitData.slot_group.Count;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < multiCount; j++)
            {
                GameObject fangkuai = Instantiate(CreditorFaintGentle, LintFlock.transform);
                fangkuai.transform.localPosition = new Vector3(x + ReadSmelt * multiCount * i + ReadSmelt * j, CreditorFaintGentle.transform.localPosition.y, 0);
                fangkuai.transform.Find("Text").GetComponent<Text>().text = "×" + NetInfoMgr.instance.InitData.slot_group[j].multi;
            }
        }
    }

    public void LumpFaint()
    {
        LintFlock.GetComponent<RectTransform>().localPosition = new Vector3(0, -10, 0);
    }

    public async UniTask<int> Wise(int index)
    {
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Sound_OneArmBandit);
        await AnimationController.HorizontalScroll(LintFlock, -(ReadSmelt * 2 + ReadSmelt * NetInfoMgr.instance.InitData.slot_group.Count * 3 + ReadSmelt * (index + 1)));
        return NetInfoMgr.instance.InitData.slot_group[index].multi;
    }
}
