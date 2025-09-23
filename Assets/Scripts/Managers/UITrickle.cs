using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �򵥻�ȡUI�Ľű�
/// </summary>
public class UITrickle : PhysicistWood<UITrickle>
{
    private GameObject KingChunk;   //��Ϸ���
    private GameObject InvaderChunk;    //�������
    private GameObject DeftChunk;   //�����
    private GameObject MowChunk;    //�������
    private GameObject MarkLiftChunk;   //Ƥ���̵����
    private GameObject WantChunk;   //��ʾ���

    public GameObject AeroChunk=> KingChunk;
    public GameObject CreatorChunk=> InvaderChunk;
    public GameObject NewsChunk=> DeftChunk;
    public GameObject HubChunk=> MowChunk;
    public GameObject RareLiftChunk=> MarkLiftChunk;
    public GameObject LeftChunk=> WantChunk;


    protected override void Awake()
    {
        base.Awake();

        KingChunk = transform.Find("GamePanel").gameObject;
        InvaderChunk = transform.Find("SettingPanel").gameObject;
        DeftChunk = transform.Find("MainPanel").gameObject;
        MowChunk = transform.Find("EndPanel").gameObject;
        MarkLiftChunk = transform.Find("SkinShopPanel").gameObject;
        WantChunk = transform.Find("TipsPanel").gameObject;
    }
}
