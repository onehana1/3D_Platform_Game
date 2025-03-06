using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed; // 이속
    private Vector2 curMovementInput;   // 이동 입력
    [SerializeField] float jumpPower;     // 점프힘
    [SerializeField] LayerMask groundLayerMask;   // 바닥 감지 레이어
    [SerializeField] float decelerationSpeed = 5.0f;
    private bool isGrounded = false;
    private bool isRunning = false;
    [SerializeField] float currentSpeed = 0;

    [Header("Look")]
    [SerializeField] Transform cameraContainer;   // 카메라 포함된 오브젝트
    [SerializeField] float minXLook;  // 캠 회전 제한
    [SerializeField] float maxXLook;  // 캠 회전 제한
    private float camCurXRot;   // 카메라 회전 각도
    [SerializeField] float lookSensitivity;   // 마우스 감도

    private Vector2 mouseDelta; // 마우스 입력값

    [HideInInspector]
    [SerializeField] bool canLook = true;

    private Rigidbody _rigidbody;

    private PlayerAnimationController animController;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        animController = GetComponentInChildren<PlayerAnimationController>();
    }

    void Start()
    {
       // Cursor.lockState = CursorLockMode.Locked;   // 커서 잠금
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        animController.SetGrounded(isGrounded);
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)    // 입력 받음
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)    // 입력 초기화
        {
            curMovementInput = Vector2.zero;
            currentSpeed = 0;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            animController.TriggerJump();
        }
    }

    public void OnRunInput(InputAction.CallbackContext context)
    {
        isRunning = context.phase == InputActionPhase.Performed;
    }

    private void Move()
    {
        float targetMoveSpeed = isRunning ? moveSpeed * 2f : moveSpeed;

        if (curMovementInput == Vector2.zero)
        {
            _rigidbody.velocity = Vector3.zero;
            currentSpeed = targetMoveSpeed;
            animController.SetMoveDirection(0, 0, 0); // Idle 상태로 변경
            return;
        }

        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= currentSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, dir, Time.deltaTime * decelerationSpeed);

        float speedMagnitude = _rigidbody.velocity.magnitude / (moveSpeed * 2f);
        float moveSpeedValue = Mathf.Lerp(animController.GetMoveSpeed(), speedMagnitude, Time.deltaTime * 10f);

        animController.SetMoveDirection(curMovementInput.x, curMovementInput.y, moveSpeedValue);
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(transform.position + Vector3.down * 0.1f, 0.2f, groundLayerMask);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f); // 반투명한 빨간색
        Gizmos.DrawSphere(transform.position + Vector3.down * 0.1f, 0.2f);
    }


    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}