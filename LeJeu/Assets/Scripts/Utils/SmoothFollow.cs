using UnityEngine;

public class SmoothFollowAndLook : MonoBehaviour
{
    [Header("Suivi")]
    public Transform followTarget;
    public float smoothSpeed;  
    public Vector3 offset;               

    [Header("Rotation")]
    public Transform lookTarget;         
    public float rotationSpeed = 5f;     

    void LateUpdate()
    {
        // --- Mouvement ---
        if (followTarget != null)
        {
            Vector3 desiredPosition = followTarget.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }

        // --- Rotation ---
        if (lookTarget != null)
        {
            Vector3 direction = lookTarget.position - transform.position;
            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180f, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
