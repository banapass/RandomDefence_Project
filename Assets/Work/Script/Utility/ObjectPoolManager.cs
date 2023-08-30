using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectable
{
    public string ObjectID { get; set; }
    public void ReturnPool();
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

    private Transform parent;
    public Transform Parent { get { return parent; } }

    public void CreatePool(ObjectSet<T> _objectSet, Transform _parent = null, string _key = null)
    {
        objectSet = _objectSet;
        parent = _parent;

        for (int i = 0; i < objectSet.createCount; i++)
        {
            if (objectSet.createObj == null) return;

            T temp = GameObject.Instantiate(objectSet.createObj, _parent);

            if (temp.gameObject.GetComponent<IObjectable>() != null)
                temp.gameObject.GetComponent<IObjectable>().ObjectID = _key;
            else
                Logger.LogError("[ObjectPoolManager/CreatePool] ObjectID Setting Error : IObjectable Is null");

            pool.Enqueue(temp);
            temp.gameObject.SetActive(false);
        }
    }
}

public class ObjectPoolManager : framework.Singleton<ObjectPoolManager>
{
    Dictionary<string, ObjectPool<Component>> poolList = new Dictionary<string, ObjectPool<Component>>();
    [SerializeField] List<ObjectSet<Component>> objectSetsList = new List<ObjectSet<Component>>(); // 인스펙터에서 직접 설정

    private Transform uiParent;

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
    public void SetUIParent(Transform _uiParent)
    {
        uiParent = _uiParent;
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
    public void AddPool<T>(T _obj, int _createCount, string _key, Transform _parent = null) where T : Component
    {
        if (poolList.ContainsKey(_key)) return;

        Transform _targetParent = _parent == null ? this.transform : _parent;

        ObjectPool<Component> objectPool = new ObjectPool<Component>();
        ObjectSet<Component> _objSet = new ObjectSet<Component>(_obj, _createCount);
        objectPool.CreatePool(_objSet, _targetParent, _key);

        poolList.Add(_key, objectPool as ObjectPool<Component>);
    }
    public bool IsExist(string _prefabName)
    {
        return poolList.ContainsKey(_prefabName);
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
    public void GetParts<T>(string _key, bool _isUi = false, System.Action<T> _onComplete = null) where T : Component
    {
        if (poolList.ContainsKey(_key))
        {
            if (poolList[_key].pool.Count > 0)
            {
                Component obj = poolList[_key].pool.Dequeue();
                obj.gameObject.SetActive(true);
                _onComplete?.Invoke(obj as T);
            }
            else
            {
                Component newObj = Instantiate(poolList[_key].objectSet.createObj, poolList[_key].Parent);
                if (newObj.GetComponent<IObjectable>() != null)
                    newObj.GetComponent<IObjectable>().ObjectID = _key;
                else
                    Logger.LogError("Object Key Setting Failed : IObjetable Is null}");

                _onComplete?.Invoke(newObj as T);
            }
        }
        else
        {
            // Debug.LogError("GetObjectError : Object Key is Not Founded");
            // AddPool<T>(await framework.ResourceStorage.GetResource<T>(_objName), 5, _objName);
            // return GetParts<T>(_objName);

            framework.ResourceStorage.GetComponentAsset<T>(_key, _comp =>
            {
                Transform _targetParent = _isUi ? uiParent : this.transform;
                AddPool<T>(_comp, 2, _key, _targetParent);
                GetParts<T>(_key, _isUi, _comp =>
                {
                    _onComplete?.Invoke(_comp);
                });
                // Component obj = poolList[_objName].pool.Dequeue();
                // obj.gameObject.SetActive(true);

            });
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
            Logger.LogError($"ReturnObjectError : Object Key is Not Founded {_objName}");
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
