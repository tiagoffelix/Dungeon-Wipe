using UnityEngine;

public class RandomSkybox : MonoBehaviour
{
    [SerializeField] private Material[] skyboxMaterials;

    void Start()
    {
        if (skyboxMaterials.Length > 0)
        {
            int randomIndex = Random.Range(0, skyboxMaterials.Length);
            RenderSettings.skybox = skyboxMaterials[randomIndex];

            // Update ambient lighting to default values
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;

            // Recalculate lighting
            DynamicGI.UpdateEnvironment();
        }
        else
        {
            Debug.LogWarning("No skybox materials assigned.");
        }
    }
}