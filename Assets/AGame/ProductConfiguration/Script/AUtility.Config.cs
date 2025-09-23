using UnityEngine;

public static partial class AUtility
{
    private static ABuildConfig CurrentBuildConfig;
    public static class Config
    {
        public static ABuildConfig GetTargetBuildConfig()
        {
            if (CurrentBuildConfig != null) return CurrentBuildConfig;
            
            CurrentBuildConfig = Resources.Load<ABuildConfig>("AGame/ABuildConfig");
            return CurrentBuildConfig;
        }
    }
    
}