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
        if (source == null) source = GetComponent<AudioSource>();
        source.loop = _bundle.audioType == SoundType.Music;
        source.clip = _bundle.clip;
        isPlaying = false;
    }
    public virtual void Init(AudioSource _source, AudioBundle _bundle)
    {
        this.source = _source;
        source.loop = _bundle.audioType == SoundType.Music;
        source.clip = _bundle.clip;
        isPlaying = false;
    }

    public void Play()
    {
        if (isPlaying) return;

        isPlaying = true;
        source.Play();
    }

}
