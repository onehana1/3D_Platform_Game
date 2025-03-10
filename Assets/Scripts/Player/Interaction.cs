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


    private Vector3 rayStart;  
    private Vector3 rayEnd;  
    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
            RaycastHit hit;

            Vector3 rayDirection = ray.direction; // 기본 방향            

            rayStart = ray.origin;
            rayEnd = ray.origin + rayDirection * maxCheckDistance;

            if (Physics.Raycast(ray.origin, rayDirection, out hit, maxCheckDistance, layerMask))
            {
                curInteractGameObject = hit.collider.gameObject;
                curInteractable = hit.collider.GetComponent<IInteractable>();
                SetPromptText();
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

        Gizmos.color = (curInteractable != null) ? Color.green : Color.red;
        Gizmos.DrawLine(rayStart, rayEnd);

        if (curInteractable != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(rayEnd, 0.05f);
        }
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