using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using framework;

public class LoadingPopup : BaseUi
{
    [SerializeField] Slider progressSlider;

    private void OnEnable()
    {
        LoadingManager.OnLoadingProgress += UpdateProgress;
    }
    private void OnDisable()
    {
        LoadingManager.OnLoadingProgress -= UpdateProgress;
    }

    private void UpdateProgress(float _progress)
    {
        progressSlider.value = _progress;
    }
}
