using UnityEngine;
using framework;
using System;
using DG.Tweening;

public class IntroPage : BaseUi
{
    public override void OnOpen()
    {
        Debug.Log("On Open");

        UIManager.Instance.Hide(this);        
    }
    public override void OnClose(TweenCallback _onComplete)
    {
        transform.DOScale(0, 1.5f)
        .OnComplete(_onComplete);
    }
}