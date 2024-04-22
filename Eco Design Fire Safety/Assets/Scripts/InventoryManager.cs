using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;
using System.Net;

[System.Serializable]
public class FurnitureItemWithCount
{
    public FurnitureItem item;
    public int initialCount;
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public GameObject slotPrefab;
    public GameObject inventoryCanvas;
    public Transform inventoryContentPanel;
    public GameObject emptyInventoryText;
    public GameObject bedsEmptyText;
    private Dictionary<FurnitureItem, GameObject> inventorySlots = new Dictionary<FurnitureItem, GameObject>();
    private Dictionary<FurnitureItem, int> furnitureCounts = new Dictionary<FurnitureItem, int>();

    public PlacementManager placementManager;
    public GameObject confirmationPanel;
    public GameObject allFurnitureScrollView;
    public GameObject bedsScrollView;
    public FurnitureItem selectedItemForPlacement;

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

    public Image selectedItemImage;

    public List<FurnitureItemWithCount> predefinedFurnitureItemsWithCount = new List<FurnitureItemWithCount>();
    public GameObject materialSelectionPanel;
    public GameObject currentItemForPlacement = null;
    public MaterialData defaultMaterialData;
    public HouseInteraction houseInteraction;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else if (Instance != this)
        {
            Destroy(gameObject); 
        }
    }

    void Start()
    {
        foreach (var item in predefinedFurnitureItemsWithCount)
        {
            for (int i = 0; i < item.initialCount; i++)
            {
                AddItemToInventory(item.item);
            }
        }
    }

    public void ShowMaterialSelectionPanel(FurnitureItem item, MaterialData materialData)
    {
        selectedItemForPlacement = item;
        selectedItemForPlacement.materialData = materialData;

        Image selectedItemImage = materialSelectionPanel.transform.Find("SelectedItemImage").GetComponent<Image>();
        if (selectedItemImage != null)
        {
            selectedItemImage.sprite = item.icon;
        }
        else
        {
            Debug.LogError("SelectedItemImage not found in the Material Selection Panel");
        }
        houseInteraction.crosshair.SetActive(false);
        materialSelectionPanel.SetActive(true);
        inventoryCanvas.SetActive(false);
    }

    public void PrepareItemForPlacement(FurnitureItem item, MaterialData materialData)
    {
        MaterialData useMaterial = materialData ?? item.materialData;

        if (currentItemForPlacement != null)
            Destroy(currentItemForPlacement);

        GameObject itemInstance = Instantiate(item.prefab);
        ApplyMaterialToInstance(itemInstance, useMaterial);

        currentItemForPlacement = itemInstance;
    }

    private void ApplyMaterialToInstance(GameObject instance, MaterialData materialData)
    {
        Renderer[] renderers = instance.GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            foreach (Renderer rend in renderers)
            {
                rend.material = materialData.material;
            }
        }
        else
        {
            Debug.LogError("No Renderer found on the instance to apply material.");
        }
    }


    public void ShowInventoryUI()
    {
        materialSelectionPanel.SetActive(false);
        inventoryCanvas.SetActive(true);
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
        if (currentItemForPlacement == null)
        {
            PrepareItemForPlacement(selectedItemForPlacement, selectedItemForPlacement.materialData);
        }

        placementManager.SelectItemForPlacement(selectedItemForPlacement);
        materialSelectionPanel.SetActive(false);
        houseInteraction.crosshair.SetActive(true);
        HideInventoryUI();
        HideConfirmationPanel();
        CheckIfInventoryIsEmpty();
        Debug.Log("Confirming placement of " + selectedItemForPlacement.name);
    }


    public void ToggleInventoryUI()
    {
        inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
        materialSelectionPanel.SetActive(false);
        houseInteraction.crosshair.SetActive(true);
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

    public void OnRightClick(PointerEventData data, FurnitureItem item)
    {
        if (data.button == PointerEventData.InputButton.Right)
        {
            ShowMaterialSelectionPanel(item, item.materialData);
        }
    }

    public void AddItemToInventory(FurnitureItem furnitureItem)
    {
        if (furnitureCounts.TryGetValue(furnitureItem, out var count))
        {
            furnitureCounts[furnitureItem] = count + 1;
            UpdateSlotCount(furnitureItem);
        }
        else
        {
            furnitureCounts[furnitureItem] = 1;
            CreateSlotForItem(furnitureItem, inventoryContentPanel);
        }
        CheckIfInventoryIsEmpty();
    }


    private void UpdateSlotCount(FurnitureItem furnitureItem)
    {
        if (inventorySlots.TryGetValue(furnitureItem, out var slot))
        {
            TMP_Text quantityText = slot.transform.Find("QuantityText").GetComponent<TMP_Text>();
            quantityText.text = "x" + furnitureCounts[furnitureItem];
        }
    }


    public void UpdateInventoryUI()
    {
        ClearInventorySlots(allFurnitureScrollView.transform.Find("Viewport/Content"));
        ClearInventorySlots(bedsScrollView.transform.Find("Viewport/Content"));
        ClearInventorySlots(tableDeskScrollView.transform.Find("Viewport/Content"));
        ClearInventorySlots(sofaChairScrollView.transform.Find("Viewport/Content"));
        ClearInventorySlots(cabinetScrollView.transform.Find("Viewport/Content"));
        ClearInventorySlots(decorationScrollView.transform.Find("Viewport/Content"));
        ClearInventorySlots(otherScrollView.transform.Find("Viewport/Content"));

        foreach (var item in furnitureCounts)
        {
            TMP_Text quantityText = inventorySlots[item.Key].transform.Find("QuantityText").GetComponent<TMP_Text>();
            quantityText.text = "x" + item.Value;

            CreateSlotForItemInAllCategory(item.Key);

            if (item.Key.category != null) 
            {
                CreateSlotForItem(item.Key, GetContentPanelForCategory(item.Key.category)); 
            }
        }

        UpdateEmptyTexts();
    }

    private void CreateSlotForItemInAllCategory(FurnitureItem furnitureItem)
    {

        GameObject slotInAll = Instantiate(slotPrefab, allFurnitureScrollView.transform.Find("Viewport/Content"));
        slotInAll.GetComponent<Image>().sprite = furnitureItem.icon;

        TMP_Text quantityText = slotInAll.transform.Find("QuantityText").GetComponent<TMP_Text>();
        if (quantityText != null)
        {
            quantityText.text = "x" + furnitureCounts[furnitureItem];
        }

        Button button = slotInAll.GetComponent<Button>();
        button.onClick.AddListener(() => ShowMaterialSelectionPanel(furnitureItem, furnitureItem.materialData));

        EventTrigger trigger = slotInAll.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
        entry.callback.AddListener((data) => OnRightClick((PointerEventData)data, furnitureItem));
        trigger.triggers.Add(entry);

        inventorySlots[furnitureItem] = slotInAll;
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

    public void ClearInventory()
    {
        furnitureCounts.Clear();
        foreach (var slot in inventorySlots.Values)
        {
            Destroy(slot.gameObject);
        }
        inventorySlots.Clear();
        UpdateInventoryUI();
    }




    public Dictionary<string, int> GetInventoryCounts()
    {
        var inventoryCounts = new Dictionary<string, int>();
        foreach (var pair in furnitureCounts)
        {
            if (pair.Value > 0) 
            {
                inventoryCounts[pair.Key.itemName] = pair.Value;
                Debug.Log($"Saving {pair.Key.itemName} with count {pair.Value}");
            }
        }
        return inventoryCounts;
    }



    private void CreateSlotForItem(FurnitureItem furnitureItem, Transform contentPanel)
    {
        GameObject newSlot = Instantiate(slotPrefab, contentPanel);
        newSlot.GetComponent<Image>().sprite = furnitureItem.icon;

        TMP_Text quantityText = newSlot.transform.Find("QuantityText").GetComponent<TMP_Text>();
        quantityText.text = "x" + furnitureCounts[furnitureItem]; 

        Button button = newSlot.GetComponent<Button>();
        button.onClick.AddListener(() => ShowMaterialSelectionPanel(furnitureItem, furnitureItem.materialData));

        EventTrigger trigger = newSlot.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
        entry.callback.AddListener((data) => OnRightClick((PointerEventData)data, furnitureItem));
        trigger.triggers.Add(entry);

        inventorySlots[furnitureItem] = newSlot;
    }

    private void UpdateEmptyTexts()
    {
        bedsEmptyText.SetActive(!furnitureCounts.Any(kv => kv.Key.category == FurnitureCategory.Bed));
        tableDeskEmptyText.SetActive(!furnitureCounts.Any(kv => kv.Key.category == FurnitureCategory.TableDesk));
        sofaChairEmptyText.SetActive(!furnitureCounts.Any(kv => kv.Key.category == FurnitureCategory.SofaChair));
        cabinetEmptyText.SetActive(!furnitureCounts.Any(kv => kv.Key.category == FurnitureCategory.Cabinet));
        decorationEmptyText.SetActive(!furnitureCounts.Any(kv => kv.Key.category == FurnitureCategory.Decoration));
        otherEmptyText.SetActive(!furnitureCounts.Any(kv => kv.Key.category == FurnitureCategory.Other));
    }


    public void HideInventoryUI()
    {
        inventoryCanvas.SetActive(false);
    }

    private void CheckIfInventoryIsEmpty()
    {

        bool isInventoryEmpty = !furnitureCounts.Any();
        emptyInventoryText.SetActive(isInventoryEmpty && allFurnitureScrollView.activeSelf);
        bedsEmptyText.SetActive(!furnitureCounts.Any(kv => kv.Key.category == FurnitureCategory.Bed) && bedsScrollView.activeSelf);
        tableDeskEmptyText.SetActive(!furnitureCounts.Any(kv => kv.Key.category == FurnitureCategory.TableDesk) && tableDeskScrollView.activeSelf);
        sofaChairEmptyText.SetActive(!furnitureCounts.Any(kv => kv.Key.category == FurnitureCategory.SofaChair) && sofaChairScrollView.activeSelf);
        cabinetEmptyText.SetActive(!furnitureCounts.Any(kv => kv.Key.category == FurnitureCategory.Cabinet) && cabinetScrollView.activeSelf);
        decorationEmptyText.SetActive(!furnitureCounts.Any(kv => kv.Key.category == FurnitureCategory.Decoration) && decorationScrollView.activeSelf);
        otherEmptyText.SetActive(!furnitureCounts.Any(kv => kv.Key.category == FurnitureCategory.Other) && otherScrollView.activeSelf);
    }



    public void RemoveFurnitureFromInventory(FurnitureItem itemToRemove, int quantity = 1)
    {
        if (furnitureCounts.ContainsKey(itemToRemove) && furnitureCounts[itemToRemove] >= quantity)
        {
            furnitureCounts[itemToRemove] -= quantity;
            if (furnitureCounts[itemToRemove] == 0)
            {
                Destroy(inventorySlots[itemToRemove]);
                inventorySlots.Remove(itemToRemove);
                furnitureCounts.Remove(itemToRemove);
            }
            else
            {
                UpdateSlotCount(itemToRemove);
            }
            UpdateInventoryUI();
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
