using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSignal : PlayerEnterExit
{
    Upgrade up;
    ParticleSystem sys;

    void Start()
    {
        sys = GetComponent<ParticleSystem>();
        up = GetComponentInParent<Upgrade>();
    }

    protected override void OnPlayerEnter()
    {
        if (!up.GetIsPicked()) { sys.Play();  }
    }

    protected override void OnPlayerExit()
    {
        sys.Stop();
    }
}
