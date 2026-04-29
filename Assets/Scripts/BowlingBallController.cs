using UnityEngine;

public class BowlingBallController : MonoBehaviour
{
    public timingbar timingBar;
    public float launchForce = 2f;

    [Header("Horizontal Movement")]
    public float moveSpeed = 5f;
    public float gutterLeftEdge = -11.1f;
    public float gutterRightEdge = 11.1f;
    public float sidePadding = 1.5f;

    [Header("Aiming")]
    public float currentAngle = 0f;
    public float maxAngle = 25f;
    public float angleSpeed = 60f;

    [Header("Controls")]
    public KeyCode toggleKey = KeyCode.Tab;

    private bool isAimingMode = false;

    public AudioSource audioSource;
    public AudioClip rollSound;
    public AudioClip dropSound;
    public AudioClip pinsStrike;

    private bool pinSoundPlayed = false;
    public int throwCOUNT;

    private Rigidbody rb;
    private bool hasLaunched = false;

    private bool missedRedZone = false;

    // ✅ NEW: true spawn position
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    // delay system
    private float resetDelayTimer = 0f;
    private bool waitingToReset = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // ✅ store ORIGINAL spawn once
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
    }

    void Update()
    {
        if (rb == null) return;

        // ✅ delayed reset
        if (waitingToReset)
        {
            resetDelayTimer -= Time.deltaTime;

            if (resetDelayTimer <= 0f)
            {
                ResetBall();

                if (timingBar != null)
                {
                    timingBar.ResetBar();
                }

                waitingToReset = false;
            }

            return;
        }

        if (Input.GetKeyDown(toggleKey))
        {
            isAimingMode = !isAimingMode;
            Debug.Log(isAimingMode ? "AIM MODE" : "MOVE MODE");
        }

        if (!hasLaunched)
        {
            if (!isAimingMode)
            {
                float moveInput = 0f;

                if (Input.GetKey(KeyCode.A))
                    moveInput = -1f;
                if (Input.GetKey(KeyCode.D))
                    moveInput = 1f;

                float leftLimit = gutterLeftEdge + sidePadding;
                float rightLimit = gutterRightEdge - sidePadding;

                Vector3 pos = transform.position;
                pos.x += moveInput * moveSpeed * Time.deltaTime;
                pos.x = Mathf.Clamp(pos.x, leftLimit, rightLimit);
                transform.position = pos;
            }
            else
            {
                float angleInput = 0f;

                if (Input.GetKey(KeyCode.A))
                    angleInput = -1f;
                if (Input.GetKey(KeyCode.D))
                    angleInput = 1f;

                currentAngle += angleInput * angleSpeed * Time.deltaTime;
                currentAngle = Mathf.Clamp(currentAngle, -maxAngle, maxAngle);

                transform.rotation = Quaternion.Euler(0f, currentAngle, 0f);
            }
        }

        if (hasLaunched) return;
        if (timingBar == null) return;

        if (timingBar.HasStopped() && !hasLaunched)
        {
            if (timingBar.IsValidHit())
            {
                throwCOUNT++;

                Vector3 launchDirection = Quaternion.Euler(0f, currentAngle, 0f) * Vector3.forward;
                rb.AddForce(launchDirection.normalized * launchForce, ForceMode.Impulse);

                hasLaunched = true;

                if (audioSource != null && dropSound != null)
                {
                    audioSource.PlayOneShot(dropSound);
                }

                if (audioSource != null && rollSound != null)
                {
                    audioSource.clip = rollSound;
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
            else
            {
                Debug.Log("Missed red zone. Resetting in 3 seconds...");

                throwCOUNT++;
                missedRedZone = true;

                // ✅ start delay
                resetDelayTimer = 3f;
                waitingToReset = true;

                hasLaunched = true;
            }
        }
    }

    public bool getLaunch()
    {
        return hasLaunched;
    }
    public Vector3 getStartPos()
    {
        return spawnPosition;
    }

    public int getThrowCount()
    {
        return throwCOUNT;
    }

    public void setThrowCount(int throwCNT)
    {
        throwCOUNT = throwCNT;
    }

    public void setLaunch(bool launched)
    {
        hasLaunched = launched;
    }

    public bool getMissedRedZone()
    {
        return missedRedZone;
    }

    public void clearMissedRedZone()
    {
        missedRedZone = false;
    }

    // ✅ UPDATED RESET (always goes to original spawn)
    public void ResetBall()
    {
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
        currentAngle = 0f;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        hasLaunched = false;
        pinSoundPlayed = false;

        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pin") && !pinSoundPlayed)
        {
            if (audioSource != null)
            {
                audioSource.Stop();

                if (pinsStrike != null)
                {
                    audioSource.PlayOneShot(pinsStrike);
                }
            }

            pinSoundPlayed = true;
        }
    }
}