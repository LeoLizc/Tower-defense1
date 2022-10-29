using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    private GameObject target;
    private int damage;
    [SerializeField] private float velocity;

    public void seek(GameObject _target, int _damage, float _velocity, LayerMask layer, Color color)
    {
        int lay = (int) Mathf.Log(layer.value,2);
        gameObject.layer = lay;
        gameObject.GetComponent<SpriteRenderer>().color = color;
        seek(_target, _damage, _velocity);
    }
    public void seek(GameObject _target, int _damage, float _velocity)
    {
        seek(_target, _damage);
        velocity = _velocity;
    }

    public void seek(GameObject _target, int _damage)
    {
        target = _target;
        damage = _damage;
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction = target.transform.position - transform.position;
        transform.Translate(direction.normalized * velocity * Time.fixedDeltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<HealthSystem>(out HealthSystem health))
        {
            health.makeDamage(damage);
            Destroy(gameObject);
        }
    }
}
