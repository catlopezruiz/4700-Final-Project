using UnityEngine;

public class BowlingBallController : MonoBehaviour
{
    public timingbar timingBar;
    public float launchForce = 2f;
    public AudioSource audioSource;
    public AudioClip rollSound;
    public AudioClip dropSound;
    public AudioClip pinsStrike;

    private bool pinSoundPlayed = false;

    private Rigidbody rb;
    private bool hasLaunched = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (hasLaunched) return;
        if (timingBar == null) return;
        if (rb == null) return;

        if (timingBar.HasStopped())
        {
            if (timingBar.IsValidHit())
            {
                rb.AddForce(Vector3.forward * launchForce, ForceMode.Impulse);
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
    

    public void ResetBall(Vector3 startPosition)
    {
        transform.position = startPosition;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        hasLaunched = false;
    }
    public Vector3 getStartPos()
    {
        return transform.position;
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