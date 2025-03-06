using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f; // �������� ��


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) // �÷��̾�� �浹 Ȯ��
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // ���� Y �ӵ� �ʱ�ȭ
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // �������� �� �߰�
            }
        }
    }
}
