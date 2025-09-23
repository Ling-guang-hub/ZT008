using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleMask : BaseUIForms
{

    public GameObject targetObj;

    public float CurrentRadius;
    public float TargetRadius;
    public float shrinkTime =0f;

    private Material material;



    private GuidanceEventPenetrate eventPenetrate;


    private void Start()
    {

        Vector3 targetPos = targetObj.transform.localPosition;
        Vector4 centerMat = new Vector4(targetPos.x, targetPos.y, 0, 0);
        material = GetComponent<Image>().material;
        material.SetVector("_Center", centerMat);


        eventPenetrate = GetComponent<GuidanceEventPenetrate>();
        if (eventPenetrate != null)
        {
            eventPenetrate.SetTargetImage(targetObj.gameObject.GetComponent<Image>());
        }

    }

  
    /// <summary>
    /// 收缩速度
    /// </summary>
    private float shrinkVelocity = 0f;
    private void Update()
    {
  
        float value = Mathf.SmoothDamp(CurrentRadius, TargetRadius, ref shrinkVelocity, shrinkTime);
        if (!Mathf.Approximately(value, CurrentRadius))
        {
            CurrentRadius = value;
            material.SetFloat("_Slider", CurrentRadius);

        }

    }


}
