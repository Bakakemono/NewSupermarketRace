using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaceCircleTest : MonoBehaviour
{
    float circleRadius = 1.0f;
    Vector2 initialDirection = Vector2.right;
    [SerializeField] Vector2 pos = Vector2.right;
    [SerializeField] float currentAngle = 0;
    [SerializeField] [Range(0.0f, 359.0f)] float anglePos = 0;
    [SerializeField] [Range(0.0f, 359.0f)] float aimedAngle = 0;
    
    

    // Give a postion relative to a center with a length of one
    Vector2 GetPos(float radius, float angle) {
        return
            new Vector2(
                Mathf.Sin(Mathf.Deg2Rad * (angle + 90.0f)),
                Mathf.Cos(Mathf.Deg2Rad * (angle + 90.0f))
                )
            * radius;
    }

    private void OnDrawGizmos() {
        pos = GetPos(circleRadius, anglePos);
        currentAngle = Mathf.Rad2Deg * Mathf.Acos(Vector2.Dot(initialDirection, pos) / (initialDirection.magnitude * pos.magnitude));
        if(pos.y > 0) {
            currentAngle += Mathf.Rad2Deg * Mathf.Acos(Vector2.Dot(Vector2.left, pos) / (initialDirection.magnitude * pos.magnitude)) * 2;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + (Vector3)GetPos(circleRadius, currentAngle), 0.3f);


        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circleRadius);
        Gizmos.DrawSphere(transform.position + (Vector3)GetPos(circleRadius, currentAngle + aimedAngle), 0.3f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + (Vector3)GetPos(circleRadius * 2, currentAngle), 0.3f);


        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circleRadius * 2);
        Gizmos.DrawSphere(transform.position + (Vector3)GetPos(circleRadius * 2, currentAngle + aimedAngle * 2), 0.3f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + (Vector3)GetPos(circleRadius * 3, currentAngle), 0.3f);


        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circleRadius * 3);
        Gizmos.DrawSphere(transform.position + (Vector3)GetPos(circleRadius * 3, currentAngle + aimedAngle * 3), 0.3f);

    }
}
