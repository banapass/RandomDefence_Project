namespace Utility
{
    using System;
    using Newtonsoft.Json;
    using UnityEngine;

    public static class JsonWrapper
    {
        public static T[] FromJson<T>(string json)
        {
            string _wrappedJson = "{\"items\":" + json + "}";
            Wrapper<T> wrapper = JsonConvert.DeserializeObject<Wrapper<T>>(_wrappedJson);
            return wrapper.items;

            // JsonUtility.FromJson<Wrapper<T>>(_wrappedJson);
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.items = array;
            return JsonConvert.SerializeObject(wrapper); //JsonUtility.ToJson(wrapper, true);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] items;
        }
    }

}