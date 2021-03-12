using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    private float northEdge;
    private float eastEdge;
    private float southEdge;
    private float westEdge;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 horizontalVector = GetHorizontalEdge();
        Vector2 verticalVector = GetVerticalEdge();
        northEdge = verticalVector.y;
        southEdge = -verticalVector.y;
        eastEdge = horizontalVector.x;
        westEdge = -horizontalVector.x;
    }

    private Vector2 GetHorizontalEdge()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        float spriteWidth = sRenderer.sprite.bounds.size.x * transform.lossyScale.x;

        
        return new Vector2(transform.position.x + (spriteWidth / 2), transform.position.y);

    }

    private Vector2 GetVerticalEdge()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        float spriteWidth = sRenderer.sprite.bounds.size.y * transform.lossyScale.y;


        return new Vector2(transform.position.x, transform.position.y + (spriteWidth / 2));

    }

    //getter methods for edges
    public float getNorthEdge()
    {
        return northEdge;
    }

    public float getEastEdge()
    {
        return eastEdge;
    }

    public float getSouthEdge()
    {
        return southEdge;
    }

    public float getWestEdge()
    {
        return westEdge;
    }
}
