using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public GameObject slotPrefab; 
    public GameObject inventoryCanvas;
    public Transform inventoryContentPanel;
    public GameObject emptyInventoryText; 
    private List<GameObject> inventorySlots = new List<GameObject>(); 
    private List<FurnitureItem> furnitureItems = new List<FurnitureItem>(); 
    public PlacementManager placementManager; 
    public GameObject confirmationPanel; 
    private FurnitureItem selectedItemForPlacement;


    void Start()
    {
        inventoryCanvas.SetActive(false);
        CheckIfInventoryIsEmpty();
    }

    public void ShowConfirmationPanel(FurnitureItem item)
    {
        selectedItemForPlacement = item; 
        confirmationPanel.SetActive(true); 
    }

    public void HideConfirmationPanel()
    {
        confirmationPanel.SetActive(false); 
    }

    public void ConfirmPlacement()
    {
        placementManager.SelectItemForPlacement(selectedItemForPlacement); 
        HideInventoryUI();
        HideConfirmationPanel(); 
        CheckIfInventoryIsEmpty();
    }


    public void ToggleInventoryUI()
    {
        inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);

        if (inventoryCanvas.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f; 
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f; 
        }
        UpdateInventoryUI();
    }

    public void AddFurnitureToInventory(FurnitureItem furnitureItem)
    {
        GameObject newSlot = Instantiate(slotPrefab, inventoryContentPanel);
        newSlot.GetComponent<Image>().sprite = furnitureItem.icon;
        furnitureItems.Add(furnitureItem);
        inventorySlots.Add(newSlot);

        EventTrigger trigger = newSlot.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => {
            OnRightClick((PointerEventData)data, furnitureItem);
        });
        trigger.triggers.Add(entry);
    }

    public void OnRightClick(PointerEventData data, FurnitureItem item)
    {
        if (data.button == PointerEventData.InputButton.Right)
        {
            ShowConfirmationPanel(item);
        }
    }

    public void AddItemToInventory(FurnitureItem furnitureItem)
    {
        furnitureItems.Add(furnitureItem); 
        UpdateInventoryUI();
        CheckIfInventoryIsEmpty();
    }
    public void ClearInventory()
    {
        foreach (GameObject slot in inventorySlots)
        {
            Destroy(slot); 
        }
        inventorySlots.Clear(); 
    }

    private void UpdateInventoryUI()
    {
        foreach (GameObject slot in inventorySlots)
        {
            Destroy(slot);
        }
        inventorySlots.Clear();

        foreach (FurnitureItem item in furnitureItems)
        {
            GameObject newSlot = Instantiate(slotPrefab, inventoryContentPanel);
            newSlot.GetComponent<Image>().sprite = item.icon;
            inventorySlots.Add(newSlot);

            Button button = newSlot.GetComponent<Button>();
            button.onClick.AddListener(() => ShowConfirmationPanel(item));

            EventTrigger trigger = newSlot.GetComponent<EventTrigger>() ?? newSlot.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => OnRightClick((PointerEventData)data, item));
            trigger.triggers.Add(entry);

            CheckIfInventoryIsEmpty();

        }
    }
    public void HideInventoryUI()
    {
        inventoryCanvas.SetActive(false);
 
    }

    private void CheckIfInventoryIsEmpty()
    {
        bool isInventoryEmpty = furnitureItems.Count == 0;
        emptyInventoryText.SetActive(isInventoryEmpty);
    }

    public void RemoveFurnitureFromInventory(FurnitureItem itemToRemove)
    {
        if (furnitureItems.Contains(itemToRemove))
        {
            furnitureItems.Remove(itemToRemove);
            UpdateInventoryUI(); 
            CheckIfInventoryIsEmpty();
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventoryUI();
        }
    }
}
