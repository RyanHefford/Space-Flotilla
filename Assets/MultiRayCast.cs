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

    public LayerMask obstacleLayerMask;

    public float rayLength = .9f;
    public float sideRadius = .6f;

    public bool isHittingObstacle = false;

    bool leftStatus = false;
    bool rightStatus = false;
    bool centerStatus = false;
    bool leftUpperStatus = false;
    bool rightUpperStatus = false;

    //collect the outcomes of this raycast hits
    public bool[] hitOutcomes;

    public void Awake()
    {
        hitOutcomes = new bool[5];
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

        Collider2D leftCollider = Physics2D.OverlapCircle(leftUpper.position, sideRadius, obstacleLayerMask);
        Collider2D rightCollider = Physics2D.OverlapCircle(rightUpper.position, sideRadius, obstacleLayerMask);

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
        // UPPER LEFT
        if (leftCollider != null)
        {
            leftUpperStatus = true;
            Debug.Log("UPPER LEFT HIT:: " + leftUpperStatus);
        }
        else
        {
            leftUpperStatus = false;
            Debug.Log("UPPER RIGHT HIT:: " + leftUpperStatus);
        }
        // UPPER RIGHT
        if (rightCollider != null)
        {
            rightUpperStatus = true;
            Debug.Log("UPPER RIGHT HIT:: " + rightUpperStatus);
        }
        else
        {
            rightUpperStatus = false;
            Debug.Log("UPPER RIGHT HIT:: " + rightUpperStatus);
        }

        hitOutcomes[0] = leftStatus;
        hitOutcomes[1] = rightStatus;
        hitOutcomes[2] = centerStatus;
        hitOutcomes[3] = leftUpperStatus;
        hitOutcomes[4] = rightUpperStatus;

        for (int i = 0; i < 5; i++)
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

        Gizmos.DrawWireSphere(leftUpper.position, sideRadius);
        Gizmos.DrawWireSphere(rightUpper.position, sideRadius);
    }




}
