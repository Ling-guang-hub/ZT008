using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Loadingҳ��
/// </summary>
public class SorghumChunk : MonoBehaviour
{
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        transform.Find("ProgressBar/Bar").GetComponent<Image>().DOFillAmount(1, 2.5f)
            .SetUpdate(true)
            .SetEase(Ease.Linear)
            .OnComplete(() => { gameObject.SetActive(false); });    // 2.5�������Loadingҳ��
    }
}
