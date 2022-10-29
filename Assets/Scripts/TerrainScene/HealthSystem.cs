using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int HP = 50;
    public bool isAlive => HP > 0;
    public event Action<GameObject> OnDeathEvent;
    public event Action OnReceiveDamage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*Debug.Log("Hit");
        if (collision.gameObject.tag == "Bullet")
        {
            HP -= 20;
            Debug.Log("Hit by a bullet, new HP " + HP);
            Destroy(collision.gameObject);
            if (HP < 0)
            {
                OnDeathEvent?.Invoke(gameObject);
                Destroy(this.gameObject);
            }
        }*/
    }

    public void makeDamage(int damage)
    {
        HP -= damage;
        Debug.Log("Hit by a bullet, new HP " + HP);
        OnReceiveDamage?.Invoke();
        if (HP <= 0)
        {
            OnDeathEvent?.Invoke(gameObject);
            Destroy(this.gameObject);
        }
    }

    public void KILL()
    {
        makeDamage(HP);
/*        OnDeathEvent = null;
        OnReceiveDamage = null;*/
    }
}
