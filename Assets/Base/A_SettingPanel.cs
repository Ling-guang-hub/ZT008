using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����ҳ��
/// </summary>
public class A_SettingPanel : PhysicistWood<A_SettingPanel>
{
    public Image img;   //����
    public Button soundBtn;
    public Button replayBtn;
    public Button backGameBtn;

    private void Start()
    {
        Application.targetFrameRate = 60;
        soundBtn.onClick.AddListener(Sound);
        replayBtn.onClick.AddListener(Replay);
        backGameBtn.onClick.AddListener(BackGame);
        img.rectTransform.anchoredPosition = PlayerPrefs.GetInt("Plummet9999_Sound", 1) == 1 ? new Vector2(65.5f, 0): new Vector2(-65.5f, 0);
    }

    public void Sound()
    {
        A_AudioManager.Instance.PlaySound("Click");
        int sound = PlayerPrefs.GetInt("Plummet9999_Sound", 1);
        sound = sound == 1 ? 0 : 1;
        PlayerPrefs.SetInt("Plummet9999_Sound", sound);
        img.rectTransform.anchoredPosition = sound == 1 ? new Vector2(65.5f, 0) : new Vector2(-65.5f, 0);
        A_AudioManager.Instance.ToggleSound();
    }

    /// <summary>
    /// ���¿�ʼ
    /// </summary>
    public void Replay()
    {
        A_AudioManager.Instance.PlaySound("Click");
        AeroTrickle.Religion.LadeAero();
    }

    /// <summary>
    /// ������Ϸ
    /// </summary>
    public void BackGame()
    {
        A_AudioManager.Instance.PlaySound("Click");
        AeroTrickle.Religion.SchoolAero();
    }
}
