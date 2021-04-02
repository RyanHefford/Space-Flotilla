using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Stick To Overlord")]
public class StickToOverlordBehaviour : FlockBehaviour
{
    private GameObject overlord;
    private Rigidbody2D rigidbody;
    public Vector2 overlord_centre;
    public float overlordRadius = 2.0f;

    private void Start()
    {
        overlord = GameObject.Find("Player");
        rigidbody = overlord.GetComponent<Rigidbody2D>();
        overlord_centre = rigidbody.centerOfMass;
    }

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        Vector2 overlordOffset = overlord_centre - (Vector2)agent.transform.position;
        float t = overlordOffset.magnitude / overlordRadius;
        if (t < 0.9f)
        {
            return Vector2.zero;
        }

        return overlordOffset * t * t;
    }
}
