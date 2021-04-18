using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamPickupScript : MonoBehaviour
{
    public ParticleSystem pickupEffect;

    // Start is called before the first frame update
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<HyperBeamScipt>().chargeBeam();
            ParticleSystem partical = Instantiate<ParticleSystem>(pickupEffect);
            partical.transform.SetPositionAndRotation(transform.position, transform.rotation);
            partical.transform.localScale = new Vector3(2,2,2);
            Destroy(this.gameObject);
        }
    }
}
