using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class TestCode : MonoBehaviour
{
    [SerializeField] float angle;
    [SerializeField] float count;
    [SerializeField] AssetReference[] assets;
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI percent_text;

    private void Awake()
    {
        StartCoroutine(LoadingCo(() => Debug.Log("On Completed Loading"), "Effector", "Data"));
    }

    private async Task Loading()
    {
        float totalAssets = assets.Length;
        float loadedAssets = 0;

        foreach (var assetReference in assets)
        {
            var handle = assetReference.LoadAssetAsync<UnityEngine.Object>();

            while (!handle.IsDone)
            {
                await Task.Yield(); // 비동기 처리를 위해 프레임 넘김
            }

            loadedAssets += 1.0f / totalAssets;
            slider.value = loadedAssets;


            Addressables.Release(handle);
        }
    }

    private IEnumerator LoadingCo(System.Action _onLoadCompleted = null, params string[] _labels)
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

                GameObject _gameobj = _handle.Result as GameObject;
                if (_gameobj != null)
                    Debug.Log(_handle.Result.name);
                loadCompletedAssets++;
                loadedAssets = (loadCompletedAssets / totalAssets);
                slider.value = loadedAssets;
                percent_text.text = string.Format("{0}%", (int)(loadedAssets * 100));
            }
        }


        // for (int i = 0; i < _labels.Length; i++)
        // {
        //     var _locationsHandle = Addressables.LoadResourceLocationsAsync(_labels[i]);
        //     yield return _locationsHandle.Task;

        //     var _locations = _locationsHandle.Result;
        //     totalAssets += _locations.Count;

        //     foreach (var _location in _locations)
        //     {
        //         var _handle = Addressables.LoadAssetAsync<UnityEngine.Object>(_location.PrimaryKey);
        //         while (!_handle.IsDone)
        //             yield return _handle.Task;


        //         loadCompletedAssets++;
        //         loadedAssets = (loadCompletedAssets / totalAssets);
        //         slider.value = loadedAssets;
        //         percent_text.text = string.Format("{0}%", (int)(loadedAssets * 100));

        //     }
        // }

        // foreach (var assetReference in assets)
        // {
        //     var handle = assetReference.LoadAssetAsync<UnityEngine.Object>();

        //     while (!handle.IsDone)
        //     {
        //         // loadedAssets = Mathf.Clamp01(loadedAssets + handle.PercentComplete);
        //         // slider.value = loadedAssets;
        //         yield return handle.Task;
        //     }
        //     loadCompletedAssets++;
        //     loadedAssets = (loadCompletedAssets / totalAssets);
        //     slider.value = loadedAssets;
        //     percent_text.text = string.Format("{0}%", (int)(loadedAssets * 100));


        //     Addressables.Release(handle);
        // }

        _onLoadCompleted?.Invoke();
    }
    public void LaunchProjectile(Vector2 _dir)
    {
        float angleStep = angle / (count - 1);  // 각도 간격 계산
        float halfAngle = angle / 2;

        for (int i = 0; i < count; i++)
        {
            float currentAngle = -halfAngle + i * angleStep;
            Vector2 direction = Quaternion.Euler(0f, 0f, currentAngle) * _dir;

            DebugDrawLaunchDirection(Vector2.zero, direction);
        }
    }
    private void DebugDrawLaunchDirection(Vector2 startPos, Vector2 direction)
    {
        Debug.DrawRay(startPos, direction, Color.red, 1f);
    }
}