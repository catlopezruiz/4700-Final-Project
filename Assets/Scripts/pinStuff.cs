using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.ProBuilder.MeshOperations;

public class pinStuff : MonoBehaviour
{
    public Rigidbody[] allpins;
    GameObject[] pins;
    public BowlingBallController ball;
    bool[] pinKnocked;
    float countdowntime = 3f;
    float timer = 0f;
   
    public Scoretrack scoretrack;
    bool startcount = false;
    int score;
    Vector3[] intialposPINS;
    public timingbar timebar;
    int roundIndex = 0;
    void Start()
    {
         pins = GameObject.FindGameObjectsWithTag("Pin");
        allpins = new Rigidbody[pins.Length];
        intialposPINS = new Vector3[pins.Length];
        scoretrack = GetComponent<Scoretrack>();
        for (int i = 0; i < pins.Length; i++)
        {
            allpins[i] = pins[i].GetComponent<Rigidbody>();
            intialposPINS[i] = allpins[i].transform.position;
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
            if (countdowntime <= 0.01f && ball.getLaunch() == true) { //call this when pins stop knocking over bassically reseting the ball and timer info
            {
                startcount = false;
                countdowntime = 3.0f;
                ball.setLaunch(false);
                timebar.ResetBar();
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
        Debug.Log("called endtimer : \n");
      
        ball.ResetBall(ball.getStartPos());
        if (ball.getThrowCount() <= 2)
        {
            for (int i = 0; i < pinKnocked.Length; i++)
            {
                if (pinKnocked[i] == true)
                {
                    pins[i].SetActive(false);
                    score++;
                }
                //grab score and delete the ones that fell over for now
            }

            scoretrack.setScore(score, ball.getThrowCount());


            if (score == 10)
                {
                    if (ball.getThrowCount() == 1) Debug.Log("Strike!!!!!!!!");
                    if (ball.getThrowCount() == 2) Debug.Log("Spare!!!!!!!!");
       
                } //call out a strike or spare

            if (ball.getThrowCount() == 2 || score == 10)  //check if the score is 10 strike or spare or that the throw count is == 2 
            {
                Debug.Log("checking that the pins tried to reset\n");
                for (int e = 0; e < pins.Length; e++)
                {
                    pins[e].SetActive(true);
                    pins[e].transform.position = intialposPINS[e];
                    pins[e].transform.position = intialposPINS[e] + new Vector3(0, 0.1f, 0);
                    pins[e].transform.rotation = Quaternion.identity;
                    allpins[e].linearVelocity = Vector3.zero;
                    allpins[e].angularVelocity = Vector3.zero;//set all the pins active 
                }
                ball.setThrowCount(0);
                score = 0;
            }
            else  //else if we have another throw reset the pins to positions that are not knocked over 
            {
                for (int j = 0; j < pinKnocked.Length; j++)
                {
                    if (pinKnocked[j] == false)
                    {
                        pins[j].transform.position = intialposPINS[j] + new Vector3(0, 0.1f, 0);
                        pins[j].transform.rotation = Quaternion.identity;
                        allpins[j].linearVelocity = Vector3.zero;      
                        allpins[j].angularVelocity = Vector3.zero;
                    }
                } 
            }


        } //end huge IF (ball.getThrowCount() <= 2)

        scoretrack.setRoundIndex(roundIndex);
        roundIndex++;

    }//end timer function 
    }

