using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

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

        foreach (var file in levelFiles)
        {
            GameObject button = Instantiate(levelButtonPrefab, levelScrollViewContent.transform);

            var textMeshComp = button.GetComponentInChildren<TextMeshProUGUI>();
            textMeshComp.text = file.name;

            var buttonComp = button.GetComponent<Button>();

            buttonComp.onClick.AddListener(() => SelectLevel(file.name));
        }
    }


    private void SelectLevel(string levelName)
    {
        string filePath;

        #if UNITY_EDITOR
                filePath = "Assets/Resources/Levels/" + levelName + ".txt";
        #elif UNITY_STANDALONE_WIN
                filePath = Path.Combine(Application.dataPath, "Resources", "Levels", levelName + ".txt");
        #endif
        stats.SelectedLevelPath = filePath;

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
