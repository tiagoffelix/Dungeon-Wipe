using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Collections;

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
    [SerializeField] private GameObject CollectiblesCanvas;
    [SerializeField] private GameObject onlyHealthButton;
    [SerializeField] private SpawnCollectibles healthSettings;
    [SerializeField] private GameObject loadingScreen; // Loading screen GameObject
    [SerializeField] private Slider progressBar; // Progress bar Slider

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

        Image indicatorImage = onlyHealthButton.transform.Find("Indicator").GetComponent<Image>();
        indicatorImage.gameObject.SetActive(healthSettings.OnlyHealth);

        onlyHealthButton.GetComponent<Button>().onClick.AddListener(()
            => ToggleGridActivation(indicatorImage));

        levelEditor.LevelLoaded = false;
    }

    /// <summary>
    /// Updates the menu to check for user input.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            CollectiblesCanvas.SetActive(!CollectiblesCanvas.activeSelf);
        }
    }

    /// <summary>
    /// Toggles the activation of only health collectibles.
    /// </summary>
    /// <param name="indicatorImage">Indicator image to be toggled.</param>
    private void ToggleGridActivation(Image indicatorImage)
    {
        healthSettings.OnlyHealth = !healthSettings.OnlyHealth;

        if (healthSettings.OnlyHealth)
        {
            indicatorImage.gameObject.SetActive(true); // Hide indicator when active
        }
        else
        {
            indicatorImage.gameObject.SetActive(false); // Show indicator when inactive
        }
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
        StartCoroutine(LoadSceneAsync("Game"));
    }

    /// <summary>
    /// Opens the level editor for a specific level.
    /// </summary>
    /// <param name="filePath">Path to the level file.</param>
    public void OpenEditor(string filePath)
    {
        levelEditor.SelectedLevelPath = filePath;
        StartCoroutine(LoadSceneAsync("Editor"));
    }

    /// <summary>
    /// Coroutine to load a scene asynchronously with a loading screen.
    /// </summary>
    /// <param name="sceneName">Name of the scene to load.</param>
    /// <returns>IEnumerator for the coroutine.</returns>
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingScreen.SetActive(true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!levelEditor.LevelLoaded)
        {
            progressBar.value = asyncOperation.progress;

            yield return null;
        }

        loadingScreen.SetActive(false);
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
