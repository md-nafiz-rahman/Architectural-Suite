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
            // Setup a layer mask to ignore colliders tagged as "IgnoreColliderRaycast"
            int layerMask = 1 << LayerMask.NameToLayer("IgnoreColliderRaycast");
            layerMask = ~layerMask;
            // Create a ray from the camera through the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Perform a raycast to detect objects up to 100 units away that are not ignored by the layer mask
            if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
            {
                // Attempt to get the Furniture component from the hit object
                Furniture furnitureComponent = hit.collider.GetComponentInParent<Furniture>();
                if (furnitureComponent != null)
                {
                    // Identify which house area the furniture is in
                    int houseIndex = IdentifyHouseIndex(furnitureComponent.transform.position);
                    if (houseIndex != -1)
                    {
                        // Remove the furniture for scoring
                        FurnitureScoreManager.Instance.RemoveFurniturePlacement(houseIndex, furnitureComponent);
                    }
                    // Add the furniture item to the player's inventory
                    inventoryManager.AddItemToInventory(furnitureComponent.furnitureItem);
                    // Hide the furniture object in the scene
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
