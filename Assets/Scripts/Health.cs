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
    public GameObject shield;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        //healthBar = GameObject.FindGameObjectWithTag("Healthbar").GetComponent<Slider>();
        //healthBar.value = playerHealth;
    }

    private void Update()
    {
        if (Time.time - lastHit > iFrames)
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
        if(Time.time - lastHit > iFrames)
        {
            playerHealth -= damage;
            //healthBar.value = playerHealth;
            lastHit = Time.time;

            if(playerHealth <= 0)
            {
                //create explosion
                GameObject tempObject = Instantiate<GameObject>(explosion);
                tempObject.transform.SetPositionAndRotation(transform.position, transform.rotation);

                GameObject.FindGameObjectWithTag("Background").GetComponent<MapScript>().CauseDelay(this.gameObject);
            }
        }
    }
}
