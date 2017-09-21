using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;

    private new Camera camera {
        get { return GetComponent<Camera>(); }
    }


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        follow(target);
    }

    public void follow(Transform target) {
        Quaternion rotation = rotationToTarget(target);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
    }

    public Quaternion rotationToTarget(Transform target) {
        Vector3 targetDirection = target.position - transform.position;
        targetDirection = targetDirection.normalized;
        Quaternion targetRotation = Quaternion.LookRotation(((targetDirection + lowerLeftDirection) / 3).normalized, Vector3.up);
        return targetRotation;
    }

    public Vector3 upperDirection {
        get {
            return screenPointDirection(0.5f, 1f / 3f);
        }
    }

    public Vector3 lowerDirection {
        get {
            return screenPointDirection(0.5f, 2f / 3f);
        }
    }

    public Vector3 leftDirection {
        get {
            return screenPointDirection(2f / 3f, 0.5f);
        }
    }

    public Vector3 rightDirection {
        get {
            return screenPointDirection(1f / 3f, 0.5f);
        }
    }

    public Vector3 upperLeftDirection {
        get {
            return screenPointDirection(2f / 3f, 1f / 3f);
        }
    }

    public Vector3 upperRightDirection {
        get {
            return screenPointDirection(1f / 3f, 1f / 3f);
        }
    }

    public Vector3 lowerLeftDirection {
        get {
            return screenPointDirection(2f / 3f, 2f / 3f);
        }
    }

    public Vector3 lowerRightDirection {
        get {
            return screenPointDirection(1f / 3f, 2f / 3f);
        }
    }

    public Vector3 screenPointDirection(float x, float y) {
        Vector3 direction = camera.ViewportToWorldPoint(new Vector3(x, y, 1)) - camera.transform.position;
        return direction.normalized;
    }

    public Vector3 screenPointPosition(float x, float y) {
        Vector3 direction = camera.ViewportToWorldPoint(new Vector3(x, y, camera.nearClipPlane * 2));
        return direction;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(screenPointPosition(1f / 3f, 1f / 3f), 0.01f);
        Gizmos.DrawSphere(screenPointPosition(1f / 3f, 2f / 3f), 0.01f);
        Gizmos.DrawSphere(screenPointPosition(2f / 3f, 1f / 3f), 0.01f);
        Gizmos.DrawSphere(screenPointPosition(2f / 3f, 2f / 3f), 0.01f);
    }
}
