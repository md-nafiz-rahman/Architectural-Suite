
// InteractionManager.cs manages user interactions with game objects, specifically picking up furniture.

using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public InventoryManager inventoryManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            int layerMask = 1 << LayerMask.NameToLayer("IgnoreColliderRaycast");
            layerMask = ~layerMask;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
            {
                Furniture furnitureComponent = hit.collider.GetComponentInParent<Furniture>();
                if (furnitureComponent != null)
                {
                    int houseIndex = IdentifyHouseIndex(furnitureComponent.transform.position);
                    if (houseIndex != -1)
                    {
                        FurnitureScoreManager.Instance.RemoveFurniturePlacement(houseIndex, furnitureComponent);
                    }

                    inventoryManager.AddItemToInventory(furnitureComponent.furnitureItem);
                    furnitureComponent.gameObject.SetActive(false);
                }
            }
        }
    }

    private int IdentifyHouseIndex(Vector3 position)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, 0.1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("House1FurnitureArea")) return 0;
            if (hitCollider.CompareTag("House2FurnitureArea")) return 1;
            if (hitCollider.CompareTag("House3FurnitureArea")) return 2;
        }

        return -1;
    }
}
