using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMouseInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textBox;

    private void OnMouseEnter()
    {
        textBox.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        textBox.gameObject.SetActive(false);

    }
}
