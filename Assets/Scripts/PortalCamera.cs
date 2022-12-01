using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    [SerializeField] Transform PlayerCamera;
    [SerializeField] Transform portal;
    [SerializeField] Transform playerPortal;

    private void Update() {
        Vector3 playerOffsetFromPortal = PlayerCamera.position - playerPortal.position;
        transform.position = portal.position + playerOffsetFromPortal;

        float angularDifferenceBetweenPortalRotation = Quaternion.Angle(portal.rotation, playerPortal.rotation);

        Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotation, Vector3.up);
        Vector3 newCameraDirection = portalRotationalDifference * PlayerCamera.forward;
        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
    }
}
