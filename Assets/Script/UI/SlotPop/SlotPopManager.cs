// Project  BlockDropRush
// FileName  SlotPopManager.cs
// Author  AX
// Desc
// CreateAt  2025-10-21 17:10:01 
//


using System.Collections;
using Unity.VisualScripting;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class SlotPopManager: MonoSingleton<SlotPopManager>
{
    
    [FormerlySerializedAs("slotGroup01")]

    
    public GameObject slotGroup01;
    [FormerlySerializedAs("slotGroup02")]

    public GameObject slotGroup02;
    [FormerlySerializedAs("slotGroup03")]

    public GameObject slotGroup03;


    private bool musicFlag;
    private bool isPass;

    [FormerlySerializedAs("SlotSwich")]


    public bool SlotSwich;

    // private Sequence slotSeq;
    
    public void RefreshSlotGroup()
    {
        slotGroup01.transform.localPosition = new Vector3(-125f, 0, 0);
        slotGroup02.transform.localPosition = new Vector3(0, 0, 0);
        slotGroup03.transform.localPosition = new Vector3(125f, 0, 0);

        // slotGroup01.GetComponent<SlotObjGroupController>().RefreshData();
        // slotGroup02.GetComponent<SlotObjGroupController>().RefreshData();
        // slotGroup03.GetComponent<SlotObjGroupController>().RefreshData();
    }
    
    
    public void SlotStop()
    {
        isPass = true;
        slotGroup01.transform.DOPause();
        slotGroup02.transform.DOPause();
        slotGroup03.transform.DOPause();
    }

    public void SlotReStart()
    {
        isPass = false;
        slotGroup01.transform.DOPlay();
        slotGroup02.transform.DOPlay();
        slotGroup03.transform.DOPlay();
    }
    
        public void StartSlot()
    {
        // slotObjData = GameUtil.GetSlotObjData();
        // if (SaveDataManager.GetString(CConfig.sv_first_bing_win_777) == "new")
        // {
            // slotObjData.SlotObjType = SlotObjType.BigWin;
            // SaveDataManager.SetString(CConfig.sv_first_bing_win_777, "done");
        // }

        // if (slotObjData.SlotObjType == SlotObjType.Thanks)
        // {
            // SlotObjData slotObjData1 = GameUtil.GetSlotObjDataWithOutThanks();
            // SlotObjData slotObjData2 = GameUtil.GetSlotObjDataWithOutThanks();
            // SlotObjData slotObjData3 = GameUtil.GetSlotObjDataWithOutThanks();
            // while (slotObjData1.SlotObjType == slotObjData2.SlotObjType)
            // {
                // slotObjData2 = GameUtil.GetSlotObjDataWithOutThanks();
            // }

            // slotGroup01.GetComponent<SlotObjGroupController>().CreateRewardObj(slotObjData1);
            // slotGroup02.GetComponent<SlotObjGroupController>().CreateRewardObj(slotObjData2);
            // slotGroup03.GetComponent<SlotObjGroupController>().CreateRewardObj(slotObjData3);
        // }
        // else
        {
            // slotGroup01.GetComponent<SlotObjGroupController>().CreateRewardObj(slotObjData);
            // slotGroup02.GetComponent<SlotObjGroupController>().CreateRewardObj(slotObjData);
            // slotGroup03.GetComponent<SlotObjGroupController>().CreateRewardObj(slotObjData);
        }
    }

    private void MoveAct()
    {
        musicFlag = true;
        StartCoroutine(nameof(PlaySlotMusic));
        slotGroup01.transform.DOLocalMoveY(-80f * 28, 2f).OnComplete(() =>
        {
            // MusicMgr.GetInstance().PlayEffect(MusicType.SceneMusic.sound_slotwheel_stop);
        });
        slotGroup02.transform.DOLocalMoveY(-80f * 28, 2f).SetDelay(0.3f).OnComplete(() =>
        {
            // MusicMgr.GetInstance().PlayEffect(MusicType.SceneMusic.sound_slotwheel_stop);
        });
        slotGroup03.transform.DOLocalMoveY(-80f * 28, 2f).SetDelay(0.6f).OnComplete(() =>
        {
            // MusicMgr.GetInstance().PlayEffect(MusicType.SceneMusic.sound_slotwheel_stop);
            musicFlag = false;
            SlotSwich = false;
            StopCoroutine(nameof(PlaySlotMusic));
            Invoke(nameof(GetSlotReward), 0.2f);
        });
    }
    
    private void GetSlotReward()
    {
        // int num = (int) slotObjData.RewardNum;
        // if (slotObjData.SlotObjType != SlotObjType.Thanks)
        // {
        //     MusicMgr.GetInstance().PlayEffect(MusicType.SceneMusic.sound_slot_reward);
        //     HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
        // }
        //
        // PostEventScript.GetInstance().SendEvent("1003", slotObjData.SlotObjType.ToString());
        // switch (slotObjData.SlotObjType)
        // {
        //     case SlotObjType.Ball:
        //         AnimationController.SlotNormal();
        //         SteelBallManager.Instance.AutoDropBall(num);
        //         break;
        //     case SlotObjType.Bomb:
        //         AnimationController.SlotNormal();
        //         PillarManager.Instance.CreateSlotRewardByType(num, PillarObjType.Bomb);
        //         break;
        //     case SlotObjType.Cash:
        //         AnimationController.SlotNormal();
        //         PillarManager.Instance.CreateSlotRewardByType(num, PillarObjType.Cash);
        //         break;
        //     case SlotObjType.Division:
        //         AnimationController.SlotNormal();
        //         PillarManager.Instance.CreateSlotRewardByType(num, PillarObjType.Division);
        //         break;
        //     case SlotObjType.BigWin:
        //         MusicMgr.GetInstance().PlayEffect(MusicType.SceneMusic.sound_slot_777);
        //         AnimationController.Slot777();
        //         Invoke(nameof(BigWin777GetReward), 2.7f);
        //         break;
        //     default:
        //         Invoke(nameof(ShowThanksPanel), 0.5f);
        //         break;
        // }
        //
        // RefreshSlotGroup();
        //
        // if (slotObjData.SlotObjType != SlotObjType.BigWin)
        // {
        //     RefreshBucket();
        // }
    }
    
    IEnumerator PlaySlotMusic()
    {
        while (musicFlag)
        {
            // if (!isPass)
            // {
            //     MusicMgr.GetInstance().PlayEffect(MusicType.SceneMusic.sound_slotwheel_rotate, 0.1f);
            // }

            yield return new WaitForSeconds(0.1f);
        }
    }
    
}
