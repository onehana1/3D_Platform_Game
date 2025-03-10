using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform targetPosition;  // �̵��� ��ġ

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Teleport(other.transform));
        }
    }

    private IEnumerator Teleport(Transform obj)
    {
        yield return new WaitForSeconds(0.2f);

        obj.position = targetPosition.position;
        obj.rotation = targetPosition.rotation;
    }
}
