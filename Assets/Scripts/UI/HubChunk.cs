using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������Ϸҳ��
/// </summary>
public class HubChunk : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("scoreTxt")]    public Text OliveAid;   //��ʾ����
[UnityEngine.Serialization.FormerlySerializedAs("bestScoreTxt")]    public Text ShinBirchAid;   //��ʾ��߷�
[UnityEngine.Serialization.FormerlySerializedAs("rewardCountTxt")]    public Text WeakenJointAid; //��ʾ��������
[UnityEngine.Serialization.FormerlySerializedAs("GetMoreBtn")]    public Button AgeAridBus;   //������ø��ཱ����ť
[UnityEngine.Serialization.FormerlySerializedAs("ReplayBtn")]    public Button ReplayBus;    //���¿�ʼ��Ϸ��ť

    private int WeakenJoint= 50;

    private void OnEnable()
    {
        CrowBirchLeoMobileJoint(WeakenJoint);
    }

    private void Start()
    {
        AgeAridBus.onClick.AddListener(AgeAridMobile);
        ReplayBus.onClick.AddListener(LegendAero);
    }

    /// <summary>
    /// ������ø��ཱ��
    /// </summary>
    void AgeAridMobile()
    {
        A_AudioManager.Instance.PlaySound("Click");
        // ��ʾ���
        A_ADTrickle.Religion.TeamMobilePolar((b) =>
        {
            if (b)
            {
                GripTrickle.Religion.RichlyGrip(WeakenJoint * 2);
                AeroTrickle.Religion.LadeAero();    // ���¿�ʼ��Ϸ
            }
        }, "1");
    }

    /// <summary>
    /// ���¿�ʼ��Ϸ
    /// </summary>
    void LegendAero()
    {
        A_AudioManager.Instance.PlaySound("Click");
        GripTrickle.Religion.RichlyGrip(WeakenJoint);   // ��ȡ��������
        AeroTrickle.Religion.LadeAero();
    }

    /// <summary>
    /// ��ʾ�����ͽ�������
    /// </summary>
    /// <param name="rewardCount"></param>
    void CrowBirchLeoMobileJoint(int rewardCount)
    {
        OliveAid.text = BirchTrickle.Religion.Birch.ToString();
        ShinBirchAid.text = "BEST:" + BirchTrickle.Religion.BeatBirch.ToString();
        WeakenJointAid.text = rewardCount.ToString();
    }
}
