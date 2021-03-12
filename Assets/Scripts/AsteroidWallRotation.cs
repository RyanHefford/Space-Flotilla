using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidWallRotation : MonoBehaviour
{
    private float spinSpeed;

    // Start is called before the first frame update
    void Start()
    {
        spinSpeed = Random.Range(10, 60);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0,0,spinSpeed*Time.deltaTime);
    }
}
