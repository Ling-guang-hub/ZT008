using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary> 屏蔽界面 阻止玩家操作 退出游戏 </summary>
public class SpoonMagic : BaseUIForms
{
    [FormerlySerializedAs("InfoText")]

    public Text SageBent;
    [FormerlySerializedAs("QuitBtn")]

    public Button LobeBuy;

    private void Start()
    {
        LobeBuy.onClick.AddListener(Application.Quit);
    }

    public void VoleSage(string info)
    {
        SageBent.text = info;
    }
}
