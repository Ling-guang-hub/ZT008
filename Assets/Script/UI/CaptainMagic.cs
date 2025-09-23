using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CaptainMagic : MonoBehaviour
{
    [FormerlySerializedAs("sliderImage")]

    public Image ChillyBrand;
    [FormerlySerializedAs("progressText")]

    public Text ArgumentBent;
    // Start is called before the first frame update

    AsyncOperation JewelYale;

    void Start()
    {
        ChillyBrand.fillAmount = 0;
        ArgumentBent.text = "0%";
        float width = Screen.width;
        float height = Screen.height;
        LocalCommonData.ScreenRate = width / height;
        Application.targetFrameRate = 60;
        CashOutManager.GetInstance().StartTime = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    // Update is called once per frame
    void Update()
    {
        // if (ChillyBrand.fillAmount <= 0.8f || NetInfoMgr.instance.ready)
        if (ChillyBrand.fillAmount <= 0.8f || (NetInfoMgr.instance.ready && CashOutManager.GetInstance().Ready))
        {
            ChillyBrand.fillAmount += Time.deltaTime / 3f;
            ArgumentBent.text = (int)(ChillyBrand.fillAmount * 100) + "%";

            if (NetInfoMgr.instance.ready && CommonUtil.IsApple() && JewelYale == null)
            {
                JewelYale = SceneManager.LoadSceneAsync(1);
                JewelYale.allowSceneActivation = false;
                return;
            }

            if (ChillyBrand.fillAmount >= 1)
            {

                if (CommonUtil.IsApple())
                {
                    JewelYale.allowSceneActivation = true;
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