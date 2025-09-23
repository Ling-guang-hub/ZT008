// Project  ScratchCard
// FileName  ImageAlphaController.cs
// Author  AX
// Desc
// CreateAt  2025-04-27 18:04:54 
//


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAlphaController: MonoBehaviour
{

    public List<GameObject> imageList;

    public float alpha;
    
    private void Awake()
    {
        foreach (GameObject obj in imageList)
        {
            var image = obj.GetComponent<Image>();
            if (image)
            {
                image.alphaHitTestMinimumThreshold = alpha;
            }

        }
    }


}
