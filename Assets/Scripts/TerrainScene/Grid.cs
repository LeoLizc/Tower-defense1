using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Grid : ScriptableObject
{
    private int width;
    private int height;
    private int cellSize;
    private Cell cellPrefab;
    private Cell[,] gridArray;
    private GameObject _terrain;
    public GameObject Terrain => _terrain;


    public Grid(int width, int height, int cellSize, Cell cellPrefab)
    {
        
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.cellPrefab = cellPrefab;

        generateBoard();
    }

    private void generateBoard()
    {
        Cell cell;
        gridArray = new Cell[width, height];
        GameObject parent = new GameObject("TerrainParent");

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var p = new Vector2(i, j) * cellSize;
                cell = Instantiate(cellPrefab, p, Quaternion.identity);
                //cell.transform.SetParent(parent.transform);
                cell.Init(this, (int)p.x, (int)p.y, true);

                cell.transform.SetParent(parent.transform);

                //if (Random.Range(0, 10) <= 2)
                //    cell.SetWalkable(false);
                //else
                //    cell.SetColor(Color.blue);

                gridArray[i, j] = cell;
            }
        }

        this._terrain = parent; 
        /*var center = new Vector2((float)height / 2 - 0.5f, (float)width / 2 - 0.5f);

        Camera.main.transform.position = new Vector3(center.y, center.x, -5);*/
    }

    internal int GetHeight()
    {
        return height;
    }

    internal int GetWidth()
    {
        return width;
    }

    public void setBusyCell(int initialX,int initialY, int newX, int newY)
    {
        gridArray[initialX, initialY].SetWalkable(true);
        gridArray[newX, newY].SetWalkable(false);
    }

    public void setBusyCell(int initialX, int initialY, int newX, int newY, Unit unit)
    {
        gridArray[initialX, initialY].SetWalkable(true);
        gridArray[initialX, initialY].setUnit(null);
        gridArray[newX, newY].SetWalkable(false);
        gridArray[newX, newY].setUnit(unit);
    }

    public void setBusyCell(int x, int y)
    {
        if(x >= 0 && x < width && y >= 0 && y < height)
        {
            gridArray[x, y].SetWalkable(false);
        }
    }

    public void setBusyCell(int x, int y, Unit unit)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            gridArray[x, y].SetWalkable(false);
            gridArray[x, y].setUnit(unit);
        }
    }

    public void setFreeCell(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            gridArray[x, y].SetWalkable(true);
        }
    }

    public bool isWalkable(int x, int y)
    {
        return gridArray[x, y].isWalkable;
    }

    public Cell GetGridObject(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
            return gridArray[x, y];
        else
        {
            Debug.LogError("Index out of bounds grid");
            return null;
        }
    }

    internal float GetCellSize()
    {
        return cellSize;
    }
}
