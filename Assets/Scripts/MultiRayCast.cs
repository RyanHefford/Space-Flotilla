using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiRayCast : MonoBehaviour
{
    public Transform frontLeft;
    public Transform frontRight;
    public Transform frontCenter;

    public Transform leftUpper;
    public Transform rightUpper;
    public Transform bottom;

    private MapScript ms;
    private OverlordManager om;

    public LayerMask obstacleLayerMask;

    public float rayLength = .9f;
    public float sideRadius = .6f;

    public bool isHittingObstacle = false;

    bool leftStatus = false;
    bool rightStatus = false;
    bool centerStatus = false;
    bool leftUpperStatus = false;
    bool rightUpperStatus = false;
    bool bottomStatus = false;

    
    //collect the outcomes of this raycast hits
    public bool[] hitOutcomes;
    public void Awake()
    {
        
        om= GameObject.Find("Manager").GetComponent<OverlordManager>();
        ms = GameObject.Find("Background").GetComponent<MapScript>();
        hitOutcomes = new bool[6];
    }
    
    // Update is called once per frame
    public void Update()
    {

        Vector2 leftStartPoint = frontLeft.position;
        Vector2 rightStartPoint = frontRight.position;
        Vector2 centerStartPoint = frontCenter.position;

        Collider2D leftCollider = Physics2D.OverlapCircle(leftUpper.position, sideRadius, obstacleLayerMask);
        Collider2D rightCollider = Physics2D.OverlapCircle(rightUpper.position, sideRadius, obstacleLayerMask);
        Collider2D bottomCollider = Physics2D.OverlapCircle(bottom.position, sideRadius, obstacleLayerMask);

        RaycastHit2D frontLeftHit = Physics2D.Raycast(leftStartPoint, Vector2.up, rayLength, obstacleLayerMask);
        RaycastHit2D frontRightHit = Physics2D.Raycast(rightStartPoint, Vector2.up, rayLength, obstacleLayerMask);
        RaycastHit2D frontCenterHit = Physics2D.Raycast(centerStartPoint, Vector2.up, rayLength, obstacleLayerMask);

        // LEFT RAY
        if (frontLeftHit.collider != null || frontRightHit.collider != null || frontCenterHit.collider != null)
        {
            leftStatus = true;
            rightStatus = true;
            centerStatus = true;

            om.wanderLocation = new Vector2(ms.getWestEdge(), transform.position.y);

            if (leftCollider != null)
            {
                leftUpperStatus = true;
                om.wanderLocation = new Vector2(ms.getEastEdge(), transform.position.y);
            }
            if (rightCollider != null)
            {
                rightUpperStatus = true;
                om.wanderLocation = new Vector2(ms.getSouthEdge(), transform.position.y);
            }
            if (bottomCollider != null)
            {
                bottomStatus = true;
                om.wanderLocation = new Vector2(ms.getNorthEdge(), transform.position.y);
            }
        }

        hitOutcomes[0] = leftStatus;
        hitOutcomes[1] = rightStatus;
        hitOutcomes[2] = centerStatus;
        hitOutcomes[3] = leftUpperStatus;
        hitOutcomes[4] = rightUpperStatus;
        hitOutcomes[5] = bottomStatus;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(frontLeft.position, new Vector2(frontLeft.position.x, frontLeft.position.y + rayLength));
        Gizmos.DrawLine(frontRight.position, new Vector2(frontRight.position.x, frontRight.position.y + rayLength));
        Gizmos.DrawLine(frontCenter.position, new Vector2(frontCenter.position.x, frontCenter.position.y + rayLength));

        Gizmos.DrawWireSphere(leftUpper.position, sideRadius);
        Gizmos.DrawWireSphere(rightUpper.position, sideRadius);
        Gizmos.DrawWireSphere(bottom.position, sideRadius);
    }
}

