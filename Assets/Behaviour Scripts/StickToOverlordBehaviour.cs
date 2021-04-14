using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Stick To Overlord")]
public class StickToOverlordBehaviour : FlockBehaviour
{
    private GameObject overlord;
    private Rigidbody2D rigidbody;
    private Vector2 overlordCentre;
    public float overlordRadius = 5.0f;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        overlord = GameObject.FindGameObjectsWithTag("Overlord")[0];
        overlordCentre = overlord.transform.position;

        Vector2 overlordOffset = overlordCentre - (Vector2)agent.transform.position;
        float t = overlordOffset.magnitude / overlordRadius;
        if (t < 0.9f)
        {
            return Vector2.zero;
        }

        return overlordOffset * t * t;
    }
}
