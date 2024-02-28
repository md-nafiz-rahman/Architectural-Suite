using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public InventoryManager inventoryManager; 
    private FurnitureItem selectedItem; 

    void Update()
    {
        if (selectedItem != null && Input.GetKeyDown(KeyCode.F)) 
        {
            var itemPlacementHandler = GetComponent<ItemPlacementHandler>();
            if (itemPlacementHandler != null)
            {
                itemPlacementHandler.PlaceItem();
                inventoryManager.RemoveFurnitureFromInventory(selectedItem);
                selectedItem = null;
            }
        }
    }


    public void SelectItemForPlacement(FurnitureItem item)
    {
        var itemPlacementHandler = FindObjectOfType<ItemPlacementHandler>();
        selectedItem = item;
        itemPlacementHandler.BeginPlacement(item.prefab);
        inventoryManager.HideInventoryUI();
    }

}
