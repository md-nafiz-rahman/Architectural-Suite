using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacementHandler : MonoBehaviour
{
    private Camera cam;
    private bool isPlacing = false;
    private GameObject currentItem;
    private int currentRotationIndex = 0;
    private readonly Vector3[] rotations = { 
        Vector3.up * 0, 
        Vector3.up * 90, 
        Vector3.up * 180, 
        Vector3.up * 270 
    };

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
        currentItem.transform.rotation = Quaternion.Euler(rotations[currentRotationIndex]); 
        isPlacing = true;

        ToggleColliders(currentItem, false);
        Time.timeScale = 1f;
    }


    void Update()
    {
        if (isPlacing && currentItem != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                currentItem.transform.position = hit.point;
                currentItem.transform.rotation = Quaternion.Euler(rotations[currentRotationIndex]); 
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateItem();
            }
        }
    }

    private void RotateItem()
    {
        currentRotationIndex = (currentRotationIndex + 1) % rotations.Length;
        currentItem.transform.rotation = Quaternion.Euler(rotations[currentRotationIndex]);
    }

    public void PlaceItem()
    {
        if (isPlacing && currentItem != null)
        {
            isPlacing = false;
            ToggleColliders(currentItem, true);
            currentItem = null;
            Time.timeScale = 1f;
        }
    }

    private void ToggleColliders(GameObject item, bool state)
    {
        Collider[] colliders = item.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            Debug.Log("Toggling collider on object: " + collider.gameObject.name + " to " + state);
            collider.enabled = state;
        }
    }
}
