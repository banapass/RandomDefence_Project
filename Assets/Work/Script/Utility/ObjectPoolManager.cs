using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectable
{
    public string ObjectID { get; set; }
}

[System.Serializable]
public struct ObjectSet<T> where T : Component
{
    public T createObj;
    public int createCount;

    public ObjectSet(T _obj, int _createCount)
    {
        createObj = _obj;
        createCount = _createCount;
    }
}
public class ObjectPool<T> where T : Component
{
    public Queue<T> pool = new Queue<T>();
    public ObjectSet<T> objectSet;
    public void CreatePool(ObjectSet<T> _objectSet, Transform _parent = null, string _key = null)
    {
        objectSet = _objectSet;

        for (int i = 0; i < objectSet.createCount; i++)
        {
            if (objectSet.createObj == null) return;

            T temp = GameObject.Instantiate(objectSet.createObj, _parent);

            if (temp.gameObject.GetComponent<IObjectable>() != null)
                temp.gameObject.GetComponent<IObjectable>().ObjectID = _key;
            else
                Debug.LogError("[ObjectPoolManager/CreatePool] ObjectID Setting Error : IObjectable Is null");

            pool.Enqueue(temp);
            temp.gameObject.SetActive(false);
        }
    }
}

public class ObjectPoolManager : framework.Singleton<ObjectPoolManager>
{
    Dictionary<string, ObjectPool<Component>> poolList = new Dictionary<string, ObjectPool<Component>>();
    [SerializeField] List<ObjectSet<Component>> objectSetsList = new List<ObjectSet<Component>>(); // 인스펙터에서 직접 설정


    // 인스펙터에서 직접 설정한 Pool이 있을 시 호출
    public void Init()
    {
        if (objectSetsList == null) return;

        for (int i = 0; i < objectSetsList.Count; i++)
        {
            if (objectSetsList[i].createObj == null)
                return;

            ObjectPool<Component> objectPool = new ObjectPool<Component>();
            objectPool.CreatePool(objectSetsList[i], this.transform);
            poolList.Add(objectSetsList[i].createObj.name, objectPool);
        }
    }
    public bool IsExist(string _prefabName)
    {
        return poolList.ContainsKey(_prefabName);
    }

    // 오브젝트의 Name을 Key값으로 Pool 등록
    public void AddPool<T>(T _obj, int _createCount) where T : Component
    {
        if (poolList.ContainsKey(_obj.name)) return;

        ObjectPool<Component> objectPool = new ObjectPool<Component>();
        ObjectSet<Component> _objSet = new ObjectSet<Component>(_obj, _createCount);
        objectPool.CreatePool(_objSet, this.transform);
        poolList.Add(_objSet.createObj.name, objectPool);
    }
    // Key값을 직접 매개변수로 Pool 등록
    public void AddPool<T>(T _obj, int _createCount, string _key) where T : Component
    {
        if (poolList.ContainsKey(_key)) return;

        ObjectPool<Component> objectPool = new ObjectPool<Component>();
        ObjectSet<Component> _objSet = new ObjectSet<Component>(_obj, _createCount);
        objectPool.CreatePool(_objSet, this.transform, _key);

        poolList.Add(_key, objectPool as ObjectPool<Component>);
    }
    public bool IsExistPool(string _id)
    {
        return poolList.ContainsKey(_id);
    }

    public GameObject GetGameObject(string _objName)
    {
        if (poolList.ContainsKey(_objName))
        {
            if (poolList[_objName].pool.Count > 0)
            {
                Component obj = poolList[_objName].pool.Dequeue();
                obj.gameObject.SetActive(true);

                return obj.gameObject;
            }
            else
            {
                Component newObj = Instantiate(poolList[_objName].objectSet.createObj, transform);

                return newObj.gameObject;
            }
        }
        else
        {
            Debug.LogError("GetObjectError : Object Key is Not Founded");
            return null;
        }
    }
    public T GetParts<T>(string _objName) where T : Object
    {
        if (poolList.ContainsKey(_objName))
        {
            Debug.Log($"Pool Count : {poolList[_objName].pool}");
            if (poolList[_objName].pool.Count > 0)
            {
                Component obj = poolList[_objName].pool.Dequeue();
                obj.gameObject.SetActive(true);

                return obj as T;
            }
            else
            {
                Component newObj = Instantiate(poolList[_objName].objectSet.createObj, transform);
                if (newObj.GetComponent<IObjectable>() != null)
                    newObj.GetComponent<IObjectable>().ObjectID = _objName;
                else
                    Debug.LogError("Object Key Setting Failed : IObjetable Is null}");

                return newObj as T;
            }
        }
        else
        {
            Debug.LogError("GetObjectError : Object Key is Not Founded");
            return null;
        }
    }
    public void ReturnParts(Component _obj, string _objName)
    {
        Component returnObj = _obj;

        if (poolList.ContainsKey(_objName))
        {
            returnObj.gameObject.SetActive(false);
            poolList[_objName].pool.Enqueue(returnObj);
        }
        else
        {
            Debug.LogError($"ReturnObjectError : Object Key is Not Founded {_objName}");
        }
    }
    private void OnDestroy()
    {
        foreach (var _item in poolList)
        {
            while (_item.Value.pool.Count > 0)
            {
                Destroy(_item.Value.pool.Dequeue());
            }
        }
    }
}
