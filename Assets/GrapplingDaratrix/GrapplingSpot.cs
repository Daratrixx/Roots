using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingSpot : MonoBehaviour {


    public static List<GrapplingSpot> spots = new List<GrapplingSpot>();

    public bool isHighlighted = false;
    public bool isInRange = false;
    
    void Start() {
        spots.Add(this);
    }

    private void OnDrawGizmos() {
        if (isHighlighted) {
            if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
                Gizmos.color = Color.blue;
            else
                Gizmos.color = Color.green;
        } else if (isInRange)
            Gizmos.color = Color.yellow;
        else
            Gizmos.color = Color.grey;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}
