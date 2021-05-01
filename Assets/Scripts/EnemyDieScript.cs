using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieScript : MonoBehaviour
{
    public GameObject explosion;
    public GameObject pickup;
    private HyperBeamScipt playerBeam;

    // Start is called before the first frame update
    void Start()
    {
        playerBeam = GameObject.FindGameObjectWithTag("Player").GetComponent<HyperBeamScipt>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Die()
    {
        GameObject startExplosion = Instantiate<GameObject>(explosion);

        if (Random.Range(0, 100) < 1 && !playerBeam.isFiring())
        {
            Instantiate<GameObject>(pickup).transform.SetPositionAndRotation(transform.position, transform.rotation);
        }

        startExplosion.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
        startExplosion.transform.localScale = new Vector3(.5f, .5f, .5f);
        Destroy(this.gameObject);
    }
}
