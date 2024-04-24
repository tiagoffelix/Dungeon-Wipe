using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu3D : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuCanvas;

    [SerializeField] private GameObject playerCanvas;

    private bool isPaused;
    private float elapsedTime;

    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private TextMeshProUGUI gameOverText;

    [SerializeField] private GameObject inputObject;
    [SerializeField] private GameObject saveGameObject;

    [SerializeField] private PlayerMovement player;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image timerImage;

    [SerializeField] private AudioSource buttonSound;

    [SerializeField] private AudioSource endingSound;

    [SerializeField] private TMP_InputField inputField;

    private float totalDuration;
    private bool end;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
        elapsedTime = 0f;
        totalDuration = player.PlayerStats.LevelMinutes * 60;
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
                    Time.timeScale = 0f;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    timerImage.gameObject.SetActive(false);
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
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

    public void TogglePauseMenu()
    {
        //buttonSound.Play();
        isPaused = !isPaused;
        pauseMenuCanvas.SetActive(isPaused);
    }

    public void SaveCurrentGame()
    {
        if (!PlayerPrefsManager.Instance)
        {
            Debug.LogError("PlayerPrefsManager not found on the scene!");
            return;
        }
        PlayerPrefsManager.Instance.SaveHighScore(inputField.text, elapsedTime);
    }

    public void RestartGame()
    {
        isPaused = false;
        SceneManager.LoadScene("Game");
    }

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
