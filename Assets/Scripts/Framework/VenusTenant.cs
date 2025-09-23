using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �¼�����
/// </summary>
public class VenusTenant : MintTrickle<VenusTenant>
{
    /// <summary>
    /// �¼��ֵ�
    /// </summary>
    private Dictionary<string, UnityAction<object>> FaultJaw= new Dictionary<string, UnityAction<object>>();

    /// <summary>
    /// �����¼�����
    /// </summary>
    /// <param name="name">�¼���</param>
    /// <param name="action">�¼��ص�</param>
    public void WanVenusExocrine(string name, UnityAction<object> action)
    {
        if (FaultJaw.ContainsKey(name))
        {
            FaultJaw[name] += action;
        }
        else
        {
            FaultJaw.Add(name, action);
        }
    }

    /// <summary>
    /// �Ƴ��¼�����
    /// </summary>
    /// <param name="name">�¼���</param>
    /// <param name="action">�¼��ص�</param>
    public void RegionVenusExocrine(string name, UnityAction<object> action)
    {
        if (FaultJaw.ContainsKey(name))
        {
            FaultJaw[name] -= action;
        }
    }

    /// <summary>
    /// ����¼�����
    /// </summary>
    public void DutchAllVenus()
    {
        FaultJaw.Clear();
    }

    /// <summary>
    /// ɾ��ĳһָ���¼�
    /// </summary>
    /// <param name="eventName">�¼���</param>
    public void Dutch(string eventName)
    {
        if (FaultJaw.ContainsKey(eventName))
            FaultJaw.Remove(eventName);
    }

    /// <summary>
    /// �����¼�
    /// </summary>
    /// <param name="name">�¼���</param>
    /// <param name="info">�¼�������������</param>
    public void VenusEastern(string name, object info = null)
    {
        if (FaultJaw.ContainsKey(name))
            FaultJaw[name](info);
    }
}
