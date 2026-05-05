using UnityEngine;

public class PlayerTennisController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float moveLimit = 8f; // how far left/right player can go

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal"); // A/D or Arrow Keys

        Vector3 movement = new Vector3(-moveInput * moveSpeed * Time.deltaTime, 0f, 0f);
        transform.Translate(movement);

        // Clamp position so player stays on their side
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -moveLimit, moveLimit);
        transform.position = pos;
    }
} 
