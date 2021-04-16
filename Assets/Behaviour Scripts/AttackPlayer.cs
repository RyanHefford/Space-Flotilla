using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Attack player")]
public class AttackPlayer : FlockBehaviour
{
    private OverlordManager om;
    private int radius = 5;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        om = GameObject.Find("Manager").GetComponent<OverlordManager>();

        bool foundPlayer = false;
        //WHEN THE AGENT IS AN OVERLORD.
        if (om.overlordExists && agent.isOverlord)
        {

            //GET ALL OF THE GAME OBJECTS NEARBY
            List<Transform> transforms = GetNearbyObjects(agent);

            //DO WE HAVE THE PLAYER VISIBLE TO US?
            foreach(Transform t in transforms)
            {
                if(t.gameObject.tag == "Player")
                {
                    foundPlayer = true;
                }
            }



            //now all of the agents that are neighbors of this overlord should attack.
            if (!flock.initiatedAnAttack && foundPlayer)
            {
                flock.initiatedAnAttack = true;
                //have not started an attack YET.
                flock.attackingTimeLeft = flock.timerForEachAttack;

                foreach (Transform t in context)
                {
                    if (t.gameObject.tag == "Agent")
                    {
                        FlockAgent attackingAgent = t.gameObject.GetComponent<FlockAgent>();
                        //if the agent is with the overlord.
                        if (attackingAgent.stickingToOverlord)
                        {
                            
                            attackingAgent.enemyAI.target = GameObject.Find("Player").transform;
                            attackingAgent.GetComponent<EnemyAI>().enabled = true;
                            attackingAgent.stickingToOverlord = false;
                            //flock.agentsAttacking.Add(attackingAgent);
                            attackingAgent.attacking = true;
                            attackingAgent.nowPathFinding = true;
                            attackingAgent.name += "  ATTACKING";
                        }
                        
                    }
                }
            }
            

            return Vector2.zero;

        }
        else
        {
            return Vector2.zero;
        }

    }



    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, radius);
        foreach (Collider2D c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }
}
