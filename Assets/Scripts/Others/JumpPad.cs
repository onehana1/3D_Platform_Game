using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f; // 점프대의 힘


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어와 충돌 확인
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // 기존 Y 속도 초기화
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 위쪽으로 힘 추가
            }
        }
    }
}
