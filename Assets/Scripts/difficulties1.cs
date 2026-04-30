using UnityEngine;

public class difficulties1 : MonoBehaviour
{
    public static float speed = 400f;

    public void Easy() { speed = 250f; }
    public void Medium() { speed = 400f; }
    public void Hard() { speed = 550f; }
}