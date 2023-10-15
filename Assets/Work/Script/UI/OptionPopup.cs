using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using framework;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class OptionPopup : BaseUi
{
    [SerializeField] Slider music_slider;
    [SerializeField] Slider sfx_slider;

    [SerializeField] Button close_Btn;
    public override void OnOpen()
    {
        music_slider.value = PlayerPrefs.GetFloat(Constants.MUSIC_KEY, 1);
        sfx_slider.value = PlayerPrefs.GetFloat(Constants.SFX_KEY, 1);

        close_Btn.OnClickAsObservable()
        .Subscribe(_ob =>
        {
            UIManager.Instance.Hide(this);
        });
    }

    public void OnChangedMusicValue(float _value)
    {
        PlayerPrefs.SetFloat(Constants.MUSIC_KEY, _value);
    }
    public void OnChangedSFXValue(float _value)
    {
        PlayerPrefs.SetFloat(Constants.SFX_KEY, _value);
    }
    public override void OnClose(TweenCallback<BaseUi> _onComplete)
    {
        _onComplete(this);
    }
}
