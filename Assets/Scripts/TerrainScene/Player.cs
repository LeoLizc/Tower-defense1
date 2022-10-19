using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ref: https://drive.google.com/file/d/1WiF2LwM-6WvEnas9vw32YrYPly9K0Qrv/view

[RequireComponent(typeof(EnemyDetector))]
public class Player : MonoBehaviour
{
    List<Cell> path;
    [SerializeField]
    private float moveSpeed = 2f;
    public Vector2 GetPosition => transform.localPosition;
    private bool startMoving = false;
    private Grid grid;
    private bool changedCells = false;
    private Rigidbody2D rb;

    // Index of current waypoint from which Enemy walks
    // to the next one
    private int waypointIndex = 0;

    // Need to create an enemy detector
    private EnemyDetector detector;

    private void Awake()
    {
        detector = GetComponent<EnemyDetector>();
    }

    private void Start()
    {
        detector.OnEnemyDetected += onTowerDetected;
        detector.OnEnemyExit += onTowerExit;
        GetComponent<PlayerHealth>().OnDeathEvent += OnPlayerDeath;
    }

    private void OnPlayerDeath(GameObject obj)
    {
        if(path != null)
        {
            if (!changedCells)
                path[waypointIndex].SetWalkable(true);
            else
                path[waypointIndex - 1].SetWalkable(true);
        }
    }

    private void onTowerExit()
    {
        Debug.Log("Do it");
        //path = null;
        startMoving = true;
    }

    private void onTowerDetected(Collider2D obj)
    {
        Debug.Log("Made it");
        //path = null;
        startMoving = false;
    }

    void FixedUpdate()
    {
        if (startMoving)
            Move();
    }



    public void starMoving(Grid grid, float speed)
    {
        this.grid = grid;
        calculatePath();
        //Debug.Log("Position X: " + (int)GetPosition.x + "Position Y: " + (int)GetPosition.y);
        startMoving = true;
        moveSpeed = speed;
    }

    private void calculatePath()
    {
        waypointIndex = 0;
        path = PathManager.Instance.FindPath(grid, (int)GetPosition.x, (int)GetPosition.y);
    }

    public void ResetPosition()
    {
        transform.position = new Vector2(0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PowerSource")
        {
            Debug.Log("Made it");
            //path = null;
            startMoving = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PowerSource")
        {
            Debug.Log("Do it");
            //path = null;
            startMoving = true;
        }
    }

    private void Move()
    {
        // If player didn't reach last waypoint it can move
        // If player reached last waypoint then it stops
        if (path == null)
            return;

        if (waypointIndex <= path.Count - 1)
        {
            //Debug.Log("Moving to " + path[waypointIndex].transform.position.x.ToString() + " "
            //    + path[waypointIndex].transform.position.y.ToString());

            if (changedCells) {
                changedCells = false;
                if (!grid.isWalkable((int)path[waypointIndex].Position.x, (int)path[waypointIndex].Position.y))
                {
                    //Debug.Log("not walkable");
                    //path = null;
                    calculatePath();
                    return;
                } else
                {
                    grid.setBusyCell((int)path[waypointIndex - 1].Position.x,
                        (int)path[waypointIndex - 1].Position.y,
                        (int)path[waypointIndex].Position.x,
                        (int)path[waypointIndex].Position.y);
                }
                
            }
            // Move player from current waypoint to the next one
            // using MoveTowards method
            transform.localPosition = Vector2.MoveTowards(GetPosition,
               path[waypointIndex].Position,
               moveSpeed * Time.deltaTime);

            // If player reaches position of waypoint he walked towards
            // then waypointIndex is increased by 1
            // and player starts to walk to the next waypoint
            if (GetPosition == path[waypointIndex].Position)
            {
                waypointIndex += 1;
                changedCells = true;
            }
        }
    }
}
