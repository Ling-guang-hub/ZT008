// Project  ScratchCard
// FileName  PassportDragAct.cs
// Author  AX
// Desc
// CreateAt  2025-04-21 10:04:50 
//


using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.EventSystems;

public class PassportDragAct : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private float _zCoord;

    private float _startY;

    private float _offsetY;

    [FormerlySerializedAs("topPosY")]


    public float topPosY;

    [FormerlySerializedAs("downPosY")]


    public float downPosY;

    [FormerlySerializedAs("itemCount")]


    public int itemCount;


    public void SetCurrentItem(float y)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, y,
            transform.localPosition.z);
        ResetPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = _zCoord;
        return Camera.main.ScreenToWorldPoint(screenPos);
    }


    private void ObjOnDrag()
    {
        float endPosY = GetMouseWorldPos().y;
        _offsetY = endPosY - _startY;
        transform.position = new Vector3(transform.position.x, transform.position.y + _offsetY,
            transform.position.z);

        _startY = endPosY;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        _startY = GetMouseWorldPos().y;
    }

    public void OnDrag(PointerEventData eventData)
    {
        ObjOnDrag();
    }


    private void ResetPos()
    {
        if (transform.localPosition.y < topPosY)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, topPosY,
                transform.localPosition.z);
        }
        else if (itemCount < 4 && transform.localPosition.y > topPosY)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, topPosY,
                transform.localPosition.z);
        }
        else if (itemCount > 3 && transform.localPosition.y > downPosY)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, downPosY,
                transform.localPosition.z);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ResetPos();
    }
}