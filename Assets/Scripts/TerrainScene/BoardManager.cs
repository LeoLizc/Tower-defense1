using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;
    [SerializeField] private Cell CellPrefab;
    [SerializeField] private Player PlayerPrefab;
    [SerializeField] private PowerSource PowerSourcePrefab;
    [SerializeField] private Tower TowerPrefab;
    private List<Player> units;
    private List<Tower> towers;
    private Grid grid;
    private GameObject _parent;
    [SerializeField]
    //private float moveSpeed = 2f;

    private void Awake()
    {
        Instance = this;
        _parent = gameObject;
        units = new List<Player>();
        towers = new List<Tower>();
    }

    public Player createUnit(float x, float y)
    {
        Player player = Instantiate(PlayerPrefab, new Vector2(x, y), Quaternion.identity, transform);
        units.Add(player);
        player.GetComponent<PlayerHealth>().OnDeathEvent += checkPlayers;
        return player;
    }

    public void createTower(int x, int y)
    {
        Tower tower = Instantiate(TowerPrefab, new Vector2(x, y), Quaternion.identity, transform);
        tower.locate(x, y);
        tower.OnDeathEvent += OnTowerDead;
        towers.Add(tower);
        grid.setBusyCell(x, y);
    }

    public void SetupBoard()
    {

        grid = new Grid(11, 20, 1, CellPrefab);
        grid.Terrain.transform.SetParent(transform);

        //Instantiate(PowerSourcePrefab, new Vector2(5, 19), Quaternion.identity);
        Instantiate(PowerSourcePrefab, new Vector2(5, 19), Quaternion.identity, transform);
        createTower(8, 8);

        PathManager.Instance.powerUnitLocation = new Vector2Int(5, 19);

        /*Instantiate(PlayerPrefab, new Vector2(8, 2), Quaternion.identity, transform);
        Instantiate(PlayerPrefab, new Vector2(8, 0), Quaternion.identity, transform);*/
        createUnit(5,0);
        createUnit(6,0);

        //transform.position = new Vector3(3, 3, transform.position.z);

        foreach (Player unit in units)
        {
            unit.starMoving(grid, 3);
        }
        //player.starMoving(grid, 3);
        //player.starMoving(grid, 2);

    }

    public void checkPlayers(GameObject g)
    {
        units.Remove(g.GetComponent<Player>());
        if(units.Count<=0)
        GameManager.Instance.UpdateGameState(GameManager.GameStateEnum.end);
    }

    public void OnTowerDead(Tower tower)
    {
        grid.setFreeCell(tower.x, tower.y);
    }
}
