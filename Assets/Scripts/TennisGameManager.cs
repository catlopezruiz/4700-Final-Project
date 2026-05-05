using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TennisGameManager : MonoBehaviour
{
    [Header("Score")]
    public int playerOuts = 0;
    public int aiOuts = 0;
    public int maxOuts = 4;

    [Header("UI")]
    public TextMeshProUGUI playerOutsText;
    public TextMeshProUGUI aiOutsText;
    public TextMeshProUGUI resultText;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverResultText;

    [Header("References")]
    public TennisBallController ballController;
    public Transform playerTransform;
    public Transform aiTransform;

    [Header("Scenes")]
    public string mainMenuSceneName = "MainMenu";

    private Vector3 playerStartPosition;
    private Vector3 aiStartPosition;

    public bool matchOver = false;

    void Start()
    {
        if (playerTransform != null)
            playerStartPosition = playerTransform.position;

        if (aiTransform != null)
            aiStartPosition = aiTransform.position;

        matchOver = false;
        playerOuts = 0;
        aiOuts = 0;

        UpdateUI();

        if (resultText != null)
            resultText.text = "";

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void PlayerMissed()
    {
        if (matchOver) return;

        playerOuts++;
        UpdateUI();

        if (playerOuts >= maxOuts)
        {
            EndMatch("AI Wins!");
        }
    }

    public void AIMissed()
    {
        if (matchOver) return;

        aiOuts++;
        UpdateUI();

        if (aiOuts >= maxOuts)
        {
            EndMatch("Player Wins!");
        }
    }

    void EndMatch(string message)
    {
        matchOver = true;

        if (resultText != null)
            resultText.text = message;

        if (gameOverResultText != null)
            gameOverResultText.text = message;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    void UpdateUI()
    {
        if (playerOutsText != null)
            playerOutsText.text = "Player Outs: " + playerOuts + "/" + maxOuts;

        if (aiOutsText != null)
            aiOutsText.text = "AI Outs: " + aiOuts + "/" + maxOuts;
    }

    public void RestartMatch()
    {
        matchOver = false;
        playerOuts = 0;
        aiOuts = 0;

        UpdateUI();

        if (resultText != null)
            resultText.text = "";

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (playerTransform != null)
            playerTransform.position = playerStartPosition;

        if (aiTransform != null)
            aiTransform.position = aiStartPosition;

        if (ballController != null)
            ballController.ResetMatch();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}