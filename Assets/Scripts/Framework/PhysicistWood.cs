using UnityEngine;

/// <summary>
/// �̳�MonoBehaviour�ĵ���ģʽ����
/// </summary>
/// <typeparam name="T"></typeparam>
public class PhysicistWood<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Religion=> instance;


    protected virtual void Awake()
    {
        instance = this as T;
    }
}

