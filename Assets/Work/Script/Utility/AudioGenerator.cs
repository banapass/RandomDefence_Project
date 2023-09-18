#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;


public class AudioGenerator
{
    public static readonly string MUSIC_PATH = Application.dataPath + "/AudioDatabase/Enum/Music.cs";
    public static readonly string SFX_PATH = Application.dataPath + "/AudioDatabase/Enum/SFX.cs";

    public static readonly string AUDIODATABASE_DIRECTORY = Application.dataPath + "/AudioDatabase";
    public static readonly string AUDIOENUM_DIRECTORY = Application.dataPath + "/AudioDatabase/Enum";
    public static readonly string AUDIOBUNDLE_DIRECTORY = Application.dataPath + "/AudioDatabase/Bundle";

    public const string MUSIC_FORMAT =
@"namespace framework.Audio
{{
    public enum Music 
    {{ 
        {0} 
    }}
}}
";
    public const string SFX_FORMAT =
@"namespace framework.Audio
{{
    public enum SFX
    {{ 
        {0} 
    }}
}}
";

    [UnityEditor.MenuItem("Audio/Regenerate")]
    public static void Regenerate()
    {

        TryCreateDirectoryAndFiles();

        var _audioBundles = GetAudioBundles();

        List<string> _sfxList = GetBundleNames(_audioBundles, SoundType.SFX);
        List<string> _musicList = GetBundleNames(_audioBundles, SoundType.Music);

        string _musicEnum = ConvertEnumText(_musicList);
        string _sfxEnum = ConvertEnumText(_sfxList);

        string _newMusicText = string.Format(MUSIC_FORMAT, _musicEnum);
        string _newSFXText = string.Format(SFX_FORMAT, _sfxEnum);

        File.WriteAllText(MUSIC_PATH, _newMusicText);
        File.WriteAllText(SFX_PATH, _newSFXText);

    }
    private static List<string> GetBundleNames(List<AudioBundle> _audioBundles, SoundType _type)
    {
        List<string> _bundleNames = new List<string>();
        for (int i = 0; i < _audioBundles.Count; i++)
        {
            if (_audioBundles[i].audioType != _type) continue;
            if (_bundleNames.Contains(_audioBundles[i].audioName)) continue;

            _bundleNames.Add(_audioBundles[i].audioName);
        }

        return _bundleNames;
    }
    private static string ConvertEnumText(List<string> _bundleNames)
    {
        string _enums = string.Empty;
        for (int i = 0; i < _bundleNames.Count; i++)
        {
            _enums += _bundleNames[i];

            if (i < _bundleNames.Count - 1)
                _enums += ",";
        }
        return _enums;
    }
    private static void TryCreateDirectoryAndFiles()
    {
        string _newMusicText = string.Format(MUSIC_FORMAT, "");
        string _newSFXText = string.Format(SFX_FORMAT, "");

        if (!Directory.Exists(AUDIODATABASE_DIRECTORY))
        {
            Directory.CreateDirectory(AUDIODATABASE_DIRECTORY);
        }
        if (!Directory.Exists(AUDIOENUM_DIRECTORY))
        {
            Directory.CreateDirectory(AUDIOENUM_DIRECTORY);
        }
        if (!Directory.Exists(AUDIOBUNDLE_DIRECTORY))
        {
            Directory.CreateDirectory(AUDIOBUNDLE_DIRECTORY);
        }

        if (!File.Exists(SFX_PATH))
        {
            Stream _stream = new FileStream(SFX_PATH, FileMode.CreateNew);
            _stream.Close();

            File.WriteAllText(SFX_PATH, _newSFXText);
        }
        if (!File.Exists(MUSIC_PATH))
        {
            Stream _stream = new FileStream(MUSIC_PATH, FileMode.CreateNew);
            _stream.Close();

            File.WriteAllText(MUSIC_PATH, _newMusicText);
        }

    }
    private static List<AudioBundle> GetAudioBundles()
    {
        List<AudioBundle> _bundles = new List<AudioBundle>();
        string[] _files = Directory.GetFiles(AUDIOBUNDLE_DIRECTORY);

        for (int i = 0; i < _files.Length; i++)
        {
            string _filePath = _files[i];
            string _slicePath = _filePath.Substring(_filePath.IndexOf("Assets/"));
            var _bundle = AssetDatabase.LoadAssetAtPath<AudioBundle>(_slicePath);
            if (_bundle == null) continue;

            SetAudioBundleAddress(_bundle, _slicePath);
            _bundles.Add(_bundle);
        }

        return _bundles;
    }
    private static void SetAudioBundleAddress(AudioBundle _bundle, string _path)
    {
        AddressableAssetSettings _settings = AddressableAssetSettingsDefaultObject.Settings;
        AddressableAssetGroup _group = _settings.DefaultGroup;

        string _guid = AssetDatabase.AssetPathToGUID(_path);
        _settings.CreateOrMoveEntry(_guid, _group);

        AddressableAssetEntry _entry = _settings.FindAssetEntry(_guid);
        _entry.address = _bundle.audioName;
        _entry.SetLabel(_bundle.audioType.ToString(), true, true);
    }

}
#endif