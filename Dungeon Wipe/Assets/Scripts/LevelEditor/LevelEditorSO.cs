using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelEditor", menuName = "LevelEditor")]
public class LevelEditorSO : ScriptableObject
{
    [SerializeField] private int gridSizeX;

    [SerializeField] private int gridSizeY;

    [SerializeField] private int grids;

    [SerializeField] private string selectedLevelPath;

    public string SelectedLevelPath
    {
        get { return selectedLevelPath; }
        set { selectedLevelPath = value; }
    }

    public int GridSizeX
    {
        get { return this.gridSizeX; }
        set { this.gridSizeX = value; }
    }

    public int GridSizeY
    {
        get { return this.gridSizeY; }
        set { this.gridSizeY = value; }
    }
    public int Grids
    {
        get { return this.grids; }
        set { this.grids = value; }
    }
}
