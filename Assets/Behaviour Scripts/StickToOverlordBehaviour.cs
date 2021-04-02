using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Stick To Overlord")]
public class StickToOverlordBehaviour : FlockBehaviour
{
    private GameObject overlord;
    private Rigidbody2D rigidbody;
    private Vector2 overlordCentre;
    public float overlordRadius = 2.0f;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        overlord = GameObject.Find("Overlord");
        rigidbody = overlord.GetComponent<Rigidbody2D>();
        overlordCentre = rigidbody.centerOfMass;

        Vector2 overlordOffset = overlordCentre - (Vector2)agent.transform.position;
        float t = overlordOffset.magnitude / overlordRadius;
        if (t < 0.9f)
        {
            return Vector2.zero;
            // return overlordCentre;
        }

        return overlordOffset * t * t;
    }
}
