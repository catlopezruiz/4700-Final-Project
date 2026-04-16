using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class timingbar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public RectTransform pointer;
    public RectTransform start;
    public RectTransform end;
    public RectTransform perfectHit1;
    public RectTransform perfectHit2;
    bool moving = true;
    GameObject BowlingBall;
    
    bool isRunning = false;
    public int speed;
    float pointerlocation = 0f;
    float perfecthittop = 0f;
    float perfecthitbot = 0f;
    float perfectY = 0f;
    void Start()
    {


        pointer.anchoredPosition = start.anchoredPosition;


    }


    // Update is called once per frame
    void Update()
    {
        perfecthittop = perfectHit2.anchoredPosition.y;
        perfecthitbot = perfectHit1.anchoredPosition.y;
        perfectY = (perfecthitbot + perfecthittop) / 2f;




        if (!isRunning)         //space bar to start the timing bar temporarly 
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isRunning = true; // run based on if this is running 
            }
        }
        if (!isRunning) return;

        Vector2 pointerPosition = pointer.anchoredPosition;  //Pos of pointer
        if (isRunning)
        {
           
             if (moving)
            pointer.anchoredPosition += Vector2.up * speed * Time.deltaTime;  //move pointer up
            if (pointer.anchoredPosition.y > end.anchoredPosition.y)
                moving = false;  //if it reaches the end stop 

            if (Input.GetKeyDown(KeyCode.Space) && pointer.anchoredPosition.y > perfectHit1.anchoredPosition.y && pointer.anchoredPosition.y < perfectHit2.anchoredPosition.y)  //checking if its bewteen the 2 red spaces
            {
                pointerlocation = pointer.anchoredPosition.y;

                if (pointerlocation < perfectY * 1.1 && pointerlocation < perfectY * 0.9)  //checking wether its in the middle of the 2 red spaces by a small margin 
                    Debug.Log("SUPER ");

                Debug.Log("perfectHit!!!\n");  //this calls if the spacebars hits when the pointer is between. 
                moving = false;
            }

            else if (Input.GetKeyDown(KeyCode.Space) && pointer.anchoredPosition.y < perfectHit1.anchoredPosition.y && pointer.anchoredPosition.y > perfectHit2.anchoredPosition.y)
            {
                Debug.Log("you missed!!!!\n");     
                moving = false;
            }
             //stop moving on hit

        }

      

    }
}
