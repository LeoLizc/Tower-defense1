using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSource : Unit
{
    public int HP = 1000;

    new private void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        healthSystem.HP = HP;
    }
}
