using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraResolution : MonoBehaviour
{
    public float targetAspectRatioWidth = 16f;
    public float targetAspectRatioHeight = 9f;
    public Orientation targetOrientation;
    public float TargetAspectRatio
    {
        get
        {
            if (targetOrientation == Orientation.Landscape)
                return targetAspectRatioWidth / targetAspectRatioHeight;
            else
                return targetAspectRatioHeight / targetAspectRatioWidth;
        }
    }

    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        UpdateOrthographicSize();
    }
    // void AdjustCameraSize()
    // {
    //     Rect rect = mainCamera.rect;

    //     float scaleheight = ((float)Screen.width / Screen.height) / ((float)16 / 9);
    //     // float scaleheight = ((float)Screen.safeArea.width / Screen.safeArea.height) / ((float)16 / 9);
    //     float scalewidth = 1f / scaleheight;
    //     if (scaleheight < 1)
    //     {
    //         rect.height = scaleheight;
    //         rect.y = (1f - scaleheight) / 2f;
    //     }
    //     else
    //     {
    //         rect.width = scalewidth;
    //         rect.x = (1f - scalewidth) / 2f;
    //     }
    //     mainCamera.rect = rect;
    // }

    // 정해 놓은 비율에 맞게 OrthSize 변경
    private void UpdateOrthographicSize()
    {
        float currentAspectRatio = mainCamera.aspect;
        float orthographicSize = mainCamera.orthographicSize;
        float newOrthographicSize = orthographicSize * (TargetAspectRatio / currentAspectRatio);

        mainCamera.orthographicSize = newOrthographicSize;
        UpdateCameraPosition(orthographicSize, newOrthographicSize);
    }

    // 카메라 OrthSize가 변경되면서 바뀐 위치값을 재조정
    private void UpdateCameraPosition(float _prevOrthSize, float _currentOrthSize)
    {
        float _newY = _prevOrthSize - _currentOrthSize;

        transform.position = new Vector3(0, _newY, -10);
    }
    // private void OnPreCull()
    // {
    //     GL.Clear(true, true, Color.black);
    // }
}
