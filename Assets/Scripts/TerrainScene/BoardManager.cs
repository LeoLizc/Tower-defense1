using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;
    [SerializeField] private Cell CellPrefab;
    [SerializeField] private List<Player> PlayerPrefabs;
    [SerializeField] private PowerSource PowerSourcePrefab;
    [SerializeField] private List<Tower> TowerPrefabs;
    public int UnitCount=5, TowerCount=5;
    private List<Player> units;
    [SerializeField] private List<Tower> towers;
    private Grid grid;
    private GameObject _parent;
    private PowerSource powerSource;
    public State state;

    public WriteManager manager;
    public Dictionary<string, int> dic;
    string tempOut;
    //private float moveSpeed = 2f;

    private void Awake()
    {
        dic = new Dictionary<string, int>();
        Instance = this;
        _parent = gameObject;
        units = new List<Player>();
        towers = new List<Tower>();
        state = State.Setup;
        manager.Open();

        //Create Dictionary
        dic.Add(PowerSourcePrefab.gameObject.tag, 1);
        for (int i = 0; i < TowerPrefabs.Count; i++)
        {
            dic.Add(TowerPrefabs[i].gameObject.tag, i + 2);
        }

        for (int i = 0; i < PlayerPrefabs.Count; i++)
        {
            dic.Add(PlayerPrefabs[i].gameObject.tag, i + 1);
        }
    }

    private void Start()
    {
        SetupBoard();
    }

    public Player createUnit(int x, int y)
    {
        if (!grid.isWalkable(x, y))
            return null;
        Player player = Instantiate(PlayerPrefabs[Random.Range(0, PlayerPrefabs.Count)], new Vector2(x, y), Quaternion.identity, transform);
        units.Add(player);
        player.GetComponent<HealthSystem>().OnDeathEvent += (GameObject)=>{
            if (state != State.End) units.Remove(player);
            if (units.Count <= 0)
                ReloadInstance(0);
        };
        return player;
    }

    public void createRandomTower(int x, int y)
    {
        if (!grid.isWalkable(x, y))
            return;
        Tower tower = Instantiate(TowerPrefabs[Random.Range(0, TowerPrefabs.Count)], new Vector2(x, y), Quaternion.identity, transform);
        tower.locate(x, y);
        towers.Add(tower);
        grid.setBusyCell(x, y, tower);

        tower.healthSystem.OnDeathEvent += (GameObject g) => { if(state != State.End) towers.Remove(tower); };
    }

    public void SetupPowerSource()
    {
        powerSource = Instantiate(PowerSourcePrefab, new Vector2(5, 19), Quaternion.identity, transform);
        PathManager.Instance.powerUnitLocation = new Vector2Int(5, 19);
        grid.setBusyCell(5, 19, powerSource);
        powerSource.healthSystem.OnDeathEvent += (GameObject) => ReloadInstance(1);
    }

    private void ReloadInstance(int win)
    {
        if (state == State.End)
            return;
        state = State.End;
        //Eliminando Elementos
        Debug.Log("Deleting Units");
        if (powerSource != null) { 
            powerSource.healthSystem.KILL();
        }

        for (int i = 0; i < towers.Count; i++)
        {
            towers[i].healthSystem.KILL();
        }
        towers.Clear();

        for (int i = 0; i < units.Count; i++)
        {
            units[i].healthSystem.KILL();
        }
        units.Clear();

        manager.write(tempOut+";"+win);

        SetupUnits();
    }

    public void SetupBoard()
    {
        Vector3 actualPos = transform.position;
        transform.position = Vector3.zero;
        grid = new Grid(11, 20, 1, CellPrefab);
        grid.Terrain.transform.SetParent(transform);

        //Instantiate(PowerSourcePrefab, new Vector2(5, 19), Quaternion.identity);
        SetupUnits(actualPos);
    }

    public void SetupUnits()
    {
        Vector3 actualPos = transform.position;
        transform.position = Vector3.zero;
        SetupUnits(actualPos);
    }

    public void SetupUnits(Vector3 actualPos)
    {
        SetupPowerSource();

        // CREATING TOWERS
        while (towers.Count < TowerCount)
        {
            createRandomTower(Random.Range(0, 11), Random.Range(15, 20));
        }

        // CREATING UNITS
        while (units.Count < UnitCount)
        {
            createUnit(Random.Range(0, 11), Random.Range(0, 5));
        }

        transform.position = actualPos;
        startGame();
    }

    public void startGame()
    {
        tempOut = manager.CalcState(grid, 5, dic);
        state = State.Running;
        foreach (Player unit in units)
        {
            unit.starMoving(grid, 3);
        }
    }

    public enum State
    {
        Setup,
        Running,
        End
    }
}
