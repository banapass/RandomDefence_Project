using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISafeAreaManager
{
    public static void ApplySafeAreaPosition(RectTransform rt)
    {
        Rect safeArea = Screen.safeArea;

        // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        // 기존 anchor x 좌표 사용
        // anchorMin.x = rt.anchorMin.x;
        // anchorMax.x = rt.anchorMax.x;

        // anchorMin.y /= Screen.height;
        // anchorMax.y /= Screen.height;

        anchorMin.y = rt.anchorMin.y;
        anchorMax.y = rt.anchorMax.y;

        anchorMin.x /= Screen.width;
        anchorMax.x /= Screen.width;

        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
    }
}
