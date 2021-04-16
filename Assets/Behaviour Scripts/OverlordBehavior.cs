using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Overlord Behavior")]
public class OverlordBehavior : FlockBehaviour
{

    private OverlordManager om;
    

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        om = GameObject.Find("Manager").GetComponent<OverlordManager>();
        

        if (om.overlordExists && agent.isOverlord)
        {
            //Debug.Log("calling");
            return om.wanderLocation;

        }
        else
        {
            return Vector2.zero;
        }
        
    }
}


//select a random location
//change the target but NOT every frame. Every few seconds