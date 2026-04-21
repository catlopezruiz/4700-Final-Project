using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class pinStuff : MonoBehaviour
{
    public Rigidbody[] allpins;
    GameObject[] pins;
    public BowlingBallController ball;
    bool[] pinKnocked;
    float countdowntime = 3f;
    float timer = 0f;
    bool startcount = false;
    int score;
    void Start()
    {
         pins = GameObject.FindGameObjectsWithTag("Pin");
        allpins = new Rigidbody[pins.Length];
        for (int i = 0; i < pins.Length; i++)
        {
            allpins[i] = pins[i].GetComponent<Rigidbody>();
        }
        pinKnocked = new bool[pins.Length];

    }

    // Update is called once per frame
    void Update()
    {
        if (ball.getLaunch() == true)
        {
            for (int i = 0; i < allpins.Length; i++)
            {
                if (allpins[i].transform.up.y < 0.7f)
                {
                    if (pinKnocked[i] == true)
                        continue;
                    pinKnocked[i] = true; // to call out pins on the list that have been knocked
                    countdowntime = 3.0f;
                    startcount = true;
                   //need to implement a score counter that doesnt call +60 per fram score++
                   
                }
                else
                    pinKnocked[i] = false;

            }



            if (startcount)
            {

                countdowntime -= Time.deltaTime;
            }
            if (countdowntime <= 0.01f && ball.getLaunch() == true) { 
            {
                ball.setLaunch(false);
                endtimer();
            }
         
                //if pin is less than this angle
                //pin counts as knocked down
                //remove pin from next hit when ball resets
                //if pin is less than this angle set timer to countdown
                //if timer = 0 reset the ball back to its position and pins in place 
                //update score based knocked down pins 
            }
        }
    }
    void endtimer()
    {
        Debug.Log("called endtimer");
        Debug.Log(score);
        ball.ResetBall(ball.getStartPos());
        if (score != 10) //delete all pins that were knocked UNLess all of them were knocked then it resets (strike)
        {
            for (int i = 0; i < pinKnocked.Length; i++)
            {
                if (pinKnocked[i] == true)
                    pins[i].SetActive(false);


            }
        }
        else
            Debug.Log("Strike!!!!!!!!");
        


    }
}
