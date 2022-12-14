using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    //[SerializeField] private TextMeshPro textMeshPro;
    //[SerializeField] private GameObject Inner;
    private Grid grid;
    public bool isWalkable;
    public int x, y ;
    public int gCost, hCost, fCost;
    public Cell pastCell;
    [Header("Action filter/colors")]
    public SpriteRenderer filter;
    public Color selectedColor, disabledColor;
    private Unit unit;

    public void Init(Grid grid, int x, int y, bool isWalkable)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.isWalkable = isWalkable;
        SetText(x + "," + y);
    }

    public Vector2 Position => transform.localPosition;

    public void SetText(string text)
    {
       // textMeshPro.text = text;
    }

    public void SetColor(Color color)
    {
        //Inner.GetComponent<SpriteRenderer>().color = color;
    }

    internal void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    internal void SetWalkable(bool v)
    {
        isWalkable = v;
        SetColor(Color.black);
    }

    public override string ToString()
    {
        return "Cell "+x + "," + y;
    }

    public void setUnit(Unit _unit)
    {
        if (unit != null)
        {
            unit.healthSystem.OnDeathEvent -= OnUnitDeath;
        }
        unit = _unit;
        if (unit != null)
        {
            unit.healthSystem.OnDeathEvent += OnUnitDeath;
            SetWalkable(false);
        }
        else
        {
            this.SetWalkable(true);
        }
    }

    private void OnUnitDeath(GameObject obj)
    {
        SetWalkable(true);
        setUnit(null);
    }

    private void Update()
    {
        GetComponent<SpriteRenderer>().color = isWalkable? Color.blue: Color.red;
    }

    public Unit getUnit()
    {
        return unit;
    }
}
