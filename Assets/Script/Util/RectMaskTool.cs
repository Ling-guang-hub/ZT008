
// Project: HappyBingo-Real
// FileName: BRectMaskTool.cs
// Author: AX
// CreateDate: 20230220
// CreateTime: 9:55
// Description:

using UnityEngine;
using UnityEngine.UI;

public class RectMaskTool : BaseUIForms
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

    public Image targetImg;

    private float shrinkVelocityX = 0f;
    private float shrinkVelocityY = 0f;

    protected override void Awake()
    {
        DoAdaptation();
    }

    private void Start()
    {
        // InitData();
    }


    private void DoAdaptation()
    {
        if (LocalCommonData.ScreenRate > 0.5f)
        {
            targetOffsetX = 400f;
            targetOffsetY = 360f;
            targetPosX = 0f;
            targetPosY = -15f;
        }
        else
        {
            // 1080*2340
            targetOffsetX = 445;
            targetOffsetY = 405;
            targetPosX = 0f;
            targetPosY = -5;
        }
    }


    // public void 

    public void InitData()
    {

        DoAdaptation();
        // Vector3 targetPos = targetObj.transform.localPosition;
        Vector4 centerMat = new Vector4(targetPosX, targetPosY, 0, 0);
        // Vector4 centerMat = new Vector4(targetPos.x, targetPos.y, 0, 0);
        material = GetComponent<Image>().material;
        material.SetVector("_Center", centerMat);


        eventPenetrate = GetComponent<GuidanceEventPenetrate>();
        if (eventPenetrate != null)
        {
            eventPenetrate.SetTargetImage(targetObj != null ? targetObj.gameObject.GetComponent<Image>() : targetImg);
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