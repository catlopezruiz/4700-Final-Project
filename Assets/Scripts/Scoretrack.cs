using UnityEditor.Build.Player;
using UnityEngine;

public class Scoretrack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    scoreInfo[] scores;
    public int frames = 10;
    int roundIndex = 0;

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
            Debug.LogError("Scoreboard is full or not initialized!");
            return;
        }
        if (throws == 1)
            scores[roundIndex].firstThrow = points;
        else
            scores[roundIndex].SecondThrow = points;

        if (scores[roundIndex].firstThrow == 10)
            scores[roundIndex].SecondThrow = 0; //give second a dummy value if there is a strike 

    }

}
