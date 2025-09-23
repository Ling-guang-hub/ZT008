// Project  BlockDropRush
// FileName  PanelManager.cs
// Author  AX
// Desc
// CreateAt  2025-09-12 10:09:14 
//


using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void ShowCardStore()
    {
        MessageCenterLogic.GetInstance().Send(CConfig.mg_PassAnim);
        UIManager.GetInstance().ShowUIForms(nameof(KnapThingMagic));
    }
    
    
    
    
    
    
    
    
}