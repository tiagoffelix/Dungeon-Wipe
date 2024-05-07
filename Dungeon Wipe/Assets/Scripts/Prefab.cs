using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class Prefab : MonoBehaviour
{
    private NavMeshSurface nav;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshSurface>();
        nav.BuildNavMesh();
    }
}
