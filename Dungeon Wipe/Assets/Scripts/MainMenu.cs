using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

/// <summary>
/// Manages the main menu, including level selection and high score management.
/// </summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject scrollViewContent; // Content for high scores
    [SerializeField] private GameObject levelScrollViewContent; // Content for levels
    [SerializeField] private GameObject levelEditorScrollViewContent; // Content for levels in the editor
    [SerializeField] private GameObject levelButtonPrefab; // Prefab for level selection buttons
    [SerializeField] private LevelEditorSO levelEditor; // ScriptableObject for level editor data
    private string levelsFolderPath; // Path to the levels folder
    private string[] levelFiles; // Array of level file paths

    /// <summary>
    /// Initializes the menu by loading levels and high scores.
    /// </summary>
    private void Start()
    {
        levelsFolderPath = Path.Combine(Application.dataPath, "Resources", "Levels");
        levelFiles = Directory.GetFiles(levelsFolderPath, "*.json");

        if (PlayerPrefsManager.Instance != null)
        {
            PlayerPrefsManager.Instance.LoadScoresIntoScrollView(scrollViewContent);
        }

        LoadLevelButtons(levelScrollViewContent);
        LoadEditorButtons(levelEditorScrollViewContent);
    }

    /// <summary>
    /// Creates a button in the specified content.
    /// </summary>
    /// <param name="content">Parent object for the button.</param>
    /// <param name="buttonText">Text to display on the button.</param>
    /// <param name="action">Action to trigger on button click.</param>
    private void CreateButton(GameObject content, string buttonText, UnityEngine.Events.UnityAction action)
    {
        GameObject button = Instantiate(levelButtonPrefab, content.transform);

        var textMeshComp = button.GetComponentInChildren<TextMeshProUGUI>();
        textMeshComp.text = buttonText;

        var buttonComp = button.GetComponent<Button>();
        buttonComp.onClick.AddListener(action);
    }

    /// <summary>
    /// Loads level buttons into the specified content.
    /// </summary>
    /// <param name="content">Parent object to load the level buttons into.</param>
    private void LoadLevelButtons(GameObject content)
    {
        foreach (var filePath in levelFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);

            // Add a space between "Level" and the number
            string spacedFileName = AddSpaceToLevelName(fileName);

            CreateButton(content, spacedFileName, () => SelectLevel(filePath));
        }
    }

    /// <summary>
    /// Loads level editor buttons into the specified content.
    /// </summary>
    /// <param name="content">Parent object to load the editor buttons into.</param>
    private void LoadEditorButtons(GameObject content)
    {
        foreach (var filePath in levelFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);

            // Add a space between "Level" and the number
            string spacedFileName = AddSpaceToLevelName(fileName);

            CreateButton(content, spacedFileName, () => OpenEditor(filePath));
        }

        // Button for creating a new level
        int totalLevels = levelFiles.Length;
        CreateButton(content, "New Level", () => OpenEditor(Path.Combine(levelsFolderPath, $"Level{totalLevels + 1}.json")));
    }

    /// <summary>
    /// Adds a space between "Level" and the number in the file name.
    /// </summary>
    /// <param name="fileName">The original file name without extension.</param>
    /// <returns>The modified file name with a space added.</returns>
    private string AddSpaceToLevelName(string fileName)
    {
        if (fileName.StartsWith("Level") && fileName.Length > 5)
        {
            return fileName.Insert(5, " ");
        }
        return fileName;
    }

    /// <summary>
    /// Selects a level to load in the game.
    /// </summary>
    /// <param name="filePath">Path to the level file.</param>
    private void SelectLevel(string filePath)
    {
        levelEditor.SelectedLevelPath = filePath;
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// Opens the level editor for a specific level.
    /// </summary>
    /// <param name="filePath">Path to the level file.</param>
    public void OpenEditor(string filePath)
    {
        levelEditor.SelectedLevelPath = filePath;
        SceneManager.LoadScene("Editor");
    }

    /// <summary>
    /// Exits the game application.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Resets all high scores to zero.
    /// </summary>
    public void ResetHighScores()
    {
        if (PlayerPrefsManager.Instance != null)
        {
            PlayerPrefsManager.Instance.ClearAllHighScores(scrollViewContent);
        }
    }

    /// <summary>
    /// Generates sample high scores for testing purposes.
    /// </summary>
    public void GenerateHighScores()
    {
        if (PlayerPrefsManager.Instance != null)
        {
            PlayerPrefsManager.Instance.CreateSampleHighScores();
            PlayerPrefsManager.Instance.LoadScoresIntoScrollView(scrollViewContent);
        }
    }
}
