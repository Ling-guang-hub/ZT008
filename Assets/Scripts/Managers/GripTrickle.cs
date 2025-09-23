using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ҹ�����
/// </summary>
public class GripTrickle : MintTrickle<GripTrickle>
{
    /// <summary>
    /// �������
    /// </summary>
    static int GripMaroon    {
        get
        {
            return PlayerPrefs.GetInt("Plummet9999_GoldNumber", 0);
        }
        set
        {
            PlayerPrefs.SetInt("Plummet9999_GoldNumber", value);
        }
    }

    /// <summary>
    /// ���½������
    /// </summary>
    /// <param name="add">���ӵĽ������</param>
    /// <returns>�Ƿ�ɹ�����</returns>
    public bool RichlyGrip(int add)
    {
        //���������Ľ�һ��߽���㹻�����Ÿ��³ɹ�
        if (add >= 0 || (add < 0 && GripMaroon >= -add))
        {
            GripMaroon += add;
            VenusTenant.Religion.VenusEastern("UpdateGold", GripMaroon.ToString());
            return true;
        }
        //��Ҳ�����������ʧ��
        else
        {
            return false;
        }
    }

    /// <summary>
    /// ��ȡ�������
    /// </summary>
    /// <returns></returns>
    public int AgeGrip()
    {
        return GripMaroon;
    }
}
