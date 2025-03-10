using UnityEngine;
using System.Collections;

public class LaunchPad : MonoBehaviour
{
    [Header("Launch Settings")]
    [SerializeField] private float launchForce = 500f;  // �߻� ��
    [SerializeField] private Vector3 launchDirection = new Vector3(1, 1, 0); // �߻� ����
    [SerializeField] private float delayBeforeLaunch = 2f; // �ڵ� �߻� ������

    private bool isPlayerOnPad = false;
    private Rigidbody playerRb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� �Ӵ�");
            isPlayerOnPad = true;
            playerRb = other.GetComponent<Rigidbody>();

            // ���� �ð��� ������ �ڵ� �߻�
            StartCoroutine(AutoLaunchAfterDelay());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPad = false;
            playerRb = null;
        }
    }

    private IEnumerator AutoLaunchAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeLaunch);

        if (isPlayerOnPad && playerRb != null)
        {
            LaunchPlayer();
        }
    }

    private void Update()
    {
        if (isPlayerOnPad && Input.GetKeyDown(KeyCode.F))
        {
            LaunchPlayer();
        }
    }

    private void LaunchPlayer()
    {
        if (playerRb == null) return;

        Debug.Log("�÷��̾� �߻�!");
        playerRb.velocity = Vector3.zero; // ���� �ӵ� �ʱ�ȭ
        playerRb.AddForce(launchDirection.normalized * launchForce, ForceMode.Impulse);

        isPlayerOnPad = false; // �߻� �� ���� �ʱ�ȭ
        playerRb = null;
    }
}
