using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public bool followAngle = true;
    public float slerpFactorAngle = 0.15f;
    public bool followDistance = true;
    public float slerpFactorDistance = 0.15f;
    public float minDistance = 10;
    public float maxDistance = 15;
    private Vector3 defaultDistance = new Vector3(0,0,0);

    private new Camera camera {
        get { return GetComponent<Camera>(); }
    }


    // Use this for initialization
    void Start() {
        defaultDistance = (target.position - transform.position).normalized;
    }

    // Update is called once per frame
    void LateUpdate() {
        follow(target);
    }

    public void follow(Transform target) {
        if (followAngle) {
            Quaternion rotation = rotationToTarget(target);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, slerpFactorAngle);
        }
        if (followDistance) {
            Vector3 distance = distanceToTarget(target);
            transform.position = Vector3.Slerp(transform.position, target.position - distance, slerpFactorDistance);
        }
    }

    public Quaternion rotationToTarget(Transform target) {
        Vector3 targetDirection = target.position - transform.position;
        targetDirection = targetDirection.normalized;
        Quaternion targetRotation = Quaternion.LookRotation(((targetDirection + lowerLeftDirection) / 3).normalized, Vector3.up);
        return targetRotation;
    }

    public Vector3 distanceToTarget(Transform target) {
        Vector3 targetDirection = target.position - transform.position;
        float targetDistance = targetDirection.magnitude;
        targetDirection = targetDirection.normalized;
        if (targetDistance > maxDistance)
            return Vector3.Slerp(defaultDistance * maxDistance, targetDirection * maxDistance, 0.5f);
        if (targetDistance < minDistance)
            return Vector3.Slerp(defaultDistance * minDistance, targetDirection * minDistance, 0.5f);
        return Vector3.Slerp(defaultDistance * targetDistance, targetDirection * targetDistance, 0.5f);
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
