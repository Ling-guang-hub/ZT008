using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FestiveMagic : BaseUIForms
{
    [FormerlySerializedAs("soundIcon")]

    public Image ClaimUtah;
    [FormerlySerializedAs("musicIcon")]

    public Image DutchUtah;

    [FormerlySerializedAs("soundBtn")]


    public Button ClaimBuy;
    [FormerlySerializedAs("musicBtn")]

    public Button DutchBuy;


    // public Button AlikeBuy;

    [FormerlySerializedAs("continueBtn")]


    public Button DissolveBuy;

    [FormerlySerializedAs("privacyBtn")]


    public Button OutcomeBuy;

    // public Sprite MusicCloseSprite;
    // public Sprite MusicOpenSprite;
    // public Sprite SoundCloseSprite;
    // public Sprite SoundOpenSprite;

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Panel_pop);
        // MusicIcon.sprite = MusicMgr.GetInstance().BgMusicSwitch ? MusicOpenSprite : MusicCloseSprite;
        // ClaimUtah.sprite = MusicMgr.GetInstance().EffectMusicSwitch ? SoundOpenSprite : SoundCloseSprite;
        ClaimUtah.gameObject.SetActive(MusicMgr.GetInstance().EffectMusicSwitch);
        DutchUtah.gameObject.SetActive(MusicMgr.GetInstance().BgMusicSwitch);
    }

    // Start is called before the first frame update
    void Start()
    {
        DissolveBuy.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            MessageCenterLogic.GetInstance().Send(CConfig.mg_ClosePanel);
            CloseUIForm(GetType().Name);
        });

        OutcomeBuy.onClick.AddListener(() =>
        {
            string tempUrl = "http://nexusgames.top/privacy_policy.html";
            PeckURL(tempUrl);
            // MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_2);
            // MessageCenterLogic.GetInstance().Send(CConfig.mg_ClosePanel);
            // CloseUIForm(GetType().Name);
        });

        DutchBuy.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            MusicMgr.GetInstance().BgMusicSwitch = !MusicMgr.GetInstance().BgMusicSwitch;
            DutchUtah.gameObject.SetActive(MusicMgr.GetInstance().BgMusicSwitch);
            // MusicIcon.sprite = MusicMgr.GetInstance().BgMusicSwitch ? MusicOpenSprite : MusicCloseSprite;
        });

        ClaimBuy.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            MusicMgr.GetInstance().EffectMusicSwitch = !MusicMgr.GetInstance().EffectMusicSwitch;
            ClaimUtah.gameObject.SetActive(MusicMgr.GetInstance().EffectMusicSwitch);
            // SoundIcon.sprite = MusicMgr.GetInstance().EffectMusicSwitch ? SoundOpenSprite : SoundCloseSprite;
        });

        // AlikeBuy.onClick.AddListener(() =>
        // {
        //     TameMagic.Instance.ShowUIAfterClose();
        //     CloseUIForm(GetType().Name);
        // });
    }

#if UNITY_IOS
    [DllImport("__Internal")]
    internal extern static void openUrl(string url);
#endif

    public static void PeckURL(string url)
    {
#if UNITY_ANDROID || UNITY_EDITOR
        Application.OpenURL(url);
#elif UNITY_IOS
        openUrl(url);
#endif
    }
}