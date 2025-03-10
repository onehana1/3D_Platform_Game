using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject infoPanelPrefab;
    private GameObject activeInfoPanel;
    private TextMeshProUGUI infoText;
    private Transform target;
    private bool isShowing = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
        if (isShowing && activeInfoPanel != null && target != null)
        {

            activeInfoPanel.transform.position = target.position + Vector3.up * 1.5f;
            activeInfoPanel.transform.LookAt(Camera.main.transform); 
        }
    }

    public void ShowInfo(string text, Transform objTransform)
    {
        if (activeInfoPanel == null)
        {
            activeInfoPanel = Instantiate(infoPanelPrefab, objTransform.position + Vector3.up * 1.5f, Quaternion.identity);
            infoText = activeInfoPanel.GetComponentInChildren<TextMeshProUGUI>();
        }

        target = objTransform;
        infoText.text = text;
        activeInfoPanel.SetActive(true);
        isShowing = true;
    }

    public void ShowTextBox(TextMeshProUGUI textBox, string text)
    {
        textBox.text = text;
        textBox.gameObject.SetActive(true);
    }

    public void HideTextBox(TextMeshProUGUI textBox)
    {
        textBox.gameObject.SetActive(false);
    }

    public void HideInfo()
    {
        if (activeInfoPanel != null)
        {
            activeInfoPanel.SetActive(false);
        }
        isShowing = false;
    }
}
