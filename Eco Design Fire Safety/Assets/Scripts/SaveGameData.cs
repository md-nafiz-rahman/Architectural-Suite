// SaveGameData.cs is responsible for saving the current game state, including player position, furniture count and applied material, inventory state, 
// and material selections of room walls and floors for a specific house into a JSON file.

using UnityEngine;
using System.Collections.Generic;
using System.IO;
using TMPro;
using SojaExiles;
using System.Linq;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public List<FurnitureData> furnitures = new List<FurnitureData>();
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();
    public List<HouseMaterialData> houseMaterials = new List<HouseMaterialData>();
    public string saveTime;
}
[System.Serializable]
public class FurnitureData
{
    public Vector3 position;
    public Quaternion rotation;
    public string itemName;
    public string materialName;
    public int houseIndex;
}
[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public int count;
}
[System.Serializable]
public class MaterialSelectionData
{
    public string roomName;
    public string materialName;
}

[System.Serializable]
public class HouseMaterialData
{
    public string houseTag;
    public List<MaterialSelectionData> roomWallMaterials = new List<MaterialSelectionData>();
    public List<MaterialSelectionData> roomFloorMaterials = new List<MaterialSelectionData>();
}


public class SaveGameData : MonoBehaviour
{
    public TextMeshProUGUI[] saveButtons;
    public ItemPlacementHandler placementHandler;

    private void Start()
    {
        LoadGameData.UpdateButtonLabels(saveButtons);
    }

    // Creates and fill game data structure with the current state, it converts data to a JSON file and saves in local computer
    public void SaveGame(int slot)
    {
        GameData data = new GameData();
        data.playerPosition = FindObjectOfType<PlayerMovement>().transform.position;

        foreach (var furniture in FindObjectsOfType<Furniture>())
        {
            if (furniture.furnitureItem != null && furniture.transform.position != Vector3.zero)
            {
                Renderer renderer = furniture.GetComponent<Renderer>();
                string materialName = renderer ? renderer.material.name : "No Material";
                int houseIndex = IdentifyHouseIndex(furniture.transform.position);

                data.furnitures.Add(new FurnitureData
                {
                    position = furniture.transform.position,
                    rotation = furniture.transform.rotation,
                    itemName = furniture.furnitureItem.itemName,
                    materialName = materialName,
                    houseIndex = houseIndex
                });
            }
            else
            {
                Debug.Log("Skipping furniture with no item data: " + furniture.name);
            }
        }

        for (int i = 1; i <= 3; i++)
        {
            HouseInteraction houseInteraction = FindObjectOfType<HouseInteraction>();
            if (houseInteraction != null)
            {
                HouseMaterialData houseMaterial = houseInteraction.GetMaterialSelections(i);
                data.houseMaterials.Add(houseMaterial);
            }
        }

        data.inventoryItems = InventoryManager.Instance.GetInventoryCounts()
                .Select(kvp => new InventoryItem { itemName = kvp.Key, count = kvp.Value }).ToList();

        data.saveTime = System.DateTime.Now.ToString("g");
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/gameSave" + slot + ".json", json);

        if (saveButtons[slot] != null)
        {
            saveButtons[slot].text = "Slot " + (slot + 1) + "\nSaved: " + data.saveTime;
        }
        LoadGameData.UpdateButtonLabels(saveButtons);

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


}
