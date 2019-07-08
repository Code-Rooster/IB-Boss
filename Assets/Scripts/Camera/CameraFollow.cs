using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    private Vector3 dampVec;
    private Vector2 clampedPos;

    public Vector2 colSize;

    public Transform[] hitTransforms = {null, null, null, null};

    public int hitWallHoriz = 0;
    public int hitWallVert = 0;

    private void Start()
    {
        colSize = new Vector2(GetComponent<BoxCollider2D>().size.x, GetComponent<BoxCollider2D>().size.y);
    }

    void Update()
    {
        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.TransformPoint(new Vector3(0, 0, -10));

        dampVec = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        

        if (hitTransforms[0] != null && hitTransforms[1] != null)
        {
            dampVec.x = transform.position.x;
        }
        else if (hitTransforms[0] != null || hitTransforms[1] != null)
        {
            dampVec.x = clampedPos.x;
        }

        if (hitTransforms[2] != null && hitTransforms[3] != null)
        {
            dampVec.y = transform.position.y;
        }
        else if (hitTransforms[2] != null || hitTransforms[3] != null)
        {
            dampVec.y = clampedPos.y;
        }

        

        if (hitWallVert < 0)
        {
            if (target.transform.position.x - (colSize.x / 2) > hitTransforms[1].position.x)
            {
                hitTransforms[1] = null;

                hitWallVert = 0;
            }
        }

        else if (hitWallVert > 0)
        {
            if (target.transform.position.x + (colSize.x / 2) < hitTransforms[0].position.x)
            {
                hitTransforms[0] = null;

                hitWallVert = 0;
            }
        }

        if (hitWallHoriz < 0)
        {
            if (target.transform.position.y - (colSize.y / 2) > hitTransforms[3].position.y)
            {
                hitTransforms[3] = null;

                hitWallHoriz = 0;
            }
        }

        else if (hitWallHoriz > 0)
        {
            if (target.transform.position.y + (colSize.y / 2) < hitTransforms[2].position.y)
            {
                hitTransforms[2] = null;

                hitWallHoriz = 0;
            }
        }

        if (Vector3.Distance(target.transform.position, transform.position) > Mathf.Sqrt((colSize.x * 2 / 3 * colSize.x * 2 / 3) + (colSize.y * 2 / 3 * colSize.y * 2 / 3)))
        {
            print("Too far");
        }

        // Smoothly move the camera towards that target position
        transform.position = dampVec;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        print("Collided with " + col.name);

        if (col.tag == "VerticalWall")
        {
            if (col.transform.position.x > transform.position.x)
            {
                hitTransforms[0] = col.transform;

                hitWallVert = 1;
                clampedPos = transform.position;
                clampedPos.x = Mathf.Clamp(target.transform.position.x, clampedPos.x, col.transform.position.x - (colSize.x / 2));
            }
        
            if (col.transform.position.x < transform.position.x)
            {
                hitTransforms[1] = col.transform;

                //REMEMBER: DO NOT TEST FOR HITWALLVERT OR HITWALLHORIZ, THEY CAN ONLY STORE 1 VALUE
                hitWallVert = -1;
                clampedPos = transform.position;
                clampedPos.x = Mathf.Clamp(target.transform.position.x, clampedPos.x, col.transform.position.x + (colSize.x / 2));
            }

        }
        else if (col.tag == "HorizontalWall")
        {
            if (col.transform.position.y > transform.position.y)
            {
                hitTransforms[2] = col.transform;

                hitWallHoriz = 1;
                clampedPos = transform.position;
                clampedPos.y = Mathf.Clamp(target.transform.position.y, clampedPos.y, col.transform.position.y - (colSize.y / 2));
            }

            if (col.transform.position.y < transform.position.y)
            {
                hitTransforms[3] = col.transform;

                hitWallHoriz = -1;
                clampedPos = transform.position;
                clampedPos.y = Mathf.Clamp(target.transform.position.y, clampedPos.y, col.transform.position.y + (colSize.y / 2));
            }
        }
    }
}
