using UnityEngine;

public class ballrestFloor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public BowlingBallController ball;
    bool balltouched = false;
    float countdowntime = 5f;
    public timingbar timebar;
    public pinStuff pin;
    public int pincheck;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        if (balltouched && ball.getLaunch() == true && pin.startcount == false) //if no pins were touched the score will not update and we can go
        {
            countdowntime -= Time.deltaTime;
        }

        if (countdowntime <= 0.01f)
        {
            ball.setLaunch(false);
            timebar.ResetBar();
            ball.ResetBall(ball.getStartPos());
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
