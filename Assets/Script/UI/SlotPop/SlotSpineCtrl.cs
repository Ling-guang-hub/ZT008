// Project  BlockDropRush
// FileName  SlotSpineCtrl.cs
// Author  AX
// Desc
// CreateAt  2025-10-29 15:10:00 
//


using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;

public class SlotSpineCtrl: MonoBehaviour
{
   
    [FormerlySerializedAs("coin01Spine")]

   
    public GameObject coin01Spine;
    [FormerlySerializedAs("coin02Spine")]

    public GameObject coin02Spine;
    [FormerlySerializedAs("coin03Spine")]

    public GameObject coin03Spine;
    [FormerlySerializedAs("cash01Spine")]

    public GameObject cash01Spine;
    [FormerlySerializedAs("cash02Spine")]

    public GameObject cash02Spine;
    [FormerlySerializedAs("cash03Spine")]

    public GameObject cash03Spine;
    [FormerlySerializedAs("big777Spine")]

    public GameObject big777Spine;

    private GameObject _curSpineObj;

    public void Init(SlotType slotType)
    {
        _curSpineObj = null;
        coin01Spine.gameObject.SetActive(false);
        coin02Spine.gameObject.SetActive(false);
        coin03Spine.gameObject.SetActive(false);
        cash01Spine.gameObject.SetActive(false);
        cash02Spine.gameObject.SetActive(false);
        cash03Spine.gameObject.SetActive(false);
        big777Spine.gameObject.SetActive(false);

        switch (slotType)
        {
            case SlotType.Coin01:
                coin01Spine.gameObject.SetActive(true);
                _curSpineObj = coin01Spine;
                break;
            case SlotType.Coin02:
                coin02Spine.gameObject.SetActive(true);
                _curSpineObj = coin02Spine;
                break;
            case SlotType.Coin03:
                coin03Spine.gameObject.SetActive(true);
                _curSpineObj = coin03Spine;
                break;
            case SlotType.Cash01:
                cash01Spine.gameObject.SetActive(true);
                _curSpineObj = cash01Spine;
                break;
            case SlotType.Cash02:
                cash02Spine.gameObject.SetActive(true);
                _curSpineObj = cash02Spine;
                break;
            case SlotType.Cash03:
                cash03Spine.gameObject.SetActive(true);
                _curSpineObj = cash03Spine;
                break;
            default:
                big777Spine.gameObject.SetActive(true);
                _curSpineObj = big777Spine;
                break;
        }
        
    }

    public void MyAla()
    {
        _curSpineObj.GetComponent<SkeletonGraphic>().Initialize(true);
        _curSpineObj.GetComponent<SkeletonGraphic>().AnimationState.SetEmptyAnimation(0, 0);
        _curSpineObj.GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "animation", false);
    }
        

}
