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

    [SerializeField] private LayerMask collisionMask;
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

        // 플레이어와 카메라 사이에 장애물이 있는지 레이캐스트로 확인ㅇㅇ
        Vector3 desiredCameraPos = cameraContainer.TransformPoint(offset);
        Vector3 direction = desiredCameraPos - player.position;
        float distance = offset.magnitude;

        if (Physics.Raycast(player.position, direction.normalized, out RaycastHit hit, distance, collisionMask))
        {
            // 장애물이 있으면 장애물 바로 앞에 카메라 세팅
            transform.position = hit.point + hit.normal * 0.2f;
        }
        else
        {
            // 뭐 없으면 다시 위치로 옴
            transform.position = Vector3.SmoothDamp(transform.position, desiredCameraPos, ref velocity, smoothTime);
        }
    }
}
