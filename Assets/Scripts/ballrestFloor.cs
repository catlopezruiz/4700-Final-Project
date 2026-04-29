using UnityEngine;

public class ballrestFloor : MonoBehaviour
{
    public BowlingBallController ball;
    bool balltouched = false;
    float countdowntime = 5f;
    public timingbar timebar;
    public pinStuff pin;
    public int pincheck;

    void Start()
    {
       
    }

    void Update()
    {
        if (balltouched && ball.getLaunch() == true && pin.startcount == false)
        {
            countdowntime -= Time.deltaTime;
        }

        if (countdowntime <= 0.01f)
        {
            ball.setLaunch(false);
            timebar.ResetBar();
            ball.ResetBall();

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