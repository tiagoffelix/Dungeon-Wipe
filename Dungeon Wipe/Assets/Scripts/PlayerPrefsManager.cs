using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerPrefsManager : MonoBehaviour
{
    public static PlayerPrefsManager Instance;

    [SerializeField] private Stats playerStats;

    [SerializeField] private GameObject textPrefab;
    private Dictionary<int, (int score, float time, string name)> highScores;

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

    private void Start()
    {
        highScores = LoadAllHighScores();
    }

    // Updated method to include time and player name parameters
    public void SaveHighScore(string playerName, float time)
    {
        string scoreKey = GetScoreKey(playerStats.NumberOfSpawns);
        string timeKey = GetTimeKey(playerStats.NumberOfSpawns);
        string nameKey = GetNameKey(playerStats.NumberOfSpawns);

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

    public bool IsCurrentScoreHighScore()
    {
        // Generate the keys for the high score associated with the given number of spawns
        string scoreKey = GetScoreKey(playerStats.NumberOfSpawns);

        // Get the high score from PlayerPrefs for the specified number of spawns
        int highScore = PlayerPrefs.GetInt(scoreKey, 0); // Default to 0 if no score is found

        // Check if the current score is higher than the high score for this number of spawns
        return playerStats.Score > highScore;
    }

    public Dictionary<int, (int score, float time, string name)> LoadAllHighScores()
    {
        Dictionary<int, (int score, float time, string name)> highScores = new Dictionary<int, (int score, float time, string name)>();

        // Assume there is a fixed maximum number of high scores we want to check, for example, 1000.
        int maxHighScores = 1000;
        for (int i = 0; i < maxHighScores; i++)
        {
            string scoreKey = GetScoreKey(i);
            string timeKey = GetTimeKey(i);
            string nameKey = GetNameKey(i);

            if (PlayerPrefs.HasKey(scoreKey))
            {
                int score = PlayerPrefs.GetInt(scoreKey, 0);
                float time = PlayerPrefs.GetFloat(timeKey, 0f);
                string name = PlayerPrefs.GetString(nameKey, "Unknown Player");

                highScores.Add(i, (score, time, name));
            }
        }

        return highScores;
    }

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
                entryText.text = $"Highscore: {entry.Value.score} - {entry.Key} Spawns - {(int)entry.Value.time} Seconds - {entry.Value.name}";
            }
        }
        else
        {
            GameObject noScoreEntry = Instantiate(textPrefab, scrollViewContent.transform);
            TextMeshProUGUI entryText = noScoreEntry.GetComponent<TextMeshProUGUI>();
            entryText.text = "No high scores to load!";
        }
    }

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

    public void CreateSampleHighScores()
    {
        // Sample data to create.
        string[] names = { "Mario", "Jaime", "Xico", "Edu", "Manu", "Speazyy"};
        int[] scores = { 21350, 35420, 2, 23450, 1240, 223430};
        float[] times = { 120.5f, 115.0f, 110.5f, 105.0f, 100.5f, 95.0f, 90.5f, 85.0f, 80.0f, 75.5f };

        // Save sample high scores.
        for (int i = 0; i < 6; i++)
        {
            string scoreKey = GetScoreKey(i);
            string timeKey = GetTimeKey(i);
            string nameKey = GetNameKey(i);

            PlayerPrefs.SetInt(scoreKey, scores[i]);
            PlayerPrefs.SetFloat(timeKey, times[i]);
            PlayerPrefs.SetString(nameKey, names[i]);
        }

        // Ensure all changes are saved to disk.
        PlayerPrefs.Save();

        // Reload scores to reflect new data.
        highScores = LoadAllHighScores();
    }

    private string GetScoreKey(int numberOfSpawns)
    {
        return "HighScore_" + numberOfSpawns;
    }

    private string GetTimeKey(int numberOfSpawns)
    {
        return "HighScoreTime_" + numberOfSpawns;
    }

    private string GetNameKey(int numberOfSpawns)
    {
        return "HighScoreName_" + numberOfSpawns;
    }
}
