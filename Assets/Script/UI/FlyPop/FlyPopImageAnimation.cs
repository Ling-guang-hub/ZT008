// Project  BlockDropRush
// FileName  FlyPopImageAnimation.cs
// Author  AX
// Desc
// CreateAt  2025-10-21 14:10:41 
//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class FlyPopImageAnimation: MonoBehaviour
{

    [FormerlySerializedAs("imageList")]


    public List<Sprite> imageList;
    
    [FormerlySerializedAs("Speen")]

    
    public float Speen;
    
    [FormerlySerializedAs("bgImg")]

    
    public Image bgImg;
    
    IEnumerator PlayAction ()
    {
        foreach(Sprite sprite in imageList)
        {
            bgImg.sprite = sprite;
            yield return new WaitForSeconds(Speen);
        }
        StartCoroutine(nameof(PlayAction));
    }
    private void OnEnable()
    {
        // _image = transform.GetComponent<Image>();
        StartCoroutine(nameof(PlayAction));
    }
    private void OnDisable()
    {
        StopCoroutine(nameof(PlayAction));
    }



}
