using UnityEngine;

public class FallingTrap : MonoBehaviour
{
    [SerializeField] private float destroyAfterSeconds = 5f;
    [SerializeField] private int damageAmount = 20; // �÷��̾�� �� ������

    private void Start()
    {
        Destroy(gameObject, destroyAfterSeconds); // ���� �ð� �� ����
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"���������̼�");

        }
    }
}
