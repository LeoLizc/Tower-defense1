using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Unit
{
    Rigidbody2D rb;
    public int x { get; private set; }
    public int y { get; private set; }

    [Header("Detection")]
    
    [SerializeField] private float range;
    [SerializeField] private int searchPerSecond = 15;
    [SerializeField] private LayerMask playerLayer;
    private GameObject target;
    public bool isPlayerDetected { get { return target!=null; } }

    [Header("Attack")]
    public Bullet bulletPrefab;
    public Transform firePoint;
    [SerializeField] private int attack;
    [SerializeField] private int bulletForce;
    [SerializeField] private float attackInterval = 1/5f;
    [SerializeField] private LayerMask bulletLayer = 0;
    [SerializeField] private Color bulletColor;
    [Space]
    private float time = 0f;
    public bool gizmosPersistent = false;

    [Header("Health")]
    [SerializeField] private int hp;
    [SerializeField] private int cost;

    new private void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        time = attackInterval;
        InvokeRepeating("lookForEnemy", 0, (float)1 / searchPerSecond);
        healthSystem.HP = hp;
    }

    public void locate(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*Debug.Log("Hit");
        if (collision.gameObject.tag == "Bullet")
        {
            hp -= 20;
            Debug.Log("Hit by a bullet, new HP " + hp);
            Destroy(collision.gameObject);
            if (hp < 0)
            {
                Destroy(this.gameObject);
            }
            //Destroy(collision.gameObject);
        }*/
    }
    private void lookForEnemy()
    {
        Collider2D collider = Physics2D.OverlapCircle((Vector2)transform.position, range, playerLayer);
        if (collider != null)
        {
            if(!isPlayerDetected)
            target = collider.gameObject;
        }
        else
        {
            target = null;
        }
    }

    public void lookAtTarget()
    {
        Transform tTransform = target.transform;
        Vector2 lookDir = new Vector2(tTransform.position.x, tTransform.position.y) - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;
        rb.rotation = angle;
    }

    private void Update()
    {
        if (isPlayerDetected)
        {
            lookAtTarget();
            if (time <= 0)
            {
                shoot();
                time += attackInterval;
            }
            time -= Time.deltaTime;
        }
    }

    public void shoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        //bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        bullet.seek(target, attack, bulletForce, bulletLayer, bulletColor);
    }

    private void drawGizmos()
    {
        Gizmos.color = isPlayerDetected ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.color = Color.blue;
        if(firePoint != null)
            Gizmos.DrawSphere(firePoint.position, 0.13f);
    }

    private void OnDrawGizmosSelected()
    {
        if (!gizmosPersistent)
        {
            drawGizmos();
        }
    }

    private void OnDrawGizmos()
    {
        if (gizmosPersistent)
        {
            drawGizmos();
        }
    }
}
