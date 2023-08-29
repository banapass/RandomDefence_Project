using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class MonsterHpSlider : PoolingObject
{
    public Slider hpSlider;

    public void UpdateSlider(float _percent)
    {
        if (hpSlider == null) TryGetComponent<Slider>(out hpSlider);

        hpSlider.value = _percent;
    }

    public void UpdatePosition(Vector2 _pos)
    {
        Vector2 _uiPos = CameraController.Instance.MainCamera.WorldToScreenPoint(_pos);
        hpSlider.transform.position = _uiPos;
    }
}
