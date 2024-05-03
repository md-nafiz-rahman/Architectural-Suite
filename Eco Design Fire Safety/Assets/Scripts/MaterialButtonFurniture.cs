
// MaterialButtonFurniture.cs script is responsible for handling the UI material button that applies material to selected furniture from the inventory.

using UnityEngine;
using UnityEngine.UI;

public class MaterialButtonFurniture : MonoBehaviour
{
    public MaterialData materialData;
    public InventoryManager inventoryManager;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ApplyMaterialToFurniture);
    }

    void ApplyMaterialToFurniture()
    {
        if (inventoryManager.selectedItemForPlacement != null)
        {
            // Determine the material to apply: either the current materialData or the selected item's default material
            MaterialData materialToApply = materialData ?? inventoryManager.selectedItemForPlacement.materialData;

            // Prepare the selected item for placement by setting up its prefab and applying the chosen material
            inventoryManager.PrepareItemForPlacement(inventoryManager.selectedItemForPlacement, materialToApply);

            inventoryManager.ShowMaterialSelectionPanel(inventoryManager.selectedItemForPlacement, materialToApply);
        }
    }


}
