
// ItemPlacementHandler.cs manages the placement mechanisms and rotation of items within the scene, allowing users to interactively place and orient furniture.

using UnityEngine;
public class ItemPlacementHandler : MonoBehaviour
{
    private Camera cam;
    public bool isPlacing = false;
    private GameObject currentItem;
    private float currentRotation = 0f;
    private MaterialData currentMaterialData;
    public Material defaultMaterial; 



    void Start()
    {
        cam = Camera.main;
    }

    // Beging placement process of the furniture, it is called from PlacementManager.cs when user clicks on 'The Place Furniture' button
    // in material selection panel for furniture. 
    public void BeginPlacement(GameObject itemPrefab, MaterialData materialData)
    {
        currentMaterialData = materialData;

        if (currentItem != null)
        {
            Destroy(currentItem);
        }

        currentItem = Instantiate(itemPrefab);
        ApplyMaterial(currentItem, materialData);
        currentItem.transform.rotation = Quaternion.Euler(Vector3.up * currentRotation);
        isPlacing = true;

        ToggleColliders(currentItem, false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Beginning placement of item: " + itemPrefab.name);

    }

    // Apply the selected material to the currently placed item.
    private void ApplyMaterial(GameObject item, MaterialData materialData)
    {
        Renderer renderer = item.GetComponent<Renderer>();
        if (renderer == null)
        {
            renderer = item.GetComponentInChildren<Renderer>();
        }
        if (renderer != null)
        {
            renderer.material = materialData.material;
        }
        else
        {
            renderer.material = defaultMaterial;  
        }
    }

    // Update item position and rotation based on user input during placement.
    void Update()
    {
        if (isPlacing && currentItem != null)
        {
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            int layerMask = LayerMask.GetMask("IgnoreColliderRaycast");
            layerMask = ~layerMask; 

            if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
            {
                currentItem.transform.position = hit.point;
                currentItem.transform.rotation = Quaternion.Euler(Vector3.up * currentRotation);
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            RotateItem(scroll);
        }
    }


    private void RotateItem(float scrollInput)
    {
        if (scrollInput != 0)
        {
            currentRotation += (scrollInput > 0) ? 45f : -45f;
            currentRotation = (currentRotation + 360f) % 360f;
            if (currentItem != null)
                currentItem.transform.rotation = Quaternion.Euler(Vector3.up * currentRotation);
        }
    }

    // Finalizes the placement of the current item, find the correct house index and updates the score for the house.
    public void PlaceItem()
    {
        if (isPlacing && currentItem != null)
        {
            int houseIndex = CheckHouseColliderArea(currentItem.transform.position);
            if (houseIndex != -1)
            {
                Furniture furnitureComponent = currentItem.GetComponent<Furniture>() ?? currentItem.AddComponent<Furniture>();
                furnitureComponent.materialData = currentMaterialData; 

                FurnitureScoreManager.Instance.AddFurniturePlacement(houseIndex, furnitureComponent);
            }
            else
            {
                Debug.Log("Placed furniture outside of any designated house area. It will not contribute to scores.");
            }

            FinalizePlacement();
        }
    }

    public int CheckHouseColliderArea(Vector3 position)
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

    // Update scene state
    private void FinalizePlacement()
    {
        isPlacing = false;
        ToggleColliders(currentItem, true);
        currentItem = null;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void ToggleColliders(GameObject item, bool state)
    {
        Collider[] colliders = item.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = state;
        }
    }
}
