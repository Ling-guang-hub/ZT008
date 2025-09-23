using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BulkMeMagic : BaseUIForms
{
    public Button[] Civic;

    [FormerlySerializedAs("closeBtn")]


    public Button AlikeBuy;
    
    [FormerlySerializedAs("star1Sprite")]

    
    public Sprite Feel1Mormon;
    [FormerlySerializedAs("star2Sprite")]

    public Sprite Feel2Mormon;

    // Start is called before the first frame update
    void Start()
    {
        AlikeBuy.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_2);
            RecurMagic();
        });
        
        foreach (Button star in Civic)
        {
            star.onClick.AddListener(() =>
            {
                string indexStr = System.Text.RegularExpressions.Regex.Replace(star.gameObject.name, @"[^0-9]+", "");
                int index = indexStr == "" ? 0 : int.Parse(indexStr);
                VenomHilly(index);
            });
        }
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Panel_pop);
        for (int i = 0; i < 5; i++)
        {
            Civic[i].gameObject.GetComponent<Image>().sprite = Feel2Mormon;
        }
    }

    
    private void VenomHilly(int index)
    {
        
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_2);
        for (int i = 0; i < 5; i++)
        {
            Civic[i].gameObject.GetComponent<Image>().sprite = i <= index ? Feel1Mormon : Feel2Mormon;
        }
        // PostEventScript.GetInstance().SendEvent("1010", (index + 1).ToString());
        if (index < 3)
        {
            RecurMagic();
        } else
        {
            // 跳转到应用商店
            RateUsManager.instance.OpenAPPinMarket();
            RecurMagic();
        }
        
        // 打点
        //PostEventScript.GetInstance().SendEvent("1210", (index + 1).ToString());
    }


    private async void RecurMagic()
    {
        await UniTask.Delay(500);
        CloseUIForm(GetType().Name);
    }

}
