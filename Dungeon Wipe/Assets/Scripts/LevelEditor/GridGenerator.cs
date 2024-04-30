using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private LevelEditorSO levelEditor;
    [SerializeField] private GameObject cubePrefab; // Prefab do cubo

    private List<GameObject> gridCubes;

    [SerializeField] private Slider sliderX;
    [SerializeField] private Slider sliderY;

    [SerializeField] private TextMeshProUGUI textX;
    [SerializeField] private TextMeshProUGUI textY;

    void Start()
    {
        gridCubes = new List<GameObject>();
        sliderX.value = levelEditor.GridSizeX;
        sliderY.value = levelEditor.GridSizeY;
        textX.text = "Grid Width: " + sliderY.value;
        textY.text = "Grid Height: " + sliderY.value;
        sliderX.onValueChanged.AddListener(UpdateGridSizeX);
        sliderY.onValueChanged.AddListener(UpdateGridSizeY);
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        Vector3 startPos = new Vector3(-levelEditor.GridSizeX / 2 + 0.5f, 0, -levelEditor.GridSizeY / 2 + 0.5f);

        for (int x = 0; x < levelEditor.GridSizeX; x++)
        {
            for (int y = 0; y < levelEditor.GridSizeY; y++)
            {
                Vector3 spawnPos = new Vector3(startPos.x + x, 0, startPos.z + y);
                GameObject cube = Instantiate(cubePrefab, spawnPos, Quaternion.identity);
                gridCubes.Add(cube);

                CubeScript cubeScript = cube.GetComponent<CubeScript>();
                if (cubeScript != null)
                {
                    cubeScript.GridX = x;
                    cubeScript.GridY = y;
                }
            }
        }
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

    private void OnDestroy()
    {
        // Remove listener to avoid memory leaks
        sliderX.onValueChanged.RemoveListener(UpdateGridSizeX);
        sliderY.onValueChanged.RemoveListener(UpdateGridSizeY);
    }
}
