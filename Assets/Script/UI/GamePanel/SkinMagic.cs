// Project  ScratchCard
// FileName  ScratchPanel.cs
// Author  AX
// Desc
// CreateAt  2025-03-31 17:03:35 
//


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Lofelt.NiceVibrations;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkinMagic : BaseUIForms
{
    public static SkinMagic Instance;

    [FormerlySerializedAs("mainAreaObj")]


    public GameObject DireFlapSum;

    [FormerlySerializedAs("topBar")]


    public TopBar topBar;

    [FormerlySerializedAs("passportBtn")]


    public Button passportBtn;

    [FormerlySerializedAs("taskBtn")]


    public Button taskBtn;

    [FormerlySerializedAs("cashShopObj")]


    public GameObject cashShopObj;

    [FormerlySerializedAs("wheelBar")]


    public WheelBar wheelBar;

    [FormerlySerializedAs("cardObj")]


    public GameObject cardObj;

    [FormerlySerializedAs("settingBtn")]


    public Button settingBtn;

    [FormerlySerializedAs("mainBtn")]


    public Button mainBtn;

    [FormerlySerializedAs("mainBtnText")]


    public Text mainBtnText;

    [FormerlySerializedAs("closeBtn")]


    public Button AlikeBuy;

    [FormerlySerializedAs("cardArea")]


    public GameObject cardArea;

    [FormerlySerializedAs("cardObjPool")]


    public List<GameObject> cardObjPool;

    [FormerlySerializedAs("timeBar")]


    public TimeBarCtrl timeBar;

    [FormerlySerializedAs("collectBar")]


    public GameObject collectBar;

    [FormerlySerializedAs("holdOnTimerObj")]


    public GameObject holdOnTimerObj;

    private Dictionary<CardType, GameObject> _cardsDict;

    [FormerlySerializedAs("currentCardType")]


    public CardType currentCardType;

    [FormerlySerializedAs("currentCardId")]


    public int currentCardId;

    [FormerlySerializedAs("guideStep1")]


    public GameObject guideStep1;

    // public GameObject guideStep2;

    [FormerlySerializedAs("guideStep3")]


    public GameObject guideStep3;

    [FormerlySerializedAs("guideTextBoard")]


    public GameObject guideTextBoard;

    [FormerlySerializedAs("guideText3")]


    public GameObject guideText3;

    [FormerlySerializedAs("guideStep4")]


    public GameObject guideStep4;

    [FormerlySerializedAs("guideText4")]


    public GameObject guideText4;

    [FormerlySerializedAs("guideStopShop")]


    public GameObject guideStopShop;

    [FormerlySerializedAs("checkFxObj")]


    public GameObject checkFxObj;

    private bool _mainBtnCleanFlag;

    private float _mainCardRate;

    private bool _scratchMusicFlag;

    private bool _scratchFxFlag;

    private int _bigWinLimit;

    private void Awake()
    {
        Instance = this;
        DoAdaptation();
        InitCardToDic();
        _mainCardRate = LocalCommonData.ScreenRate > 0.5f ? 0.9f : 1f;
        _scratchMusicFlag = true;
        _scratchFxFlag = true;
        _bigWinLimit = NetInfoMgr.instance.GameData.bigwin_limit;
    }

    public void Start()
    {
        mainBtn.onClick.AddListener(() =>
        {
            if (LocalCommonData.IsGamePass) return;

            if (!SaveDataManager.GetBool(CConfig.sv_FinishFirstBigWin)) return;
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);

            ClickMainBtn();
        });

        AlikeBuy.onClick.AddListener(() =>
        {
            if (LocalCommonData.IsGamePass) return;
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_2);
            TameMagic.Instance.ShowHomeArea();
            SetMainBtn(true);
            CloseUIForm(GetType().Name);
        });

        settingBtn.onClick.AddListener(() =>
        {
            if (LocalCommonData.IsGamePass) return;
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_2);
            MessageCenterLogic.GetInstance().Send(CConfig.mg_PassAnim);
            OpenUIForm(nameof(FestiveMagic));
        });


        passportBtn.onClick.AddListener(() =>
        {
            if (LocalCommonData.IsGamePass) return;
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            MessageCenterLogic.GetInstance().Send(CConfig.mg_PassAnim);
            OpenUIForm(nameof(EdgeBothMagic));
        });

        taskBtn.onClick.AddListener(() =>
        {
            if (LocalCommonData.IsGamePass) return;
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            MessageCenterLogic.GetInstance().Send(CConfig.mg_PassAnim);
            OpenUIForm(nameof(KeepMagic));
        });


        MessageCenterLogic.GetInstance().Register(CConfig.mg_PassAnim, (md) =>
        {
            // if (guideStep2.gameObject.activeInHierarchy)
            // {
            //     guideStep2.SetActive(false);
            // }

            CloseSomeUI();
            CloseShopMask();
        });

        MessageCenterLogic.GetInstance().Register(CConfig.mg_ClosePanel, (md) =>
        {
            if (gameObject.activeInHierarchy)
            {
                passportBtn.GetComponent<PassportSlider>().ShowSlider();
                taskBtn.GetComponent<TaskBtnController>().ShowPoint();
            }

            RestartHoldTimer();
            LocalCommonData.IsGamePass = false;
        });

        MessageCenterLogic.GetInstance()
            .Register(CConfig.mg_FinishFlyGoods, (md) => { LocalCommonData.IsGamePass = false; });


        MessageCenterLogic.GetInstance().Register(CConfig.mg_GameSuspend, (md) =>
        {
            if (!SaveDataManager.GetBool(CConfig.sv_FinishNewGuide) &&
                SaveDataManager.GetBool(CConfig.sv_InitFirstCard))
            {
                // guideStep2.gameObject.SetActive(false);
                ShowGuide();
            }
            else
            {
                RestartHoldTimer();
                LocalCommonData.IsGamePass = false;
            }
        });

        MessageCenterLogic.GetInstance().Register(CConfig.mg_PlayScratchMusic, (md) =>
        {
            if (cardObj.GetComponent<BaseCard>().GetCardIsFinish()) return;
            PlayScratchFx();
            PlayScratchMusic();
        });
    }

    private async void PlayScratchMusic()
    {
        if (!_scratchMusicFlag) return;
        _scratchMusicFlag = false;
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Scrape);
        await Task.Delay(400);
        _scratchMusicFlag = true;
    }

    private async void PlayScratchFx()
    {
        if (!_scratchFxFlag) return;
        _scratchFxFlag = false;
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.Selection);
        // Vector3 newPos = Camera.main.ScreenToWorldPoint(mdPos);
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject fxObj = Instantiate(checkFxObj, checkFxObj.transform.parent.transform, false);
        fxObj.transform.position = new Vector3(newPos.x, newPos.y, 0);
        fxObj.gameObject.SetActive(true);
        var fxSys = fxObj.GetComponent<ParticleSystem>();
        Destroy(fxObj.gameObject, fxSys.main.duration); // 粒子播放
        await Task.Delay(50);
        _scratchFxFlag = true;
    }

    private void DoAdaptation()
    {
        if (LocalCommonData.ScreenRate > 0.5f)
        {
            wheelBar.transform.localPosition = new Vector3(-150f, -250f, 0);
            timeBar.transform.localPosition = new Vector3(250, -230f, 0);
            topBar.transform.localPosition = new Vector3(200f, -145f, 0);

            AlikeBuy.gameObject.transform.localPosition = new Vector3(100f, -120f, 0);
            settingBtn.gameObject.transform.localPosition = new Vector3(-100f, -120f, 0);

            // passportBtn.transform.localScale = new Vector3(0.7f, 0.7f, 0);
            passportBtn.gameObject.transform.localPosition = new Vector3(150f, 120f, 0);

            // taskBtn.transform.localScale = new Vector3(0.8f, 0.8f, 0);
            taskBtn.gameObject.transform.localPosition = new Vector3(150f, 300f, 0);

            mainBtn.transform.localPosition = new Vector3(0, 210, 0);
            cardArea.transform.localPosition = new Vector3(0, 100, 0);

            cashShopObj.transform.localScale = new Vector3(0.75f, 0.75f, 0);
            cashShopObj.transform.localPosition = new Vector3(0f, -120f, 0);

            collectBar.transform.localPosition = new Vector3(0, -550f, 0);
        }
        else
        {
            // 1080*2340
            wheelBar.transform.localPosition = new Vector3(-150f, -320f, 0);
            timeBar.transform.localPosition = new Vector3(250, -320f, 0);
            topBar.transform.localPosition = new Vector3(200f, -145f, 0);

            AlikeBuy.gameObject.transform.localPosition = new Vector3(100f, -150f, 0);
            settingBtn.gameObject.transform.localPosition = new Vector3(-100f, -150f, 0);

            // passportBtn.transform.localScale = new Vector3(0.7f, 0.7f, 0);
            passportBtn.gameObject.transform.localPosition = new Vector3(150f, 150f, 0);

            // taskBtn.transform.localScale = new Vector3(0.85f, 0.85f, 0);
            taskBtn.gameObject.transform.localPosition = new Vector3(150f, 350f, 0);

            mainBtn.transform.localPosition = new Vector3(0, 250, 0);
            cardArea.transform.localPosition = new Vector3(0, 120, 0);

            cashShopObj.transform.localScale = new Vector3(0.8f, 0.8f, 0);
            cashShopObj.transform.localPosition = new Vector3(0f, -150f, 0);
            
            collectBar.transform.localPosition = new Vector3(0, -600f, 0);
        }
    }

    private void ShowFirstMask()
    {
        guideStep1.GetComponent<RectMaskTool>().targetImg =
            cardObj.gameObject.GetComponent<BaseCard>().top.GetComponent<Image>();
        // guideStep1.GetComponent<RectMaskTool>().targetObj = cardObj;
        guideStep1.GetComponent<RectMaskTool>().InitData();
        guideStep1.gameObject.SetActive(true);
        guideTextBoard.gameObject.SetActive(true);
        guideTextBoard.transform.DOScale(1, 0.3f).SetDelay(0.5f).OnComplete(() =>
        {
            mainBtn.enabled = true;
            LocalCommonData.IsGamePass = false;
            var image = guideStep1.GetComponent<Image>();
            if (image)
            {
                image.alphaHitTestMinimumThreshold = 0.1f;
            }
        });
    }


    private async void ShowSecondMask()
    {
        PostEventScript.GetInstance().SendEvent("1011");
        // guideStep2.gameObject.SetActive(true);
        await UniTask.Delay(1000);
        mainBtn.enabled = true;
        LocalCommonData.IsGamePass = false;
    }

    private void ShowThirdMask()
    {
        PostEventScript.GetInstance().SendEvent("1015");
        mainBtn.enabled = false;
        guideText3.gameObject.SetActive(true);
        guideText3.transform.DOScale(1, 0.3f).SetDelay(0.5f).OnComplete(() =>
        {
            mainBtn.enabled = true;
            LocalCommonData.IsGamePass = false;
        });

        guideStep3.gameObject.SetActive(true);
    }


    private void ShowFourthMask()
    {
        PostEventScript.GetInstance().SendEvent("1016");

        SaveDataManager.SetBool(CConfig.sv_FinishFirstCheckCard, true);
        guideText4.gameObject.SetActive(true);
        guideStep4.gameObject.SetActive(true);
        guideText4.transform.DOScale(1, 0.3f).SetDelay(0.5f).OnComplete(() => { });
        // await UniTask.Delay(1000);
        // LocalCommonData.IsGamePass = false;
        // mainBtn.enabled = true;
    }


    private void ShowShopMask()
    {
        LocalCommonData.IsGamePass = false;
        SaveDataManager.SetBool(CConfig.sv_FinishShopGuide, true);

        if (!SaveDataManager.GetBool(CConfig.sv_FinishNewGuide))
        {
            SaveDataManager.SetBool(CConfig.sv_FinishNewGuide, true);
            PostEventScript.GetInstance().SendEvent("1002");
        }

        guideStopShop.gameObject.SetActive(true);
        mainBtn.enabled = true;
    }

    private void CloseShopMask()
    {
        if (guideStopShop.activeInHierarchy)
        {
            guideStopShop.gameObject.SetActive(false);
        }
    }


    private async void ShowGuide()
    {
        if (SaveDataManager.GetBool(CConfig.sv_FinishNewGuide)) return;

        if (!SaveDataManager.GetBool(CConfig.sv_FinishFirstBigWin))
        {
            LocalCommonData.IsGamePass = true;
            mainBtn.enabled = false;
            await UniTask.Delay(800);
            ShowFirstMask();
            return;
        }

        guideStep1.gameObject.SetActive(false);

        // if (!SaveDataManager.GetBool(CConfig.sv_FinishFirstShowShop))
        // {
        //     LocalCommonData.IsGamePass = true;
        //     mainBtn.enabled = false;
        //     await UniTask.Delay(1500);
        //     ShowSecondMask();
        //     return;
        // }

        // guideStep2.gameObject.SetActive(false);

        if (!SaveDataManager.GetBool(CConfig.sv_FinishFirstCheckCard))
        {
            LocalCommonData.IsGamePass = true;
            mainBtn.enabled = false;
            await UniTask.Delay(800);
            ShowThirdMask();
            return;
        }

        guideStep3.gameObject.SetActive(false);
        guideStep4.gameObject.SetActive(false);
        // await UniTask.Delay(1000);
        // ShowFourthMask();
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        // cardObj = cardObjPool[4];
        passportBtn.gameObject.SetActive(!CommonUtil.IsApple());
        taskBtn.gameObject.SetActive(!CommonUtil.IsApple());

        ShowAct();
        OpenMainBtn();
        mainBtn.enabled = false;

        LocalCommonData.IsGamePass = true;
        LocalCommonData.IsGamePanel = true;
        ShowGuide();
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Panel_pop);
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Card_switch);
        InitDataObj();
        cardObj.gameObject.SetActive(true);
        // InitCardData();
        timeBar.cardNumText.text = GameDataManager.GetInstance().GetCard() + "";
        RefreshTopData();
        HoldTimerStart();
    }


    private void HoldTimerStart()
    {
        holdOnTimerObj.gameObject.SetActive(true);
        holdOnTimerObj.GetComponent<HoldOnSpine>().StartTimer();
    }

    private void HoldTimerPass()
    {
        if (holdOnTimerObj.gameObject.activeInHierarchy)
        {
            holdOnTimerObj.GetComponent<HoldOnSpine>().PassTimer();
        }
    }

    private void RestartHoldTimer()
    {
        if (holdOnTimerObj.gameObject.activeInHierarchy)
        {
            holdOnTimerObj.GetComponent<HoldOnSpine>().RestartTimer();
        }

        OpenSomeUI();
    }


    public void HoldTimerEnd()
    {
        // holdOnTimerObj.GetComponent<HoldOnSpine>().StopTimer();
        holdOnTimerObj.gameObject.SetActive(false);
    }


    private void ShowAct()
    {
        DireFlapSum.transform.localScale = Vector3.zero;
        DireFlapSum.transform.DOScale(1f, 0.2f).OnComplete(() => { InitCardData(); });
    }


    private void CloseSomeUI()
    {
        // cardObj.gameObject.SetActive(false);
        cardObj.gameObject.GetComponent<BaseCard>().CloseSuperSpine();
    }

    private void OpenSomeUI()
    {
        cardObj.gameObject.GetComponent<BaseCard>().OpenSuperSpine();

        // if (!cardObj.gameObject.activeInHierarchy)
        // {
        //     cardObj.gameObject.SetActive(true);
        // }
    }

    private void ShowLosePanel()
    {
        CloseSomeUI();
        OpenUIForm(nameof(PuffMagic));
    }

    private void ShowCompletePanel()
    {
        WinPanelData thisData = new WinPanelData()
        {
            CoinAmount = LocalRewardData.CompleteData.CoinAmount,
            IsCard = true
        };
        if (LocalRewardData.ShowRewardPanel)
        {
            if (LocalRewardData.CompleteData.IsSpecial)
            {
                UIManager.GetInstance().ShowUIForms(nameof(PlainFlyMagic), thisData);
            }
            else
            {
                UIManager.GetInstance().ShowUIForms(LocalRewardData.CompleteData.CoinAmount >= _bigWinLimit
                    ? nameof(WebFlyMagic)
                    : nameof(CohesionMagic), thisData);
            }

            CloseSomeUI();
        }
        else
        {
            ShowItemLoopAct();
            TakeCollects();
        }
    }

    private void ShowItemLoopAct()
    {
        OpenSomeUI();
        cardObj.GetComponent<BaseCard>().DoLoopAnim();
    }

    public async void DoFinishAnimAndShowCompletePanel()
    {
        CloseMainBtn();
        mainBtn.enabled = false;

        SetMainBtn(false);
        await UniTask.Delay(200);

        PostEventScript.GetInstance().SendEvent("109" + LocalCommonData.CurrentCardId);
        passportBtn.GetComponent<PassportSlider>().ShowSlider();
        taskBtn.GetComponent<TaskBtnController>().ShowPoint();
        if (LocalRewardData.ShowThankPanel)
        {
            OpenMainBtn();
            await UniTask.Delay(500);
            ShowLosePanel();
        }
        else
        {
            float delayTime = cardObj.GetComponent<BaseCard>().DoWinAnim();
            double time = (delayTime + 0.5) * 1000;
            await UniTask.Delay((int)time);
            OpenMainBtn();
            ShowCompletePanel();
        }
    }

    public async void AfterCardBonus(int coinAmount, decimal cashAmount)
    {
        ShowGuide();
        ShowItemLoopAct();
        await UniTask.Delay(300);
        topBar.AddCoinAndDoAnima(coinAmount, cashAmount, false);
        await UniTask.Delay(200);
        TakeCollects();
    }
    
    
    public async void AfterCollectBonus(int coinAmount)
    {
        // ShowGuide();
        ShowItemLoopAct();
        await UniTask.Delay(300);
        topBar.AddCoinAndDoAnima(coinAmount, 0, true);
        await UniTask.Delay(200);
        CheckShowWheelPanel();
    } 
    
    
    
    
    private async void GoOnGame()
    {
        int delayTime = 100;
        if (TaskManager.GetInstance().CheckShouldOpenPanel())
        {
            MessageCenterLogic.GetInstance().Send(CConfig.mg_PassAnim);
            OpenUIForm(nameof(KeepMagic));
            delayTime = 1000;
        }

        await UniTask.Delay(delayTime);
        mainBtn.gameObject.GetComponent<MainBtnTimer>().StartSpine();
        mainBtn.enabled = true;

        LocalCommonData.IsGamePass = false;
        // CheckTaskPanel();
    }

    public async void AfterWheelBonus()
    {
        ShowItemLoopAct();
        switch (LocalWheelData.WheelType)
        {
            case CommonRewardType.Coin:

                int coinAmount = Mathf.CeilToInt((float)LocalWheelData.WheelAmount);
                topBar.AddCoinAndDoAnima(coinAmount, 0, true);
                break;
            case CommonRewardType.Cash:
                decimal cashAmount = decimal.Parse(LocalWheelData.WheelAmount.ToString());
                topBar.AddCoinAndDoAnima(0, cashAmount, true);
                break;
            case CommonRewardType.Card:
                int cardAmount = Mathf.CeilToInt((float)LocalWheelData.WheelAmount);
                GameDataManager.GetInstance().AddCard(cardAmount, LocalWheelData.WheelCardId);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        await UniTask.Delay(300);

        if (!SaveDataManager.GetBool(CConfig.sv_FinishShopGuide))
        {
            await UniTask.Delay(300);
            // CloseSomeUI();
            ShowShopMask();
        }
        else
        {
            GoOnGame();
        }
    }


    public void AfterLosePanel()
    {
        OpenSomeUI();
        CheckShowWheelPanel();
    }

    public async void AfterChangeCardPanel(int nextCardId)
    {
        mainBtn.gameObject.GetComponent<MainBtnTimer>().StopSpine();
        LocalCommonData.CurrentCardId = nextCardId;
        await UniTask.Delay(100);
        OpenMainBtn();
        CarbonKnap();
    }


    public void CheckShowWheelPanel()
    {
        if (WheelBarManager.GetInstance().GetCurRate() >= 1)
        {
            wheelBar.ShowLuckPanel();
        }
        else
        {
            GoOnGame();
        }
    }

    public async  void CheckShowCollectBonus()
    {
        if (CollectManager.Instance.CheckGetReward())
        {
            await UniTask.Delay(100);
            collectBar.GetComponent<CollectBarCtrl>().DoScaleAct();
            await UniTask.Delay(800);
            int reward = GameUtil.GetCollectReward();
            WinPanelData thisData = new WinPanelData()
            {
                CoinAmount = reward,
                IsCard = false
            };
            MessageCenterLogic.GetInstance().Send(CConfig.mg_PassAnim);
            
            UIManager.GetInstance().ShowUIForms(reward >= _bigWinLimit
                ? nameof(WebFlyMagic)
                : nameof(CohesionMagic), thisData);
            CollectManager.Instance.ClearCollectCount();
            collectBar.GetComponent<CollectBarCtrl>().ShowCollects();
        }
        else
        {
            CheckShowWheelPanel();
        }
    }


    public void AfterFlyGoods()
    {
        // await UniTask.Delay(300);
        GoOnGame();
    }


    private void RefreshTopData()
    {
        topBar.ShowWallet();

        // cashShopObj.GetComponent<CashOutEnter>().ShowUI();

        wheelBar.ShowSliderUI();
        CardTimeManager.GetInstance().FirstShowCardNum();

        passportBtn.GetComponent<PassportSlider>().ShowSlider();
        taskBtn.GetComponent<TaskBtnController>().ShowPoint();
    }


    private void InitCardToDic()
    {
        _cardsDict = new Dictionary<CardType, GameObject>();
        foreach (var t in cardObjPool)
        {
            CardType cardType = t.GetComponent<BaseCard>().cardType;
            _cardsDict.TryAdd(cardType, t);
        }
    }


    private async void InitCardData()
    {
        cardObj.GetComponent<BaseCard>().InitCardData();
        bool isSpecial = cardObj.GetComponent<BaseCard>().GetSpecialType();
        SetMainBtn(true);
        mainBtn.gameObject.GetComponent<MainBtnTimer>().StopSpine();
        int delayTime = 200;
        if (isSpecial)
        {
            delayTime = 2000;
            if (!SaveDataManager.GetBool(CConfig.sv_FinishFirstCheckCard))
            {
                delayTime = 3200;
            }
        }

        await UniTask.Delay(delayTime);
        mainBtn.enabled = true;
        LocalCommonData.IsGamePass = false;
    }

    private void InitDataObj()
    {
        if (cardObj != null)
        {
            Destroy(cardObj);
        }

        LocalRewardData.ResetCompleteData();

        currentCardId = LocalCommonData.CurrentCardId;
        currentCardType = LocalCardData.CardParamDict[currentCardId].Type;

        GameObject obj = _cardsDict[currentCardType];

        cardObj = Instantiate(obj, cardArea.transform, false);
        cardObj.transform.localScale = cardArea.transform.localScale * _mainCardRate;
        cardObj.transform.localPosition = new Vector3(0, 0, 0);
    }


    private void GetNewObj()
    {
        InitDataObj();
        cardObj.transform.localScale *= 0.1f;
        cardObj.gameObject.SetActive(true);
        cardObj.transform.DOScale(_mainCardRate, 0.125f).OnComplete(() =>
        {
            InitCardData();
            HoldTimerStart();
        });
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Card_switch);
    }

    private void TakeCollects()
    {
        collectBar.GetComponent<CollectBarCtrl>().AddCollectAndPlayAct();
    }



    private async void PlayChangeCardAnim()
    {
        cardObj.GetComponent<BaseCard>().CloseSpineObj();
        cardObj.transform.DOScale(0.1f, 0.2f).OnComplete(() =>
        {
            // GameObject obj = cardObj;
            // cardObj = null;
            Destroy(cardObj);
        });
        await Task.Delay(250);
        GetNewObj();
    }


    private void SetMainBtn(bool isNewCard)
    {
        if (isNewCard)
        {
            _mainBtnCleanFlag = true;
            mainBtnText.text = "CHECK CARD";
        }
        else
        {
            _mainBtnCleanFlag = false;
            mainBtnText.text = "NEW CARD";
        }
    }

    private void CarbonKnap()
    {
        // if (timeBar.TakeCard())
        if (CardTimeManager.GetInstance().TakeCard())
        {
            SetMainBtn(true);
            PlayChangeCardAnim();
        }
        else
        {
            PanelManager.Instance.ShowCardStore();
            mainBtn.enabled = true;
            OpenMainBtn();
        }
    }


    private void FinishCard()
    {
        cardObj.GetComponent<BaseCard>().FinishCard();
    }

    private async void CheckThirdGuide()
    {
        if (SaveDataManager.GetBool(CConfig.sv_InitFirstCard) &&
            !SaveDataManager.GetBool(CConfig.sv_FinishFirstCheckCard))
        {
            guideStep3.gameObject.SetActive(false);

            LocalCommonData.IsGamePass = true;
            mainBtn.enabled = false;
            await UniTask.Delay(2300);
            ShowFourthMask();
        }
    }

    private void CheckFourthGuide()
    {
        if (SaveDataManager.GetBool(CConfig.sv_FinishFirstCheckCard) &&
            !SaveDataManager.GetBool(CConfig.sv_FinishFirstNewCard))
        {
            guideStep4.gameObject.SetActive(false);
            SaveDataManager.SetBool(CConfig.sv_FinishFirstNewCard, true);
        }
    }


    public void CloseMainBtn()
    {
        mainBtn.GetComponent<Image>().DOFade(0.7f, 0.1f);
        mainBtnText.DOFade(0.7f, 0.1f);
    }

    public void OpenMainBtn()
    {
        mainBtn.GetComponent<Image>().DOFade(1f, 0.1f);
        mainBtnText.DOFade(1f, 0.1f);
    }


    private async void ClickMainBtn()
    {
        LocalCommonData.IsGamePass = true;
        mainBtn.enabled = false;
        CloseMainBtn();

        if (_mainBtnCleanFlag)
        {
            CheckFourthGuide();
            HoldTimerEnd();
            cardObj.GetComponent<BaseCard>().ShowFinishSpine();
            SetMainBtn(false);
            await UniTask.Delay(1500);
            FinishCard();
        }
        else
        {
            mainBtn.gameObject.GetComponent<MainBtnTimer>().StopSpine();
            await UniTask.Delay(100);
            OpenMainBtn();
            CheckThirdGuide();
            CarbonKnap();
        }
    }
}