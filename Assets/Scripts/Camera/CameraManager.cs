using System.Collections;
using UnityEngine;
public class CameraManager : MonoBehaviour {


    public InterestedCamera defaultCamera;
    private InterestedCamera _currentCamera;

    private void Start() {
        singleton = this;
    }

    private static CameraManager singleton;
    public static InterestedCamera currentCamera {
        get {
            if (singleton._currentCamera == null)
                return singleton.defaultCamera;
            return singleton._currentCamera;
        }
    }

    public static Vector2 screenPositionToScreenCoordonate(ScreenPosition position) {
        switch(position) {
            case ScreenPosition.upperLeft:
                return new Vector2(2/3f,1/3f);
            case ScreenPosition.lowerLeft:
                return new Vector2(2 / 3f, 2 / 3f);
            case ScreenPosition.upperRight:
                return new Vector2(1 / 3f, 1 / 3f);
            case ScreenPosition.lowerRight:
                return new Vector2(1 / 3f, 2 / 3f);
            case ScreenPosition.centerLeft:
                return new Vector2(2 / 3f, 0.5f);
            case ScreenPosition.centerRight:
                return new Vector2(1 / 3f, 0.5f);
            case ScreenPosition.centerUp:
                return new Vector2(0.5f, 1 / 3f);
            case ScreenPosition.centerLow:
                return new Vector2(0.5f, 2 / 3f);
        }
        return new Vector2(0.5f,0.5f); // absolute center
    }

    public static bool isViewportPointValid(Vector3 point) {
        return point.x >= 0 && point.x <= 1
            && point.y >= 0 && point.y <= 1;
    }

    public static bool isPointInFrontOfCamera(Vector3 point) {
        Vector3 dir = (point - currentCamera.transform.position).normalized;
        return Vector3.Dot(dir, currentCamera.transform.forward) >= 0.5f;
    }

    public static bool isPointInScreen(Vector3 point) {
        return isViewportPointValid(currentCamera.camera.WorldToViewportPoint(point))
            && isPointInFrontOfCamera(point);
    }
}
