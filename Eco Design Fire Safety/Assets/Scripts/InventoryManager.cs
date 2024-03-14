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

    public GameObject tableDeskScrollView;
    public GameObject sofaChairScrollView;
    public GameObject cabinetScrollView;
    public GameObject decorationScrollView;
    public GameObject otherScrollView;

    public GameObject tableDeskEmptyText;
    public GameObject sofaChairEmptyText;
    public GameObject cabinetEmptyText;
    public GameObject decorationEmptyText;
    public GameObject otherEmptyText;



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
        ClearInventorySlots(tableDeskScrollView.transform.Find("Viewport/Content"));
        ClearInventorySlots(sofaChairScrollView.transform.Find("Viewport/Content"));
        ClearInventorySlots(cabinetScrollView.transform.Find("Viewport/Content"));
        ClearInventorySlots(decorationScrollView.transform.Find("Viewport/Content"));
        ClearInventorySlots(otherScrollView.transform.Find("Viewport/Content"));

        foreach (FurnitureItem item in furnitureItems)
        {
            CreateSlotForItem(item, allFurnitureScrollView.transform.Find("Viewport/Content"));
        }

        if (bedsScrollView.activeSelf || tableDeskScrollView.activeSelf || sofaChairScrollView.activeSelf ||
           cabinetScrollView.activeSelf || decorationScrollView.activeSelf || otherScrollView.activeSelf)
        {
            foreach (FurnitureItem item in furnitureItems)
            {
                Transform targetContentPanel = GetContentPanelForCategory(item.category);
                CreateSlotForItem(item, targetContentPanel);
            }
        }

        UpdateEmptyTexts();
    }

    private Transform GetContentPanelForCategory(FurnitureCategory category)
    {
        switch (category)
        {
            case FurnitureCategory.Bed:
                return bedsScrollView.transform.Find("Viewport/Content");
            case FurnitureCategory.TableDesk:
                return tableDeskScrollView.transform.Find("Viewport/Content");
            case FurnitureCategory.SofaChair:
                return sofaChairScrollView.transform.Find("Viewport/Content");
            case FurnitureCategory.Cabinet:
                return cabinetScrollView.transform.Find("Viewport/Content");
            case FurnitureCategory.Decoration:
                return decorationScrollView.transform.Find("Viewport/Content");
            case FurnitureCategory.Other:
                return otherScrollView.transform.Find("Viewport/Content");
            default:
                return allFurnitureScrollView.transform.Find("Viewport/Content");
        }
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

    private void UpdateEmptyTexts()
    {
        bedsEmptyText.SetActive(furnitureItems.FindAll(item => item.category == FurnitureCategory.Bed).Count == 0);
        tableDeskEmptyText.SetActive(furnitureItems.FindAll(item => item.category == FurnitureCategory.TableDesk).Count == 0);
        sofaChairEmptyText.SetActive(furnitureItems.FindAll(item => item.category == FurnitureCategory.SofaChair).Count == 0);
        cabinetEmptyText.SetActive(furnitureItems.FindAll(item => item.category == FurnitureCategory.Cabinet).Count == 0);
        decorationEmptyText.SetActive(furnitureItems.FindAll(item => item.category == FurnitureCategory.Decoration).Count == 0);
        otherEmptyText.SetActive(furnitureItems.FindAll(item => item.category == FurnitureCategory.Other).Count == 0);
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
        DeactivateAllScrollViews();
        allFurnitureScrollView.SetActive(true);
        UpdateInventoryUI();
    }

    public void ShowBeds()
    {
        DeactivateAllScrollViews();
        bedsScrollView.SetActive(true);
        UpdateInventoryUI();
    }

    public void ShowTableDesks()
    {
        DeactivateAllScrollViews();
        tableDeskScrollView.SetActive(true);
        UpdateInventoryUI();
    }

    public void ShowSofaChairs()
    {
        DeactivateAllScrollViews();
        sofaChairScrollView.SetActive(true);
        UpdateInventoryUI();
    }

    public void ShowCabinets()
    {
        DeactivateAllScrollViews();
        cabinetScrollView.SetActive(true);
        UpdateInventoryUI();
    }

    public void ShowDecorations()
    {
        DeactivateAllScrollViews();
        decorationScrollView.SetActive(true);
        UpdateInventoryUI();
    }

    public void ShowOthers()
    {
        DeactivateAllScrollViews();
        otherScrollView.SetActive(true);
        UpdateInventoryUI();
    }

    private void DeactivateAllScrollViews()
    {
        allFurnitureScrollView.SetActive(false);
        bedsScrollView.SetActive(false);
        tableDeskScrollView.SetActive(false);
        sofaChairScrollView.SetActive(false);
        cabinetScrollView.SetActive(false);
        decorationScrollView.SetActive(false);
        otherScrollView.SetActive(false);
    }




}
