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

    public Transform bottomLeft;
    public Transform bottomRight;
    public Transform bottomCenter;

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
    bool bottomLeftStatus = false;
    bool bottomRightStatus = false;
    bool bottomCenterStatus = false;


    //collect the outcomes of this raycast hits
    public bool[] hitOutcomes;
    public void Awake()
    {
        
        om= GameObject.Find("Manager").GetComponent<OverlordManager>();
        ms = GameObject.Find("Background").GetComponent<MapScript>();
        hitOutcomes = new bool[8];
    }
    
    // Update is called once per frame
    public void Update()
    {

        Collider2D leftCollider = Physics2D.OverlapCircle(leftUpper.position, sideRadius, obstacleLayerMask);
        Collider2D rightCollider = Physics2D.OverlapCircle(rightUpper.position, sideRadius, obstacleLayerMask);

        RaycastHit2D frontLeftHit = Physics2D.Raycast(frontLeft.position, Vector2.up, rayLength, obstacleLayerMask);
        RaycastHit2D frontRightHit = Physics2D.Raycast(frontRight.position, Vector2.up, rayLength, obstacleLayerMask);
        RaycastHit2D frontCenterHit = Physics2D.Raycast(frontCenter.position, Vector2.up, rayLength, obstacleLayerMask);

        RaycastHit2D bottomLeftHit = Physics2D.Raycast(bottomLeft.position, Vector2.down, rayLength, obstacleLayerMask);
        RaycastHit2D bottomRightHit = Physics2D.Raycast(bottomRight.position, Vector2.down, rayLength, obstacleLayerMask);
        RaycastHit2D bottomCenterHit = Physics2D.Raycast(bottomCenter.position, Vector2.down, rayLength, obstacleLayerMask);

        var leftDistance = frontLeftHit.distance;
        var rightDistance = frontRightHit.distance;
        var centerDistance = frontCenterHit.distance;

        var BleftDistance = bottomLeftHit.distance;
        var BrightDistance = bottomRightHit.distance;
        var BcenterDistance = bottomCenterHit.distance;


        // LEFT RAY
        if (frontLeftHit.collider != null || frontRightHit.collider != null || frontCenterHit.collider != null)
        {
            leftStatus = true;
            rightStatus = true;
            centerStatus = true;

          //  Debug.Log("LEFT HIT:: Obstacle Name " + frontLeftHit.collider.name + " ||  Hit Distance: " + leftDistance);
          //  Debug.Log("RIGHT HIT:: Obstacle Name " + frontRightHit.collider.name + " ||  Hit Distance: " + rightDistance);
           // Debug.Log("CENTER HIT:: Obstacle Name " + frontCenterHit.collider.name + " ||  Hit Distance: " + centerDistance);

            om.wanderLocation = new Vector2(ms.getWestEdge(), this.transform.position.y).normalized;
            if (leftCollider != null)
            {
                leftUpperStatus = true;
                om.wanderLocation = new Vector2(ms.getEastEdge(), this.transform.position.y);
                
            }
            if (rightCollider != null)
            {
                rightUpperStatus = true;
                om.wanderLocation = new Vector2(ms.getSouthEdge(), transform.position.y).normalized;
            }
            if (bottomLeftHit.collider != null || bottomRightHit.collider != null || bottomCenterHit.collider != null)
            {
                bottomLeftStatus = true;
                bottomRightStatus = true;
                bottomCenterStatus = true;


              //  Debug.Log("BOTTOM LEFT HIT:: Obstacle Name " + bottomLeftHit.collider.name + " ||  Hit Distance: " + BleftDistance);
              //  Debug.Log("BOTTOM RIGHT HIT:: Obstacle Name " + bottomRightHit.collider.name + " ||  Hit Distance: " + BrightDistance);
              //  Debug.Log("BOTTOM CENTER HIT:: Obstacle Name " + bottomCenterHit.collider.name + " ||  Hit Distance: " + BcenterDistance);

                om.wanderLocation = new Vector2(ms.getNorthEdge(), this.transform.position.y).normalized;
            }
        }

        hitOutcomes[0] = leftStatus;
        hitOutcomes[1] = rightStatus;
        hitOutcomes[2] = centerStatus;
        hitOutcomes[3] = leftUpperStatus;
        hitOutcomes[4] = rightUpperStatus;
        hitOutcomes[5] = bottomLeftStatus;
        hitOutcomes[6] = bottomRightStatus;
        hitOutcomes[7] = bottomCenterStatus;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(frontLeft.position, new Vector2(frontLeft.position.x, frontLeft.position.y + rayLength));
        Gizmos.DrawLine(frontRight.position, new Vector2(frontRight.position.x, frontRight.position.y + rayLength));
        Gizmos.DrawLine(frontCenter.position, new Vector2(frontCenter.position.x, frontCenter.position.y + rayLength));

        Gizmos.DrawWireSphere(leftUpper.position, sideRadius);
        Gizmos.DrawWireSphere(rightUpper.position, sideRadius);

        Gizmos.DrawLine(bottomLeft.position, new Vector2(bottomLeft.position.x, bottomLeft.position.y - rayLength));
        Gizmos.DrawLine(bottomRight.position, new Vector2(bottomRight.position.x, bottomRight.position.y - rayLength));
        Gizmos.DrawLine(bottomCenter.position, new Vector2(bottomCenter.position.x, bottomCenter.position.y - rayLength));


    }
}

