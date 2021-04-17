using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlordMultiCast : MonoBehaviour
{ 

    public Transform frontLeft;
    public Transform frontRight;
    public Transform frontCenter;
    public LayerMask obstacleLayerMask;
    public float rayLength = .9f;
    public bool isHittingObstacle = false;
    bool leftStatus = false;
    bool rightStatus = false;
    bool centerStatus = false;
    //collect the outcomes of this raycast hits
    public bool[] hitOutcomes;

    public void Awake()
    {
        hitOutcomes = new bool[3];
    }


    // Update is called once per frame
    void Update()
    {
        MulticastRay();
    }

    void MulticastRay()
    {
        Vector2 leftStartPoint = frontLeft.position;
        Vector2 rightStartPoint = frontRight.position;
        Vector2 centerStartPoint = frontCenter.position;

        RaycastHit2D frontLeftHit = Physics2D.Raycast(leftStartPoint, Vector2.up, rayLength, obstacleLayerMask);
        RaycastHit2D frontRightHit = Physics2D.Raycast(rightStartPoint, Vector2.up, rayLength, obstacleLayerMask);
        RaycastHit2D frontCenterHit = Physics2D.Raycast(centerStartPoint, Vector2.up, rayLength, obstacleLayerMask);

        // LEFT RAY
        if (frontLeftHit.collider != null)
        {
            leftStatus = true;
            var leftDistance = frontLeftHit.distance;
            Debug.Log("LEFT HIT:: Obstacle Name " + frontLeftHit.collider.name + " || Left Hit Distance: " + leftDistance);
        }
        else
        {
            leftStatus = false;
        }
        // RIGHT RAY
        if (frontRightHit.collider != null)
        {
            rightStatus = true;
            var rightDistance = frontRightHit.distance;
            Debug.Log("RIGHT HIT:: Obstacle Name " + frontRightHit.collider.name + " || Right Hit Distance: " + rightDistance);     
        }
        else
        {
            rightStatus = false;
        }
        // CENTER RAY
        if (frontCenterHit.collider != null)
        {
            centerStatus = true;
            var centerDistance = frontCenterHit.distance;
            Debug.Log("CENTER HIT:: Obstacle Name " + frontCenterHit.collider.name + " || Center Hit Distance: " + centerDistance);
        }
        else
        {
            rightStatus = false;
        }

        hitOutcomes[0] = leftStatus;
        hitOutcomes[1] = rightStatus;
        hitOutcomes[2] = centerStatus;

        for (int i= 0; i < 3; i++)
        {
            Debug.Log("Hit on Element[" + i + "]:" + hitOutcomes[i]);
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(frontLeft.position, new Vector2(frontLeft.position.x, frontLeft.position.y + rayLength));
        Gizmos.DrawLine(frontRight.position, new Vector2(frontRight.position.x, frontRight.position.y + rayLength));
        Gizmos.DrawLine(frontCenter.position, new Vector2(frontCenter.position.x, frontCenter.position.y + rayLength));
    }


}
