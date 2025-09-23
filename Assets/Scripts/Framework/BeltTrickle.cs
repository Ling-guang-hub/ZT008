using System.Collections.Generic;
using UnityEngine;

public class PoolMgr : MintTrickle<PoolMgr>
{
    //�ֵ��е���Ϊ��ģ��
    public Dictionary<string, Queue<GameObject>> SailJaw= new Dictionary<string, Queue<GameObject>>();

    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <param name="name">������</param>
    /// <param name="prefab">����Ԥ����</param>
    /// <returns></returns>
    public GameObject AgeNss(string name, GameObject prefab)
    {
        GameObject obj = null;
        //�ж��������ģ�鲢����ģ���л����ж���
        if (SailJaw.ContainsKey(name) && SailJaw[name].Count > 0)
        {
            //�õ���0������
            obj = SailJaw[name].Dequeue();
        }
        else
        {
            //���������û�ж���ʹ�������
            obj = GameObject.Instantiate(prefab);
            //���������������Ϊ�ͳ�����ģ�������һ���������Ϳ�����PushObj���ö������ִ洢
            obj.name = name;
        }
        //���弤�������ʾ
        obj.SetActive(true);
        return obj;
    }

    /// <summary>
    /// ���ն��󵽳�����
    /// </summary>
    /// <param name="obj">��Ҫ���յĶ���</param>
    public void CardNss(GameObject obj)
    {
        //������ʧ���������
        obj.SetActive(false);
        //�ж��������ģ��
        if (!SailJaw.ContainsKey(obj.name))
        {
            //���û�������ģ��ʹ�����ģ���ٴ�
            SailJaw.Add(obj.name, new Queue<GameObject>());
        }
        SailJaw[obj.name].Enqueue(obj);
    }

    /// <summary>
    /// ��ջ����
    /// </summary>
    public void Dutch()
    {
        SailJaw.Clear();
    }
}
