using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using System;

public class AnimationController : MonoBehaviour
{
    void Start()
    {
    }


    void Update()
    {
    }

    /// <summary>
    /// 弹窗出现效果
    /// </summary>
    /// <param name="PopBarUp"></param>
    public static void PopShow(GameObject PopBarUp, System.Action finish)
    {
        /*-------------------------------------初始化------------------------------------*/
        PopBarUp.GetComponent<CanvasGroup>().alpha = 0;
        PopBarUp.transform.localScale = new Vector3(0, 0, 0);
        /*-------------------------------------动画效果------------------------------------*/
        PopBarUp.GetComponent<CanvasGroup>().DOFade(1, 0.3f);
        PopBarUp.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack).OnComplete(() => { finish?.Invoke(); });
    }


    /// <summary>
    /// 弹窗消失效果
    /// </summary>
    /// <param name="PopBarDisapper"></param>
    public static void PopHide(GameObject PopBarDisapper, System.Action finish)
    {
        /*-------------------------------------初始化------------------------------------*/
        PopBarDisapper.GetComponent<CanvasGroup>().alpha = 1;
        PopBarDisapper.transform.localScale = new Vector3(1, 1, 1);
        /*-------------------------------------动画效果------------------------------------*/
        PopBarDisapper.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        PopBarDisapper.transform.DOScale(0, 0.5f).SetEase(Ease.InBack).OnComplete(() => { finish(); });
    }

    /// <summary>
    /// 数字变化动画
    /// </summary>
    /// <param name="startNum"></param>
    /// <param name="endNum"></param>
    /// <param name="text"></param>
    /// <param name="finish"></param>
    public static void ChangeNumber(int startNum, int endNum, float delay, Text text, System.Action finish)
    {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Num_roll, delay+0.5f);
        DOTween.To(() => startNum, x => text.text = x.ToString(), endNum, 0.5f).SetDelay(delay).OnComplete(() =>
        {
            finish?.Invoke();
        });
    }

    public static void ChangeNumber(decimal startNum, decimal endNum, float delay, Text text, System.Action finish)
    {
        ChangeNumber((double)startNum, (double)endNum, delay, text, "", finish);
    }

    public static void ChangeNumber(double startNum, double endNum, float delay, Text text, System.Action finish)
    {
        ChangeNumber(startNum, endNum, delay, text, "", finish);
    }

    public static void ChangeNumber(double startNum, double endNum, float delay, Text text, string prefix,
        System.Action finish)
    {
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Num_roll, delay+0.5f);
        DOTween.To(() => startNum, x => text.text = prefix + NumberUtil.DoubleToStr(x), endNum, 0.5f).SetDelay(delay)
            .OnComplete(() => { finish?.Invoke(); });
    }


    /// <summary>
    /// 收金币
    /// </summary>
    /// <param name="GoldImage">金币图标</param>
    /// <param name="a">金币数量</param>
    /// <param name="StartPosition">起始位置</param>
    /// <param name="EndPosition">最终位置</param>
    /// <param name="finish">结束回调</param>
    public static async UniTask GoldMoveBest(GameObject GoldImage, int a, Vector2 StartPosition, Vector2 EndPosition)
    {
        //如果没有就算了
        if (a == 0)
        {
            return;
        }
        //数量不超过15个
        else if (a > 15)
        {
            a = 15;
        }

        for (int i = 0; i < a; i++)
        {
            //复制一个图标
            GameObject GoldIcon = Instantiate(GoldImage, GoldImage.transform.parent);
            GoldIcon.SetActive(true);
            //位置初始化
            GoldIcon.transform.position = StartPosition;

            await UniTask.Delay(TimeSpan.FromSeconds(0.06f), false);
            // await UniTask.Delay(1000, false);

            if (i == a - 1)
            {
                await SingleGoldMove(GoldIcon, EndPosition, i);
            }
            else
            {
                SingleGoldMove(GoldIcon, EndPosition, i).Forget();
            }
        }
    }

    private static async UniTask SingleGoldMove(GameObject GoldIcon, Vector2 EndPosition, int i)
    {
        //金币弹出随机位置
        float OffsetX = UnityEngine.Random.Range(-0.8f, 0.8f);
        float OffsetY = UnityEngine.Random.Range(-0.8f, 0.8f);
        
        await (
            GoldIcon.transform.DOMove(
                new Vector3(GoldIcon.transform.position.x + OffsetX, GoldIcon.transform.position.y + OffsetY,
                    GoldIcon.transform.position.z), 0.15f).SetDelay(0.1f).ToUniTask(),
            GoldIcon.transform.DOScale(1.2f, 0.15f).SetEase(Ease.Linear).SetDelay(0.1f).ToUniTask()
            // GoldIcon.transform.DOScale(1f, 0.3f).SetEase(Ease.InBack).SetDelay(0.2f).ToUniTask()
        );
        
        if (i % 5 == 0)
        {
            MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.Sound_GoldCoin);
        }

        await (
            GoldIcon.transform.DOMove(EndPosition, 0.6f).SetDelay(0.2f).ToUniTask(),
            GoldIcon.transform.DOScale(1f, 0.4f).SetEase(Ease.InBack).ToUniTask()
            // GoldIcon.transform.DOScale(1f, 0.3f).SetEase(Ease.InBack).SetDelay(0.2f).ToUniTask()
        );
        Destroy(GoldIcon);
    }


    /// <summary>
    /// 横向滚动
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="addPosition"></param>
    /// <param name="Finish"></param>
    public static async UniTask HorizontalScroll(GameObject obj, float addPosition)
    {
        float positionX = obj.transform.localPosition.x;
        float endPostion = positionX + addPosition;
        await obj.transform.DOLocalMoveX(endPostion, 2f).SetEase(Ease.InOutQuart);
    }

    /// <summary>
    /// UniTask和Dotween结合使用示例
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static async UniTaskVoid UniTaskDOTweenDemo(GameObject obj)
    {
        // 连续执行
        await obj.transform.DOMove(obj.transform.position + Vector3.up, 1.0f);
        await obj.transform.DOScale(Vector3.one * 2.0f, 1.0f);

        // UniTask.WhenAll同时运行并等待终止
        await (
            obj.transform.DOMove(Vector3.zero, 1.0f).ToUniTask(),
            obj.transform.DOScale(Vector3.one, 1.0f).ToUniTask()
        );
    }
}