using UnityEngine;
using framework;
using System;
using DG.Tweening;
using UnityEngine.UI;
using UniRx;

public class IntroPage : BaseUi
{
    [SerializeField] Button startBtn;
    private void SetObserver()
    {
        startBtn.OnClickAsObservable()
                .Subscribe(_observer => OnClickStart());
    }
    public override void OnOpen()
    {
        SetObserver();

        UIManager.Instance.Hide(this);
    }
    public override void OnClose(TweenCallback<BaseUi> _onComplete)
    {
        transform.DOScale(0, 1.5f)
        .OnComplete(() => _onComplete(this));
    }

    private void OnClickStart()
    {
        Debug.Log("on Click");
    }
}