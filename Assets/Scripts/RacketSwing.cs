using UnityEngine;

public class RacketSwing : MonoBehaviour
{
    public Vector3 swingRotation = new Vector3(0f, 0f, -60f);
    public float swingDuration = 0.15f;

    private Quaternion startRotation;
    private Quaternion targetRotation;
    private float timer = 0f;
    private bool swinging = false;

    void Start()
    {
        startRotation = transform.localRotation;
        targetRotation = startRotation * Quaternion.Euler(swingRotation);
    }

    void Update()
    {
        if (!swinging) return;

        timer += Time.deltaTime / swingDuration;

        if (timer < 0.5f)
        {
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, timer * 2f);
        }
        else if (timer < 1f)
        {
            transform.localRotation = Quaternion.Slerp(targetRotation, startRotation, (timer - 0.5f) * 2f);
        }
        else
        {
            transform.localRotation = startRotation;
            swinging = false;
        }
    }

    public void Swing()
    {
        timer = 0f;
        swinging = true;
    }
}
