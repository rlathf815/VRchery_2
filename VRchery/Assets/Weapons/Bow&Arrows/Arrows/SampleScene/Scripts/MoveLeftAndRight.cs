using UnityEngine;

public class MoveLeftAndRight : MonoBehaviour
{
    public float Speed;
    public float Distance;

    private bool directionSwitch = true;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        // Move the object to the right until "Distance" is reached, then reverse
        if (directionSwitch)
        {
            transform.Translate(-Vector3.right * Speed * Time.fixedDeltaTime);
            if (Vector3.Distance(startPos, transform.position) > Distance)
                directionSwitch = false;
        }
        else
        {
            transform.Translate(-Vector3.left * Speed * Time.fixedDeltaTime);
            if (Vector3.Distance(startPos, transform.position) > Distance)
                directionSwitch = true;
        }
    }
}
