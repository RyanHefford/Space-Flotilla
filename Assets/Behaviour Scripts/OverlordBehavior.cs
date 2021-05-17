using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Overlord Behavior")]
public class OverlordBehavior : FlockBehaviour
{

    

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
       
            return Vector2.zero;
        
        
    }
}


//select a random location
//change the target but NOT every frame. Every few seconds