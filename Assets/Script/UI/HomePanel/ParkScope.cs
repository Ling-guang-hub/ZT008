// 

using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ParkScope : BaseUIForms
{
    public static ParkScope Instance;

    [FormerlySerializedAs("mainAreaObj")]


    public GameObject SiltPlotAll;

    [FormerlySerializedAs("settingBtn")]


    public Button settingBtn;
    [FormerlySerializedAs("playBtn")]

    public Button playBtn;
    [FormerlySerializedAs("leftBtn")]

    public Button leftBtn;
    [FormerlySerializedAs("rightBtn")]

    public Button rightBtn;

    [FormerlySerializedAs("cardNameText")]


    public Text cardNameText;


    [FormerlySerializedAs("topBar")]



    public GameObject topBar;

    [FormerlySerializedAs("levelBar")]


    public GameObject levelBar;
    
    // public WheelBar wheelBar;

    // public GameObject timeBarObj;

    [FormerlySerializedAs("passportBtn")]


    public Button passportBtn;

    [FormerlySerializedAs("taskBtn")]


    public Button taskBtn;

    [FormerlySerializedAs("cashShopObj")]


    public GameObject cashShopObj;

    [FormerlySerializedAs("mainCardArea")]


    public GameObject mainCardArea;

    [FormerlySerializedAs("miniCardContent")]


    public GameObject miniCardContent;

    [FormerlySerializedAs("miniCardView")]


    public GameObject miniCardView;

    [FormerlySerializedAs("miniCardScroll")]


    public GameObject miniCardScroll;

    [FormerlySerializedAs("baseMiniCardObj")]


    public GameObject baseMiniCardObj;

    [FormerlySerializedAs("firstGuideMask")]


    public GameObject firstGuideMask;

    [FormerlySerializedAs("textBoardObj")]


    public GameObject textBoardObj;

    private List<GameObject> _activeCards;

    private Dictionary<int,  GameObject> _miniCardDict;
    private Dictionary<int,  int> _miniCardIdxDict;

    private static readonly int DisplayCardCount = 5;

    // private TimeBarController _timeBarController;


    private void Awake()
    {
        Instance = this;
        // _timeBarController = timeBarObj.GetComponent<TimeBarController>();
        _miniCardDict = new Dictionary<int, GameObject>();
        _miniCardIdxDict = new Dictionary<int, int>();
        _activeCards = new List<GameObject>();
        DoAdaptation();
        LintIraq();
    }

    void Start()
    {
        // CardTimeManager.GetInstance().LintIraq();

        LayoutRebuilder.ForceRebuildLayoutImmediate(miniCardContent.GetComponent<RectTransform>());

        mainCardArea.GetComponent<BigCardController>().CreateFirstMainCard();

        FirstSelected();

        playBtn.onClick.AddListener(() =>
        {
            // if (LocalCommonData.IsGamePass) return;
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            TakeOneCard();
        });


        leftBtn.onClick.AddListener(() =>
        {
            if (LocalCommonData.IsGamePass) return;
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            ClickNextBtn(true);
        });

        rightBtn.onClick.AddListener(() =>
        {
            if (LocalCommonData.IsGamePass) return;
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            ClickNextBtn(false);
        });

        settingBtn.onClick.AddListener(() =>
        {
            if (LocalCommonData.IsGamePass) return;
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_2);
            // OpenUIForm(nameof(TaleScope));
            OpenUIForm(nameof(PuritanScope));
        });

        passportBtn.onClick.AddListener(() =>
        {
            if (LocalCommonData.IsGamePass) return;
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            OpenUIForm(nameof(SagoLiraScope));
 
        });

        taskBtn.onClick.AddListener(() =>
        {
            if (LocalCommonData.IsGamePass) return;
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_1);
            OpenUIForm(nameof(BodyScope));
        });


        MessageCenterLogic.GetInstance()
            .Register(CConfig.mg_GameSuspend, (md) => { LocalCommonData.IsGamePass = false; });


        MessageCenterLogic.GetInstance().Register(CConfig.mg_ClosePanel, (md) =>
        {
            if (gameObject.activeInHierarchy)
            {
                passportBtn.GetComponent<PassportSlider>().ShowSlider();
                taskBtn.GetComponent<TaskBtnController>().ShowPoint();
            }

            LocalCommonData.IsGamePass = false;
        });
    }


    private void DoAdaptation()
    {
        // 1080*1920
        if (LocalCommonData.ScreenRate > 0.5f)
        {
            topBar.transform.localPosition = new Vector3(225f, -80f, 0);
            
            settingBtn.gameObject.transform.localPosition = new Vector3(-100f, -80f, 0);

            cashShopObj.transform.localScale = new Vector3(0.75f, 0.75f, 0);
            cashShopObj.transform.localPosition = new Vector3(0f, -190f, 0);
            
            levelBar.transform.localPosition = new Vector3(100f, -255f, 0);

            passportBtn.gameObject.transform.localPosition = new Vector3(-150f, 250f, 0);
            taskBtn.gameObject.transform.localPosition = new Vector3(150f, 250f, 0);
            playBtn.transform.localPosition = new Vector3(0, 250f, 0);
            
            cardNameText.transform.localPosition = new Vector3(0, -300, 0);
            
            miniCardScroll.transform.localPosition = new Vector3(0, 500f, 0);
            
        }
        else
        {
            
            // 1080*2340
            topBar.transform.localPosition = new Vector3(225f, -80f, 0);
            
            settingBtn.gameObject.transform.localPosition = new Vector3(-120f, -80f, 0);
            
            cashShopObj.transform.localScale = new Vector3(0.8f, 0.8f, 0);
            cashShopObj.transform.localPosition = new Vector3(0f, -190f, 0);
            
            levelBar.transform.localPosition = new Vector3(170f, -330f, 0);
            
            passportBtn.gameObject.transform.localPosition = new Vector3(-150f, 250f, 0);
            taskBtn.gameObject.transform.localPosition = new Vector3(150f, 250f, 0);
            playBtn.transform.localPosition = new Vector3(0, 250, 0);
            
            cardNameText.transform.localPosition = new Vector3(0, -400, 0);
            
            miniCardScroll.transform.localPosition = new Vector3(0, 543f, 0);

        }
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        ShowGuide();
        ShowAct();
        passportBtn.gameObject.SetActive(!CommonUtil.IsApple());
        taskBtn.gameObject.SetActive(!CommonUtil.IsApple());
        LocalCommonData.IsGamePanel = false;

        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Panel_pop);
        // _timeBarController.RefreshNumAndTime();
        // CardTimeManager.GetInstance().FirstShowCardNum();
        // _timeBarController.cardNumText.text = GameDataManager.GetInstance().GetCard() + "";
        // wheelBar.ShowSliderUI();
        topBar.GetComponent<TopBar>().ShowWallet();

        passportBtn.GetComponent<PassportSlider>().ShowSlider();
        taskBtn.GetComponent<TaskBtnController>().ShowPoint();
        ReShowMiniCardArea();
    }


    private void ReShowMiniCardArea()
    {
        foreach (GameObject card in _activeCards)
        {
            card.transform.GetComponent<MiniCard>().DoUnlock();
        }
    }


    private void ShowFirstMask()
    {
        firstGuideMask.gameObject.SetActive(true);
        textBoardObj.gameObject.SetActive(true);
        textBoardObj.transform.DOScale(1, 0.3f).SetDelay(0.5f).OnComplete(() => { playBtn.enabled = true; });
    }

    private async void ShowGuide()
    {
        if (!SaveDataManager.GetBool(CConfig.sv_FinishFirstBigWin))
        {
            LocalCommonData.IsGamePass = true;
            playBtn.enabled = false;
            await UniTask.Delay(1000);
            ShowFirstMask();
            // Invoke("ShowFirstMask", 1f);
        }
        else
        {
            firstGuideMask.gameObject.SetActive(false);
            textBoardObj.gameObject.SetActive(false);
            LocalCommonData.IsGamePass = false;
        }
    }

    private void ShowAct()
    {
        SiltPlotAll.transform.localScale = Vector3.zero;
        SiltPlotAll.transform.DOScale(1f, 0.2f);
    }

    private void ShowUIAfterClose()
    {
        passportBtn.GetComponent<PassportSlider>().ShowSlider();
        taskBtn.GetComponent<TaskBtnController>().ShowPoint();
        levelBar.GetComponent<LevelBarCtrl>().ShowLevel();
    }

    private void ShowCardText()
    {
        string name = LocalCardData.CardParamDict[LocalCommonData.CurrentCardId].CardName;
        cardNameText.text = name;
        playBtn.GetComponent<PlayBtnCtrl>().ShowUI();
        
    }

    public void SelectOneCard()
    {
        NotSelectedCards();
        SetGroupPos();
        RefreshTimeBar();
        ShowCardText();
    }

    public void ResetBigCard()
    {
        mainCardArea.GetComponent<BigCardController>().ResetBigCardList();
    }

    private void NotSelectedCards()
    {
        foreach (GameObject card in _activeCards)
        {
            int thisId = card.transform.GetComponent<MiniCard>().cardId;
            if (thisId != LocalCommonData.CurrentCardId)
            {
                card.transform.GetComponent<MiniCard>().BeNotSelected();
            }
        }
    }

    private void SetGroupPos()
    {
        if (_activeCards.Count <= DisplayCardCount) return;
        GameObject card = _miniCardDict[LocalCommonData.CurrentCardId];
        float realPosX = miniCardContent.transform.localPosition.x + card.transform.localPosition.x;
        float offsetX = (miniCardContent.GetComponent<RectTransform>().rect.width -
                         miniCardView.GetComponent<RectTransform>().rect.width) * 0.5f;

        if (realPosX > miniCardView.GetComponent<RectTransform>().rect.width / 2)
        {
            CardsDoMove(-offsetX);
        }
        else if (realPosX < -miniCardView.GetComponent<RectTransform>().rect.width / 2)
        {
            CardsDoMove(offsetX);
        }
    }


    private void RefreshTimeBar()
    {
        // CardTimeManager.GetInstance().RefreshNumAndTime();
    }


    private void LintIraq()
    {
        for (int i = 0; i < LocalCardData.ActCardIds.Count; i++)
        {
            int cardId = LocalCardData.ActCardIds[i];
            GameObject miniCard = Instantiate(baseMiniCardObj, miniCardContent.transform, false);
            miniCard.transform.GetComponent<MiniCard>().cardId = cardId;
            miniCard.transform.GetComponent<MiniCard>().cardType = LocalCardData.CardTypeDict[cardId];
            miniCard.transform.GetComponent<MiniCard>().unlockLine = LocalCardData.CardParamDict[cardId].UnlockLine;
            miniCard.transform.GetComponent<MiniCard>().LintIraq();
            miniCard.gameObject.SetActive(true);
            _miniCardDict.TryAdd(cardId, miniCard);
            _miniCardIdxDict.TryAdd(cardId, i);
            _activeCards.Add(miniCard);
        }
    }


    private void CardsDoMove(float num)
    {
        miniCardContent.transform.DOLocalMoveX(num, 0.2f)
            .SetEase(Ease.OutBounce);
    }


    public int GetNextCardIdx(bool isLeft = true)
    {
        int thisId = LocalCommonData.CurrentCardId;
        int currIdx  = _miniCardIdxDict[thisId];
        // int currIdx = pair.Key;
        if (isLeft)
        {
            return currIdx == 0 ? _activeCards.Count - 1 : currIdx - 1;
        }

        return currIdx == _activeCards.Count - 1 ? 0 : currIdx + 1;
    }

    private void NextMiniCard(bool isLeft)
    {
        GameObject card = _activeCards[GetNextCardIdx(isLeft)];
        card.transform.GetComponent<MiniCard>().BeSelect();
    }

    private void FirstSelected()
    {
        if (!SaveDataManager.GetBool(CConfig.sv_FinishFirstBigWin))
        {
            
            GameObject card = _miniCardDict[NetInfoMgr.instance.GameData.new_user_focus];
            card.transform.GetComponent<MiniCard>().BeSelect();
            ResetBigCard();
        }

        SetGroupPos();
        ShowCardText();
    }

    public void ClickNextBtn(bool isLeft)
    {
        if (mainCardArea.transform.childCount > 3) return;
        NextMiniCard(isLeft);
        mainCardArea.GetComponent<BigCardController>().NextMainCard(isLeft);
    }

    public void BigCardDoMove(float offsetX)
    {
        mainCardArea.GetComponent<BigCardController>().CardsDoMove(offsetX);
    }

    public void BigCardDoHome()
    {
        mainCardArea.GetComponent<BigCardController>().CardsResetPos();
    }


    public void ShowHomeArea()
    {
        LocalCommonData.IsGamePanel = false;
        SiltPlotAll.gameObject.SetActive(true);

        GameObject card = _miniCardDict[LocalCommonData.CurrentCardId];
        ResetBigCard();
        ReShowMiniCardArea();
        card.transform.GetComponent<MiniCard>().BeSelect();
        ShowGuide();
        ShowAct();
        ShowUIAfterClose();
        ShowRatePanel();
    }
    



    private async void ShowRatePanel()
    {
        int homeCount = SaveDataManager.GetInt(CConfig.sv_BackHomeCount) + 1;
        SaveDataManager.SetInt(CConfig.sv_BackHomeCount, homeCount);

        if (SaveDataManager.GetBool(CConfig.sv_HasShowRatePanel)) return;


        int limitCount = int.Parse(NetInfoMgr.instance.ConfigData.rate_us_limit);
        if (homeCount < limitCount) return;
        SaveDataManager.SetBool(CConfig.sv_HasShowRatePanel, true);
        await UniTask.Delay(500);
        OpenUIForm(nameof(GustMeScope));
    }

    private void TakeOneCard()
    {
        // if (CardTimeManager.GetInstance().TakeCard())
        if (CardManager.Instance.TakeCard())
        {
            LocalRewardData.ResetCompleteData();

            OpenUIForm(nameof(FishScope));
            SiltPlotAll.transform.DOLocalMoveX(0.1f, 0.2f).SetEase(Ease.OutBounce);
            SiltPlotAll.gameObject.SetActive(false);
        }
        else
        {
            PanelManager.Instance.ShowCardStore();
        }
    }
}