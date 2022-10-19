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
        if (!grid.isWalkable(x, y))
            return;
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
        PathManager.Instance.powerUnitLocation = new Vector2Int(5, 19);
        
        // CREATING TOWERS
        for(int i=0; i<5; i++)
        {
            createTower(Random.Range(0, 11), Random.Range(15, 20));
        }

        // CREATING UNITS
        for (int i = 0; i < 5; i++)
        {
            createUnit(Random.Range(0, 11), Random.Range(0, 5));
        }

        startGame();
    }

    public void startGame()
    {
        foreach (Player unit in units)
        {
            unit.starMoving(grid, 3);
        }
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
