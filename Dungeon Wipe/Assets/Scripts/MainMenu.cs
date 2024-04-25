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
        var levelFiles = Resources.LoadAll<TextAsset>("Levels"); // Ensure all level files are in Resources/Levels
        foreach (var file in levelFiles)
        {
            GameObject button = Instantiate(levelButtonPrefab, levelScrollViewContent.transform);
            button.GetComponentInChildren<Text>().text = file.name;
            button.GetComponent<Button>().onClick.AddListener(() => SelectLevel(file.name));
        }
    }

    private void SelectLevel(string levelName)
    {
        stats.SelectedLevelPath = "Assets/Resources/Levels/" + levelName + ".txt";
    }

    public void StartGame()
    {
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
