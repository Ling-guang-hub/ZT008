/// <summary>
/// ���̳�MonoBehaviour�ĵ������������
/// </summary>
/// <typeparam name="T"></typeparam>
public class MintTrickle<T> where T : class, new()
{
    private static T instance;
    public static T Religion    {
        get
        {
            if (instance == null)
                instance = new T();
            return instance;
        }
    }
}
