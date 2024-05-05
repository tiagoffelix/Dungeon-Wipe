using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Generates a grid of cubes based on editor settings.
/// </summary>
public class GridGenerator : MonoBehaviour
{
    [SerializeField] private LevelEditorSO levelEditor; // Reference to the LevelEditorSO scriptable object.
    [SerializeField] private GameObject cubePrefab; // The prefab for the cube.

    private List<GameObject> gridCubes; // List to store the generated grid cubes.

    public List<GameObject> GridCubes { get => gridCubes; set => gridCubes = value; } // Property to access the grid cubes list.

    [SerializeField] private Slider sliderX; // Slider for adjusting the grid width.
    [SerializeField] private Slider sliderY; // Slider for adjusting the grid height.
    [SerializeField] private Slider sliderGrids; // Slider for adjusting the number of grids.

    [SerializeField] private TextMeshProUGUI textGrids; // Text displaying the number of grids.
    [SerializeField] private TextMeshProUGUI textX; // Text displaying the grid width.
    [SerializeField] private TextMeshProUGUI textY; // Text displaying the grid height.

    /// <summary>
    /// Initializes the grid generator and generates the grid.
    /// </summary>
    void Start()
    {
        gridCubes = new List<GameObject>();
        sliderX.value = levelEditor.GridSizeX;
        sliderY.value = levelEditor.GridSizeY;
        sliderGrids.value = levelEditor.Grids;
        textGrids.text = "Number of Grids: " + sliderGrids.value;
        textX.text = "Grid Width: " + sliderX.value;
        textY.text = "Grid Height: " + sliderY.value;
        sliderX.onValueChanged.AddListener(UpdateGridSizeX);
        sliderY.onValueChanged.AddListener(UpdateGridSizeY);
        sliderGrids.onValueChanged.AddListener(UpdateGridSize);
        GenerateGrid();
    }

    /// <summary>
    /// Generates the grid of cubes based on editor settings.
    /// </summary>
    private void GenerateGrid()
    {
        for (int grid = 0; grid < levelEditor.Grids; grid++)
        {
            Vector3 startPos = new Vector3(-levelEditor.GridSizeX / 2 + 0.5f, grid, -levelEditor.GridSizeY / 2 + 0.5f);

            for (int x = 0; x < levelEditor.GridSizeX; x++)
            {
                for (int y = 0; y < levelEditor.GridSizeY; y++)
                {
                    Vector3 spawnPos = new Vector3(startPos.x + x, grid, startPos.z + y);
                    GameObject cube = Instantiate(cubePrefab, spawnPos, Quaternion.identity);
                    if (IsGridDisabled(grid))
                    {
                        SetLayerRecursively(cube, LayerMask.NameToLayer("DeactivatedLayer"));
                    }
                    gridCubes.Add(cube);
                }
            }
        }
    }

    /// <summary>
    /// Recursively sets the layer of a GameObject and its children.
    /// </summary>
    private void SetLayerRecursively(GameObject obj, int layer)
    {
        if (obj == null) return;

        // Check if the layer is valid
        if (layer < 0 || layer > 31)
        {
            Debug.LogWarning($"Invalid layer index {layer}. Object '{obj.name}' could not be assigned to this layer.");
            return;
        }

        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    /// <summary>
    /// Checks if a grid is disabled based on editor settings.
    /// </summary>
    private bool IsGridDisabled(int gridNumber)
    {
        List<int> disabledGrids = levelEditor.GridsDeactivated;
        return disabledGrids != null && disabledGrids.Contains(gridNumber);
    }

    /// <summary>
    /// Regenerates the grid based on updated editor settings.
    /// </summary>
    public void RegenerateGrid()
    {
        // Destroy all previous cubes
        foreach (GameObject cube in gridCubes)
        {
            Destroy(cube);
        }
        gridCubes.Clear();

        // Generate new grid
        GenerateGrid();
    }

    /// <summary>
    /// Updates the grid width based on slider value.
    /// </summary>
    public void UpdateGridSizeX(float value)
    {
        levelEditor.GridSizeX = (int)value;
        textX.text = "Grid Width: " + levelEditor.GridSizeX;
    }

    /// <summary>
    /// Updates the grid height based on slider value.
    /// </summary>
    public void UpdateGridSizeY(float value)
    {
        levelEditor.GridSizeY = (int)value;
        textY.text = "Grid Height: " + levelEditor.GridSizeY;
    }

    /// <summary>
    /// Updates the number of grids based on slider value.
    /// </summary>
    public void UpdateGridSize(float value)
    {
        levelEditor.Grids = (int)value;
        textGrids.text = "Number of Grids: " + levelEditor.Grids;
    }

    /// <summary>
    /// Removes event listeners to prevent memory leaks when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        // Remove listener to avoid memory leaks
        sliderX.onValueChanged.RemoveListener(UpdateGridSizeX);
        sliderY.onValueChanged.RemoveListener(UpdateGridSizeY);
        sliderGrids.onValueChanged.RemoveListener(UpdateGridSize);
    }
}
