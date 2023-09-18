using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using framework;
using framework.Audio;

public class AudioManager : Singleton<AudioManager>
{
    private Dictionary<SFX, string> sfxCacheDict;
    private Queue<AudioSource> audioSourceQueue;

    private void Start()
    {
        audioSourceQueue = new Queue<AudioSource>();
        sfxCacheDict = new Dictionary<SFX, string>();
    }

    public void PlaySound(SFX _sfx)
    {
        if (audioSourceQueue.Count > 0)
        {
            ResourceStorage.GetObjectRes<AudioBundle>(GetSFXName(_sfx), _audio =>
            {
                AudioSource _source = audioSourceQueue.Dequeue();
                _source.loop = false;
                _source.clip = _audio.clip;
                _source.Play();
            });
        }
        else
        {
            ResourceStorage.GetObjectRes<AudioBundle>(GetSFXName(_sfx), _audio =>
            {
                GameObject _newObj = new GameObject();
                AudioSource _newSource = _newObj.AddComponent<AudioSource>();
                _newObj.transform.parent = this.transform;

                _newSource.clip = _audio.clip;
                _newSource.loop = false;
                _newSource.Play();
            });

        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaySound(SFX.Hit);
        }
    }
    public void PlayMusic(Music _music)
    {

    }

    private string GetSFXName(SFX _sfx)
    {
        if (sfxCacheDict.TryGetValue(_sfx, out var _sfxName))
        {
            return _sfxName;
        }
        else
        {
            string _newSfxName = _sfx.ToString();
            sfxCacheDict.Add(_sfx, _newSfxName);
            return _newSfxName;
        }
    }

}
