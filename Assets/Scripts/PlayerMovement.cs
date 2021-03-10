﻿using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    private void UpdateMousePosition()
    {
        Vector2 lookDirection = mousePos - rigidbody.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        
        rigidbody.transform.rotation = Quaternion.Slerp(Quaternion.Euler(0,0, rigidbody.rotation), 
                                        Quaternion.Euler(0, 0, angle), Time.deltaTime * rotationSpeed);
    }
}
