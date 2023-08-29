using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingObject : MonoBehaviour, IObjectable
{
    public string ObjectID { get; set; }
    public bool IsPooling { get { return !string.IsNullOrEmpty(ObjectID); } }

    public void ReturnPool()
    {
        ObjectPoolManager.Instance.ReturnParts(this, ObjectID);
    }
}
