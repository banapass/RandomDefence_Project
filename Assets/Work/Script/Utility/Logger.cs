using UnityEngine;

public class Logger
{
#if UNITY_EDITOR || LOG_ENABLE
    public static void Log(object _msg)
    {
        Debug.Log($"<color=#59FF55>[LOG]</color> : {_msg}");
    }

    public static void LogError(object _msg)
    {
        Debug.Log($"<color=#FF5656>[ERROR]</color> : {_msg}");
    }

    public static void LogWarning(object _msg)
    {
        Debug.Log($"<color=#F0CB2A>[Warning]</color> : {_msg}");
    }

#endif
}