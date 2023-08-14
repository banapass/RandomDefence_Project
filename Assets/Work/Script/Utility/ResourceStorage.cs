namespace framework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ResourceStorage
    {
        private static Dictionary<string, Object> resDict = new Dictionary<string, Object>();

        public static T GetResource<T>(string _path) where T : Object
        {
            if (resDict == null) resDict = new Dictionary<string, Object>();

            if (resDict.ContainsKey(_path))
            {
                return resDict[_path] as T;
            }
            else
            {
                T _res = Resources.Load<T>(_path);
                if (_res == null)
                {
                    Debug.LogError("불러온 리소스가 Null 입니다 : 파일 경로 확인 필요");
                    return null;
                }

                resDict.Add(_path, _res);
                return _res;
            }
        }

        public static void ClearResource()
        {
            resDict.Clear();
        }
    }

}