using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using zeta_framework;

public class DebugWindow : OdinEditorWindow
{
    [MenuItem("Tools/DebugWindow")]
    private static void OpenWindow()
    {
        GetWindow<DebugWindow>().Show();
    }

    /******************************* 资源 *******************************/
    [HorizontalGroup("Item", LabelWidth = 60)]
    [LabelText("资源类型")]
    public string itemType;
    [HorizontalGroup("Item", LabelWidth = 60)]
    [InlineButton(nameof(ResetItem), "设置")]
    [InlineButton(nameof(AddItem), "增加")]
    public int ItemNum = 1000;

    // 增加资源数
    private void AddItem()
    {
        ResourceCtrl.Instance.AddItemValue(itemType, ItemNum);
    }
    // 设置资源数
    private void ResetItem()
    {
        ResourceCtrl.Instance.SetItemValue(itemType, ItemNum);
    }
}
