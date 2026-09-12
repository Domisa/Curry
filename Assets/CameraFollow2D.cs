using UnityEngine;

// Setup:
// 1. Add this component to the Main Camera.
// 2. Assign the player GameObject to Target.
// 3. Set Follow Smoothness and the camera offsets in the Inspector.
// 4. The camera follows the player horizontally and moves upward as the player climbs.
//    It does not move back down when the player falls, which keeps higher areas visible.
public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSmoothness = 0.2f;
    [SerializeField] private Vector2 cameraOffset;

    private float highestTargetY;
    private Vector3 followVelocity;

    private void Start()
    {
        highestTargetY = transform.position.y - cameraOffset.y;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        highestTargetY = Mathf.Max(highestTargetY, target.position.y);

        Vector3 desiredPosition = new Vector3(
            target.position.x + cameraOffset.x,
            highestTargetY + cameraOffset.y,
            transform.position.z);

        float smoothTime = Mathf.Max(0.01f, followSmoothness);
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref followVelocity,
            smoothTime);
    }
}