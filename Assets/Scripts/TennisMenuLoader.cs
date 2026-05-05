using UnityEngine;
using UnityEngine.SceneManagement;

public class TennisMenuLoader : MonoBehaviour
{
    public string tennisSceneName = "tennis";

    public void StartTennisEasy()
    {
        TennisGameSettings.selectedDifficulty = TennisGameSettings.Difficulty.Easy;
        SceneManager.LoadScene(tennisSceneName);
    }

    public void StartTennisMedium()
    {
        TennisGameSettings.selectedDifficulty = TennisGameSettings.Difficulty.Medium;
        SceneManager.LoadScene(tennisSceneName);
    }

    public void StartTennisHard()
    {
        TennisGameSettings.selectedDifficulty = TennisGameSettings.Difficulty.Hard;
        SceneManager.LoadScene(tennisSceneName);
    }
}