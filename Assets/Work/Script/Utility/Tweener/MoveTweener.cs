using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveTweener : Tweener
{
    Vector2 originPos;
    [SerializeField] Vector2 nextPos;
    private Vector2 EndPosition { get { return originPos + nextPos; } }

    [SerializeField] Ease startEase;
    [SerializeField] Ease endEase;
    [SerializeField] float duration;

    [SerializeField] SpaceType spaceType;

    private void Awake()
    {
        originPos = transform.localPosition;
    }

    public override void Show()
    {

        if (spaceType == SpaceType.World)
        {
            transform.DOMove(EndPosition, duration)
                     .SetEase(startEase);
        }
        else if (spaceType == SpaceType.Local)
        {
            transform.DOLocalMove(EndPosition, duration)
                     .SetEase(startEase);
        }

    }
    public override void Hide()
    {
        if (spaceType == SpaceType.World)
        {
            transform.DOMove(originPos, duration)
                     .SetEase(endEase);
        }
        else if (spaceType == SpaceType.Local)
        {
            transform.DOLocalMove(originPos, duration)
                     .SetEase(endEase);
        }
    }

    public void SetPositions(Vector2 _startPos, Vector2 _endPos)
    {
        originPos = _startPos;
        nextPos = _endPos;
    }
}
