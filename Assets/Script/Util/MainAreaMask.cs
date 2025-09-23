// Project: HappyBingo-Real
// FileName: BRectMaskTool.cs
// Author: AX
// CreateDate: 20230220
// CreateTime: 9:55
// Description:

using UnityEngine;
using UnityEngine.UI;

public class MainAreaMask : BaseUIForms
{
    public float targetOffsetX = 0;
    public float targetOffsetY = 0;
    private Material material;

    public float currentOffsetX = 0f;
    public float currentOffsetY = 0f;

    public float targetPosX = 0f;
    public float targetPosY = 0f;

    public float shrinkTime = 0.3f;
    private GuidanceEventPenetrate eventPenetrate;
    public GameObject targetObj;


    private float shrinkVelocityX = 0f;
    private float shrinkVelocityY = 0f;


    private void Start()
    {
        Vector4 centerMat = new Vector4(targetPosX, targetPosY, 0, 0);
        material = GetComponent<Image>().material;
        material.SetVector("_Center", centerMat);


        eventPenetrate = GetComponent<GuidanceEventPenetrate>();
        if (eventPenetrate != null)
        {
            eventPenetrate.SetTargetImage(targetObj.gameObject.GetComponent<Image>());
        }
    }

    private void Update()
    {
        //从当前偏移量到目标偏移量差值显示收缩动画
        float valueX = Mathf.SmoothDamp(currentOffsetX, targetOffsetX, ref shrinkVelocityX, shrinkTime);
        float valueY = Mathf.SmoothDamp(currentOffsetY, targetOffsetY, ref shrinkVelocityY, shrinkTime);
        if (!Mathf.Approximately(valueX, currentOffsetX))
        {
            currentOffsetX = valueX;
            material.SetFloat("_SliderX", currentOffsetX);
        }

        if (!Mathf.Approximately(valueY, currentOffsetY))
        {
            currentOffsetY = valueY;
            material.SetFloat("_SliderY", currentOffsetY);
        }
    }
}
