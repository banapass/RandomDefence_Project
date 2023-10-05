using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : AudioPlayer
{
    private bool IsAudioEnded { get { return isPlaying && !source.isPlaying; } }


    public override void Init(AudioBundle _bundle)
    {
        base.Init(_bundle);
        SetActive(_bundle != null);
    }
    public override void Init(AudioSource _source, AudioBundle _bundle)
    {
        base.Init(_source, _bundle);
        SetActive(_bundle != null);
    }

    private void Update()
    {
        if (IsAudioEnded)
        {
            isPlaying = false;
            SetActive(false);
            AudioManager.Instance.ReturnSFXPlayer(this);
        }
    }
    private void SetActive(bool _active)
    {
        gameObject.SetActive(_active);
    }

}
