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

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 200f;
    private float rotateInput;

    [Header("Look")]
    [SerializeField] Transform cameraContainer;   // 카메라 포함된 오브젝트
    [SerializeField] float minXLook;  // 캠 회전 제한
    [SerializeField] float maxXLook;  // 캠 회전 제한
    private float camCurXRot;   // 카메라 회전 각도
    private float camCurYRot = 0f;

    [SerializeField] float lookSensitivity;   // 마우스 감도

    private Vector2 mouseDelta; // 마우스 입력값

    [HideInInspector]
    [SerializeField] bool canLook = true;

    private Rigidbody _rigidbody;

    private PlayerAnimationController animController;

    public Action inventory;

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
        Rotate();
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

    public void OnRotateInput(InputAction.CallbackContext context)
    {
        rotateInput = context.ReadValue<float>();
    }

    private void Move()
    {
        float targetMoveSpeed = isRunning ? moveSpeed * 2f : moveSpeed;

        if (curMovementInput == Vector2.zero)
        {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
            animController.SetMoveDirection(0, 0, 0);
            return;
        }

        Vector3 moveDirection = (transform.forward * curMovementInput.y) + (transform.right * curMovementInput.x);
        moveDirection.Normalize();

        // 이동 속도 적용
        Vector3 moveVelocity = moveDirection * targetMoveSpeed;
        moveVelocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, moveVelocity, Time.deltaTime * decelerationSpeed);

        // 애니메이션 블렌드 트리 적용 (좌우 이동 추가)
        float speedMagnitude = _rigidbody.velocity.magnitude / (moveSpeed * 2f);
        float moveSpeedValue = Mathf.Lerp(animController.GetMoveSpeed(), speedMagnitude, Time.deltaTime * 10f);
        animController.SetMoveDirection(curMovementInput.x, curMovementInput.y, moveSpeedValue); // **좌우 이동 반영**
    }


    private void Rotate()
    {
        if (rotateInput != 0)
        {
            transform.Rotate(Vector3.up, rotateInput * rotationSpeed * Time.deltaTime);
        }
    }


    void CameraLook()
    {
        float mouseX = mouseDelta.x * lookSensitivity;
        float mouseY = mouseDelta.y * lookSensitivity;

        camCurXRot -= mouseY; // 위아래 반전
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); // 상하 회전 제한

        camCurYRot += mouseX;

        cameraContainer.rotation = Quaternion.Euler(camCurXRot, camCurYRot, 0);
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


    public void OnInventoryButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

}
