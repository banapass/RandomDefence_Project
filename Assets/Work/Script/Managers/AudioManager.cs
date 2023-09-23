using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using framework;
using framework.Audio;

public class AudioManager : Singleton<AudioManager>
{
    private Dictionary<SFX, string> sfxCacheDict;
    private Queue<SFXPlayer> sfxPlayQueue;
    private MusicPlayer musicPlayer;

    private void Start()
    {
        sfxPlayQueue = new Queue<SFXPlayer>();
        sfxCacheDict = new Dictionary<SFX, string>();
        CreateNewMusicPlayer();

    }

    public void PlaySound(SFX _sfx)
    {
        if (sfxPlayQueue.Count > 0)
        {
            ResourceStorage.GetObjectRes<AudioBundle>(GetSFXName(_sfx), _audio =>
            {
                AudioPlayer _player = sfxPlayQueue.Dequeue();
                _player.Init(_audio);
                _player.Play();
            });
        }
        else
        {
            ResourceStorage.GetObjectRes<AudioBundle>(GetSFXName(_sfx), _audio =>
            {
                SFXPlayer _newPlayer = CreateNewSFXPlayer(_audio);
                _newPlayer.Play();
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
        if (musicPlayer == null)
            CreateNewMusicPlayer();

        ResourceStorage.GetObjectRes<AudioBundle>(_music.ToString(), _audio =>
        {
            musicPlayer.Init(_audio);
            musicPlayer.Play();
        });
    }

    private SFXPlayer CreateNewSFXPlayer(AudioBundle _bundle)
    {
        GameObject _newObj = new GameObject();
        AudioSource _newSource = _newObj.AddComponent<AudioSource>();
        SFXPlayer _newPlayer = _newObj.AddComponent<SFXPlayer>();

        _newPlayer.transform.parent = this.transform;
        _newPlayer.Init(_newSource, _bundle);

        return _newPlayer;
    }
    private void CreateNewMusicPlayer()
    {
        musicPlayer = GetComponent<MusicPlayer>() == null ? gameObject.AddComponent<MusicPlayer>() : GetComponent<MusicPlayer>();
        // AudioSource _musicSource = GetComponent<AudioSource>() == null ? gameObject.AddComponent<AudioSource>() : GetComponent<AudioSource>();
    }
    public void ReturnSFXPlayer(SFXPlayer _player)
    {
        if (sfxPlayQueue.Contains(_player)) return;

        sfxPlayQueue.Enqueue(_player);
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
