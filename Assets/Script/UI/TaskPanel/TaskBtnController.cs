// Project  ScratchCard
// FileName  TaskBtnController.cs
// Author  AX
// Desc
// CreateAt  2025-06-12 16:06:55 
//


using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TaskBtnController : MonoBehaviour
{
    
    [FormerlySerializedAs("redPointObj")]

    
    public GameObject redPointObj;

    [FormerlySerializedAs("iconImg")]


    public Image iconImg;

    private Vector3 _iconScale;
    
    private void Awake()
    {
        _iconScale = Vector3.one;
    }
    
    private void Start()
    {
        MessageCenterLogic.GetInstance().Register(CConfig.mg_TakeTask, (md) => { ShowPoint(); });
    }


    public void ShowPoint()
    {
        bool flag = TaskManager.GetInstance().CheckTaskDoneAndNotGet();
        if (gameObject.activeInHierarchy&&flag)
        {
            MyAla();
        }
        else
        {
            // DOTween.Kill(iconImg.transform);
            iconImg.transform.DOKill();
            iconImg.transform.localScale = _iconScale;
        }
    }
    
    
    private void MyAla()
    {
        // DOTween.Kill(iconImg.transform);
        iconImg.transform.DOKill();
        iconImg.transform.localScale = _iconScale;
        iconImg.transform.DOScale(1.3f, 0.6f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);

    }
    
    
    
}