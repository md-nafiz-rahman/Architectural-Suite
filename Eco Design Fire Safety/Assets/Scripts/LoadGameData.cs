using UnityEngine;
using System.IO;
using TMPro; 
using SojaExiles;
using System.Linq;

public class LoadGameData : MonoBehaviour
{
    public TextMeshProUGUI[] loadButtons;
    public ItemPlacementHandler placementHandler;
    public InventoryManager inventoryManager;

    public void LoadGame(int slot)
    {
        string path = Application.persistentDataPath + "/gameSave" + slot + ".json";
        Debug.Log("Attempting to load game from: " + path);

        if (File.Exists(path))
        {
            ClearExistingFurniture();
            string json = File.ReadAllText(path);
            GameData data = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Loaded game data for slot " + slot);

            FindObjectOfType<PlayerMovement>().transform.position = data.playerPosition;
            Debug.Log("Player position set to: " + data.playerPosition);

            foreach (var furniture in data.furnitures)
            {
                LoadFurniture(furniture);
            }

            InventoryManager.Instance.ClearInventory();
            foreach (var inventoryItem in data.inventoryItems)
            {
                LoadInventoryItem(inventoryItem.itemName, inventoryItem.count);
            }
        }
        else
        {
            Debug.LogError("Save file not found at: " + path);
        }

        UpdateButtonLabels();
    }

    private void LoadFurniture(FurnitureData furniture)
    {
        if (furniture.position == Vector3.zero) return;

        FurnitureItem item = Resources.Load<FurnitureItem>("Furniture/" + furniture.itemName);
        if (item)
        {
            GameObject obj = Instantiate(item.prefab, furniture.position, furniture.rotation);
            if (furniture.materialName != "No Material" && furniture.materialName != null)
            {
                ApplyMaterial(obj, furniture.materialName);
            }
            else
            {
                Debug.LogWarning("Material data missing for furniture: " + furniture.itemName + ", applying default material if available.");
            }
            RegisterFurniture(obj, furniture.houseIndex);
            Debug.Log("Instantiated furniture: " + furniture.itemName);
        }
        else
        {
            Debug.LogError("Failed to load furniture item: " + furniture.itemName);
        }
    }


    private bool ApplyMaterial(GameObject obj, string materialName)
    {
        string cleanMaterialName = materialName.Replace(" (Instance)", "");
        MaterialData materialData = Resources.Load<MaterialData>("Furniture&House Materials/Furniture Materials/FurnitureMaterialData/" + cleanMaterialName);
        Renderer renderer = obj.GetComponent<Renderer>();
        if (materialData != null && renderer != null)
        {
            renderer.material = materialData.material;
            Furniture furniture = obj.GetComponent<Furniture>();
            if (furniture != null)
            {
                furniture.materialData = materialData;
            }
            Debug.Log("Applied MaterialData: " + cleanMaterialName + " to furniture: " + obj.name);
            return true;
        }
        return false;
    }



    private void RegisterFurniture(GameObject obj, int houseIndex)
    {
        Furniture furnitureComponent = obj.GetComponent<Furniture>();
        if (!furnitureComponent)
        {
            Debug.LogError("No Furniture component found on the object.");
            return;
        }

        if (houseIndex >= 0 && houseIndex < FurnitureScoreManager.Instance.houseFurnitures.Length)
        {
            FurnitureScoreManager.Instance.AddFurniturePlacement(houseIndex, furnitureComponent);
        }
        else if (houseIndex == -1)
        {
            Debug.Log($"Furniture '{furnitureComponent.name}' is not associated with any house but will remain in the environment.");
        }
        else
        {
            Debug.LogError("Invalid house index provided for furniture registration.");
        }
    }




    private void LoadInventoryItem(string itemName, int count)
    {
        Debug.Log($"Loading {count} of {itemName}");
        FurnitureItem item = Resources.Load<FurnitureItem>("Furniture/" + itemName);
        if (item)
        {
            for (int i = 0; i < count; i++)
            {
                InventoryManager.Instance.AddItemToInventory(item);
            }
        }
        else
        {
            Debug.LogError("Failed to load inventory item: " + itemName);
        }
    }


    private void ClearExistingFurniture()
    {
        Furniture[] existingFurnitures = FindObjectsOfType<Furniture>();
        foreach (var furniture in existingFurnitures)
        {
            FurnitureScoreManager.Instance.RemoveFurniturePlacement(furniture); 
            Destroy(furniture.gameObject);
        }
    }


    private int IdentifyHouseIndex(Vector3 position)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, 0.1f);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].CompareTag("House1FurnitureArea")) return 0;
            if (hitColliders[i].CompareTag("House2FurnitureArea")) return 1;
            if (hitColliders[i].CompareTag("House3FurnitureArea")) return 2;
        }
        return -1; 
    }

    void UpdateButtonLabels()
    {
        for (int i = 0; i < loadButtons.Length; i++)
        {
            string path = Application.persistentDataPath + "/gameSave" + i + ".json";
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                GameData data = JsonUtility.FromJson<GameData>(json);
                loadButtons[i].text = "Slot " + (i + 1) + "\nSaved: " + data.saveTime;
            }
            else
            {
                loadButtons[i].text = "Slot " + (i + 1) + "\nEmpty";
            }
        }
    }

    void Start()
    {
        UpdateButtonLabels();
    }
}
