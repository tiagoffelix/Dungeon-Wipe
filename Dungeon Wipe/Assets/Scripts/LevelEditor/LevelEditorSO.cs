using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject used for storing level editor settings.
/// </summary>
[CreateAssetMenu(fileName = "NewLevelEditor", menuName = "LevelEditor")]
public class LevelEditorSO : ScriptableObject
{
    [SerializeField] private int gridSizeX; // The size of the grid along the X-axis.

    [SerializeField] private int gridSizeY; // The size of the grid along the Y-axis.

    [SerializeField] private int grids; // The number of grids.

    [SerializeField] private List<int> gridsDeactivated; // List of grids that are deactivated.

    [SerializeField] private string selectedLevelPath; // The file path of the selected level.

    [SerializeField] private bool levelLoaded; // if the level is loaded

    [SerializeField] private int levelMinutes; // The time (in minutes) for each level

    /// <summary>
    /// Gets or sets the file path of the selected level.
    /// </summary>
    public string SelectedLevelPath
    {
        get { return selectedLevelPath; }
        set { selectedLevelPath = value; }
    }

    /// <summary>
    /// Gets or sets the size of the grid along the X-axis.
    /// </summary>
    public int GridSizeX
    {
        get { return this.gridSizeX; }
        set { this.gridSizeX = value; }
    }

    /// <summary>
    /// Gets or sets the size of the grid along the Y-axis.
    /// </summary>
    public int GridSizeY
    {
        get { return this.gridSizeY; }
        set { this.gridSizeY = value; }
    }

    /// <summary>
    /// Gets or sets the number of grids.
    /// </summary>
    public int Grids
    {
        get { return this.grids; }
        set { this.grids = value; }
    }

    /// <summary>
    /// Gets or sets the list of grids that are deactivated.
    /// </summary>
    public List<int> GridsDeactivated
    {
        get { return this.gridsDeactivated; }
        set { this.gridsDeactivated = value; }
    }

    /// <summary>
    /// Gets or sets if the level is loaded.
    /// </summary>
    public bool LevelLoaded
    {
        get { return this.levelLoaded; }
        set { this.levelLoaded = value; }
    }

    /// <summary>
    /// Gets or sets the time (in minutes) for each level.
    /// </summary>
    public int LevelMinutes
    {
        get { return this.levelMinutes; }
        set { this.levelMinutes = value; }
    }
}
