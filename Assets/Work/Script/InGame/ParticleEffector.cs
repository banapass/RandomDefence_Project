using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffector : Effector
{
    protected ParticleSystem particle;
    private void Awake()
    {
        if (particle == null)
            particle = GetComponent<ParticleSystem>();
    }

    protected void OnParticleSystemStopped()
    {
        ReturnPool();
    }
}
