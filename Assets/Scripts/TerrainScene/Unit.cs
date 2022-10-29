using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public abstract class Unit : MonoBehaviour
{
    [Header("Systems")]
    public HealthSystem healthSystem;
    //public EnemyDetector detector;
    protected void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        //detector = GetComponent<EnemyDetector>();
    }
}
