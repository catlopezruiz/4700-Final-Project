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

    public Scoretrack scoretrack;
    public bool startcount = false;
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

                    pinKnocked[i] = true;
                    countdowntime = 3.0f;
                    startcount = true;
                }
                else
                {
                    pinKnocked[i] = false;
                }
            }

            if (startcount)
            {
                countdowntime -= Time.deltaTime;
            }

            if (countdowntime <= 0.01f && ball.getLaunch() == true)
            {
                startcount = false;
                countdowntime = 3.0f;
                ball.setLaunch(false);
                timebar.ResetBar();
                endtimer();
            }
        }
    }

    void endtimer()
    {
        Debug.Log("called endtimer : \n");

        ball.ResetBall();

        if (ball.getThrowCount() <= 2)
        {
            int pinsKnockedThisThrow = 0;

            for (int i = 0; i < pinKnocked.Length; i++)
            {
                if (pins[i].activeSelf && pinKnocked[i] == true)
                {
                    pins[i].SetActive(false);
                    score++;
                    pinsKnockedThisThrow++;
                    pinKnocked[i] = false;
                }
            }

            if (pinsKnockedThisThrow == 0)
            {
                Debug.Log("No pins hit this throw");
            }

            scoretrack.setScore(score, ball.getThrowCount());

            if (score == 10)
            {
                if (ball.getThrowCount() == 1) Debug.Log("Strike!!!!!!!!");
                if (ball.getThrowCount() == 2) Debug.Log("Spare!!!!!!!!");
            }

            if (ball.getThrowCount() == 2 || score == 10)
            {
                Debug.Log("checking that the pins tried to reset\n");

                for (int e = 0; e < pins.Length; e++)
                {
                    pins[e].SetActive(true);
                    pins[e].transform.position = intialposPINS[e] + new Vector3(0, 0.1f, 0);
                    pins[e].transform.rotation = Quaternion.identity;

                    allpins[e].linearVelocity = Vector3.zero;
                    allpins[e].angularVelocity = Vector3.zero;
                }

                ball.setThrowCount(0);
                score = 0;
            }
            else
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
        }

        scoretrack.setRoundIndex(roundIndex);
        roundIndex++;
    }

    public int getscore()
    {
        return score;
    }
}