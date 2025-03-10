using UnityEngine;

public class FallingTrap : MonoBehaviour
{
    [SerializeField] private float destroyAfterSeconds = 5f;
    [SerializeField] private int damageAmount = 20; // 플레이어에게 줄 데미지

    private void Start()
    {
        Destroy(gameObject, destroyAfterSeconds); // 일정 시간 후 제거
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"데미지먹이셈");

        }
    }
}
