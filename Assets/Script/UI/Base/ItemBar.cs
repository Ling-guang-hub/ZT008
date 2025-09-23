using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using zeta_framework;

public class ItemBar : MonoBehaviour
{
    [FormerlySerializedAs("ItemName")]

    public string ItemName;
    [FormerlySerializedAs("Icon")]

    public GameObject Icon;
    [FormerlySerializedAs("ShowCollectAnimation")]

    public bool ShowCollectAnimation;

    [FormerlySerializedAs("itemText")]


    public Text itemText;
    
    private decimal currentValue;

    // Start is called before the first frame update
    void Start()
    {
        // currentValue = ResourceCtrl.Instance.GetItemById(ItemName).CurrentValue;
        // transform.Find("ItemText").GetComponent<Text>().text = NumberUtil.DecimalToStr(currentValue);

        // MessageCenterLogic.GetInstance().Register(CConfig.mg_ItemChange_ + ItemName, async (md) => {
            // decimal newValue = ResourceCtrl.Instance.GetItemById(ItemName).CurrentValue;
            // if (gameObject.activeInHierarchy && ShowCollectAnimation)
            // {
                // await AnimationController.GoldMoveBest(Icon, (int)(newValue - currentValue), Vector2.zero, Icon.transform.position);
            // }
            // transform.Find("ItemText").GetComponent<Text>().text = NumberUtil.DecimalToStr(newValue);
            // currentValue = newValue;
        // });
    }

    
}
