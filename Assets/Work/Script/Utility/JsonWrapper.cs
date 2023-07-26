namespace Utility
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;

    public static class JsonWrapper
    {
        public static T[] FromJson<T>(string json)
        {
            string _wrappedJson = "{\"items\":" + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(_wrappedJson);
            return wrapper.items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.items = array;
            return JsonUtility.ToJson(wrapper, true);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] items;
        }
    }

}