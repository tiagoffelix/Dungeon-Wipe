using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject scrollViewContent; // For high scores
    [SerializeField] private GameObject levelScrollViewContent; // For levels
    [SerializeField] private GameObject levelEditorScrollViewContent; // For levels
    [SerializeField] private GameObject levelButtonPrefab; // Assign a prefab for level buttons
    [SerializeField] private LevelEditorSO levelEditor;
    private TextAsset[] levelFiles;

    private void Start()
    {
        

        if (PlayerPrefsManager.Instance != null)
        {
            PlayerPrefsManager.Instance.LoadScoresIntoScrollView(scrollViewContent);
        }

        LoadLevelButtons(levelScrollViewContent);
        LoadEditorButtons(levelEditorScrollViewContent);
    }

    private void CreateButton(GameObject content, string buttonText, UnityEngine.Events.UnityAction action)
    {
        GameObject button = Instantiate(levelButtonPrefab, content.transform);

        var textMeshComp = button.GetComponentInChildren<TextMeshProUGUI>();
        textMeshComp.text = buttonText;

        var buttonComp = button.GetComponent<Button>();
        buttonComp.onClick.AddListener(action);
    }

    private void LoadLevelButtons(GameObject content)
    {
        string levelsFolderPath = Path.Combine(Application.dataPath, "Resources", "Levels");

        // Get all .json files in the directory
        var levelFiles = Directory.GetFiles(levelsFolderPath, "*.json");

        foreach (var filePath in levelFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            CreateButton(content, fileName, () => SelectLevel(fileName));
        }
    }

    private void LoadEditorButtons(GameObject content)
    {

        string levelsFolderPath = Path.Combine(Application.dataPath, "Resources", "Levels");

        // Get all .json files in the directory
        var levelFiles = Directory.GetFiles(levelsFolderPath, "*.json");

        foreach (var filePath in levelFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            CreateButton(content, fileName, () => OpenEditor(fileName));
        }

        // Instantiate button for new level
        int totalLevels = levelFiles.Length;
        CreateButton(content, $"New Level", () => OpenEditor($"Level {totalLevels + 1}"));
    }


    private void SelectLevel(string levelName)
    {
        string filePath;

        #if UNITY_EDITOR
                filePath = "Assets/Resources/Levels/" + levelName + ".json";
        #elif UNITY_STANDALONE_WIN
                        filePath = Path.Combine(Application.dataPath, "Resources", "Levels", levelName + ".json");
        #endif
        levelEditor.SelectedLevelPath = filePath;

        SceneManager.LoadScene("Game");
    }

    public void OpenEditor(string levelName) 
    {
        string filePath;

        #if UNITY_EDITOR
                filePath = "Assets/Resources/Levels/" + levelName + ".json";
        #elif UNITY_STANDALONE_WIN
                        filePath = Path.Combine(Application.dataPath, "Resources", "Levels", levelName + ".json");
        #endif
        levelEditor.SelectedLevelPath = filePath;
        SceneManager.LoadScene("Editor");
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
