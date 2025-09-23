using UnityEngine;

/// <summary>
/// 全局MonoBehavior必须继承于此
/// </summary>
/// <typeparam name="T">子类类型</typeparam>
public class ASingletonBehaviour<T> : MonoBehaviour where T : ASingletonBehaviour<T>
{
    private static T _instance;

    private void Awake()
    {
        if (CheckInstance())
        {
            OnLoad();
        }
    }

    private bool CheckInstance()
    {
        if (this == Instance)
        {
            return true;
        }

        Object.Destroy(gameObject);
        return false;
    }

    protected virtual void OnLoad()
    {
    }

    protected virtual void OnDestroy()
    {
        if (this == _instance)
        {
            Release();
        }
    }

    /// <summary>
    /// 判断对象是否有效
    /// </summary>
    public static bool IsValid
    {
        get { return _instance != null; }
    }

    public static T Active()
    {
        return Instance;
    }

    public static void Release()
    {
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
            _instance = null;
        }
    }

    /// <summary>
    /// 实例
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                System.Type thisType = typeof(T);
                string instName = thisType.Name;
                
                _instance = GameObject.FindObjectOfType<T>();

                if (_instance == null)
                {
                    GameObject go = new GameObject($"[{instName}]");
                    go.transform.position = Vector3.zero;
                    go.AddComponent<T>();
                    _instance = go.GetComponent<T>();
                }
            }

            return _instance;
        }
    }
}