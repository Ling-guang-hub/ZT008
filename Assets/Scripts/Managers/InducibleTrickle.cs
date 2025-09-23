using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

/// <summary>
/// ƽ̨������
/// </summary>
public class InducibleTrickle : PhysicistWood<InducibleTrickle>
{
    /// <summary>
    /// ƽ̨Ԥ����
    /// </summary>
    public GameObject StatuaryLavish;

#region ƽ̨����
    private float WhyFlower= 4f;    //ƽ̨����󳤶�
    private float AnyFlower= 2.5f;    //ƽ̨����С����
    private float WhyMoneyDust= 1.65f;  //ƽ̨���ֵ������ʱ��
    private float AnyMoneyDust= 1.65f;  //ƽ̨���ֵ���С���ʱ��
    private float PikeStore= 1.5f;  //ƽ̨���ƶ��ٶ�(ÿ���ƶ��ľ���)
    private float OvergrazeStarch;  //ƽ̨��ʧ�ĸ߶ȣ��෴��Ϊƽ̨���ֵĸ߶ȣ�
    private float BridgePocket;  //ƽ̨���ֵĿ���(���ߵķ�Χ)

    public float UncrumpleStarch=> OvergrazeStarch;
    public float BankStore=> PikeStore;
#endregion

    private float BustLawY;  //ƽ̨���ֵĳ�ʼ�߶�

    Coroutine FollowInducibleSubtropic;  //����ƽ̨Э��

    /// <summary>
    /// ��ʼ��
    /// </summary>
    public void Lade()
    {
        OvergrazeStarch = Camera.main.orthographicSize; //����������ĸ߶Ⱦ���Ϊ����ʧ�ĸ߶�
        BridgePocket = Camera.main.orthographicSize * ((float)Screen.width / Screen.height); //��Ļ���ȳ���������ĸ߶ȣ��õ�ƽ̨���ֵĿ���
        
        if(FollowInducibleSubtropic != null)
        {
            StopCoroutine(FollowInducibleSubtropic);  //ֹͣ����ƽ̨Э��
        }
            
        HabitSecreteInducible();  //������ʼƽ̨
    }

    /// <summary>
    /// ������ʼƽ̨
    /// </summary>
    public void HabitSecreteInducible()
    {
        float positionHeight = 0;

        for (int i = 0; ; i++)
        {
            GameObject platform = AssessReaction();
            if (i == 0)
            {
                platform.transform.position = new Vector3(0, 0, 0);  //��һ��ƽ̨�ڳ�ʼλ��
            }
            else
            {
                platform.transform.position = new Vector3(Random.Range(-BridgePocket, BridgePocket), positionHeight - (Random.Range(AnyMoneyDust, WhyMoneyDust) * PikeStore), 0);  //���λ��
            }
            positionHeight = platform.transform.position.y;     //��¼�������ɵ�ƽ̨�ĸ߶�

            if (positionHeight < -OvergrazeStarch) break;  //����߶�С�ڳ��ָ߶ȣ����˳�ѭ��
        }
        BustLawY = positionHeight;  //��¼��ʼ�߶ȣ����һ��ƽ̨�ĸ߶ȼ�Ϊ���ɵ�ƽ̨��λ�ø߶�
    }

    /// <summary>
    /// ��ʼ����ƽ̨
    /// </summary>
    public void RayonHabitInducible()
    {
        FollowInducibleSubtropic = StartCoroutine(AssessInducibleSubtropic());
    }

    /// <summary>
    /// ����ƽ̨
    /// </summary>
    /// <returns></returns>
    IEnumerator AssessInducibleSubtropic()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(AnyMoneyDust, WhyMoneyDust));  //������ʱ��
            GameObject platform = AssessReaction();
            platform.transform.position = new Vector3(Random.Range(-BridgePocket, BridgePocket), BustLawY, 0);  //���λ��
        }
    }

    /// <summary>
    /// ����ƽ̨
    /// </summary>
    /// <returns></returns>
    private GameObject AssessReaction()
    {
        GameObject platform = PoolMgr.Religion.AgeNss("Reaction", StatuaryLavish);      //����ƽ̨
        platform.GetComponent<SpriteRenderer>().size = new Vector3(Random.Range(AnyFlower, WhyFlower), platform.GetComponent<SpriteRenderer>().size.y, 1);
        platform.GetComponent<BoxCollider2D>().size = platform.GetComponent<SpriteRenderer>().size - new Vector2(0, 0.06f);
        platform.transform.parent = transform;
        platform.GetComponent<Reaction>().Lade();
        return platform;
    }
}