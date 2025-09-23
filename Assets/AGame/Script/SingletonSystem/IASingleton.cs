public interface IASingleton
{
    /// <summary>
    /// 激活接口，通常用于在某个时机手动实例化
    /// </summary>
    void Active();

    /// <summary>
    /// 释放接口
    /// </summary>
    void Release();
}

public interface IUpdate
{
    /// <summary>
    /// 游戏框架模块轮询。
    /// </summary>
    void OnUpdate();
}

public interface IFixedUpdate
{
    /// <summary>
    /// 游戏框架模块轮询。
    /// </summary>
    void OnFixedUpdate();
}

public interface ILateUpdate
{
    /// <summary>
    /// 游戏框架模块轮询。
    /// </summary>
    void OnLateUpdate();
}

public interface IDrawGizmos
{
    void OnDrawGizmos();
}
    
public interface IDrawGizmosSelected
{
    void OnDrawGizmosSelected();
}