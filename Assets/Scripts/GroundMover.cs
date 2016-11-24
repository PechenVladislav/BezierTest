using UnityEngine;
using System.Collections;

public class GroundMover : MonoBehaviour
{

    public float speed = 1f;
    public LayerMask layer;
    public Transform MoveRay;

    private DrawGround Ground;
    private int i = 1;
    Vector3 transformUp;
    Vector3 transformUpTemp;
    float distanceTemp = 0f;

    public void Start()
    {
        Ground = FindObjectOfType<DrawGround>();
        //transform.position = Ground.GetSegmentPoint(0);
        transformUpTemp = transform.up;
        transformUp = transform.up;
    }

    void FixedUpdate()
    {

        RaycastHit2D moveHit = Physics2D.Raycast(MoveRay.position, -transform.up, Mathf.Infinity, layer);
        transform.position = Vector3.MoveTowards(transform.position, moveHit.point, speed);

        transform.up = PlayerUp();
    }

    private Vector3 PlayerUp()
    {
        if (i < Ground.GetSegmentsCount - 1)
        {
            float t = 0;
            float segmentLenth = Vector3.Distance(Ground.GetSegmentPoint(i - 1), Ground.GetSegmentPoint(i));
            float completeLength = Vector3.Distance(transform.position, Ground.GetSegmentPoint(i - 1));
            t = (1f / segmentLenth * completeLength);
            if (t >= 1)
            {
                Vector2 segmentVector = Ground.GetSegmentPoint(i) - Ground.GetSegmentPoint(i - 1);
                transformUpTemp = new Vector2(-segmentVector.y, segmentVector.x) / segmentLenth;

                i++;

                segmentVector = Ground.GetSegmentPoint(i) - Ground.GetSegmentPoint(i - 1);
                transformUp = new Vector2(-segmentVector.y, segmentVector.x) / segmentLenth;
                t = t - 1f;
            }
            return Vector3.Lerp(transformUpTemp, transformUp, t);
        }
        return transform.up;
    }
}
