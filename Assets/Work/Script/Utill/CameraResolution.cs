using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraResolution : MonoBehaviour
{
    public float targetAspectRatioWidth = 16f;
    public float targetAspectRatioHeight = 9f;

    private Camera mainCamera;

    void Start()
    {

        mainCamera = Camera.main;
        AdjustCameraSize();

    }
    // private void Update()
    // {
    //     AdjustCameraSize();
    // }

    void AdjustCameraSize()
    {
        // 현재 화면의 가로와 세로 비율
        float currentAspectRatio = (float)Screen.width / Screen.height;

        // 현재 비율과 원하는 비율의 차이 계산
        float ratioDifference = currentAspectRatio / (targetAspectRatioWidth / targetAspectRatioHeight);

        // 차이가 1보다 크면 화면이 넓어졌으므로 카메라의 사이즈를 줄임
        // 차이가 1보다 작으면 화면이 높아졌으므로 카메라의 사이즈를 늘임
        if (ratioDifference > 1f)
        {
            mainCamera.orthographicSize /= ratioDifference;
        }
        else
        {
            mainCamera.orthographicSize *= ratioDifference;
        }

        // Camera camera = GetComponent<Camera>();
        // Rect rect = camera.rect;

        // float scaleheight = ((float)Screen.width / Screen.height) / ((float)16 / 9);
        // // float scaleheight = ((float)Screen.safeArea.width / Screen.safeArea.height) / ((float)16 / 9);
        // float scalewidth = 1f / scaleheight;
        // if (scaleheight < 1)
        // {
        //     rect.height = scaleheight;
        //     rect.y = (1f - scaleheight) / 2f;
        // }
        // else
        // {
        //     rect.width = scalewidth;
        //     rect.x = (1f - scalewidth) / 2f;
        // }
        // camera.rect = rect;
    }
    // private void OnPreCull()
    // {
    //     GL.Clear(true, true, Color.black);
    // }
}
