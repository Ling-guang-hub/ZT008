using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarverChina : MonoBehaviour
{
    void Awake()
    {
        //�õ���Ʒֱ���
        Vector2 designResolution = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution;
        //�õ�ʵ�ʷֱ���
        Vector2 actualResolution = new Vector2(Screen.width, Screen.height);

        //�õ���Ʒֱ��ʵĿ���ʵ�ʷֱ��ʵĿ��ı���
        float widthScale = designResolution.x / actualResolution.x;
        //�õ���ʵ�ʷֱ��ʵĿ����ŵ���Ʒֱ��ʸߵ����ʵ�ʷֱ��ʵĸ�
        float height = actualResolution.y * widthScale;

        //�����������size
        float orthoSize = Camera.main.orthographicSize;
        //���ź�������size
        Camera.main.orthographicSize = height * orthoSize / designResolution.y;
    }
}
