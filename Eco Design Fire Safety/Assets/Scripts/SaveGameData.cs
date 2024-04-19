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


public class SaveGameData : MonoBehaviour
{
    public TextMeshProUGUI[] saveButtons; 
    public ItemPlacementHandler placementHandler;

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

        data.inventoryItems = InventoryManager.Instance.GetInventoryCounts()
                .Select(kvp => new InventoryItem { itemName = kvp.Key, count = kvp.Value }).ToList();

        data.saveTime = System.DateTime.Now.ToString("g");
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/gameSave" + slot + ".json", json);

        if (saveButtons[slot] != null)
        {
            saveButtons[slot].text = "Slot " + (slot + 1) + "\nSaved: " + data.saveTime;
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


}