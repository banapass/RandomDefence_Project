namespace framework
{

    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UObject = UnityEngine.Object;

    public class ResourceStorage
    {
        private static Dictionary<string, UObject> resDict = new Dictionary<string, UObject>();

        public async static Task LoadComponentsByLabel<T>(string _label, bool _isCache, Action<string, Component> _onCompleted = null, Action _onProgress = null) where T : Component
        {
            var _handle = Addressables.LoadResourceLocationsAsync(_label);
            await _handle.Task;

            foreach (var _in in _handle.Result)
            {
                T _object = await LoadComponent<T>(_in.PrimaryKey);
                if (_isCache)
                    resDict.Add(_in.PrimaryKey, _object);

                _onCompleted?.Invoke(_in.PrimaryKey, _object);
            }
        }
        public async static Task LoadObjectByLabel<T>(string _label) where T : UObject
        {
            var _handle = Addressables.LoadResourceLocationsAsync(_label);
            await _handle.Task;

            foreach (var _obj in _handle.Result)
            {
                T _result = await LoadObject<UObject>(_obj.PrimaryKey) as T;
                resDict.Add(_obj.PrimaryKey, _result);
            }

        }

        public static async Task<GameObject> LoadGameObject(string _path)
        {
            if (resDict.ContainsKey(_path))
            {
                return resDict[_path] as GameObject;
            }


            var _handle = Addressables.LoadAssetAsync<GameObject>(_path);
            await _handle.Task;

            if (_handle.Status == AsyncOperationStatus.Succeeded)
            {
                if (!resDict.ContainsKey(_path))
                    resDict.Add(_path, _handle.Result);

                return _handle.Result;
            }
            else
            {
                Debug.Log($"Load Asset Failed : {_handle.OperationException}");
            }
            return null;
        }
        public static async Task<T> LoadComponent<T>(string _path) where T : Component
        {
            var _handle = Addressables.LoadAssetAsync<GameObject>(_path);
            await _handle.Task;

            if (_handle.Status == AsyncOperationStatus.Succeeded)
            {
                return _handle.Result.GetComponent<T>();
            }
            else
            {
                Debug.Log($"Load Asset Failed : {_handle.OperationException}");
            }

            return null;
        }
        private static async Task<T> LoadObject<T>(string _path) where T : UObject
        {
            var _handle = Addressables.LoadAssetAsync<UObject>(_path);
            await _handle.Task;

            if (_handle.Status == AsyncOperationStatus.Succeeded)
            {
                return _handle.Result as T;
            }
            else
            {
                Debug.LogError($"Load Asset Failed : {_handle.OperationException}");
            }

            return null;
        }

        public static void GetObjectRes<T>(string _path, Action<T> _onCompleted) where T : UObject
        {
            if (resDict == null) resDict = new Dictionary<string, UObject>();

            if (resDict.TryGetValue(_path, out UObject _res))
            {
                _onCompleted?.Invoke((T)_res);
                return;
            }
            else
            {
                Addressables.LoadAssetAsync<T>(_path).Completed += _handle =>
                {
                    if (_handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        resDict.Add(_path, _handle.Result);
                        _onCompleted?.Invoke(_handle.Result);
                    }
                    else
                    {
                        // Debug.Log(_handle.Status);
                        Debug.LogError($"Resource Load Failed : {_handle.OperationException}");
                        _onCompleted?.Invoke(default);
                    }
                };

            }
        }
        public static void GetComponentAsset<T>(string _path, Action<T> _onCompleted) where T : Component
        {
            if (resDict == null) resDict = new Dictionary<string, UObject>();

            if (resDict.TryGetValue(_path, out UObject _res))
            {
                _onCompleted?.Invoke((T)_res);
                return;
            }
            else
            {
                // resDict.Add(_path, null);
                Addressables.LoadAssetAsync<GameObject>(_path).Completed += _handle =>
                {
                    if (_handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        T _component = _handle.Result.GetComponent<T>();
                        if (!resDict.ContainsKey(_path))
                            resDict.Add(_path, _component);

                        _onCompleted?.Invoke(_component);
                    }
                    else
                    {
                        // Debug.Log(_handle.Status);
                        Debug.Log($"Resource Load Failed : {_handle.OperationException}");
                        _onCompleted?.Invoke(default);
                    }
                };
            }
        }
        public static void GetComponentAsset<T1, T2>(string _path, Action<T1, T2> _onCompleted, T2 _param) where T1 : Component
        {
            if (resDict == null) resDict = new Dictionary<string, UObject>();

            if (resDict.TryGetValue(_path, out UObject _res))
            {
                _onCompleted?.Invoke((T1)_res, _param);
                return;
            }
            else
            {
                Addressables.LoadAssetAsync<GameObject>(_path).Completed += _handle =>
                {
                    if (_handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        T1 _component = _handle.Result.GetComponent<T1>();
                        resDict.Add(_path, _component);
                        _onCompleted?.Invoke(_component, _param);
                    }
                    else
                    {
                        // Debug.Log(_handle.Status);
                        Debug.Log($"Resource Load Failed : {_handle.OperationException}");
                        _onCompleted?.Invoke(default, _param);
                    }
                };

            }
        }

        public static void ClearResource()
        {
            resDict.Clear();
        }
    }

}