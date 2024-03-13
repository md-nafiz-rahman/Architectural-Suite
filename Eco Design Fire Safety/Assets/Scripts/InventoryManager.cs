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
    public GameObject bedsEmptyText; 
    private List<GameObject> inventorySlots = new List<GameObject>(); 
    private List<FurnitureItem> furnitureItems = new List<FurnitureItem>(); 
    public PlacementManager placementManager; 
    public GameObject confirmationPanel;
    public GameObject allFurnitureScrollView; 
    public GameObject bedsScrollView; 
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
        ClearInventorySlots(allFurnitureScrollView.transform.Find("Viewport/Content"));
        ClearInventorySlots(bedsScrollView.transform.Find("Viewport/Content"));

        foreach (FurnitureItem item in furnitureItems)
        {
            CreateSlotForItem(item, allFurnitureScrollView.transform.Find("Viewport/Content"));
        }

        foreach (FurnitureItem item in furnitureItems)
        {
            if (item.category == FurnitureCategory.Bed)
            {
                CreateSlotForItem(item, bedsScrollView.transform.Find("Viewport/Content"));
            }
        }

        CheckIfInventoryIsEmpty();
    }

    private void ClearInventorySlots(Transform contentPanel)
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateSlotForItem(FurnitureItem item, Transform contentPanel)
    {
        GameObject newSlot = Instantiate(slotPrefab, contentPanel);
        newSlot.GetComponent<Image>().sprite = item.icon;

        Button button = newSlot.GetComponent<Button>();
        button.onClick.AddListener(() => ShowConfirmationPanel(item));

        EventTrigger trigger = newSlot.GetComponent<EventTrigger>() ?? newSlot.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => OnRightClick((PointerEventData)data, item));
        trigger.triggers.Add(entry);
    }

    public void HideInventoryUI()
    {
        inventoryCanvas.SetActive(false);
 
    }

    private void CheckIfInventoryIsEmpty()
    {
        bool isInventoryEmpty = furnitureItems.Count == 0;
        emptyInventoryText.SetActive(isInventoryEmpty && allFurnitureScrollView.activeSelf); 

        bool isBedsEmpty = furnitureItems.FindAll(item => item.category == FurnitureCategory.Bed).Count == 0;
        bedsEmptyText.SetActive(isBedsEmpty && bedsScrollView.activeSelf); 
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


    public void ShowAllFurniture()
    {
        allFurnitureScrollView.SetActive(true);
        bedsScrollView.SetActive(false);
        UpdateInventoryUI();
    }

    public void ShowBeds()
    {
        allFurnitureScrollView.SetActive(false);
        bedsScrollView.SetActive(true);
        UpdateInventoryUI();
    }



}
