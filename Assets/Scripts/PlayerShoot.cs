using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    private float shotDelay = 0.2f;
    private float lastShot = 0f;
    private float cannonOffset = 0.5f;
    //value used to alternate cannons
    private bool altCannon = false;
    public GameObject basicMissle;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        lastShot -= Time.deltaTime;
        //if left click shoot
        if (Input.GetKey(KeyCode.Mouse0) && lastShot <= 0)
        {
            lastShot = shotDelay;
            //instantiate both missles
            GameObject tempMissle = Instantiate<GameObject>(basicMissle);

            //adjust positions of both missles

            tempMissle.transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);

            if (!altCannon)
            {
                tempMissle.transform.Translate(new Vector3(cannonOffset, 0.3f, 0));
            }
            else
            {
                tempMissle.transform.Translate(new Vector3(-cannonOffset, 0.3f, 0));
            }

            altCannon = !altCannon;
        }
    }
}
