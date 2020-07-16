using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float turnSpeed = 2f;

    private void FixedUpdate()
    {
        TurnAround();
    }

    private void TurnAround()
    {
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * turnSpeed);
        transform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y") * turnSpeed);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }
}
