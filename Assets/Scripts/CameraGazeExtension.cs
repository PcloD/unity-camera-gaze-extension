using UnityEngine;

//
// Summary:
//     ///
//     Provides convenience methods for common facing or 'gaze' related camera functionality.
//     ///
public static class CameraGazeExtension {

    //
    // Summary:
    //     ///
    //     A simple high level test that determines if an object is potentially within the view of the camera.
    //     NOTE: This doesn't take into account any culling or occlusion that may mean the object is ultimately not visible to the user.
    //     ///
    //
    // Parameters:
    //   gameObject:
    //     The game object to check for being within the cameras view.
    //  
    public static bool IsWithinView(this Camera camera, GameObject gameObject) {
        Vector3 viewportPoint = camera.WorldToViewportPoint(gameObject.transform.position);

        // Point behind camera? If so it is not in view.
        if (viewportPoint.z < 0) {
            return false;
        }

        // Point within screen space?  If so it must be in view.
        if ((viewportPoint.x > 0 && viewportPoint.x < 1) ||
            (viewportPoint.y > 0 && viewportPoint.y < 1)) {
            return true;
        }

        // Not within view.
        return false;
    }

    //
    // Summary:
    //     ///
    //     Obtains the direction vector the camera is currently facing in world space
    //     ///
    public static Vector3 FacingDirection(this Camera camera) {
        // Project a point onto the near clip plane using the centre of the screen space
        Vector3 screenCentredPoint = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, camera.nearClipPlane));

        // Calculate the direction vector
        Vector3 direction = screenCentredPoint - camera.transform.position;
        direction.Normalize();
        return direction;
    }

    //
    // Summary:
    //     ///
    //     Obtains the direction vector from the camera to an object
    //     ///
    //
    // Parameters:
    //   gameObject:
    //     The game object to determine the direction to.
    //  
    public static Vector3 DirectionToObject(this Camera camera, GameObject gameObject) {
        return camera.DirectionToPosition(gameObject.transform.position);
    }

    //
    // Summary:
    //     ///
    //     Obtains the direction vector from the camera to a position
    //     ///
    //
    // Parameters:
    //   position:
    //     The point to determine the direction to.
    //  
    public static Vector3 DirectionToPosition(this Camera camera, Vector3 position) {
        Vector3 direction = position - camera.transform.position;
        direction.Normalize();
        return direction;
    }


    //
    // Summary:
    //     ///
    //     Determines if an object is within a cone of visibility of the cameras center point.
    //     NOTE: This doesn't take into account any culling or occlusion that may mean the object is ultimately not visible to the user.
    //     ///
    //
    // Parameters:
    //   gameObject:
    //     The game object to check for being within the cameras view.
    //
    //   angleInDegrees
    //     The maximum angle, in degrees, within which the camera would be considered to be facing the specified object
    //  
    public static bool IsFacingObject(this Camera camera, GameObject gameObject, float angleInDegrees) {
        return camera.IsFacingPosition(gameObject.transform.position, angleInDegrees);
    }

    //
    // Summary:
    //     ///
    //     Determines if a point is within a cone of visibility of the cameras center point.
    //     NOTE: This doesn't take into account any culling or occlusion that may mean the object is ultimately not visible to the user.
    //     ///
    //
    // Parameters:
    //   position:
    //     The game object to check for being within the cameras view.
    //
    //   angleInDegrees
    //     The maximum angle, in degrees, within which the camera would be considered to be facing the specified position
    //  
    public static bool IsFacingPosition(this Camera camera, Vector3 position, float angleInDegrees) {
        Vector3 cameraDirection = camera.FacingDirection();

        Vector3 positionToCameraDirection = camera.DirectionToPosition(position);

        float dot = Vector3.Dot(cameraDirection, positionToCameraDirection);
        float angleBetweenDirections = Mathf.Abs(Mathf.Acos(dot) * Mathf.Rad2Deg);

        //Debug.Log("Camera angle between directions = " + cameraDirectionAngle);
        return angleBetweenDirections < angleInDegrees;
    }

}
