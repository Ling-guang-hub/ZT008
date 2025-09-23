using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RewardItemUI : MonoBehaviour
{
    [FormerlySerializedAs("Icon")]

    public Image Icon;
    [FormerlySerializedAs("NumText")]

    public Text NumText;

    public void Render(Sprite icon, int num = 0)
    {
        Icon.sprite = icon;
        if (num == 0) {
            NumText.gameObject.SetActive(false);
        }
        else
        {
            NumText.text = "+" + num.ToString();
            NumText.gameObject.SetActive(true);
        }
    }
}
