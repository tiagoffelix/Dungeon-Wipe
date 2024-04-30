using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelEditor", menuName = "LevelEditor")]
public class LevelEditorSO : ScriptableObject
{
    [SerializeField] private int gridSizeX;

    [SerializeField] private int gridSizeY;

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
}
