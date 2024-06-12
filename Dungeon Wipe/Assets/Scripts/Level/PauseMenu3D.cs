using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages the pause menu and game over screen in a 3D game.
/// </summary>
public class PauseMenu3D : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuCanvas; // The canvas containing the pause menu UI.

    [SerializeField] private GameObject playerCanvas; // The canvas containing the player UI.

    private bool isPaused; // Indicates whether the game is currently paused.
    private float elapsedTime; // The elapsed time since the start of the game.

    [SerializeField] private GameObject gameOverCanvas; // The canvas containing the game over UI.
    [SerializeField] private TextMeshProUGUI gameOverText; // The text displaying game over information.

    [SerializeField] private GameObject inputObject; // The input field object for saving high scores.
    [SerializeField] private GameObject saveGameObject; // The save button object for saving high scores.

    [SerializeField] private PlayerMovement player; // Reference to the PlayerMovement script.

    [SerializeField] private TextMeshProUGUI scoreText; // The text displaying the player's score.
    [SerializeField] private Image timerImage; // The image displaying the timer bar.

    [SerializeField] private AudioSource buttonSound; // Sound played when buttons are clicked.
    [SerializeField] private AudioSource endingSound; // Sound played when the game ends.

    [SerializeField] private TMP_InputField inputField; // The input field for entering player name.

    [SerializeField] private LevelEditorSO levelEditor; // Reference to the LevelEditorSO scriptable object.

    private float totalDuration; // The total duration of the game.
    private bool end; // Indicates whether the game has ended.

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
        elapsedTime = 0f;
        totalDuration = levelEditor.LevelMinutes * 60;
        end = false;
    }

    void Update()
    {
        if (!end)
        {
            scoreText.text = "Score: " + player.PlayerStats.Score;

            if (elapsedTime >= totalDuration || player.PlayerStats.Dead)
            {
                end = true;
                gameOverText.text = "Score: " + player.PlayerStats.Score + "\nSpawns: " + player.PlayerStats.NumberOfSpawns;
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                playerCanvas.SetActive(false);
                pauseMenuCanvas.SetActive(false);
                timerImage.enabled = false;
                gameOverCanvas.SetActive(true);
                if (!PlayerPrefsManager.Instance)
                {
                    Debug.LogError("PlayerPrefsManager not found on the scene!");
                    return;
                }
                if (!PlayerPrefsManager.Instance.IsCurrentScoreHighScore())
                {
                    inputObject.SetActive(false);
                    saveGameObject.SetActive(false);
                }
            }
            else
            {
                if (isPaused)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    Time.timeScale = 0f;
                    timerImage.gameObject.SetActive(false);
                }
                else
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    Time.timeScale = 1f;
                    elapsedTime += Time.deltaTime;
                    float fraction = (totalDuration - elapsedTime) / totalDuration;

                    timerImage.fillAmount = fraction;

                    timerImage.gameObject.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.Escape) && elapsedTime < totalDuration)
                {
                    TogglePauseMenu();
                }
            }
        }
    }

    /// <summary>
    /// Toggles the visibility of the pause menu.
    /// </summary>
    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        pauseMenuCanvas.SetActive(isPaused);
        buttonSound.Play();
    }

    /// <summary>
    /// Saves the current game's high score.
    /// </summary>
    public void SaveCurrentGame()
    {
        if (!PlayerPrefsManager.Instance)
        {
            Debug.LogError("PlayerPrefsManager not found on the scene!");
            return;
        }
        PlayerPrefsManager.Instance.SaveHighScore(inputField.text, elapsedTime);
    }

    /// <summary>
    /// Restarts the game.
    /// </summary>
    public void RestartGame()
    {
        isPaused = false;
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// Returns to the main menu.
    /// </summary>
    public void MainMenu()
    {
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
