using UnityEngine;

public static class RuntimeInitializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void Init()
    {
        Screen.SetResolution((int)(1920 * 0.8f), (int)(1080 * 0.8f), FullScreenMode.Windowed);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
}