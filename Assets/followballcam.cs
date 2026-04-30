using UnityEngine;
public class followballcam : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 2f, 0f); 

    void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + offset;

        // only rotate with Y (left/right aim)
        transform.rotation = Quaternion.Euler(0f, target.eulerAngles.y, 0f);
    }
}