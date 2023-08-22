using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using framework;

public class LoadingManager : Singleton<LoadingManager>
{

    public static event Action<float> OnLoadingProgress;
    public void LoadScene(string _scene, Action _onCompleted = null, params string[] _labels)
    {
        StartCoroutine(Loading(_scene, _onCompleted, _labels));
    }

    // private IEnumerator Loading(string _scene, Action _onCompleted = null)
    // {
    //     var _handle = Addressables.LoadSceneAsync(_scene);
    //     // yield return
    // }

    private IEnumerator Loading(string _scene, System.Action _onLoadCompleted = null, params string[] _labels)
    {
        float totalAssets = 0;//assets.Length;
        float loadCompletedAssets = 0;
        float loadedAssets = 0;

        List<IList<IResourceLocation>> _locationsList = new List<IList<IResourceLocation>>();

        for (int i = 0; i < _labels.Length; i++)
        {
            var _locationsHandle = Addressables.LoadResourceLocationsAsync(_labels[i]);
            yield return _locationsHandle.Task;

            var _locations = _locationsHandle.Result;
            totalAssets += _locations.Count;

            _locationsList.Add(_locations);
        }

        for (int i = 0; i < _locationsList.Count; i++)
        {
            for (int j = 0; j < _locationsList[i].Count; j++)
            {
                var _location = _locationsList[i][j];
                var _handle = Addressables.LoadAssetAsync<UnityEngine.Object>(_location.PrimaryKey);

                while (!_handle.IsDone)
                    yield return _handle.Task;

                loadCompletedAssets++;
                loadedAssets = (loadCompletedAssets / totalAssets);
                OnLoadingProgress?.Invoke(loadedAssets);
            }
        }

        var _sceneHandle = Addressables.LoadSceneAsync(_scene);
        yield return _sceneHandle.Task;

        _sceneHandle.Completed += _op =>
        {
            _onLoadCompleted?.Invoke();
        };
    }
}
