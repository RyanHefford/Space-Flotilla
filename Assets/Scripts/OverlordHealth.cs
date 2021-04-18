using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class OverlordHealth : MonoBehaviour
{
    public float playerHealth = 1;
    private float iFrames = 1.0f;
    private float lastHit = 0f;
    private Slider healthBar;
    private OverlordManager om;


    // Start is called before the first frame update
    void Start()
    {
        healthBar = GameObject.FindGameObjectWithTag("HealthbarOverlord").GetComponent<Slider>();
        healthBar.value = playerHealth;
        om = GameObject.Find("Manager").GetComponent<OverlordManager>();
    }

   

    public void takeDamage(float damage)
    {
       
        playerHealth -= damage;
        healthBar.value = playerHealth;
        lastHit = Time.time;


    }


    public float getPlayerHealth()
    {
        return playerHealth;
    }

    

}
