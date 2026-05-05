using UnityEngine;
using UnityEngine.SceneManagement;

public class TennisPauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseMenuPanel;

    [Header("References")]
    public TennisGameManager gameManager;

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;

        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;

        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    public void RestartMatch()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }

        if (gameManager != null)
        {
            gameManager.RestartMatch();
        }
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        // Change this to your real menu scene name
        SceneManager.LoadScene("StartMenu");
    }
}