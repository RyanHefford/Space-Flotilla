using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }
    private Rigidbody2D rb;

    //Pathfinding related
    public EnemyAI enemyAI;
    public bool nowPathFinding = false;
    public bool goToCorner = false;
    public bool isOverlord = false;
    public bool stickingToOverlord = false;
    public bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        enemyAI = GetComponent<EnemyAI>();
    }

    public void Move(Vector2 velocity)
    {
        //increase speed when going to the corner.
        if (goToCorner)
        {
            velocity *= 3;
        }
        //increase speed when player attacking
        if (attacking)
        {
            velocity *= 2;
        }

        transform.up = velocity;
        // transform.position += (Vector3)velocity * Time.deltaTime;

        rb.velocity = velocity;
        rb.angularVelocity = 0.0f;
    }

    public void moveTo(Vector2 location)
    {
        // transform.up = velocity;

        Vector2 dir = (location - (Vector2)rb.transform.position).normalized * 10;
        rb.velocity = dir;
        rb.angularVelocity = 0.0f;
    }
}
