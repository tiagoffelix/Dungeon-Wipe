using Unity.AI.Navigation;
using UnityEngine;

public class PrefabScript : MonoBehaviour
{
    [SerializeField] private NavMeshSurface nav;

    void Start()
    {
        BuildMesh();
    }

    public void BuildMesh()
    {
        nav.BuildNavMesh();
    }
}
