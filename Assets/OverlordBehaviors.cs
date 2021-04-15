using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlordBehaviors : MonoBehaviour
{

    public float wanderTime;
    public float movementSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(wanderTime > 0)
        {
            transform.Translate(Vector2.up* movementSpeed);
            wanderTime -= Time.deltaTime;
        }
        else
        {
            wanderTime = Random.Range(5.0f, 10.0f);
            wander();
        }
    }

    private void wander()
    {
        transform.eulerAngles = new Vector3(0, 0, Random.Range(-370, 360));
    }
}
