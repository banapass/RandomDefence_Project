using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effector : MonoBehaviour, IObjectable
{
    public string ObjectID { get; set; }
    public ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }
    public void ReturnPool()
    {
        ObjectPoolManager.Instance.ReturnParts(this, ObjectID);

    }
    protected void OnParticleSystemStopped()
    {
        ReturnPool();
    }


}
