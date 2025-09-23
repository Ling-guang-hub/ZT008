using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ʾ���
/// </summary>
public class LeftChunk : MonoBehaviour
{
    /// <summary>
    /// ��ʾ��ʾ��Ϣ
    /// </summary>
    /// <param name="tips"></param>
    public void CrowLeft(string tips)
    {
        transform.Find("TipBox/TipTxt").GetComponent<Text>().text = tips;
    }

    private void OnEnable()
    {
        StartCoroutine(Marshal());
    }

    /// <summary>
    /// ȡ����ʾ��ʾ��Ϣ
    /// </summary>
    private IEnumerator Marshal()
    {
        yield return new WaitForSecondsRealtime(2f);
        gameObject.SetActive(false);
    }
}
