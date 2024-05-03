using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private LevelEditorSO levelEditor;
    [SerializeField] private GameObject cubePrefab; // Prefab do cubo

    private List<GameObject> gridCubes;

    public List<GameObject> GridCubes { get => gridCubes; set => gridCubes = value; }

    [SerializeField] private Slider sliderX;
    [SerializeField] private Slider sliderY;
    [SerializeField] private Slider sliderGrids;

    [SerializeField] private TextMeshProUGUI textGrids;
    [SerializeField] private TextMeshProUGUI textX;
    [SerializeField] private TextMeshProUGUI textY;

    void Start()
    {
        gridCubes = new List<GameObject>();
        sliderX.value = levelEditor.GridSizeX;
        sliderY.value = levelEditor.GridSizeY;
        sliderGrids.value = levelEditor.Grids;
        textGrids.text = "Grid Size: " + sliderGrids.value;
        textX.text = "Grid Width: " + sliderX.value;
        textY.text = "Grid Height: " + sliderY.value;
        sliderX.onValueChanged.AddListener(UpdateGridSizeX);
        sliderY.onValueChanged.AddListener(UpdateGridSizeY);
        sliderGrids.onValueChanged.AddListener(UpdateGridSize);
        GenerateGrid();
    }

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

    private bool IsGridDisabled(int gridNumber)
    {
        List<int> disabledGrids = levelEditor.GridsDeactivated;
        return disabledGrids != null && disabledGrids.Contains(gridNumber);
    }

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

    public void UpdateGridSizeX(float value)
    {
        levelEditor.GridSizeX = (int)value;
        textX.text = "Grid Width: " + levelEditor.GridSizeX;
    }

    public void UpdateGridSizeY(float value)
    {
        levelEditor.GridSizeY = (int)value;
        textY.text = "Grid Height: " + levelEditor.GridSizeY;
    }

    public void UpdateGridSize(float value)
    {
        levelEditor.Grids = (int)value;
        textGrids.text = "Grid Size: " + levelEditor.Grids;
    }

    private void OnDestroy()
    {
        // Remove listener to avoid memory leaks
        sliderX.onValueChanged.RemoveListener(UpdateGridSizeX);
        sliderY.onValueChanged.RemoveListener(UpdateGridSizeY);
        sliderGrids.onValueChanged.RemoveListener(UpdateGridSize);
    }
}
