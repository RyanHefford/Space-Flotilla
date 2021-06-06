using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public float playerHealth = 1;
    private float iFrames = 1.0f;
    private float lastHit = 0f;
    private Slider healthBar;
    public bool shieldActive;
    public GameObject shield;
    public GameObject explosion;

    //agent script
    private ShipBehavior ab;

    // Start is called before the first frame update
    void Start()
    {
        ab = GetComponent<ShipBehavior>();
        //healthBar = GameObject.FindGameObjectWithTag("Healthbar").GetComponent<Slider>();
        //healthBar.value = playerHealth;
    }

    private void Update()
    {
        if (Time.time - lastHit > iFrames || !shieldActive)
        {
            shield.SetActive(false);
        }
        else
        {
            shield.SetActive(true);
        }

    }
    
    public void gainHealth(float healing)
    {
        playerHealth += healing;
        //Check if overhealed
        playerHealth = playerHealth > 100 ? 100 : playerHealth;
        //healthBar.value = playerHealth;
        lastHit = Time.time;
    }

    public void takeDamage(float damage)
    {
        //check if iFrames are still active
        if(Time.time - lastHit > iFrames || !shieldActive)
        {
            playerHealth -= damage;
            //healthBar.value = playerHealth;
            lastHit = Time.time;

            if(playerHealth <= 0)
            {
                //create explosion
                //GameObject tempObject = Instantiate<GameObject>(explosion);
                //tempObject.transform.SetPositionAndRotation(transform.position, transform.rotation);

                //Big negative reward and reset everything.
                ab.Reward(-1);
                ab.lose();
                ab.EndEpisode();

                //GameObject.FindGameObjectWithTag("Background").GetComponent<MapScript>().CauseDelay(this.gameObject);
            }
        }
    }
}
