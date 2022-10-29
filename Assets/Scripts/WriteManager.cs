using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "FileWriter", menuName = "TrainOut/FileOut")]
public class WriteManager : ScriptableObject
{
    public string path="Assets/States.txt";
    StreamWriter writer;

    public void Open()
    {
        if (writer != null)
            return;
        if (!File.Exists(path))
        {
            writer = File.CreateText(path);
        }
        else
        {
            writer = new StreamWriter(path, true);
        }
    }

    public string CalcState(Grid grid, int height, Dictionary<string, int> dic)
    {
        int gridHeight = grid.GetHeight() - 1;
        string s = "";
        for (int i = 0; i < grid.GetWidth(); i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (grid.isWalkable(i, gridHeight-j))
                {
                    s = s+";0";
                }
                else
                {
                    string go = grid.GetGridObject(i, gridHeight - j).getUnit().tag;
                    s = s + ";" + dic[go];
                }
            }
        }
        return s.Substring(1);
    }

    public async void write(string s)
    {
        await writer.WriteLineAsync(s);
        writer.Flush();
    }

    private void OnDisable()
    {
        if(writer != null)
        {
            writer.Close();
        }
    }

    private void OnDestroy()
    {
        if (writer != null)
        {
            writer.Close();
        }
    }
}


