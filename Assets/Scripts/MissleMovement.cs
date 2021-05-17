using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleMovement : MonoBehaviour
{
    private float moveSpeed = 30;
    private float maxLifeTime = 1.0f;
    private float lifeTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(0,moveSpeed * Time.deltaTime,0);
        lifeTime += Time.deltaTime;
        //once time limit expires destory object
        if(lifeTime >= maxLifeTime)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }
}
