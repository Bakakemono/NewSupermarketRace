using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaceCircleTest : MonoBehaviour
{
    float circleRadius = 2.0f;
    Vector2 initialPos = new Vector2(1, 0);
    [SerializeField][Range(0, 359)]float aimedAngle = 0;

    Vector2 GetPos(Vector2 center, float radius, float angle) {
        return new Vector2((float)(center.x + radius * Mathf.Cos(Mathf.Deg2Rad * (angle))),
    (float)(center.y + radius * Mathf.Sin(Mathf.Deg2Rad * (angle))));
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circleRadius);
        Gizmos.DrawSphere(GetPos(transform.position, circleRadius, aimedAngle), 0.3f);
    }
}
