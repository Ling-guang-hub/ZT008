using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����Ӧ����ʾ����Ļ������
/// </summary>
public class Heat : MonoBehaviour
{
    void Start()
    {
        transform.position = new Vector3(0, -Camera.main.orthographicSize, 0);
    }
}
