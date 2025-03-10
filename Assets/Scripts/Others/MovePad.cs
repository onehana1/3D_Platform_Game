using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovePadType { Horizontal, Vertical }
public class MovePad : MonoBehaviour
{
    [Header("Move Settings")]
    public MovePadType moveType = MovePadType.Horizontal; // �̵� Ÿ�� ����
    public float moveDistance = 3f; 
    public float moveSpeed = 2f; 
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;  // ���� ��ġ ����
    }

    void FixedUpdate()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveDistance * 2) - moveDistance;

        if (moveType == MovePadType.Horizontal)
            transform.position = startPos + new Vector3(offset, 0, 0);  // �¿� �̵�
        else
            transform.position = startPos + new Vector3(0, offset, 0);  // ���� �̵�
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("��Ѵ�");
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            other.transform.SetParent(null);
        }
    }

}
