using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effector : MonoBehaviour, IObjectable
{
    public string ObjectID { get; set; }

    public void ReturnPool()
    {
        ObjectPoolManager.Instance.ReturnParts(this, ObjectID);
    }
}
