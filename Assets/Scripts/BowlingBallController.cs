using UnityEngine;

public class BowlingBallController : MonoBehaviour
{
    public float forwardForce = 800f;
    public float sideForce = 200f;

    private Rigidbody rb;
    private bool hasLaunched = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!hasLaunched)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.forward * forwardForce);
                hasLaunched = true;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += Vector3.left * 2f * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += Vector3.right * 2f * Time.deltaTime;
            }
        }
    }
}