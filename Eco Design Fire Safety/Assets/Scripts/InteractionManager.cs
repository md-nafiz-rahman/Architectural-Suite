using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public InventoryManager inventoryManager; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Furniture furnitureComponent = hit.collider.GetComponentInParent<Furniture>();
                if (furnitureComponent != null)
                {
                    inventoryManager.AddItemToInventory(furnitureComponent.furnitureItem);
                    furnitureComponent.gameObject.SetActive(false); 
                }
            }
        }
    }
}
