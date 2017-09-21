using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InterestedCamera : MonoBehaviour {

    private HashSet<PointOfInterest> focusedPointsOfInterest = new HashSet<PointOfInterest>();
    private float currentZoom = 20;
    private Vector3 preferedDirection;
    private Vector3 preferedPosition;

    public bool lockedPosition = false;

    private PointOfInterest mainPointOfInterest {
        get {
            if (focusedPointsOfInterest.Count == 0)
                return null;
            return focusedPointsOfInterest.OrderByDescending(x => x.attractionWeight).First();
        }
    }

    public new Camera camera {
        get { return GetComponent<Camera>(); }
    }
    public float secondsSinceLastInput;

    // Use this for initialization
    void Start() {
        focusedPointsOfInterest = new HashSet<PointOfInterest>();
        preferedDirection = new Vector3() + transform.eulerAngles;
        preferedPosition = new Vector3() + transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (focusedPointsOfInterest.Count > 0)
            follow();
    }

    void LateUpdate() {

    }

    void OnGUI() {
        int poiCount = 0;
        foreach (PointOfInterest poi in focusedPointsOfInterest) {
            if (poi.isPositionInScreen)
                poiCount++;
        }
        GUI.Label(new Rect(10,10,100,20), ""+poiCount);
    }

    public Vector3 getCumulatedInterestPosition() {
        Vector3 output = new Vector3(0, 0, 0);
        float totalWeight = 0;
        foreach (PointOfInterest poi in focusedPointsOfInterest) {
            output += poi.transform.position * poi.attractionWeight;
            totalWeight += poi.attractionWeight;
        }
        return output / totalWeight;
    }

    public Vector3 getCumulatedDirectionFromPosition(Vector3 position) {
        Vector3 output = new Vector3(0, 0, 0);
        float totalWeight = 0;
        Vector3 cameraDirection = camera.transform.forward;
        foreach (PointOfInterest poi in focusedPointsOfInterest) {
            Vector3 targetDirection = (poi.transform.position - position).normalized;
            Vector3 screenDirection = screenPositionToDirection(poi.interestFavoriteScreenPosition);
            targetDirection = ((targetDirection + targetDirection) / 2).normalized;
            output += targetDirection * poi.attractionWeight;
            totalWeight += poi.attractionWeight;
            float dotscreen = Vector3.Dot(targetDirection, screenDirection);
            float dotCamera = Vector3.Dot(targetDirection, cameraDirection);
            //if(dotscreen < 0.8 && dotCamera < 0.8)
            //currentZoom += dotscreen - dotCamera;
        }
        return (output / totalWeight).normalized;
    }

    public void follow() {
        if (lockedPosition)
            followLocked();
        else
            followUnlocked();
    }

    public void followLocked() {
        Vector3 cumulatedPosition = getCumulatedInterestPosition();
        Vector3 cumulatedDirection = (cumulatedPosition - transform.position).normalized;
        Quaternion cumulatedRotation = Quaternion.LookRotation(cumulatedDirection, Vector3.up);
        transform.position = preferedPosition;
        transform.rotation = Quaternion.Slerp(transform.rotation, cumulatedRotation, 0.05f);
    }

    public void followUnlocked() {
        Vector3 cumulatedPosition = getCumulatedInterestPosition();
        Vector3 cumulatedDirection = getCumulatedDirectionFromPosition(cumulatedPosition);
        Quaternion cumulatedRotation = Quaternion.LookRotation(cumulatedDirection, Vector3.up);
        transform.position = (cumulatedPosition + preferedPosition) / 2f;
        transform.rotation = Quaternion.Slerp(transform.rotation, cumulatedRotation, 0.05f);
        zoomOut(currentZoom);
    }

    public void zoomIn(float factor) {
        transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward * factor, 0.85f);
    }

    public void zoomOut(float factor) {
        transform.position = Vector3.Lerp(transform.position, transform.position - transform.forward * factor, 0.85f);
    }

    public Quaternion rotationToPointOfInterest(PointOfInterest poi) {
        Vector3 targetDirection = poi.transform.position - transform.position;
        targetDirection = targetDirection.normalized;
        Quaternion targetRotation = Quaternion.LookRotation(((targetDirection + screenPositionToDirection(poi.interestFavoriteScreenPosition)) / 2).normalized, Vector3.up);
        return targetRotation;
    }


    public bool canFocusPointOfInterest(PointOfInterest poi) {
        if (!poi.isPositionInScreen)
            return false;
        return true;
    }

    public bool registerPointOfInterest(PointOfInterest poi) {
        if (focusedPointsOfInterest.Contains(poi))
            return true;
        if (!canFocusPointOfInterest(poi))
            return false;
        if (!focusedPointsOfInterest.Add(poi))
            return false;
        return true;
    }

    public bool maintainPointOfInterest(PointOfInterest poi) {
        if (canFocusPointOfInterest(poi))
            return true;
        focusedPointsOfInterest.Remove(poi);
        return false;
    }

    public Vector3 screenPositionToDirection(ScreenPosition position) {
        return screenPointDirection(screenPositionToScreenCoordonate(position));
    }

    public Vector3 screenPointDirection(float x, float y) {
        Vector3 direction = camera.ViewportToWorldPoint(new Vector3(x, y, 1)) - camera.transform.position;
        return direction.normalized;
    }

    public Vector3 screenPointDirection(Vector2 point) {
        return screenPointDirection(point.x, point.y);
    }

    public static Vector2 screenPositionToScreenCoordonate(ScreenPosition position) {
        return CameraManager.screenPositionToScreenCoordonate(position);
    }
}
