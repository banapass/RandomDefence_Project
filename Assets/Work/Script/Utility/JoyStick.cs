using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditorInternal;

public class JoyStick : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] Image stick;

    private Vector2 backgroundRectSize;


    public Vector2 RecentDirection = Vector2.zero;

    private void Awake()
    {
        backgroundRectSize = background.rectTransform.sizeDelta;
    }
    // Update is called once per frame
    void Update()
    {
        JoyStickMove();
    }
    private void JoyStickMove()
    {
        Vector2 _centerPos = background.transform.position;
        Vector2 _clickPos = Input.mousePosition;
        Vector2 _dir = _clickPos - _centerPos;

        Vector2 _clampedPos = Vector2.ClampMagnitude(_dir, backgroundRectSize.x * 0.5f);

        stick.rectTransform.localPosition = _clampedPos;

        RecentDirection = _dir.normalized;
    }
}
