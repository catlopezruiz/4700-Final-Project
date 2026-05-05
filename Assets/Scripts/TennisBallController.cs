using UnityEngine;

public class TennisBallController : MonoBehaviour
{
    [Header("Court Targets")]
    public Transform[] playerTargets;
    public Transform[] aiTargets;
    public Transform[] playerBounceTargets;
    public Transform[] aiBounceTargets;

    [Header("Player")]
    public Transform playerTransform;
    public float playerReachDistance = 2f;

    [Header("Player Movement Script")]
    public PlayerTennisController playerMovementScript;

    [Header("Serve")]
    public Transform playerServePoint;
    public Transform playerServePosition;
    public Transform aiServePoint;
    public Transform aiServePosition;
    public float aiServeDelay = 1.0f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip bounceSound;
    public AudioClip hitSound;

    [Header("Serve Lanes")]
    public int playerServeTargetLane = 2;
    public int aiServeTargetLane = 2;

    private bool waitingForPlayerServe = true;
    private bool waitingForAIServe = false;
    private float aiServeTimer = 0f;
    private bool playerServesNext = true;

    [Header("AI Movement")]
    public Transform aiTransform;
    public float aiMoveSpeed = 6f;

    [Header("Racket Visuals")]
    public RacketSwing playerRacketSwing;
    public RacketSwing aiRacketSwing;

    [Header("Ball Speed")]
    public float baseSpeed = 9f;
    public float speedIncreasePerHit = 0.6f;
    public float ballSpeed;

    [Header("Speed Tuning")]
    public float travelToBounceSpeedMultiplier = 1.5f;
    public float bounceToHitSpeedMultiplier = 0.5f;

    [Header("References")]
    public TennisGameManager gameManager;
    public TimingBarUI timingBarUI;

    [Header("AI")]
    [Range(0f, 1f)]
    public float aiMissChance = 0.25f;

    public enum AIDifficulty { Easy, Medium, Hard }
    public AIDifficulty aiDifficulty = AIDifficulty.Medium;

    [Header("Timing")]
    public float easiestHitZoneWidth = 220f;
    public float hardestHitZoneWidth = 120f;
    public float maxSpeedForMinZone = 25f;

    public float minTimingLeadTime = 0.4f;
    public float maxTimingLeadTime = 0.9f;
    public float timingBarSpeedReference = 12f;

    public float baseMarkerSpeed = 350f;
    public float maxMarkerSpeed = 750f;

    [Header("Advanced Bounce Points")]
    public Transform aiBounceCenterToLeft;
    public Transform aiBounceCenterToRight;
    public Transform playerBounceCenterToLeft;
    public Transform playerBounceCenterToRight;

    [Header("Arc")]
    public float bounceArcHeight = 1.2f;
    public float hitArcHeight = 0.7f;

    [Header("Flow")]
    public float pauseDuration = 1.5f;

    private Transform currentTarget;

    private int currentPlayerLaneIndex;
    private int currentAILaneIndex;

    private bool travelingToBounce;
    private bool travelingToHitPoint;

    private bool isMoving = true;
    private bool isPaused = false;
    private float pauseTimer;

    private int rallyCount = 0;

    private Vector3 startPoint;
    private Vector3 endPoint;
    private float travelProgress;

    private bool playerHasAttemptedHit = false;
    private bool playerTimedHitSuccessfully = false;
    private float playerAimPosition = 0.5f;

    private bool timingBarShownThisShot = false;

    void Start()
    {
        ballSpeed = baseSpeed;
        playerServesNext = true;
        StartPlayerServe();
    }

    void Update()
{
    if (gameManager != null && gameManager.matchOver)
        return;

    if (isPaused)
    {
        pauseTimer -= Time.deltaTime;

        if (pauseTimer <= 0f)
        {
            isPaused = false;

            if (playerServesNext)
                StartPlayerServe();
            else
                StartAIServe();
        }

        return;
    }

    if (waitingForPlayerServe)
    {
        HandlePlayerServeInput();
        return;
    }

    if (waitingForAIServe)
    {
        HandleAIServe();
        return;
    }

    MoveAI();

    UpdateTimingBarDuringPlayerHitPhase();
    HandlePlayerInput();

    if (isMoving && currentTarget != null)
    {
        MoveBall();

        if (travelProgress >= 1f)
            HandleArrival();
    }
}

    void StartPlayerServe()
    {
        waitingForPlayerServe = true;
        waitingForAIServe = false;

        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        isMoving = false;
        rallyCount = 0;
        ballSpeed = baseSpeed;

        currentPlayerLaneIndex = 1;

        playerHasAttemptedHit = false;
        playerTimedHitSuccessfully = false;
        timingBarShownThisShot = false;

        if (playerServePosition != null)
            playerTransform.position = playerServePosition.position;

        if (playerServePoint != null)
            transform.position = playerServePoint.position;

        UpdateTimingBarDifficulty();
        timingBarUI.ShowBar();
    }

    void HandlePlayerServeInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerAimPosition = timingBarUI.GetMarkerNormalizedPositionInHitZone();
            playerTimedHitSuccessfully = timingBarUI.IsInHitZone();

            timingBarUI.HideBar();

            if (playerTimedHitSuccessfully)
        {

            if (audioSource != null && hitSound != null)
                {
                audioSource.PlayOneShot(hitSound);
                }
            if (playerRacketSwing != null)
                playerRacketSwing.Swing();

            if (playerMovementScript != null)
                playerMovementScript.enabled = true;

            waitingForPlayerServe = false;
            PlayerServeToAI();
        }
            else
            {
                UpdateTimingBarDifficulty();
                timingBarUI.ShowBar();
            }
        }
    }

 void PlayerServeToAI()
{
    rallyCount = 1;
    IncreaseSpeed();

    currentAILaneIndex = Mathf.Clamp(playerServeTargetLane, 0, aiTargets.Length - 1);

    travelingToBounce = true;
    travelingToHitPoint = false;

    Transform serveBounceTarget;

    if (currentAILaneIndex == 0)
    {
        serveBounceTarget = aiBounceCenterToLeft;
    }
    else if (currentAILaneIndex == 2)
    {
        serveBounceTarget = aiBounceCenterToRight;
    }
    else
    {
        serveBounceTarget = aiBounceTargets[1];
    }

    BeginTravel(serveBounceTarget);
}

    void StartAIServe()
{
    waitingForAIServe = true;
    waitingForPlayerServe = false;

    if (playerMovementScript != null)
        playerMovementScript.enabled = false;

    isMoving = false;
    rallyCount = 0;
    ballSpeed = baseSpeed;

    currentAILaneIndex = 1;

    if (aiServePosition != null && aiTransform != null)
        aiTransform.position = aiServePosition.position;

    if (playerServePosition != null && playerTransform != null)
        playerTransform.position = playerServePosition.position;

    if (aiServePoint != null)
        transform.position = aiServePoint.position;

    timingBarUI.HideBar();

    aiServeTimer = aiServeDelay;
}
    void HandleAIServe()
    {
        aiServeTimer -= Time.deltaTime;

        if (aiServeTimer <= 0f)
        {
            waitingForAIServe = false;
            AIServeToPlayer();
        }
    }

   void AIServeToPlayer()
{
    if (audioSource != null && hitSound != null)
    {
    audioSource.PlayOneShot(hitSound);
    }
    
    if (playerMovementScript != null)
        playerMovementScript.enabled = true;

    if (aiRacketSwing != null)
        aiRacketSwing.Swing();

    rallyCount = 1;
    IncreaseSpeed();

    currentPlayerLaneIndex = Mathf.Clamp(aiServeTargetLane, 0, playerTargets.Length - 1);

    playerHasAttemptedHit = false;
    timingBarShownThisShot = false;

    travelingToBounce = true;
    travelingToHitPoint = false;

    Transform serveBounceTarget;

    if (currentPlayerLaneIndex == 0)
    {
        serveBounceTarget = playerBounceCenterToLeft;
    }
    else if (currentPlayerLaneIndex == 2)
    {
        serveBounceTarget = playerBounceCenterToRight;
    }
    else
    {
        serveBounceTarget = playerBounceTargets[1];
    }

    BeginTravel(serveBounceTarget);
}
    void UpdateTimingBarDuringPlayerHitPhase()
{
    if (!travelingToHitPoint) return;
    if (currentTarget != playerTargets[currentPlayerLaneIndex]) return;
    if (timingBarShownThisShot) return;

    float distance = Vector3.Distance(transform.position, playerTargets[currentPlayerLaneIndex].position);
    float timeToArrive = distance / Mathf.Max(ballSpeed, 0.01f);

    float speedT = Mathf.InverseLerp(baseSpeed, timingBarSpeedReference, ballSpeed);
    float leadTime = Mathf.Lerp(minTimingLeadTime, maxTimingLeadTime, speedT);

    if (timeToArrive <= leadTime)
    {
        timingBarShownThisShot = true;
        playerHasAttemptedHit = false;

        UpdateTimingBarDifficulty();
        timingBarUI.ShowBar();
    }
}

bool IsPlayerSideBounceTarget(Transform target)
{
    if (target == null) return false;

    if (currentPlayerLaneIndex >= 0 && currentPlayerLaneIndex < playerBounceTargets.Length)
    {
        if (target == playerBounceTargets[currentPlayerLaneIndex])
            return true;
    }

    if (target == playerBounceCenterToLeft)
        return true;

    if (target == playerBounceCenterToRight)
        return true;

    return false;
}

    void HandlePlayerInput()
    {
        if (!timingBarShownThisShot || !travelingToHitPoint || playerHasAttemptedHit)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerHasAttemptedHit = true;

            playerAimPosition = timingBarUI.GetMarkerNormalizedPositionInHitZone();
            playerTimedHitSuccessfully = timingBarUI.IsInHitZone();

            timingBarUI.HideBar();
        }
    }

    void HandleArrival()
{
    if (travelingToBounce)
    {
        if (audioSource != null && bounceSound != null)
        {
            audioSource.PlayOneShot(bounceSound);
        }
        travelingToBounce = false;
        travelingToHitPoint = true;

        if (IsPlayerSideBounceTarget(currentTarget))
        {
            timingBarShownThisShot = false;
            BeginTravel(playerTargets[currentPlayerLaneIndex]);
        }
        else
        {
            BeginTravel(aiTargets[currentAILaneIndex]);
        }

        return;
    }

    if (travelingToHitPoint)
    {
        travelingToHitPoint = false;

        if (currentTarget == playerTargets[currentPlayerLaneIndex])
            ResolvePlayerHit();
        else
            ResolveAIHit();
    }
}

    void ResolveAIHit()
{
    if (Random.value < aiMissChance)
    {
        if (gameManager != null)
            gameManager.AIMissed();

        playerServesNext = true;
        StartPause();
    }
    else
    {

        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
        // Trigger AI racket swing
        if (aiRacketSwing != null)
            aiRacketSwing.Swing();

        rallyCount++;
        IncreaseSpeed();
        SendToPlayer();
    }
}

    public void ReturnFromPlayer()
    {
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (playerRacketSwing != null)
            playerRacketSwing.Swing();

        rallyCount++;
        IncreaseSpeed();

        int aimZone = GetAimZone();

        Transform bounceTarget;
        int hitLane;

        GetShotPattern(currentPlayerLaneIndex, aimZone, out bounceTarget, out hitLane);

        currentAILaneIndex = hitLane;

        travelingToBounce = true;
        travelingToHitPoint = false;

BeginTravel(bounceTarget);
    }

    int GetAimZone()
{
    if (playerAimPosition < 0.33f) return 0;
    if (playerAimPosition < 0.66f) return 1;
    return 2;
}

    void ResolvePlayerHit()
{
    if (IsPlayerInRange() && playerHasAttemptedHit && playerTimedHitSuccessfully)
    {
        ReturnFromPlayer();
    }
    else
    {
        if (gameManager != null)
            gameManager.PlayerMissed();

        playerServesNext = false;
        StartPause();
    }
}

public void SendToPlayer()
{
    int fromLane = currentAILaneIndex;
    int targetLane = Random.Range(0, 3);

    currentPlayerLaneIndex = targetLane;

    playerHasAttemptedHit = false;
    playerTimedHitSuccessfully = false;
    timingBarShownThisShot = false;

    travelingToBounce = true;
    travelingToHitPoint = false;

    Transform bounceTarget = playerBounceTargets[targetLane];

    // Cross-court from AI left to player right
    if (fromLane == 0 && targetLane == 2 && playerBounceCenterToRight != null)
    {
        bounceTarget = playerBounceCenterToRight;
    }
    // Cross-court from AI right to player left
    else if (fromLane == 2 && targetLane == 0 && playerBounceCenterToLeft != null)
    {
        bounceTarget = playerBounceCenterToLeft;
    }
    // Otherwise same-lane / normal shots use original bounce targets

    BeginTravel(bounceTarget);
}
   void GetShotPattern(int fromLane, int aimZone, out Transform bounceTarget, out int hitLane)
{
    // Default: straight lane shot
    bounceTarget = aiBounceTargets[aimZone];
    hitLane = aimZone;

    // Player hits from left, aims right
    if (fromLane == 0 && aimZone == 2)
    {
        bounceTarget = aiBounceCenterToRight;
        hitLane = 2;
    }

    // Player hits from right, aims left
    else if (fromLane == 2 && aimZone == 0)
    {
        bounceTarget = aiBounceCenterToLeft;
        hitLane = 0;
    }
}

    void MoveBall()
    {
        float distance = Vector3.Distance(startPoint, endPoint);

        float phaseSpeed = travelingToBounce ? travelToBounceSpeedMultiplier : bounceToHitSpeedMultiplier;

        travelProgress += ((ballSpeed * phaseSpeed) / distance) * Time.deltaTime;
        travelProgress = Mathf.Clamp01(travelProgress);

        Vector3 flat = Vector3.Lerp(startPoint, endPoint, travelProgress);
        float height = Mathf.Sin(travelProgress * Mathf.PI) *
                       (travelingToBounce ? bounceArcHeight : hitArcHeight);

        transform.position = new Vector3(flat.x, flat.y + height, flat.z);
    }

    void BeginTravel(Transform target)
    {
        currentTarget = target;
        startPoint = transform.position;
        endPoint = target.position;
        travelProgress = 0f;
        isMoving = true;
    }

    void MoveAI()
    {
        float targetX = aiTargets[currentAILaneIndex].position.x;
        Vector3 pos = aiTransform.position;

        pos.x = Mathf.MoveTowards(pos.x, targetX, aiMoveSpeed * Time.deltaTime);
        aiTransform.position = pos;
    }

    bool IsPlayerInRange()
    {
        float dist = Mathf.Abs(playerTransform.position.x -
                               playerTargets[currentPlayerLaneIndex].position.x);

        return dist <= playerReachDistance;
    }

    void IncreaseSpeed()
    {
        ballSpeed = baseSpeed + rallyCount * speedIncreasePerHit;
    }

    void UpdateTimingBarDifficulty()
    {
        float t = Mathf.InverseLerp(baseSpeed, maxSpeedForMinZone, ballSpeed);

        float width = Mathf.Lerp(easiestHitZoneWidth, hardestHitZoneWidth, t);
        width = Mathf.Max(width, 100f);

        timingBarUI.SetHitZoneWidth(width);

        float markerSpeed = Mathf.Lerp(baseMarkerSpeed, maxMarkerSpeed, t);
        timingBarUI.SetMarkerSpeed(markerSpeed);
    }

    void StartPause()
    {
        timingBarUI.HideBar();

        isPaused = true;
        pauseTimer = pauseDuration;
        isMoving = false;

        waitingForPlayerServe = false;
        waitingForAIServe = false;
    }

    public void ResetMatch()
    {
        isPaused = false;
        rallyCount = 0;
        ballSpeed = baseSpeed;

        playerServesNext = true;
        StartPlayerServe();
    }
}