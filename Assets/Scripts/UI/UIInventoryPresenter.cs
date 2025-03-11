using System.Collections.Generic;
using UnityEngine;

public class UIInventoryPresenter
{
    private UIInventoryView view;
    private InventoryModel model;
    private PlayerController playerController;

    public UIInventoryPresenter(UIInventoryView inventoryView, PlayerController playerController, InventoryModel inventoryModel)
    {
        view = inventoryView;
        this.playerController = playerController;
        model = inventoryModel;

        playerController.onInventoryToggle += ToggleInventory;
        model.OnInventoryUpdated += UpdateView;
        view.OnItemClicked += HandleItemClicked;
    }

    public void AddItem(ItemData item)
    {
        model.AddItem(item);
        UpdateView();
    }

    private void HandleItemClicked(int index)
    {
        if (index < 0 || index >= model.GetInventory().Count) return;

        ItemSlotData selectedItem = model.GetInventory()[index];
        Debug.Log($"아이템 선택: {selectedItem.item.displayName}");
    }

    private void UpdateView()
    {
        view.UpdateInventoryUI(model.GetInventory());
    }

    private void ToggleInventory()
    {
        view.ToggleInventory();
    }
}
