using UnityEngine;

public class TennisDifficultyManager : MonoBehaviour
{
    public TennisBallController ballController;

    void Start()
    {
        ApplyDifficulty();
    }

    public void ApplyDifficulty()
    {
        if (ballController == null) return;

        switch (TennisGameSettings.selectedDifficulty)
        {
            case TennisGameSettings.Difficulty.Easy:
                ballController.baseSpeed = 7f;
                ballController.speedIncreasePerHit = 0.45f;
                ballController.aiMissChance = 0.40f;
                ballController.easiestHitZoneWidth = 240f;
                ballController.hardestHitZoneWidth = 150f;
                ballController.maxSpeedForMinZone = 28f;
                ballController.aiDifficulty = TennisBallController.AIDifficulty.Easy;
                break;

            case TennisGameSettings.Difficulty.Medium:
                ballController.baseSpeed = 9f;
                ballController.speedIncreasePerHit = 0.6f;
                ballController.aiMissChance = 0.25f;
                ballController.easiestHitZoneWidth = 220f;
                ballController.hardestHitZoneWidth = 120f;
                ballController.maxSpeedForMinZone = 25f;
                ballController.aiDifficulty = TennisBallController.AIDifficulty.Medium;
                break;

            case TennisGameSettings.Difficulty.Hard:
                ballController.baseSpeed = 11f;
                ballController.speedIncreasePerHit = 0.8f;
                ballController.aiMissChance = 0.10f;
                ballController.easiestHitZoneWidth = 190f;
                ballController.hardestHitZoneWidth = 90f;
                ballController.maxSpeedForMinZone = 22f;
                ballController.aiDifficulty = TennisBallController.AIDifficulty.Hard;
                break;
        }

        ballController.ballSpeed = ballController.baseSpeed;
    }
}