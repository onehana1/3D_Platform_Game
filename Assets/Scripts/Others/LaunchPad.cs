using UnityEngine;
using System.Collections;

public class LaunchPad : MonoBehaviour
{
    [Header("Launch Settings")]
    [SerializeField] private float launchForce = 500f;  // 발사 힘
    [SerializeField] private Vector3 launchDirection = new Vector3(1, 1, 0); // 발사 방향
    [SerializeField] private float delayBeforeLaunch = 2f; // 자동 발사 딜레이

    private bool isPlayerOnPad = false;
    private Rigidbody playerRb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 왓다");
            isPlayerOnPad = true;
            playerRb = other.GetComponent<Rigidbody>();

            // 일정 시간이 지나면 자동 발사
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

        Debug.Log("플레이어 발사!");
        playerRb.velocity = Vector3.zero; // 기존 속도 초기화
        playerRb.AddForce(launchDirection.normalized * launchForce, ForceMode.Impulse);

        isPlayerOnPad = false; // 발사 후 상태 초기화
        playerRb = null;
    }
}
