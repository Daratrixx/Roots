using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {

    public float maxGrapplingRange = 10; // distance to detect a grappling spot
    public float minGrapplingAngle = 0.8f; // angle to highlight a grappling spot

    private HashSet<GrapplingSpot> spotsInRange = new HashSet<GrapplingSpot>();
    private GrapplingSpot currentHighlightSpot = null;

    private Rigidbody body;
    
    void Start() {
        body = GetComponent<Rigidbody>();
    }


    void Update() {
        UpdateGrapplingSpotInRange();
        if (!Input.GetMouseButton(0))
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
        Vector3 vectorDirection = GetCursorDirection();
        GrapplingSpot newHighlight = null;
        if (spotsInRange.Count > 0)
            newHighlight = spotsInRange
                .OrderByDescending(spot => Vector3.Dot(vectorDirection, (spot.transform.position - transform.position).normalized))
                .DefaultIfEmpty(null)
                .First();
        if (newHighlight == null || Vector3.Dot(vectorDirection, (newHighlight.transform.position - transform.position).normalized) < minGrapplingAngle) {
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
        if (currentHighlightSpot != null && Input.GetMouseButton(0)) {
            float time = Time.deltaTime;
            Vector3 dir = currentHighlightSpot.transform.position - transform.position;
            body.AddForce(dir * time * 250 + Vector3.Scale(dir, Vector3.up) * time * 30, ForceMode.Acceleration);
        } else {
            float time = Time.deltaTime;
            if (Input.GetKey(KeyCode.LeftArrow))
                body.AddForce(Vector3.left * time * 100 + Vector3.up * time * 30, ForceMode.Acceleration);
            if (Input.GetKey(KeyCode.RightArrow))
                body.AddForce(Vector3.right * time * 100 + Vector3.up * time * 30, ForceMode.Acceleration);
        }
    }

    Vector3 GetCursorDirection() {
        return (GetCursorPosition() - transform.position).normalized;
    }

    Vector3 GetCursorPosition() {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        return mouseWorldPosition;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + GetCursorDirection() * 2);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxGrapplingRange);
        foreach (GrapplingSpot spot in spotsInRange) {
            if (spot.isHighlighted) {
                if (Input.GetMouseButton(0))
                    Gizmos.color = Color.blue;
                else
                    Gizmos.color = Color.green;
            } else
                Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, spot.transform.position);
        }
    }
}
