using System.Threading;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class pinStuff : MonoBehaviour
{
    public Rigidbody[] allpins;
    public BowlingBallController ball;
    bool[] pinKnocked = new bool[9];
    float countdowntime = 3f;
    float timer = 0f;
    bool startcount = false;
    void Start()
    {
        GameObject[] pins = GameObject.FindGameObjectsWithTag("Pin");
        allpins = new Rigidbody[pins.Length];
        for (int i = 0; i < pins.Length; i++)
        {
            allpins[i] = pins[i].GetComponent<Rigidbody>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ball.getLaunch() == true)
        {
            for (int i = 0; i < allpins.Length; i++)
            {
                if (allpins[i].transform.up.y < 0.7f )
                {
                    if  (pinKnocked[i] == true)
                        continue;
                    pinKnocked[i] = true; // to call out pins on the list that have been knocked
                    countdowntime = 3.0f;
                    startcount = true;
                }
                else
                    pinKnocked[i] = false;

            }



        if (startcount)
                countdowntime -= Time.deltaTime;
            if (countdowntime == 0)
                ball.ResetBall(ball.getStartPos());
            //if pin is less than this angle
            //pin counts as knocked down
            //remove pin from next hit when ball resets
            //if pin is less than this angle set timer to countdown
            //if timer = 0 reset the ball back to its position and pins in place 
            //update score based knocked down pins 
        }
    }
}
