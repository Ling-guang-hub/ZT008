using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��Ϸҳ��
/// </summary>
public class AeroChunk : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("goldTxt")]    public Text KindAid;        // �����ʾ
[UnityEngine.Serialization.FormerlySerializedAs("scoreTxt")]    public Text OliveAid;       // �÷���ʾ
[UnityEngine.Serialization.FormerlySerializedAs("bestScoreTxt")]    public Text ShinBirchAid;   // ��߷���ʾ
[UnityEngine.Serialization.FormerlySerializedAs("shoppingBtn")]    public Button ExposureBus;  // �̵갴ť
[UnityEngine.Serialization.FormerlySerializedAs("settingBtn")]    public Button InvaderBus;   // ���ð�ť

    void Start()
    {
        ExposureBus.onClick.AddListener(OnShoppingBtnClick);
        InvaderBus.onClick.AddListener(OnSettingBtnClick);

        //������������ҵĸı�
        VenusTenant.Religion.WanVenusExocrine("UpdateScore", t => OliveAid.text = t.ToString());
        VenusTenant.Religion.WanVenusExocrine("UpdateBestScore", t => ShinBirchAid.text = t.ToString());
        VenusTenant.Religion.WanVenusExocrine("UpdateGold", t => KindAid.text = t.ToString());
    }

    /// <summary>
    /// ��ʼ�������ʾ
    /// </summary>
    public void Lade()
    {
        OliveAid.text =BirchTrickle.Religion.Birch.ToString();
        ShinBirchAid.text = BirchTrickle.Religion.BeatBirch.ToString();
        KindAid.text = GripTrickle.Religion.AgeGrip().ToString();
    }

    /// <summary>
    /// ���̵갴ť
    /// </summary>
    void OnShoppingBtnClick()
    {
        A_AudioManager.Instance.PlaySound("Click");
        AeroTrickle.Religion.LabelAero();
        transform.parent.Find("SkinShopPanel").gameObject.SetActive(true);
    }

    /// <summary>
    /// �����ð�ť
    /// </summary>
    void OnSettingBtnClick()
    {
        A_AudioManager.Instance.PlaySound("Click");
        AeroTrickle.Religion.LabelAero();
        transform.parent.Find("SettingPanel").gameObject.SetActive(true);
    }
}
