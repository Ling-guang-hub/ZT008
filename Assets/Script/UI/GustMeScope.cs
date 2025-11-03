using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GustMeScope : BaseUIForms
{
    public Button[] Elect;

    [FormerlySerializedAs("closeBtn")]


    public Button BoastJet;
    
    [FormerlySerializedAs("star1Sprite")]

    
    public Sprite Emit1Beside;
    [FormerlySerializedAs("star2Sprite")]

    public Sprite Emit2Beside;

    // Start is called before the first frame update
    void Start()
    {
        BoastJet.onClick.AddListener(() =>
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_2);
            TroutScope();
        });
        
        foreach (Button star in Elect)
        {
            star.onClick.AddListener(() =>
            {
                string indexStr = System.Text.RegularExpressions.Regex.Replace(star.gameObject.name, @"[^0-9]+", "");
                int index = indexStr == "" ? 0 : int.Parse(indexStr);
                AminoLeaky(index);
            });
        }
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Panel_pop);
        for (int i = 0; i < 5; i++)
        {
            Elect[i].gameObject.GetComponent<Image>().sprite = Emit2Beside;
        }
    }

    
    private void AminoLeaky(int index)
    {
        
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Button_2);
        for (int i = 0; i < 5; i++)
        {
            Elect[i].gameObject.GetComponent<Image>().sprite = i <= index ? Emit1Beside : Emit2Beside;
        }
        // PostEventScript.GetInstance().SendEvent("1010", (index + 1).ToString());
        if (index < 3)
        {
            TroutScope();
        } else
        {
            // 跳转到应用商店
            RateUsManager.instance.OpenAPPinMarket();
            TroutScope();
        }
        
        // 打点
        //PostEventScript.GetInstance().SendEvent("1210", (index + 1).ToString());
    }


    private async void TroutScope()
    {
        await UniTask.Delay(500);
        CloseUIForm(GetType().Name);
    }

}
