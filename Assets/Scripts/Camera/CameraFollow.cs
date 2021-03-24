using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Intended to use with player
public class CameraFollow : MonoSingleton<CameraFollow>
{
    [SerializeField]
    private float flipOffset = 0.6f;
    [SerializeField]
    private float normalSmoothSpeed = 0.125f;
    [SerializeField]
    private float fallingSmoothSpeed = 0.375f;
    private Vector3 desiredPosition = Vector3.zero;
    private Transform followTransform;

    private void Start()
    {
        EventPublisher.PlayerSpawn += AssignTransform;
    }

    private void OnDestroy()
    {
        EventPublisher.PlayerSpawn -= AssignTransform;
    }

    private void AssignTransform(Transform player)
    {
        followTransform = player;
    }

    private void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        // Check first if player is referenced
        if (followTransform == null)
        {
            followTransform = PlayerFinder.FindByTag().transform;
        }

        Transform mainCamera = Camera.main.transform;

        float x = followTransform.position.x;
        float y = followTransform.position.y;
        float z = mainCamera.position.z;

        MovementState turnDirection = PlayerMovement.Instance.TurnDirection;
        switch (turnDirection)
        {
            case MovementState.Left:
                // Player is looking left, camera should pan to left a bit
                x -= flipOffset;
                break;
            case MovementState.Right:
                // Player is looking right, camera should pan to right a bit
                x += flipOffset;
                break;
            default:
                throw new System.ArgumentException("Unhandled enum MovementState");
        }

        // The position camera wants to move to, namely player's position

        // If the camera is chasing falling player, use falling smooth speed
        float smoothSpeedY = mainCamera.position.y > y ? fallingSmoothSpeed : normalSmoothSpeed;

        // Interpolated position
        float interpolatedX = Mathf.Lerp(mainCamera.position.x, x, normalSmoothSpeed);
        float interpolatedY = Mathf.Lerp(mainCamera.position.y, y, smoothSpeedY);
        Vector3 smoothedPosition = new Vector3(interpolatedX, interpolatedY, z);

        mainCamera.position = smoothedPosition;
    }
}
