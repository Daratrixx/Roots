using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour {

    public InterestedCamera currentCamera {
        get { return CameraManager.currentCamera; }
    }

    public int attractionRange = 500; // camera must be close to the object to focuson it
    public float attractionWeight = 1;
    public int minCameraDistance = 10;
    public ScreenPosition interestFavoriteScreenPosition;

    public bool focusIfCameraIsInactive;
    public bool overwritePlayerInput;
    public bool overwriteOtherPointOfInterest;

    private bool isFocused;



	// Update is called once per frame
	void Update () {
        if (!isFocused)
            isFocused = currentCamera.registerPointOfInterest(this);
        else isFocused = currentCamera.maintainPointOfInterest(this);

    }

    public bool getFocusState() {
        return isFocused;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + Vector3.up, attractionWeight/2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward*2);
    }

    public bool isPositionInScreen {
        get {
            return CameraManager.isPointInScreen(transform.position);
        }
    }

}
