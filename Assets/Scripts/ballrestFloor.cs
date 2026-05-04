using UnityEngine;

public class ballrestFloor : MonoBehaviour
{
    public BowlingBallController ball;
    bool balltouched = false;
    float countdowntime = 2f;
    public timingbar timebar;
    public pinStuff pin;
    public int pincheck;

    void Start()
    {
       
    }

    void Update()
    {
        if (countdowntime <= 0.01f && balltouched && ball.getLaunch() && pin.startcount == false)
        {
            ball.setLaunch(false);
            timebar.ResetBar();

            pin.endtimer();

            balltouched = false;
            countdowntime = 5f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log("ball touched");
            balltouched = true;
        }
    }
}