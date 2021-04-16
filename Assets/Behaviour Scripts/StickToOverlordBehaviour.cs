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

    private OverlordManager om;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        om = GameObject.Find("Manager").GetComponent<OverlordManager>();


        //see if the overlord exists and is within the nearby objects inside the context variable.
        bool overlordNearby = false;
        foreach(Transform transform in context)
        {
            if(transform.gameObject.tag == "Agent" || transform.gameObject.tag == "Overlord")
            {
                FlockAgent flockAgent = transform.gameObject.GetComponent<FlockAgent>();


                //agent nearby IS AN OVERLORD and the agent calculating the loop IS NOT an overlord. 
                if (flockAgent.isOverlord && !agent.isOverlord)
                {
                    //the current agent doing the calculation should do it
                    overlordNearby = true;
                }
            }
            
        }

        if (overlordNearby)
        {
            agent.stickingToOverlord = true;
        }

        if (om.overlordExists && !agent.isOverlord && agent.stickingToOverlord)
        {
            Debug.Log("INSIDE");
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
        else
        {
            return Vector2.zero;
        }

        
    }
}
