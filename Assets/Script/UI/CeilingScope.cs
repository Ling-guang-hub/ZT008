using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CeilingScope : MonoBehaviour
{
    [FormerlySerializedAs("sliderImage")]

    public Image UnfoldScent;
    [FormerlySerializedAs("progressText")]

    public Text PenchantIraq;
    // Start is called before the first frame update

    AsyncOperation SouthGibe;

    void Start()
    {
        UnfoldScent.fillAmount = 0;
        PenchantIraq.text = "0%";
        float width = Screen.width;
        float height = Screen.height;
        LocalCommonData.ScreenRate = width / height;
        Application.targetFrameRate = 60;
        CashOutManager.GetInstance().StartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    // Update is called once per frame
    void Update()
    {
        // if (UnfoldScent.fillAmount <= 0.8f || NetInfoMgr.instance.ready)
        if (UnfoldScent.fillAmount <= 0.8f || (NetInfoMgr.instance.ready && CashOutManager.GetInstance().Ready))
        {
            UnfoldScent.fillAmount += Time.deltaTime / 3f;
            PenchantIraq.text = (int)(UnfoldScent.fillAmount * 100) + "%";

            if (NetInfoMgr.instance.ready && CommonUtil.IsApple() && SouthGibe == null)
            {
                SouthGibe = SceneManager.LoadSceneAsync(1);
                SouthGibe.allowSceneActivation = false;
                return;
            }

            if (UnfoldScent.fillAmount >= 1)
            {

                if (CommonUtil.IsApple())
                {
                    SouthGibe.allowSceneActivation = true;
                    Destroy(transform.parent.gameObject, 0.3f);
                }
                else
                {
                    CashOutManager.GetInstance().ReportEvent_LoadingTime();
                    PostEventScript.GetInstance().SendEvent("1001");
                    MainManager.Instance.GameInit();
                    Destroy(transform.parent.gameObject);
                }

             
            }
        }
    }
}