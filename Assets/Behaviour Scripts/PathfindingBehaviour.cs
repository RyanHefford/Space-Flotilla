using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/behavior/Pathfinding")]
public class PathfindingBehaviour : FlockBehaviour
{

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //if pathfinding is not on then just return.
        if (agent.GetComponent<EnemyAI>().enabled == false)
        {
            return Vector2.zero;
        }

        Vector2 pathfindingMove = Vector2.zero;
        //List<Transform> filteredContext = (filter == null) ? context : filter.filter(agent, context);
        //foreach (Transform item in filteredContext)
        foreach (Transform item in context)
        {
            //adding where the agent is facing
            pathfindingMove += agent.enemyAI.getDirection();
        }
        //pathfindingMove /= context.Count;


        return pathfindingMove;
    }
}
