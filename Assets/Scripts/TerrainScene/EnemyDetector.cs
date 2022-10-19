using UnityEngine;
using System;

public class EnemyDetector : MonoBehaviour
{
    public event Action<Collider2D> OnEnemyDetected;
    public event Action OnEnemyExit;

    public float range;
    [SerializeField] private int searchPerSecond = 15;
    public LayerMask enemyLayer;
    public GameObject target { get; private set; }
    public bool isEnemyDetected { get { return target != null; } }
    private bool _isEnemyDetected = false;

    private void Start()
    {
        InvokeRepeating("LookForEnemy", 0, 1f / searchPerSecond);
    }

    public void LookForEnemy()
    {
        Collider2D collider = Physics2D.OverlapCircle((Vector2)transform.position, range, enemyLayer);
        if (collider != null)
        {
            if (!isEnemyDetected)
            {
                target = collider.gameObject;
            }
        }
        else
        {
            target = null;
        }
        if(isEnemyDetected != _isEnemyDetected)
        {
            if (isEnemyDetected)
                OnEnemyDetected?.Invoke(collider);
            else
                OnEnemyExit?.Invoke();
            _isEnemyDetected = isEnemyDetected;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
