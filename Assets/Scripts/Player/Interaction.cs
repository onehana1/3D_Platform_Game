using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera camera;


    private Vector3 rayStart;  // Ray ������ ����
    private Vector3 rayEnd;    // Ray ���� ����
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            // ����� �ð�ȭ�� ���� Ray ��ġ ����
            rayStart = ray.origin;
            rayEnd = ray.origin + ray.direction * maxCheckDistance;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                curInteractGameObject = hit.collider.gameObject;
                curInteractable = hit.collider.GetComponent<IInteractable>();
                SetPromptText();

                // ���� �浹 ������ Gizmo�� �ݿ�
                rayEnd = hit.point;
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (camera == null) return;

        Gizmos.color = Color.red;  // Ray�� ���������� ǥ��
        Gizmos.DrawLine(rayStart, rayEnd);  // Ray ���� �׸���
        Gizmos.DrawSphere(rayEnd, 0.05f);  // Ray ������ ���� ��(Sphere) ǥ��
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}