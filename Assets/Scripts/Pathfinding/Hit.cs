using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public Flock flock;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //remove from the list.
        FlockAgent agentToDelete = collision.gameObject.GetComponent<FlockAgent>();
        flock.agents.Remove(agentToDelete);
        Destroy(collision.gameObject);
    }


    //private void removeDestroyedAgentFromList(FlockAgent agentToDelete)
    //{
    //    foreach (FlockAgent agent in flock.agents)
    //    {
    //        if(agent == agentToDelete)
    //        {
    //            flock.agents.Remove(agentToDelete)
    //        }
    //    }

    //}

}
