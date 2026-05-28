using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform monkeyObject;
    void Update ()
    {
        FollowMonkey();
    }

    public void FollowMonkey()
    {
        Vector3 targetPosition = transform.position;
        targetPosition.x = monkeyObject.position.x;

        if (monkeyObject.position.y < -3.1f)
        {
            targetPosition.y = -3.1f;
        }
        else {
            targetPosition.y = monkeyObject.position.y;
        }

        transform.position = targetPosition;
    }
}