using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PuritanScope : BaseUIForms
{
    [FormerlySerializedAs("soundIcon")]

    public Image TwainCorp;
    [FormerlySerializedAs("musicIcon")]

    public Image MouseCorp;

    [FormerlySerializedAs("soundBtn")]


    public Button TwainJet;
    [FormerlySerializedAs("musicBtn")]

    public Button MouseJet;


    // public Button BoastJet;

    [FormerlySerializedAs("continueBtn")]


    public Button TheorizeJet;

    [FormerlySerializedAs("privacyBtn")]


    public Button ProgramJet;

    // public Sprite MusicCloseSprite;
    // public Sprite MusicOpenSprite;
    // public Sprite SoundCloseSprite;
    // public Sprite SoundOpenSprite;

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Panel_pop);
        // MusicIcon.sprite = MusicMgr.GetInstance().BgMusicSwitch ? MusicOpenSprite : MusicCloseSprite;
        // TwainCorp.sprite = MusicMgr.GetInstance().EffectMusicSwitch ? SoundOpenSprite : SoundCloseSprite;
        TwainCorp.gameObject.SetActive(MusicMgr.GetInstance().EffectMusicSwitch);
        MouseCorp.gameObject.SetActive(MusicMgr.GetInstance().BgMusicSwitch);
    }

    // Start is called before the first frame update
    void Start()
    {
        TheorizeJet.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            MessageCenterLogic.GetInstance().Send(CConfig.mg_ClosePanel);
            CloseUIForm(GetType().Name);
        });

        ProgramJet.onClick.AddListener(() =>
        {
            string tempUrl = "http://nexusgames.top/privacy_policy.html";
            CallURL(tempUrl);
            // MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_2);
            // MessageCenterLogic.GetInstance().Send(CConfig.mg_ClosePanel);
            // CloseUIForm(GetType().Name);
        });

        MouseJet.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            MusicMgr.GetInstance().BgMusicSwitch = !MusicMgr.GetInstance().BgMusicSwitch;
            MouseCorp.gameObject.SetActive(MusicMgr.GetInstance().BgMusicSwitch);
            // MusicIcon.sprite = MusicMgr.GetInstance().BgMusicSwitch ? MusicOpenSprite : MusicCloseSprite;
        });

        TwainJet.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            MusicMgr.GetInstance().EffectMusicSwitch = !MusicMgr.GetInstance().EffectMusicSwitch;
            TwainCorp.gameObject.SetActive(MusicMgr.GetInstance().EffectMusicSwitch);
            // SoundIcon.sprite = MusicMgr.GetInstance().EffectMusicSwitch ? SoundOpenSprite : SoundCloseSprite;
        });

        // BoastJet.onClick.AddListener(() =>
        // {
        //     ParkScope.Instance.ShowUIAfterClose();
        //     CloseUIForm(GetType().Name);
        // });
    }

#if UNITY_IOS
    [DllImport("__Internal")]
    internal extern static void openUrl(string url);
#endif

    public static void CallURL(string url)
    {
#if UNITY_ANDROID || UNITY_EDITOR
        Application.OpenURL(url);
#elif UNITY_IOS
        openUrl(url);
#endif
    }
}