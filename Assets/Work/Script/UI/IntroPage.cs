using UnityEngine;
using framework;
using UnityEngine.UI;
using UniRx;

public class IntroPage : BaseUi
{

    [SerializeField] Button startBtn;
    public override void OnOpen()
    {
        startBtn.OnClickAsObservable()
        .Subscribe(_observer =>
        {
            APP.Instance.EnterInGame();
        });

    }
    // public override void OnClose(TweenCallback<BaseUi> _onComplete)
    // {
    //     // transform.DOScale(0, 1.5f)
    //     // .OnComplete(() => _onComplete(this));
    // }

    private void OnClickStart()
    {
        Debug.Log("on Click");
    }
}