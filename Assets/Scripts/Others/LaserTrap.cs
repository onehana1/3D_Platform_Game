using System.Collections;
using UnityEngine;

public class LaserTrap : MonoBehaviour
{
    [Header("Laser Settings")]
    [SerializeField] private Transform laserOrigin;
    [SerializeField] private Transform laserEnd; 
    [SerializeField] private LayerMask playerLayer; 
    [SerializeField] private LineRenderer laserRenderer;

    [Header("Trap Settings")]
    [SerializeField] private GameObject fallingTrap;
    [SerializeField] private Transform targetPosition;
    [SerializeField] private float fallDelay = 0.5f;
    [SerializeField] private float resetTime = 3f; // 다시 시작
    [SerializeField] private int maxTrapCount = 5;
    [SerializeField] private float trapSpawnInterval = 1.0f;


    private bool isTrapActivated = false;

    private void Update()
    {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        Vector3 direction = (laserEnd.position - laserOrigin.position).normalized;
        float distance = Vector3.Distance(laserOrigin.position, laserEnd.position);


        if (Physics.Raycast(laserOrigin.position, direction, out RaycastHit hit, distance, playerLayer))
        {
            if (!isTrapActivated)
            {
                Debug.Log("잡앗다!!!!!!");
                isTrapActivated = true;
                StartCoroutine(ActivateTrap());
            }
        }

        // LineRenderer 해보기
        if (laserRenderer != null)
        {
            laserRenderer.SetPosition(0, laserOrigin.position);
            laserRenderer.SetPosition(1, laserEnd.position);
        }
    }

    private IEnumerator ActivateTrap()
    {



        for (int i = 0; i < maxTrapCount; i++) 
        {
            Vector3 spawnPos = new Vector3(targetPosition.position.x, targetPosition.position.y + 2.0f, targetPosition.position.z);

            GameObject trap = Instantiate(fallingTrap, spawnPos, Quaternion.identity);
            Rigidbody rb = trap.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.useGravity = true;
            }

            yield return new WaitForSeconds(trapSpawnInterval);
        }

        yield return new WaitForSeconds(resetTime);
        isTrapActivated = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(laserOrigin.position, laserEnd.position);
    }
}
