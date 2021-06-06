using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    private float shotDelay = 0.2f;
    private float lastShot = 0f;
    private float cannonOffset = 0.5f;

    private AudioSource audioSource;
    private AudioClip shootSound;

    public bool alternatingShots; 
    public bool sound;

    //value used to alternate cannons
    private bool altCannon = false;
    public GameObject basicMissle;

    //agent script
    public bool shoot = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        shootSound = Resources.Load<AudioClip>("Sounds/PlayerShootSound");
    }

    // Update is called once per frame
    void Update()
    {
        lastShot -= Time.deltaTime;
        //if left click shoot
        if (shoot && lastShot <= 0)
        {
            if (sound) { audioSource.PlayOneShot(shootSound); }
            lastShot = shotDelay;
            //instantiate both missles
            GameObject tempMissle = Instantiate<GameObject>(basicMissle);
            tempMissle.GetComponent<Hit>().AddParent(this.gameObject);

            //adjust positions of both missles

            tempMissle.transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);

            if (alternatingShots)
            {
                if (!altCannon)
                {
                    tempMissle.transform.Translate(new Vector3(cannonOffset, 0.3f, 0));
                }
                else
                {
                    tempMissle.transform.Translate(new Vector3(-cannonOffset, 0.3f, 0));
                }
            }
            else
            {
                tempMissle.transform.Translate(new Vector3(0, 0.3f, 0));
            }
            

            altCannon = !altCannon;
        }

        //add negative reward if not shooting
        if (lastShot <= -5)
        {
            GetComponent<ShipBehavior>().Reward(-0.1f);
        }
    }

    public void newEpisode()
    {
        shotDelay = Academy.Instance.EnvironmentParameters.GetWithDefault("shot_cooldown", 0.2f);
    }
}
