using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ƥ���̵�ҳ��
/// </summary>
public class RareLiftChunk : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("lastSkinBtn")]    public Button EmitRareBus;  //��һ��Ƥ��
[UnityEngine.Serialization.FormerlySerializedAs("nextSkinBtn")]    public Button AcidRareBus;  //��һ��Ƥ��
[UnityEngine.Serialization.FormerlySerializedAs("unlockBtn")]    public Button TundraBus;    //����Ƥ����ť
[UnityEngine.Serialization.FormerlySerializedAs("backGameBtn")]    public Button SpitAeroBus;  //������Ϸ��ť
[UnityEngine.Serialization.FormerlySerializedAs("priceText")]    public Text HaitiTube;      //Ƥ���۸��ı�
[UnityEngine.Serialization.FormerlySerializedAs("skinList")]    public RectTransform MarkCome;  //Ƥ���б�

    private int MarkScent= 200; //Ƥ���۸�
[UnityEngine.Serialization.FormerlySerializedAs("skins")]
    public Sprite[] Squat;      //Ƥ���б�
    private int MarkWaste= 0;   //��ǰƤ������

    bool USAInfluxRare= true;  //�Ƿ�����л�Ƥ��


    void OnEnable()
    {
        MarkWaste = PlayerPrefs.GetInt("Plummet9999_PlayerSkin", 0); //��ȡ��ǰƤ��;
        //��Ƥ��ҳ����ʾ��ǰ����ʹ�õ�Ƥ��
        float deltaX = (MarkCome.GetChild(0) as RectTransform).sizeDelta.x * MarkWaste + MarkCome.GetComponent<HorizontalLayoutGroup>().spacing * MarkWaste;
        MarkCome.anchoredPosition = new Vector2(-deltaX, 0);

        GetSpiralAdorn();
    }

    void Awake()
    {
        HaitiTube.text = MarkScent.ToString();
        EmitRareBus.onClick.AddListener(OnLastSkinBtnClick);
        AcidRareBus.onClick.AddListener(OnNextSkinBtnClick);
        TundraBus.onClick.AddListener(OnUnlockBtnClick);
        SpitAeroBus.onClick.AddListener(OnBackGameBtnClick);

        //����Ƥ���б�
        for (int i = 0; i < Squat.Length; i++)
        {
            GameObject skinObj = new GameObject("Skin", typeof(RectTransform), typeof(Image));
            skinObj.transform.SetParent(MarkCome, false);
            skinObj.GetComponent<Image>().sprite = Squat[i];
            (skinObj.transform as RectTransform).sizeDelta = new Vector2(220, 220);
        }
        //��0��Ƥ��ΪĬ��Ƥ��
        PlayerPrefs.SetInt("Plummet9999_SkinIndex0", 1);
    }

    /// <summary>
    /// ��һ��Ƥ����ť����¼�
    /// </summary>
    void OnLastSkinBtnClick()
    {
        if (!USAInfluxRare) return;
        if(MarkWaste > 0)
        {
            A_AudioManager.Instance.PlaySound("Click");
            USAInfluxRare = false;
            float moveX1 = (MarkCome.GetChild(MarkWaste) as RectTransform).sizeDelta.x / 2; //���Ƥ���Ŀ�
            float moveX2 = (MarkCome.GetChild(MarkWaste - 1) as RectTransform).sizeDelta.x / 2; //��һ��Ƥ���Ŀ�
            float moveX3 = MarkCome.GetComponent<HorizontalLayoutGroup>().spacing;  //���
            MarkCome.DOAnchorPosX(MarkCome.anchoredPosition.x + moveX1 + moveX2 + moveX3, 0.2f)
                .SetUpdate(true)
                .OnComplete(() => { USAInfluxRare = true; }); //��ֹ��ε��

            //������ʾ
            MarkWaste--;
            GetSpiralAdorn();
        }
    }

    /// <summary>
    /// ��һ��Ƥ����ť����¼�
    /// </summary>
    void OnNextSkinBtnClick()
    {
        if (!USAInfluxRare) return;
        if (MarkWaste < Squat.Length - 1)
        {
            A_AudioManager.Instance.PlaySound("Click");
            USAInfluxRare = false;
            float moveX1 = (MarkCome.GetChild(MarkWaste) as RectTransform).sizeDelta.x / 2; //���Ƥ���Ŀ�
            float moveX2 = (MarkCome.GetChild(MarkWaste + 1) as RectTransform).sizeDelta.x / 2; //��һ��Ƥ���Ŀ�
            float moveX3 = MarkCome.GetComponent<HorizontalLayoutGroup>().spacing;  //���
            MarkCome.DOAnchorPosX(MarkCome.anchoredPosition.x - moveX1 - moveX2 - moveX3, 0.2f)
                .SetUpdate(true)
                .OnComplete(() => { USAInfluxRare = true; }); //��ֹ��ε��

            //������ʾ
            MarkWaste++;
            GetSpiralAdorn();
        }
    }

    /// <summary>
    /// ����ť
    /// </summary>
    void OnUnlockBtnClick() 
    {
        A_AudioManager.Instance.PlaySound("Click");
        if (GripTrickle.Religion.RichlyGrip(-MarkScent))
        {
            PlayerPrefs.SetInt("Plummet9999_SkinIndex" + MarkWaste, 1); //����Ƥ��
            PlayerPrefs.SetInt("Plummet9999_PlayerSkin", MarkWaste); //����Ƥ��
            Amount.Religion.GetRare(MarkWaste); //���ý�ɫƤ��
            GetSpiralAdorn();
        }
        else
        {
            //������ʾ�򣬽�Ҳ���
            UITrickle.Religion.LeftChunk.SetActive(true);
            UITrickle.Religion.LeftChunk.GetComponent<LeftChunk>().CrowLeft("You don't have enough coins.");
        }
    }

    /// <summary>
    /// ������Ϸ��ť����¼�
    /// </summary>
    void OnBackGameBtnClick()
    {
        A_AudioManager.Instance.PlaySound("Click");
        AeroTrickle.Religion.SchoolAero();
    }

    /// <summary>
    /// ���ð�ť״̬
    /// </summary>
    void GetSpiralAdorn()
    {
        //���û��Ƥ������ֻ��һ��Ƥ��������һ��Ƥ������һ��Ƥ����ť������
        if (Squat == null || Squat.Length == 0 || Squat.Length == 1)
        {
            EmitRareBus.interactable = false;
            AcidRareBus.interactable = false;
        }
        //�����ǰ����Ϊ0������һ��Ƥ����ť������
        else if (MarkWaste == 0)
        {
            EmitRareBus.interactable = false;
            AcidRareBus.interactable = true;
        }
        //�����ǰ����Ϊ���һ��������һ��Ƥ����ť������
        else if (MarkWaste == Squat.Length - 1)
        {
            EmitRareBus.interactable = true;
            AcidRareBus.interactable = false;
        }
        //�����ǰ��������0�����һ��֮�䣬����һ��Ƥ������һ��Ƥ����ť������
        else
        {
            EmitRareBus.interactable = true;
            AcidRareBus.interactable = true;
        }

        //�����ǰ�����Ƥ�������л������Ƥ��
        if(PlayerPrefs.GetInt("Plummet9999_SkinIndex" + MarkWaste, 0) == 1)
        {
            TundraBus.gameObject.SetActive(false);
            PlayerPrefs.SetInt("Plummet9999_PlayerSkin", MarkWaste); //����Ƥ��
            Amount.Religion.GetRare(MarkWaste); //���ý�ɫƤ��
        }
        //���û�е�ǰ���Ƥ�����������ť��ʾ
        else
        {
            TundraBus.gameObject.SetActive(true);
        }
    }
}
