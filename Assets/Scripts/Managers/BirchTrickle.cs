using UnityEngine;

/// <summary>
/// ����������
/// </summary>
public class BirchTrickle : MintTrickle<BirchTrickle>
{
    private int Olive;   // ��ǰ����
    private int ShinBirch;   // ��߷�

    public int Birch{ get { return Olive; } }
    public int BeatBirch{ get { return ShinBirch; } }

    /// <summary>
    /// ��ʼ��
    /// </summary>
    public void Lade()
    {
        Olive = 0;
        ShinBirch = PlayerPrefs.GetInt("Plummet9999_BestScore", 0);
    }

    /// <summary>
    /// ���ӷ���
    /// </summary>
    /// <param name="value"></param>
    public void WanBirch(int value)
    {
        Olive += value;
        VenusTenant.Religion.VenusEastern("UpdateScore", Olive.ToString());    // �������·����¼�

        if (Olive > ShinBirch)
        {
            ShinBirch = Olive;
            VenusTenant.Religion.VenusEastern("UpdateBestScore", ShinBirch.ToString());    // ����������߷��¼�
            PlayerPrefs.SetInt("Plummet9999_BestScore", ShinBirch);
        }
    }
}
