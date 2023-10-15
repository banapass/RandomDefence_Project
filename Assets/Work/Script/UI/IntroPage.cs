using UnityEngine;
using framework;
using UnityEngine.UI;
using UniRx;
using framework.Audio;

public class IntroPage : BaseUi
{

    [SerializeField] Button startBtn;
    [SerializeField] Button optionBtn;
    public override void OnOpen()
    {
        startBtn.OnClickAsObservable()
        .Subscribe(_observer =>
        {
            AudioManager.Instance.PlaySound(SFX.UI_Click);
            APP.Instance.EnterInGame();
        });

        optionBtn.OnClickAsObservable()
        .Subscribe(_observer =>
        {
            UIManager.Instance.Show(UIPath.OPTION, true);
        });

    }
    // public override void OnClose(TweenCallback<BaseUi> _onComplete)
    // {
    //     // transform.DOScale(0, 1.5f)
    //     // .OnComplete(() => _onComplete(this));
    // }

}