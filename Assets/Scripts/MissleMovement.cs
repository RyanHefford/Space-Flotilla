using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleMovement : MonoBehaviour
{
    private float moveSpeed = 50;
    private float maxLifeTime = 2.0f;
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
}
