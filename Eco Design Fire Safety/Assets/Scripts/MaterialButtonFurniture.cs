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
            MaterialData materialToApply = materialData ?? inventoryManager.selectedItemForPlacement.materialData;

            inventoryManager.PrepareItemForPlacement(inventoryManager.selectedItemForPlacement, materialToApply);

            inventoryManager.ShowMaterialSelectionPanel(inventoryManager.selectedItemForPlacement, materialToApply);

        }
    }

}
