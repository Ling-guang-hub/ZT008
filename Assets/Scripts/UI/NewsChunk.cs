using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ҳ��
/// </summary>
public class NewsChunk : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(RayonAero);
    }

    void RayonAero()
    {
        AeroTrickle.Religion.RayonAero();
    }
}
