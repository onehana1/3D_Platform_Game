using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;

    [Header("Resource Gathering")]
    public bool doesGatherResources;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;
    public float useStamina;

    public Transform playerPos;

    [SerializeField] private float gatherRadius = 1.5f; 
    [SerializeField] private LayerMask resourceLayer;

    void Start()
    {
        playerPos = GameObject.FindWithTag("Player").transform;
    }

    public void OnHit()
    {

        Vector3 hitPosition = transform.position;

        Collider[] hitColliders = Physics.OverlapSphere(hitPosition, gatherRadius, resourceLayer);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out Resource resource))
            {
                resource.Gather(hitCollider.transform.position, Vector3.up);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, gatherRadius);
    }

    public float GetNecessaryStamina()
    {
        return useStamina;
    }
}

