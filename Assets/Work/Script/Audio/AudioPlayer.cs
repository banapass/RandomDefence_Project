using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Operators;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    protected AudioSource source;
    protected bool isPlaying;

    public virtual void Init(AudioBundle _bundle)
    {
        if (_bundle == null) return;
        if (source == null) source = GetComponent<AudioSource>();

        source.loop = _bundle.audioType == SoundType.Music;
        source.clip = _bundle.clip;
        source.volume = _bundle.audioType == SoundType.Music ? PlayerPrefs.GetFloat(Constants.MUSIC_KEY, 1) : PlayerPrefs.GetFloat(Constants.SFX_KEY, 1);

        isPlaying = false;
    }
    public virtual void Init(AudioSource _source, AudioBundle _bundle)
    {
        if (_bundle == null) return;

        this.source = _source;
        source.loop = _bundle.audioType == SoundType.Music;
        source.clip = _bundle.clip;
        source.volume = _bundle.audioType == SoundType.Music ? PlayerPrefs.GetFloat(Constants.MUSIC_KEY, 1) : PlayerPrefs.GetFloat(Constants.SFX_KEY, 1);

        isPlaying = false;
    }

    public void Play()
    {
        if (isPlaying) return;

        isPlaying = true;
        source.Play();
    }

}
