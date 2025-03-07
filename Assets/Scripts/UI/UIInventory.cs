using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public GameObject inventoryWindow;
    private PlayerController controller;

    private bool isOpen = false;

    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        controller.inventory += ToggleInventory;

        inventoryWindow.SetActive(false);
    }

    private void ToggleInventory()
    {
        if (inventoryWindow.activeSelf)
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }
}
