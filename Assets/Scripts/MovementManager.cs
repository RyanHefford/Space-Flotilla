using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{

    public TestMovement[] tm;
    public Transform target;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            tm[0].moveAgent(target);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            tm[1].moveAgent(target);
        }
    }


}
