using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10;
    public float rotationSpeed = 5;
    public Camera cam;
    private Rigidbody2D rigidbody;
    private Vector2 movementVector;
    private Vector2 mousePos;
    private float hitBoxRadius = 2f;
    private MapScript map;

    //float positions for each edge of the map
    private float northEdge;
    private float eastEdge;
    private float southEdge;
    private float westEdge;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        //get edges of map
        map = GameObject.FindGameObjectsWithTag("Background")[0].GetComponent<MapScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (northEdge == 0)
        {
            eastEdge = map.getEastEdge();
            westEdge = map.getWestEdge();
            northEdge = map.getNorthEdge();
            southEdge = map.getSouthEdge();
        }
        CalculateMovement();
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        Move();
        UpdateMousePosition();
    }

    private void CalculateMovement()
    {
        float xMovement = Input.GetAxisRaw("Horizontal");
        float yMovement = Input.GetAxisRaw("Vertical");

        movementVector = new Vector2(xMovement, yMovement).normalized;
    }

    private void Move()
    {
        rigidbody.AddForce(new Vector2(movementVector.x * moveSpeed, movementVector.y * moveSpeed));
        //counter current force so that ship automaticly slows
        rigidbody.AddForce(new Vector2(-rigidbody.velocity.x, -rigidbody.velocity.y));
        //check if on edge of map

        //east edge
        if (transform.position.x + hitBoxRadius >= eastEdge)
        {
            transform.position = new Vector3(eastEdge - hitBoxRadius, transform.position.y, 0);
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            rigidbody.angularVelocity = 0;

        }
        //north edge
        if(transform.position.y + hitBoxRadius >= northEdge)
        {
            transform.position = new Vector3(transform.position.x, northEdge - hitBoxRadius, 0);
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
            rigidbody.angularVelocity = 0;
        }
        //south edge
        if(transform.position.y - hitBoxRadius <= southEdge)
        {
            transform.position = new Vector3(transform.position.x, southEdge + hitBoxRadius, 0);
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
            rigidbody.angularVelocity = 0;
        }
        //west edge
        if (transform.position.x - hitBoxRadius <= westEdge)
        {
            transform.position = new Vector3(westEdge + hitBoxRadius, transform.position.y, 0);
            rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            rigidbody.angularVelocity = 0;
        }
    }

    private void UpdateMousePosition()
    {
        Vector2 lookDirection = mousePos - rigidbody.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        
        rigidbody.transform.rotation = Quaternion.Slerp(Quaternion.Euler(0,0, rigidbody.rotation), 
                                        Quaternion.Euler(0, 0, angle), Time.deltaTime * rotationSpeed);
    }
}
