using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterMask : PlayerEnterExit
{
    SpriteMask mask;
    

    private void Start()
    {
        mask = GetComponent<SpriteMask>();
    }



    protected override void OnPlayerEnter()
    {
        mask.enabled = true;
    }

    protected override void OnPlayerExit()
    {
        mask.enabled = false;
    }

}
