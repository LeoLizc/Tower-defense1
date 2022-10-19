using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyDetector))]
public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint;
    public Bullet bulletPrefab;
    public int attack = 10;
    public float bulletForce = 20f;
    public float shootingInterval = 0.3f;
    public LayerMask bulletLayer;
    public Color bulletColor;
    [Space]
    private float period = 0.0f;
    //public float range = 3 * 0.7f;
    public bool startShotting { get; private set; } = false;
    private Rigidbody2D rb;

    private EnemyDetector _detector;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _detector = GetComponent<EnemyDetector>();
    }

    private void Start()
    {
        _detector.OnEnemyDetected += OnTowerDetected;
        _detector.OnEnemyExit += OnTowerExit;
    }

    private void OnTowerExit()
    {
        startShotting = false;
    }

    private void OnTowerDetected(Collider2D collider)
    {
        startShotting = true;
        lookAtTarget(collider.gameObject.transform);
    }

    //https://www.youtube.com/watch?v=LNLVOjbrQj4

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PowerSource")
        {
            startShotting = true;
            lookAtTarget(collision.gameObject.transform);
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PowerSource")
        {
            startShotting = false;
        }
    }*/

    public void lookAtTarget(Transform target)
    {
        Vector2 lookDir = new Vector2(target.position.x, target.position.y) - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;
        rb.rotation = angle;
    }

    void Update()
    {
        if (!startShotting)
            return;

        if (period > shootingInterval)
        {
            shoot();
            period = 0;
        }
        period += UnityEngine.Time.deltaTime;
    }

    private void shoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        //bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        bullet.seek(_detector.target, attack, bulletForce, bulletLayer, bulletColor);
    }
}
