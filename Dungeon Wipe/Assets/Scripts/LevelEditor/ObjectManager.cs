using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] private Button toggleButton; // Serialized field for the button

    private bool deleting; // Public variable to control deletion
    private GameObject lastHoveredObject;

    // Dictionary to store original colors
    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();

    private void Start()
    {
        deleting = false;
        UpdateButtonAlpha();
    }

    // In ObjectManager.cs
    void Update()
    {
        if (deleting)
        {
            // Raycast to detect the object the mouse is pointing at
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            int combinedMask = Physics.DefaultRaycastLayers & ~(1 << LayerMask.NameToLayer("DeactivatedLayer"));

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, combinedMask))
            {
                GameObject hoveredObject = hit.collider.gameObject;

                // Only process objects with the "Prefab" tag
                if (hoveredObject.CompareTag("Prefab"))
                {
                    // Change the hover color if object has a Renderer component
                    if (hoveredObject != lastHoveredObject)
                    {
                        // Reset color of the previous object
                        if (lastHoveredObject != null)
                        {
                            ResetColor(lastHoveredObject);
                        }

                        // Apply hover color
                        ApplyHoverColor(hoveredObject);
                        lastHoveredObject = hoveredObject;
                    }

                    // Check for mouse click and delete the object if `deleting` is true
                    if (Input.GetMouseButtonDown(0) && deleting)
                    {
                        // Check if the object has "Prefab" tag and "Player" layer
                        if (hoveredObject.CompareTag("Prefab") && hoveredObject.layer == LayerMask.NameToLayer("Player"))
                        {
                            // Reset PlayerPlaced in PrefabManager
                            PrefabManager.Instance.PlayerPlaced = false;
                        }

                        // Use PrefabManager to remove the prefab instead of Destroy
                        PrefabManager.Instance.RemovePrefab(hoveredObject);
                        lastHoveredObject = null; // Reset the last hovered object
                    }
                }
                else
                {
                    // Reset color if the object is not tagged "Prefab"
                    if (lastHoveredObject != null)
                    {
                        ResetColor(lastHoveredObject);
                        lastHoveredObject = null;
                    }
                }
            }
            else
            {
                // Reset color if no object is being hovered over
                if (lastHoveredObject != null)
                {
                    ResetColor(lastHoveredObject);
                    lastHoveredObject = null;
                }
            }
        }
    }


    public void ChangeDeleting()
    {
        deleting = !deleting;
        UpdateButtonAlpha();
    }

    public void SetDeletingFalse()
    {
        deleting = false;
        UpdateButtonAlpha();
    }

    void ApplyHoverColor(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Store the original color if not already stored
            if (!originalColors.ContainsKey(obj))
            {
                originalColors[obj] = renderer.material.color;
            }

            renderer.material.color = Color.red;
        }
    }

    void ResetColor(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Restore the original color if it was stored
            if (originalColors.ContainsKey(obj))
            {
                renderer.material.color = originalColors[obj];
                originalColors.Remove(obj); // Remove entry after resetting color
            }
            else
            {
                renderer.material.color = Color.white; // Assuming original color is white
            }
        }
    }

    void UpdateButtonAlpha()
    {
        if (toggleButton != null)
        {
            Image buttonImage = toggleButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                Color buttonColor = buttonImage.color;
                buttonColor.a = deleting ? 186 / 255f : 0f; // Convert 186 to a value between 0 and 1
                buttonImage.color = buttonColor;
            }
        }
    }
}
