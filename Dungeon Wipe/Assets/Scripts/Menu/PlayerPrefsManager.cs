using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

/// <summary>
/// Manages player preferences and high scores in the game.
/// </summary>
public class PlayerPrefsManager : MonoBehaviour
{

    [SerializeField] private Stats playerStats; // Reference to the player's statistics
    [SerializeField] private LevelEditorSO levelEditor; // Reference to the player's statistics

    [SerializeField] private GameObject textPrefab; // Prefab for high score display
    private Dictionary<string, (int score, float time, string name)> highScores; // Dictionary to store high scores
    private string levelName;

    /// <summary>
    /// Singleton instance of PlayerPrefsManager.
    /// </summary>
    public static PlayerPrefsManager Instance;
    private string levelsFolderPath;
    private string[] levelFiles;

    /// <summary>
    /// Initializes the singleton instance and ensures it is not destroyed on scene load.
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Loads all high scores when the script starts.
    /// </summary>
    private void Start()
    {
        highScores = LoadAllHighScores();
    }

    /// <summary>
    /// Saves the high score if the current score is higher than the existing one.
    /// </summary>
    /// <param name="playerName">Name of the player.</param>
    /// <param name="time">Time taken to achieve the score.</param>
    public void SaveHighScore(string playerName, float time)
    {
        levelName = Path.GetFileNameWithoutExtension(levelEditor.SelectedLevelPath);

        string scoreKey = GetScoreKey(levelName);
        string timeKey = GetTimeKey(levelName);
        string nameKey = GetNameKey(levelName);

        // Save score if it's higher than the previous one
        if (playerStats.Score > PlayerPrefs.GetInt(scoreKey, 0))
        {
            PlayerPrefs.SetInt(scoreKey, playerStats.Score);
            PlayerPrefs.SetFloat(timeKey, time);
            PlayerPrefs.SetString(nameKey, playerName);
            PlayerPrefs.Save();
        }
        highScores = LoadAllHighScores();
    }

    /// <summary>
    /// Checks if the current score is the highest score recorded.
    /// </summary>
    /// <returns>True if current score is higher than saved high score.</returns>
    public bool IsCurrentScoreHighScore()
    {
        // Generate the keys for the high score associated with the given number of spawns
        string scoreKey = GetScoreKey(levelName);

        // Get the high score from PlayerPrefs for the specified number of spawns
        int highScore = PlayerPrefs.GetInt(scoreKey, 0); // Default to 0 if no score is found

        // Check if the current score is higher than the high score for this number of spawns
        return playerStats.Score > highScore;
    }

    /// <summary>
    /// Loads all saved high scores into a dictionary.
    /// </summary>
    /// <returns>A dictionary of high scores mapped by spawn count.</returns>
    public Dictionary<string, (int score, float time, string name)> LoadAllHighScores()
    {
        Dictionary<string, (int score, float time, string name)> highScores = new Dictionary<string, (int score, float time, string name)>();

        levelsFolderPath = Path.Combine(Application.dataPath, "Resources", "Levels");
        levelFiles = Directory.GetFiles(levelsFolderPath, "*.json");

        foreach (var filePath in levelFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);

            string scoreKey = GetScoreKey(fileName);
            string timeKey = GetTimeKey(fileName);
            string nameKey = GetNameKey(fileName);

            if (PlayerPrefs.HasKey(scoreKey))
            {
                int score = PlayerPrefs.GetInt(scoreKey, 0);
                float time = PlayerPrefs.GetFloat(timeKey, 0f);
                string name = PlayerPrefs.GetString(nameKey, "Unknown Player");

                highScores.Add(fileName, (score, time, name));
            }
        }

        return highScores;
    }

    /// <summary>
    /// Loads high scores into a scroll view for display.
    /// </summary>
    /// <param name="scrollViewContent">Parent object for high score entries.</param>
    public void LoadScoresIntoScrollView(GameObject scrollViewContent)
    {
        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        if (highScores.Count > 0)
        {
            foreach (var entry in highScores)
            {
                GameObject highScoreEntry = Instantiate(textPrefab, scrollViewContent.transform);
                TextMeshProUGUI entryText = highScoreEntry.GetComponent<TextMeshProUGUI>();
                entryText.text = $"{entry.Key} - Highscore: {entry.Value.score}  - {(int)entry.Value.time} Seconds - {entry.Value.name}";
            }
        }
        else
        {
            GameObject noScoreEntry = Instantiate(textPrefab, scrollViewContent.transform);
            TextMeshProUGUI entryText = noScoreEntry.GetComponent<TextMeshProUGUI>();
            entryText.text = "No high scores to load!";
        }
    }

    /// <summary>
    /// Clears all high scores from PlayerPrefs and the scroll view.
    /// </summary>
    /// <param name="scrollViewContent">Parent object for high score entries.</param>
    public void ClearAllHighScores(GameObject scrollViewContent)
    {
        foreach (var key in highScores.Keys)
        {
            string scoreKey = GetScoreKey(key);
            string timeKey = GetTimeKey(key);
            string nameKey = GetNameKey(key);

            PlayerPrefs.DeleteKey(scoreKey);
            PlayerPrefs.DeleteKey(timeKey);
            PlayerPrefs.DeleteKey(nameKey);
        }
        highScores.Clear();
        PlayerPrefs.Save();

        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }
        GameObject noScoreEntry = Instantiate(textPrefab, scrollViewContent.transform);
        TextMeshProUGUI entryText = noScoreEntry.GetComponent<TextMeshProUGUI>();
        entryText.text = "No high scores to load!";
    }

    /// <summary>
    /// Generates the key string for a high score based on spawn count.
    /// </summary>
    /// <param name="numberOfSpawns">Number of spawns to use for the key.</param>
    /// <returns>A formatted string key.</returns>
    private string GetScoreKey(string levelName)
    {
        return "HighScore_" + levelName;
    }

    /// <summary>
    /// Generates the key string for high score time based on spawn count.
    /// </summary>
    /// <param name="numberOfSpawns">Number of spawns to use for the key.</param>
    /// <returns>A formatted string key.</returns>
    private string GetTimeKey(string levelName)
    {
        return "HighScoreTime_" + levelName;
    }

    /// <summary>
    /// Generates the key string for high score name based on spawn count.
    /// </summary>
    /// <param name="numberOfSpawns">Number of spawns to use for the key.</param>
    /// <returns>A formatted string key.</returns>
    private string GetNameKey(string levelName)
    {
        return "HighScoreName_" + levelName;
    }
}
