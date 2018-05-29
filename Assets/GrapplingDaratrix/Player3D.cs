using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Player3D : MonoBehaviour {

    public float maxGrapplingRange = 10; // distance to detect a grappling spot
    public float minGrapplingAngle = 0.8f; // angle to highlight a grappling spot

    public bool needLineOfSight = false;

    private HashSet<GrapplingSpot> spotsInRange = new HashSet<GrapplingSpot>();
    private GrapplingSpot currentHighlightSpot = null;

    private Rigidbody body;
    
    void Start() {
        body = GetComponent<Rigidbody>();
    }
    
    void Update() {
        UpdateGrapplingSpotInRange();
        if (!Input.GetMouseButton(0) && !Input.GetKey(KeyCode.Space))
            UpdateGrapplingSpotHighlight();
        UpdateInput();
    }

    void UpdateGrapplingSpotInRange() {
        foreach (GrapplingSpot spot in GrapplingSpot.spots) {
            bool inRange = Vector3.Distance(spot.transform.position, transform.position) < maxGrapplingRange;
            if (spot.isInRange != inRange) {
                spot.isInRange = inRange;
                if (spot.isInRange) {
                    spotsInRange.Add(spot);
                } else {
                    spotsInRange.Remove(spot);
                    if (currentHighlightSpot == spot) {
                        currentHighlightSpot.isHighlighted = false;
                        currentHighlightSpot = null;
                    }
                }
            }
        }
    }

    void UpdateGrapplingSpotHighlight() {
        Vector3 aimDirection = GetGrapplingAimDirection();
        GrapplingSpot newHighlight = null;
        if (spotsInRange.Count > 0)
            newHighlight = spotsInRange
                .Where(spot => !needLineOfSight || !Physics.Raycast(transform.position, spot.transform.position - transform.position
                    , (spot.transform.position - transform.position).magnitude, 1 << 10))
                .OrderByDescending(spot => Vector3.Dot(aimDirection, (spot.transform.position - transform.position).normalized))
                .DefaultIfEmpty(null)
                .First();
        if (newHighlight == null || Vector3.Dot(aimDirection, (newHighlight.transform.position - transform.position).normalized) < minGrapplingAngle) {
            if (currentHighlightSpot != null) {
                currentHighlightSpot.isHighlighted = false;
                currentHighlightSpot = null;
            }
        } else if (newHighlight != currentHighlightSpot) {
            if (currentHighlightSpot != null) {
                currentHighlightSpot.isHighlighted = false;
            }
            currentHighlightSpot = newHighlight;
            currentHighlightSpot.isHighlighted = true;
        }
    }

    void UpdateInput() {
        if (currentHighlightSpot != null && (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))) {
            float time = Time.deltaTime;
            Vector3 dir = currentHighlightSpot.transform.position - transform.position;
            body.AddForce(dir * time * 200 + Vector3.Scale(dir, Vector3.up) * time * 50, ForceMode.Acceleration);
        } else {
            float time = Time.deltaTime;
            Vector3 forward = Camera.main.transform.forward;
            forward = Vector3.Scale(forward, new Vector3(1, 0, 1)).normalized;
            Vector3 right = Camera.main.transform.right;
            right = Vector3.Scale(right, new Vector3(1, 0, 1)).normalized;
            if (Input.GetKey(KeyCode.UpArrow))
                body.AddForce(forward * time * 500 + Vector3.up * time * 5, ForceMode.Acceleration);
            if (Input.GetKey(KeyCode.DownArrow))
                body.AddForce(-forward * time * 500 + Vector3.up * time * 5, ForceMode.Acceleration);
            if (Input.GetKey(KeyCode.RightArrow))
                body.AddForce(right * time * 500 + Vector3.up * time * 5, ForceMode.Acceleration);
            if (Input.GetKey(KeyCode.LeftArrow))
                body.AddForce(-right * time * 500 + Vector3.up * time * 5, ForceMode.Acceleration);
        }
    }

    Vector3 GetGrapplingAimDirection() {
        return Camera.main.transform.forward;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxGrapplingRange);
        foreach (GrapplingSpot spot in spotsInRange) {
            if (spot.isHighlighted) {
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
                    Gizmos.color = Color.blue;
                else
                    Gizmos.color = Color.green;
            } else
                Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, spot.transform.position);
        }
    }
}
