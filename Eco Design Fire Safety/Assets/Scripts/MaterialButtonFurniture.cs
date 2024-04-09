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
            var prefab = inventoryManager.selectedItemForPlacement.prefab;
            if (prefab != null)
            {
                Renderer renderer = prefab.GetComponent<Renderer>();
                if (renderer == null)
                {
                    renderer = prefab.GetComponentInChildren<Renderer>();
                }
                if (renderer != null)
                {
                    renderer.material = materialData.material;
                }
            }
        }
    }
}
