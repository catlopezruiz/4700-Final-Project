using UnityEngine;

public class BowlingBallController : MonoBehaviour
{
    public timingbar timingBar;
    public float launchForce = 2f;

    [Header("Aiming")]
    public float currentAngle = 0f;
    public float maxAngle = 25f;
    public float angleSpeed = 60f;

    public AudioSource audioSource;
    public AudioClip rollSound;
    public AudioClip dropSound;
    public AudioClip pinsStrike;

    public Vector3 intialpos;
    private bool pinSoundPlayed = false;

    private Rigidbody rb;
    private bool hasLaunched = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        intialpos = transform.position;
    }

    void Update()
    {
        if (rb == null) return;

    
        if (!hasLaunched)
        {
            float input = 0f;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                input = -1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                input = 1f;

            currentAngle += input * angleSpeed * Time.deltaTime;
            currentAngle = Mathf.Clamp(currentAngle, -maxAngle, maxAngle);

          
            transform.rotation = Quaternion.Euler(0f, currentAngle, 0f);
        }

        if (hasLaunched) return;
        if (timingBar == null) return;

        if (timingBar.HasStopped())
        {
            if (timingBar.IsValidHit())
            {
                Vector3 launchDirection = Quaternion.Euler(0f, currentAngle, 0f) * Vector3.forward;
                rb.AddForce(launchDirection.normalized * launchForce, ForceMode.Impulse);

                hasLaunched = true;

                audioSource.PlayOneShot(dropSound);

                audioSource.clip = rollSound;
                audioSource.loop = true;
                audioSource.Play();
            }
            else
            {
                Debug.Log("Ball will not launch because the slider missed the red zone.");
                hasLaunched = true;
            }
        }
    }

    public bool getLaunch()
    {
        return hasLaunched;
    }

    public void setLaunch(bool launched)
    {
        hasLaunched = launched;
    }

    public void ResetBall(Vector3 startPosition)
    {
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
        currentAngle = 0f;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        hasLaunched = false;
        pinSoundPlayed = false;
    }

    public Vector3 getStartPos()
    {
        return intialpos;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pin") && !pinSoundPlayed)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(pinsStrike);
            pinSoundPlayed = true;
        }
    }
}