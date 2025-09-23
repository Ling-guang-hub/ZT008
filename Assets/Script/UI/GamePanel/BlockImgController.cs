// Project  ScratchCard
// FileName  BlockImgController.cs
// Author  AX
// Desc
// CreateAt  2025-05-16 10:05:09 
//


using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BlockImgController : MonoBehaviour
{
    [FormerlySerializedAs("longImg")]

    public Image longImg;

    [FormerlySerializedAs("shortImg")]


    public Image shortImg;


    private void Start()
    {
        longImg.gameObject.SetActive(LocalCommonData.ScreenRate <= 0.5f);
        shortImg.gameObject.SetActive(LocalCommonData.ScreenRate > 0.5f);
    }
}