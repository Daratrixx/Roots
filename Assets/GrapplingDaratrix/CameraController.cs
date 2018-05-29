using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class CameraController : MonoBehaviour {

    public Player3D player;
    public Camera playerCamera;

    public Vector2 cameraAngleLimits;
    public float cameraAngle = 20;
    public float cameraRotation = -90;
    public float cameraDistance = 4;
    public Vector3 cameraOffset = Vector3.up * 2;
    public float cameraFollowSpeed = 5;
    public float cameraCollisionOffset = 0.5f;

    public const int cameraLayer = 1 << 10;

    private Vector3 lastAnchor;
    private Vector3 cameraAnchor {
        get {
            if (player != null) return lastAnchor = player.transform.position + cameraOffset;
            return lastAnchor;
        }
    }

    private void Start() {
        ResetCamera();
    }

    public void LateUpdate() {
        UpdateCamera(cameraFollowSpeed * Time.deltaTime);
    }

    public void UpdateCamera(float deltaTime) {
            Vector3 direction = new Vector3(
                Mathf.Cos(cameraAngle * Mathf.Deg2Rad) * Mathf.Sin(cameraRotation * Mathf.Deg2Rad),
                Mathf.Sin(cameraAngle * Mathf.Deg2Rad),
                Mathf.Cos(cameraAngle * Mathf.Deg2Rad) * Mathf.Cos(cameraRotation * Mathf.Deg2Rad));

            playerCamera.transform.forward = Vector3.Lerp(playerCamera.transform.forward, direction, deltaTime);
            RaycastHit cameraAdjutCast;
            if (Physics.Raycast(cameraAnchor, -direction, out cameraAdjutCast, cameraDistance, cameraLayer)) {
                playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position,
                    cameraAnchor - direction * (cameraAdjutCast.distance - cameraCollisionOffset),
                    deltaTime);
            } else {
                playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position,
                    cameraAnchor - direction * cameraDistance,
                    deltaTime);
            }
    }
    public void ResetCamera() {
        cameraRotation = player.transform.eulerAngles.y;
        Vector3 direction = new Vector3(Mathf.Cos(cameraAngle * Mathf.Deg2Rad) * Mathf.Sin(cameraRotation * Mathf.Deg2Rad),
            Mathf.Sin(cameraAngle * Mathf.Deg2Rad),
            Mathf.Cos(cameraAngle * Mathf.Deg2Rad) * Mathf.Cos(cameraRotation * Mathf.Deg2Rad));

        playerCamera.transform.forward = direction;
        RaycastHit cameraAdjutCast;
        if (Physics.Raycast(cameraAnchor, -direction, out cameraAdjutCast, cameraDistance, cameraLayer)) {
            playerCamera.transform.position = cameraAnchor - direction * (cameraAdjutCast.distance - cameraCollisionOffset);
        } else {
            playerCamera.transform.position = cameraAnchor - direction * cameraDistance;
        }
    }

    public void Update() {
        ControllCamera();
    }

    public void ControllCamera() {
        cameraAngle += UnityEngine.Input.GetAxis("Mouse Y");
        cameraRotation += UnityEngine.Input.GetAxis("Mouse X");
        cameraAngle = Mathf.Clamp(cameraAngle, cameraAngleLimits.x, cameraAngleLimits.y);
        if (cameraRotation >= 360)
            cameraRotation -= 360;
        if (cameraRotation < 0)
            cameraRotation += 360;
    }

}

