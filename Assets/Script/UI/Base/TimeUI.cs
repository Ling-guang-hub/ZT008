using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using DG.Tweening;

public class TimeUI : MonoBehaviour
{
    [FormerlySerializedAs("ClockText")]

    public Text ClockText;
    [FormerlySerializedAs("Pointer")]

    public RectTransform Pointer;

    private long countdown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitEndTime(long endTime)
    {
        countdown = endTime - DateUtil.Current();

        StopCoroutine(nameof(RefreshClock));
        StartCoroutine(nameof(RefreshClock));
    }

    private IEnumerator RefreshClock()
    {
        float angle = 0;
        while(countdown > 0)
        {
            ClockText.text = DateUtil.SecondsFormat(countdown);
            Pointer.DORotate(new Vector3(0, 0, angle), 0.5f);
            angle = angle - 90 == -360 ? 0 : angle - 90;
            countdown--;
            yield return new WaitForSeconds(1);
        }
        if (countdown <= 0)
        {
            ClockText.text = "Finished";
            Pointer.rotation = Quaternion.identity;
        }
    }
}
