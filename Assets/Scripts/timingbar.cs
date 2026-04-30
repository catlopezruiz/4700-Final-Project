using UnityEngine;

public class timingbar : MonoBehaviour
{
    public RectTransform pointer;
    public RectTransform start;
    public RectTransform end;
    public RectTransform perfectHit1;
    public RectTransform perfectHit2;

    public float speed = 250f;

    private bool movingUp = true;
    private bool hasStopped = false;
    private bool validHit = false;

    private float perfectTop;
    private float perfectBottom;

    void Start()
    {
        speed = difficulties1.speed;
        pointer.anchoredPosition = start.anchoredPosition;
    }

    void Update()
    {
        if (pointer == null || start == null || end == null || perfectHit1 == null || perfectHit2 == null)
            return;

        perfectTop = Mathf.Max(perfectHit1.anchoredPosition.y, perfectHit2.anchoredPosition.y);
        perfectBottom = Mathf.Min(perfectHit1.anchoredPosition.y, perfectHit2.anchoredPosition.y);

        if (hasStopped) return;

        // Move the pointer up and down automatically
        Vector2 pos = pointer.anchoredPosition;

        if (movingUp)
        {
            pos.y += speed * Time.deltaTime;

            if (pos.y >= end.anchoredPosition.y)
            {
                pos.y = end.anchoredPosition.y;
                movingUp = false;
            }
        }
        else
        {
            pos.y -= speed * Time.deltaTime;

            if (pos.y <= start.anchoredPosition.y)
            {
                pos.y = start.anchoredPosition.y;
                movingUp = true;
            }
        }

        pointer.anchoredPosition = pos;

        // Stop on Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hasStopped = true;

            float pointerY = pointer.anchoredPosition.y;

            if (pointerY >= perfectBottom && pointerY <= perfectTop)
            {
                validHit = true;
                Debug.Log("Good hit! Ball can launch.");
            }
            else
            {
                validHit = false;
                Debug.Log("Missed the red zone.");
            }
        }
    }

    public bool HasStopped()
    {
        return hasStopped;
    }

    public bool IsValidHit()
    {
        return validHit;
    }

    public void ResetBar()
    {
        pointer.anchoredPosition = start.anchoredPosition;
        movingUp = true;
        hasStopped = false;
        validHit = false;
    }

    public void DisableBar()
    {
        hasStopped = true; 
        this.enabled = false;

        
    }
}