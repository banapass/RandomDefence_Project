using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using framework;

public class CameraController : Singleton<CameraController>
{
    private Camera mainCamera;
    public Camera MainCamera { get { return mainCamera; } }

    protected override void Awake()
    {
        base.Awake();
        TryGetComponent<Camera>(out mainCamera);
    }
}
