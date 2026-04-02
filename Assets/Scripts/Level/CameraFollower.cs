using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new(0f, 0f, -10f);

    public Vector3 ShakeOffset { get; set; }

    private void LateUpdate()
    {
        Transform followTarget = target != null ? target : PlayerRegistry.Player;
        if (followTarget == null)
        {
            return;
        }

        Vector3 desiredPosition = followTarget.position + offset + ShakeOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
