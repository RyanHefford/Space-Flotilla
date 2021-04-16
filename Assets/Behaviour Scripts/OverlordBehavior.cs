using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Overlord Behavior")]
public class OverlordBehavior : FlockBehaviour
{

    private OverlordManager om;
    private MapScript ms;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {

        om = GameObject.Find("Manager").GetComponent<OverlordManager>();
        ms = GameObject.Find("Background").GetComponent<MapScript>();

        Debug.Log(ms.getNorthEdge());
        Debug.Log(ms.getSouthEdge());


        if (om.overlordExists)
        {

            return Vector2.zero;

        }
        else
        {
            return Vector2.zero;
        }
        
    }
}


//select a random location
//change the target but NOT every frame. Every few seconds