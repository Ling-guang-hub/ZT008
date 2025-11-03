using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary> 屏蔽界面 阻止玩家操作 退出游戏 </summary>
public class ShapeScope : BaseUIForms
{
    [FormerlySerializedAs("InfoText")]

    public Text FameIraq;
    [FormerlySerializedAs("QuitBtn")]

    public Button CentJet;

    private void Start()
    {
        CentJet.onClick.AddListener(Application.Quit);
    }

    public void BoreFame(string info)
    {
        FameIraq.text = info;
    }
}
