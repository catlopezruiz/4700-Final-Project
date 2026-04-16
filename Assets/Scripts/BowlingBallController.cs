using UnityEngine;

public class BowlingBallController : MonoBehaviour
{
    public timingbar timingBar;
    public float launchForce = 2f;

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
            }
            else
            {
                Debug.Log("Ball will not launch because the slider missed the red zone.");
                hasLaunched = true;
            }
        }
    }

    public void ResetBall(Vector3 startPosition)
    {
        transform.position = startPosition;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        hasLaunched = false;
    }
}