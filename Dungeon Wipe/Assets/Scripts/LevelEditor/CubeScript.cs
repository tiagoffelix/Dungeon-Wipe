using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    private new Renderer renderer;
    [SerializeField] private Color hoverColorFree;
    [SerializeField] private Color hoverColorOccupied;
    [SerializeField] private Color defaultColor;

    private bool hasObjectOnTop;

    void Start()
    {
        hasObjectOnTop = false;
        renderer = GetComponent<Renderer>();
        renderer.material.color = defaultColor;
    }

    public void UpdateHoverState()
    {
        hasObjectOnTop = HasObjectOnTop();

        if (hasObjectOnTop)
        {
            renderer.material.color = hoverColorOccupied;
        }
        else
        {
            renderer.material.color = hoverColorFree;
        }

        if (PrefabManager.Instance.CurrentPrefab != null && !hasObjectOnTop)
        {
            PrefabManager.Instance.CurrentPrefab.transform.position = transform.position;
            PrefabManager.Instance.CurrentPrefab.GetComponent<PrefabFollower>().enabled = false;
        }
    }

    public void ResetCubeColor()
    {
        renderer.material.color = defaultColor;
        if (PrefabManager.Instance.CurrentPrefab != null)
        {
            PrefabManager.Instance.CurrentPrefab.GetComponent<PrefabFollower>().enabled = true;
        }
    }

    public void HandleMouseDown()
    {
        if (PrefabManager.Instance.CurrentPrefab != null)
        {
            // Check if a player prefab has already been placed
            if (PrefabManager.Instance.CurrentPrefab.layer == LayerMask.NameToLayer("Player") && PrefabManager.Instance.PlayerPlaced)
            {
                return; // Exit if a player prefab is already placed
            }

            // Check if there is no object on top
            if (!hasObjectOnTop)
            {
                GameObject newPrefab = PrefabManager.Instance.InstantiatePrefab(
                    PrefabManager.Instance.CurrentPrefab, transform.position,
                    PrefabManager.Instance.CurrentPrefab.transform.rotation);

                if (newPrefab.layer == LayerMask.NameToLayer("Player"))
                {
                    PrefabManager.Instance.PlayerPlaced = true;
                    PrefabManager.Instance.DestroyCurrentPrefab();
                }
            }
        }
    }

    private bool HasObjectOnTop()
    {
        Vector3 boxSize = new Vector3(transform.localScale.x * 0.15f, transform.localScale.y * 0.15f, transform.localScale.z * 0.15f);
        Collider[] colliders = Physics.OverlapBox(transform.position, boxSize / 2, Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.gameObject != PrefabManager.Instance.CurrentPrefab)
            {
                return true;
            }
        }
        return false;
    }
}
