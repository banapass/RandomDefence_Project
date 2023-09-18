namespace framework
{

    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UObject = UnityEngine.Object;
    using Unity.VisualScripting;

    public class ResourceStorage
    {
        private static Dictionary<string, KeyValuePair<bool, UObject>> resDict = new Dictionary<string, KeyValuePair<bool, UObject>>();


        public async static Task LoadObjectByLabel<T>(string _label, bool _isCache = false) where T : UObject
        {
            var _handle = Addressables.LoadResourceLocationsAsync(_label);
            await _handle.Task;

            foreach (var _obj in _handle.Result)
            {
                T _result = await LoadObject<UObject>(_obj.PrimaryKey) as T;
                if (_isCache)
                    resDict.Add(_obj.PrimaryKey, new KeyValuePair<bool, UObject>(true, _result));
            }

        }

        public static async Task<GameObject> LoadGameObject(string _path)
        {
            if (resDict.ContainsKey(_path))
            {
                return resDict[_path].Value as GameObject;
            }


            var _handle = Addressables.LoadAssetAsync<GameObject>(_path);
            await _handle.Task;

            if (_handle.Status == AsyncOperationStatus.Succeeded)
            {
                if (!resDict.ContainsKey(_path))
                    resDict.Add(_path, new KeyValuePair<bool, UObject>(true, _handle.Result));

                return _handle.Result;
            }
            else
            {
                Log.Logger.Log($"Load Asset Failed : {_handle.OperationException}");
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
                Log.Logger.Log($"Load Asset Failed : {_handle.OperationException}");
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
                Log.Logger.LogError($"Load Asset Failed : {_handle.OperationException}");
            }

            return null;
        }

        public static void GetObjectRes<T>(string _path, Action<T> _onCompleted) where T : UObject
        {
            if (resDict == null) resDict = new Dictionary<string, KeyValuePair<bool, UObject>>();

            if (resDict.TryGetValue(_path, out KeyValuePair<bool, UObject> _res))
            {
                _onCompleted?.Invoke((T)_res.Value);
                return;
            }
            else
            {
                Addressables.LoadAssetAsync<T>(_path).Completed += _handle =>
                {
                    if (_handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        resDict.Add(_path, new KeyValuePair<bool, UObject>(true, _handle.Result));
                        _onCompleted?.Invoke(_handle.Result);
                    }
                    else
                    {
                        Log.Logger.LogError($"Resource Load Failed : {_handle.OperationException}");
                        _onCompleted?.Invoke(default);
                    }
                };

            }
        }
        public static void GetComponentAsset<T>(string _path, Action<T> _onCompleted) where T : Component
        {
            if (resDict == null) resDict = new Dictionary<string, KeyValuePair<bool, UObject>>();

            if (resDict.TryGetValue(_path, out KeyValuePair<bool, UObject> _res))
            {
                if (!_res.Key)
                {
                    T _component = _res.Value.GetComponent<T>();
                    resDict[_path] = new KeyValuePair<bool, UObject>(true, _component);
                    _onCompleted?.Invoke(_component);
                }
                else
                {
                    _onCompleted?.Invoke((T)_res.Value);
                }

                return;
            }
            else
            {
                Addressables.LoadAssetAsync<GameObject>(_path).Completed += _handle =>
                {
                    if (_handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        T _component = _handle.Result.GetComponent<T>();
                        if (!resDict.ContainsKey(_path))
                            resDict.Add(_path, new KeyValuePair<bool, UObject>(true, _component));

                        _onCompleted?.Invoke(_component);
                    }
                    else
                    {
                        Log.Logger.Log($"Resource Load Failed : {_handle.OperationException}");
                        _onCompleted?.Invoke(default);
                    }
                };
            }
        }
        public static void GetComponentAsset<T1, T2>(string _path, Action<T1, T2> _onCompleted, T2 _param) where T1 : Component
        {
            if (resDict == null) resDict = new Dictionary<string, KeyValuePair<bool, UObject>>();

            if (resDict.TryGetValue(_path, out KeyValuePair<bool, UObject> _res))
            {

                if (!_res.Key)
                {
                    T1 _component = _res.Value.GetComponent<T1>();
                    resDict[_path] = new KeyValuePair<bool, UObject>(true, _component);
                    _onCompleted?.Invoke(_component, _param);
                }
                else
                {
                    _onCompleted?.Invoke((T1)_res.Value, _param);
                }

                //_onCompleted?.Invoke((T1)_res, _param);
                return;
            }
            else
            {
                Addressables.LoadAssetAsync<GameObject>(_path).Completed += _handle =>
                {
                    if (_handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        T1 _component = _handle.Result.GetComponent<T1>();
                        if (!resDict.ContainsKey(_path))
                            resDict.Add(_path, new KeyValuePair<bool, UObject>(true, _component));
                        _onCompleted?.Invoke(_component, _param);
                    }
                    else
                    {
                        // Debug.Log(_handle.Status);
                        Log.Logger.Log($"Resource Load Failed : {_handle.OperationException}");
                        _onCompleted?.Invoke(default, _param);
                    }
                };

            }
        }
        public static void AddResource(string _key, UObject _obj)
        {
            if (resDict == null) return;
            if (resDict.ContainsKey(_key)) return;
            bool _isComponent = _obj as GameObject != null;

            if (_isComponent)
            {
                resDict.Add(_key, new KeyValuePair<bool, UObject>(false, _obj as GameObject));
            }
            else
            {
                resDict.Add(_key, new KeyValuePair<bool, UObject>(true, _obj));
            }

        }

        public static void ClearResource()
        {
            resDict.Clear();
        }

        //public async static Task LoadComponentsByLabel<T>(string _label, bool _isCache, Action<string, Component> _onCompleted = null, Action _onProgress = null) where T : Component
        //{
        //    var _handle = Addressables.LoadResourceLocationsAsync(_label);
        //    await _handle.Task;

        //    foreach (var _in in _handle.Result)
        //    {
        //        T _object = await LoadComponent<T>(_in.PrimaryKey);
        //        if (_isCache)
        //            resDict.Add(_in.PrimaryKey, new KeyValuePair<bool, UObject>(true, _object));

        //        _onCompleted?.Invoke(_in.PrimaryKey, _object);
        //    }
        //}
    }

}