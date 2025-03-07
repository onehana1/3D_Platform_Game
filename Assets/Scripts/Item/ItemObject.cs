using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        InventoryManager.Instance.AddItem(data); // �κ��丮�� �߰�
        Destroy(gameObject);
    }
}