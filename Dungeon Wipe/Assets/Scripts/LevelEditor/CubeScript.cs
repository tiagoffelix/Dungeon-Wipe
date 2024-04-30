using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    private int gridX; // Coordenada X do cubo na grade
    private int gridY; // Coordenada Y do cubo na grade

    public int GridX { get { return gridX; } set { gridX = value; } }

    public int GridY { get { return gridY; } set { gridY = value; } }
}
