// LoadGameData.cs script is responsible for loading the game data from JSON file. The game data inlcude player's position, furniture count and applied material, inventory state, 
// and material selections of room walls and floors for a specific house. 

using UnityEngine;
using System.IO;
using TMPro;
using SojaExiles;
using System.Collections;

public class LoadGameData : MonoBehaviour
{
    public TextMeshProUGUI[] loadButtons;
    public ItemPlacementHandler placementHandler;
    public InventoryManager inventoryManager;
    public GameObject helpMenuPanel;
    public PauseMenu pauseMenu;


    // Load game data from a JSON file and restore the game state.
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

            var houseInteraction = FindObjectOfType<HouseInteraction>();
            var materialSelection = FindObjectOfType<MaterialSelection>();
            if (materialSelection != null)
            {
                materialSelection.ClearAllMaterials();
                HouseScoreManager.Instance.ResetScores();
            }
            if (houseInteraction != null)
            {
                foreach (var houseMaterial in data.houseMaterials)
                {
                    houseInteraction.ApplySavedMaterials(houseMaterial);
                }
            }
            if (materialSelection != null)
            {
                materialSelection.ResetAndReapplyScores();
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
        StartCoroutine(RecalculateScoresAfterLoad());
        UpdateButtonLabels(loadButtons);
    }

    // Recalculate the scores after the game data has been loaded.
    private IEnumerator RecalculateScoresAfterLoad()
    {
        yield return new WaitForEndOfFrame();
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            for (int i = 0; i < 3; i++)
            {
                scoreManager.CalculateTotalScores(i);
            }
        }
    }

    // Initialise furniture in the scene based on the loaded data.
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

    // Applies the saved material to the loaded furniture.
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


    // Registers the loaded furniture in the score management list for FurnitueScoreManager.cs
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



    // Loads the inventory items based on the saved data.
    private void LoadInventoryItem(string itemName, int count)
    {
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


    // Removes all existing furniture from the scene before loading new data.
    public void ClearExistingFurniture()
    {
        Furniture[] existingFurnitures = FindObjectsOfType<Furniture>();
        foreach (var furniture in existingFurnitures)
        {
            FurnitureScoreManager.Instance.RemoveFurniturePlacement(furniture);
            Destroy(furniture.gameObject);
        }
    }

    // Updates the UI labels for load buttons based on the save file's time stamp.
    public static void UpdateButtonLabels(TextMeshProUGUI[] buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            string path = Application.persistentDataPath + "/gameSave" + i + ".json"; // Construct the file path and name for each game save file.
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                GameData data = JsonUtility.FromJson<GameData>(json); // Deserialize the JSON content to a GameData object.
                buttons[i].text = "Slot " + (i + 1) + "\nSaved: " + data.saveTime; // Set the button's text to display the slot number and the saved time.
            }
            else
            {
                buttons[i].text = "Slot " + (i + 1) + "\nEmpty";
            }
        }
    }


    public void ShowHelpMenu()
    {
        // If the pause menu is currently displayed, hide it
        if (pauseMenu.pauseMenuUI.activeSelf)
        {
            pauseMenu.pauseMenuUI.SetActive(false);
        }
        helpMenuPanel.SetActive(true); // Activate help menu panel
        Time.timeScale = 0f; // Stop game time
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor so it can be moved freely
        Cursor.visible = true; // Make the cursor visible on the screen
    }


    public void HideHelpMenu()
    {
        helpMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Start()
    {
        UpdateButtonLabels(loadButtons);
        ShowHelpMenu();

    }
}
