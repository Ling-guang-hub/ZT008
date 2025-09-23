using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// ��Ϸ������
/// </summary>
public class AeroTrickle : PhysicistWood<AeroTrickle>
{
    bool ByEntity= false;  //��Ϸ�Ƿ����ڽ���
    public bool OfEntity=> ByEntity;

    void Start()
    {
        base.Awake();
        LadeAero();
    }

    /// <summary>
    /// ��ʼ����Ϸ
    /// </summary>
    public void LadeAero()
    {
        ByEntity = true;
        //����֮ǰ��ƽ̨
        foreach(Transform child in InducibleTrickle.Religion.transform)
        {
            if (child.gameObject.activeSelf)
            {
                PoolMgr.Religion.CardNss(child.gameObject);
            }
        }

        //���������ϵĳ�ʼƽ̨
        InducibleTrickle.Religion.Lade();

        //��ʼ�����
        Amount.Religion.Lade();

        //��ʼ������������
        BirchTrickle.Religion.Lade();

        //��ʼ����Ϸҳ����ʾ
        UITrickle.Religion.AeroChunk.GetComponent<AeroChunk>().Lade();

        //����ҳ������
        UITrickle.Religion.AeroChunk.SetActive(true);
        UITrickle.Religion.NewsChunk.SetActive(true);
        UITrickle.Religion.CreatorChunk.SetActive(false);
        UITrickle.Religion.HubChunk.SetActive(false);
        UITrickle.Religion.RareLiftChunk.SetActive(false);

        LabelAero();    //��ʼ������ͣ
    }

    /// <summary>
    /// ��ʼ��Ϸ
    /// </summary>
    public void RayonAero()
    {
        InducibleTrickle.Religion.RayonHabitInducible();    //������̬ƽ̨
        SchoolAero();    //�ָ���Ϸ

        //����ҳ������
        UITrickle.Religion.AeroChunk.SetActive(true);
        UITrickle.Religion.NewsChunk.SetActive(false);
        UITrickle.Religion.CreatorChunk.SetActive(false);
        UITrickle.Religion.HubChunk.SetActive(false);
        UITrickle.Religion.RareLiftChunk.SetActive(false);
    }


    /// <summary>
    /// ������Ϸ
    /// </summary>
    public void HubAero()
    {
        LabelAero();    //��ͣ��Ϸ

        ByEntity = false;

        //����ҳ������
        UITrickle.Religion.AeroChunk.SetActive(true);
        UITrickle.Religion.NewsChunk.SetActive(false);
        UITrickle.Religion.CreatorChunk.SetActive(false);
        UITrickle.Religion.HubChunk.SetActive(true);
        UITrickle.Religion.RareLiftChunk.SetActive(false);
    }

    /// <summary>
    /// ��ͣ��Ϸ
    /// </summary>
    public void LabelAero()
    {
        if (ByEntity)
        {
            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// �ָ���Ϸ
    /// </summary>
    public void SchoolAero()
    {
        if (ByEntity)
        {
            Time.timeScale = 1;

            UITrickle.Religion.AeroChunk.SetActive(true);
            UITrickle.Religion.NewsChunk.SetActive(false);
            UITrickle.Religion.CreatorChunk.SetActive(false);
            UITrickle.Religion.HubChunk.SetActive(false);
            UITrickle.Religion.RareLiftChunk.SetActive(false);
        }
    }
}
