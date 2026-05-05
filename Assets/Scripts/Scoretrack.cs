
using UnityEngine;
using UnityEngine.UI; // For Unity UI Text
using TMPro; // For TextMeshPro support


public class Scoretrack : MonoBehaviour
{
    scoreInfo[] scores;
    public int frames = 10;
    int roundIndex = 0;

    // UI reference (assign in Inspector)
    public Text scoreText; // For Unity UI
    public TMP_Text scoreTMPText; // For TextMeshPro
    public TMP_Text[] roundScoreTexts; // Assign 10 elements in Inspector, one for each round

    void Awake()
    {
        scores = new scoreInfo[10];
        for (int i = 0; i < scores.Length; i++)
        {
            scores[i] = new scoreInfo();
        }
    }
    public void setRoundIndex(int roundIndex)
    {
        this.roundIndex = roundIndex;
    }
    //call function with first and second score per round
    //update final score
    //if second shot = 0 and first shot = 10 update score to strike as final
    //else if final = 10 set update board to spare.
    //update scoreboard 
    public void setScore(int points, int throws)
    {
        if (scores == null || roundIndex >= scores.Length)
        {
            Debug.LogError("Scoreboard is full");
            return;
        }

        if (throws == 1)
            scores[roundIndex].firstThrow = points;
        else
            scores[roundIndex].SecondThrow = points;

        if (scores[roundIndex].firstThrow == 10)
            scores[roundIndex].SecondThrow = 0;

        if (throws == 2 || scores[roundIndex].firstThrow == 10)
        {
            UpdateScoreUI();
            roundIndex++;
        }
    }

    // Call this to update the UI text with all scores
    void UpdateScoreUI()
    {
        string scoreDisplay = "";
        int cumulative = 0;

        for (int i = 0; i < scores.Length; i++)
        {
            int roundTotalLoop = scores[i].firstThrow + scores[i].SecondThrow;
            cumulative += roundTotalLoop;
            scoreDisplay += "Round " + (i + 1) + ": " + scores[i].firstThrow + " | " + scores[i].SecondThrow + "\n";
        }

        int roundTotal = scores[roundIndex].firstThrow + scores[roundIndex].SecondThrow;

        if (roundScoreTexts != null && roundIndex >= 0 && roundIndex < roundScoreTexts.Length && roundScoreTexts[roundIndex] != null)
        {
            roundScoreTexts[roundIndex].text = "R" + (roundIndex + 1) + ": " + roundTotal;
        }

        if (scoreText != null)
            scoreText.text = scoreDisplay;

        if (scoreTMPText != null)
            scoreTMPText.text = scoreDisplay;
    }

}
