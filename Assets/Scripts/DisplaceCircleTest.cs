using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaceCircleTest : MonoBehaviour
{
    [SerializeField] float circleRadius = 2.0f;
    Vector2 initialPos = Vector2.right;
    [SerializeField] Vector2 pos = Vector2.right;
    [SerializeField] [Range(0.0f, 359.0f)] float anglePos = 0;
    [SerializeField] [Range(0.0f, 359.0f)] float aimedAngle = 0;
    
    [SerializeField] float currentAngle = 0;

    Vector2 GetPos(float radius, float angle) {
        return
            new Vector2(
                Mathf.Sin(Mathf.Deg2Rad * (angle + 90.0f)),
                Mathf.Cos(Mathf.Deg2Rad * (angle + 90.0f))
                );
    }

    private void OnDrawGizmos() {
        pos = GetPos(circleRadius, anglePos);
        currentAngle = Mathf.Rad2Deg * Mathf.Acos(Vector2.Dot(initialPos, pos));

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + (Vector3)GetPos(circleRadius, currentAngle) * circleRadius, 0.3f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circleRadius);
        Gizmos.DrawSphere(transform.position + (Vector3)GetPos(circleRadius, currentAngle + aimedAngle) * circleRadius, 0.3f);
    
    }
}
