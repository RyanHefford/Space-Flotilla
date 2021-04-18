using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperBeamScipt : MonoBehaviour
{
    private bool beamReady = false;
    private ParticleSystem beamCharge;
    private ParticleSystem beamPrepare;
    private ParticleSystem beamFire;

    private AudioSource audio;
    private Flock flock;
    private AudioClip beamPrepareSound;
    private AudioClip beamFireSound;

    private PolygonCollider2D beamCollider;

    // Start is called before the first frame update
    void Start()
    {
        beamCharge = GameObject.Find("BeamCharge").GetComponent<ParticleSystem>();
        beamPrepare = GameObject.Find("BeamPrepare").GetComponent<ParticleSystem>();
        beamFire = GameObject.Find("BeamFire").GetComponent<ParticleSystem>();

        beamCollider = GameObject.FindGameObjectWithTag("HyperBeam").GetComponent<PolygonCollider2D>();
        beamCollider.enabled = false;

        audio = GetComponent<AudioSource>();

        beamPrepareSound = Resources.Load<AudioClip>("Sounds/LaserPrepare");
        beamFireSound = Resources.Load<AudioClip>("Sounds/FiringLaser");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && beamReady)
        {
            fireBeam();
        }
    }

    public void chargeBeam()
    {
        audio.Play();
        beamReady = true;
        beamCharge.Play();
    }

    private void fireBeam()
    {
        audio.Stop();
        beamReady = false;
        beamCharge.Stop();

        audio.PlayOneShot(beamPrepareSound);
        beamPrepare.Play();
        StartCoroutine(FireDelay());
        StartCoroutine(FinishFiring());
    }

    private IEnumerator FireDelay()
    {
        audio.clip = beamFireSound;
        audio.loop = false;
        //yield on a new YieldInstruction that waits for 2 seconds.
        yield return new WaitForSeconds(.5f);
        beamFire.Play();
        audio.Play();
        beamCollider.enabled = true;
        GetComponent<PlayerMovement>().moveSpeed /= 4;
        GetComponent<PlayerMovement>().rotationSpeed /= 4;
    }

    private IEnumerator FinishFiring()
    {
        
        //yield on a new YieldInstruction that waits for 2 seconds.
        yield return new WaitForSeconds(6.25f);
        beamCollider.enabled = false;
        audio.clip = Resources.Load<AudioClip>("Sounds/LaserReady"); ;
        audio.loop = true;
        GetComponent<PlayerMovement>().moveSpeed *= 4;
        GetComponent<PlayerMovement>().rotationSpeed *= 4;
    }

}
