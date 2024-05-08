using Unity.AI.Navigation;
using UnityEngine;

public class PrefabScript : MonoBehaviour
{
    private NavMeshSurface nav;

    void Start()
    {
        nav = GetComponent<NavMeshSurface>();
        if (nav == null)
        {
            Debug.LogError("NavMeshSurface component not found on the GameObject");
            return;
        }
        BuildMesh();
    }

    public void BuildMesh()
    {
        nav.BuildNavMesh();
    }
}
