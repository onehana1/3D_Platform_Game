using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData item;

    public UIInventory inventory;
    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    private Outline outline;

    public int index;
    public bool equipped;
    public int quantity;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    public void SetOutline(bool state)
    {
        if (outline != null)
        {
            outline.enabled = state;
        }
    }

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;

        if (outline != null)
        {
            outline.enabled = equipped;
        }

        if (quantityText == null)return;

        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;


    }

    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        if(quantityText == null)return;
        quantityText.text = string.Empty;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inventory == null || item == null)
        {
            inventory?.HideItemTooltip();
            return;
        }

        inventory.ShowItemTooltip(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (inventory != null)
        {
            inventory.HideItemTooltip();
        }
    }
    public void OnClickButton() // 버튼 클릭
    {
        if (inventory == null)
        {
            return;
        }

        if (item == null)
        {
            return;
        }

        inventory.SelectItem(index);
    }
}

