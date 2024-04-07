using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacementHandler : MonoBehaviour
{
    private Camera cam;
    private bool isPlacing = false;
    private GameObject currentItem;
    private float currentRotation = 0f;

    void Start()
    {
        cam = Camera.main;
    }

    public void BeginPlacement(GameObject itemPrefab)
    {
        if (currentItem != null)
        {
            Destroy(currentItem);
        }
        currentItem = Instantiate(itemPrefab);
        currentItem.transform.rotation = Quaternion.Euler(Vector3.up * currentRotation);
        isPlacing = true;

        ToggleColliders(currentItem, false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
            isPlacing = false;
            ToggleColliders(currentItem, true);
            currentItem = null;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
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
