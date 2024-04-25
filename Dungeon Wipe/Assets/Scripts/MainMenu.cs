using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject scrollViewContent; // For high scores
    [SerializeField] private GameObject levelScrollViewContent; // For levels
    [SerializeField] private GameObject levelButtonPrefab; // Assign a prefab for level buttons
    [SerializeField] private Stats stats; // Reference to the Stats ScriptableObject

    private void Start()
    {
        if (PlayerPrefsManager.Instance != null)
        {
            PlayerPrefsManager.Instance.LoadScoresIntoScrollView(scrollViewContent);
        }

        LoadLevelButtons();
    }

    private void LoadLevelButtons()
    {
        var levelFiles = Resources.LoadAll<TextAsset>("Levels");
        if (levelFiles.Length == 0)
        {
            Debug.LogWarning("No level files found in Resources/Levels");
            return;
        }

        if (levelButtonPrefab == null)
        {
            Debug.LogError("levelButtonPrefab is not assigned in the inspector");
            return;
        }

        if (levelScrollViewContent == null)
        {
            Debug.LogError("levelScrollViewContent is not assigned in the inspector");
            return;
        }

        foreach (var file in levelFiles)
        {
            GameObject button = Instantiate(levelButtonPrefab, levelScrollViewContent.transform);
            if (button == null)
            {
                Debug.LogError("Instantiation of levelButtonPrefab failed");
                continue;
            }

            var textMeshComp = button.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (textMeshComp == null)
            {
                Debug.LogError("TextMeshProUGUI component not found on the instantiated button prefab");
                continue;
            }
            textMeshComp.text = file.name;

            var buttonComp = button.GetComponent<Button>();
            if (buttonComp == null)
            {
                Debug.LogError("Button component not found on the instantiated button prefab");
                continue;
            }

            buttonComp.onClick.AddListener(() => SelectLevel(file.name));
            Debug.Log(file.name);
        }
    }


    private void SelectLevel(string levelName)
    {
        stats.SelectedLevelPath = "Assets/Resources/Levels/" + levelName + ".txt";
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetHighScores()
    {
        if (PlayerPrefsManager.Instance != null)
        {
            PlayerPrefsManager.Instance.ClearAllHighScores(scrollViewContent);
        }
    }

    public void GenerateHighScores()
    {
        if (PlayerPrefsManager.Instance != null)
        {
            PlayerPrefsManager.Instance.CreateSampleHighScores();
            PlayerPrefsManager.Instance.LoadScoresIntoScrollView(scrollViewContent);
        }
    }
}
