using System.Collections;
using System.Collections.Generic;
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
    }

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

    void Update()
    {
        if (isPlacing && currentItem != null)
        {
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
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

    public void PlaceItem()
    {
        if (isPlacing && currentItem != null)
        {
            int houseIndex = CheckHouseColliderArea(currentItem.transform.position);

            FurnitureScoreManager.Instance.AddFurniturePlacement(houseIndex, currentMaterialData);

            isPlacing = false;
            ToggleColliders(currentItem, true);
            currentItem = null;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private int CheckHouseColliderArea(Vector3 position)
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



    private void ToggleColliders(GameObject item, bool state)
    {
        Collider[] colliders = item.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = state;
        }
    }
}
