using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform cameraContainer;

    [SerializeField] private Vector3 offset = new Vector3(0, 7f, -10f);
    [SerializeField] private Vector3 defaultRotation = new Vector3(30f, 0f, 0f);
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float smoothTime = 0.1f;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        transform.position = offset;
        transform.rotation = Quaternion.Euler(defaultRotation);
    }

    private void FixedUpdate() 
    {
        if (player == null) return;

        Vector3 targetPosition = player.position;

        cameraContainer.position = Vector3.Lerp(cameraContainer.position, targetPosition, Time.fixedDeltaTime * followSpeed);
    }
}
