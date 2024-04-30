
// PlacementManager.cs handles the interaction of placing furniture within the scene, it coordinates between user input and the ItemPlacementHandler.

using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public InventoryManager inventoryManager;
    private FurnitureItem selectedItem;

    // Checks if an item is selected and the player presses the 'F' key to place it to begin placement process.
    void Update()
    {
        if (selectedItem != null && Input.GetKeyDown(KeyCode.F))
        {
            var itemPlacementHandler = GetComponent<ItemPlacementHandler>();
            if (itemPlacementHandler != null && itemPlacementHandler.isPlacing)
            {
                itemPlacementHandler.PlaceItem();
                inventoryManager.RemoveFurnitureFromInventory(selectedItem);
                selectedItem = null;
            }
        }
    }

    // Initialize the selected furniture for placement with ItemPlacementHandler.cs
    public void SelectItemForPlacement(FurnitureItem item)
    {
        var itemPlacementHandler = FindObjectOfType<ItemPlacementHandler>();
        if (itemPlacementHandler != null)
        {
            selectedItem = item;
            itemPlacementHandler.BeginPlacement(item.prefab, item.materialData);
            inventoryManager.HideInventoryUI();
        }
        Debug.Log("Selecting item for placement: " + item.name);

    }

}
