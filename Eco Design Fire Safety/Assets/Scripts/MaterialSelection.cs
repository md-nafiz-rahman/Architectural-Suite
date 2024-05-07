
// MaterialSelection.cs is responsible for applying materials to the specific room's wall or floor of a house. It is also responsible for
// calculating score of fire-safety and sustainability score based on material selection of walls and floors of a house using the HouseScoreManager.cs  

using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MaterialSelection : MonoBehaviour
{
    [System.Serializable]

    public class Room
    {
        public List<GameObject> walls;
        public List<GameObject> floors;

    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("MaterialSelection Instance set.");
        }
        else if (Instance != this)
        {
            Debug.Log("Duplicate MaterialSelection instance found, destroying duplicate.");
            Destroy(gameObject);
        }
    }

    public struct MaterialEntry
    {
        public string materialName;
        public string roomName;
    }

    public MaterialData defaultWallMaterialData; 
    public MaterialData defaultFloorMaterialData;
    public static MaterialSelection Instance { get; private set; }


    public Dictionary<string, MaterialData> currentMaterials = new Dictionary<string, MaterialData>();

    public Room livingRoomHouse1, drawingRoomHouse1, guestRoomHouse1, kitchenHouse1, staircaseHouse1,
                bedroom1House1, bedroom2House1, masterBedroomHouse1, loungeHouse1, roofTopHouse1;

    public Room livingRoomHouse2, drawingRoomHouse2, guestRoomHouse2, kitchenHouse2, staircaseHouse2,
                bedroom1House2, bedroom2House2, masterBedroomHouse2, loungeHouse2, roofTopHouse2;

    public Room livingRoomHouse3, drawingRoomHouse3, guestRoomHouse3, kitchenHouse3, staircaseHouse3,
                bedroom1House3, bedroom2House3, masterBedroomHouse3, loungeHouse3, roofTopHouse3;

    private Dictionary<string, string> feedbackTemplates = new Dictionary<string, string>
    {
        {"BasaltBasedCladding", "- {0}: You have choosen to use {0} for walls in {1}. {0} is naturally fire resistant and production of it is sustainable, because it can be mined near production sites."},

        {"Brick", "- {0}: You have choosen to use {0} for walls in {1}. {0} is highly fire-resistant, production of {0} are slowly shifting to be more sustainabable."},

        {"CalciumSilicateBoard", "- {0}: You have choosen to use {0} for walls in {1}. {0} is good for fire protection and made of recycled materials."},

        {"CrossLaminatedTimber", "- {0}: You have choosen to use {0} for walls in {1}. {0} is very sustainable material, it is also fire-resistant."},

        {"FiberCementSiding", "- {0}: You have choosen to use {0} for walls in {1}. {0} is highly fire-resistant, however production is not as sustainable as Brick."},

        {"GypsumBoard", "- {0}: You have choosen to use {0} for walls in {1}. {0} is fire-resistant, however production of it can be energy intensive."},

        {"InsulatedConcrete", "- {0}: You have choosen to use {0} for walls in {1}. {0} does not burn easily making it highly fire-resistant. However, production of {0} can lead a high CO2 emission."},

        {"InsulatedFireResistantPanels", "- {0}: You have choosen to use {0} for walls in {1}. {0} is one of the most fire-resistant material for wall as the name suggests. {0} can be sustainable as it helps to reduce gas or energy bill during winter as it keeps the house warm."},

        {"RockwoolInsulation", "- {0}: You have choosen to use {0} for walls in {1}. {0} can withstand very high temperatures, upto 1000°C. It also keeps the house warm in winter and cold in summer making it very sustainable."},

        {"Steel", "- {0}: You have choosen to use {0} for walls in {1}. {0} is also one of the highest fire-resistant material for wall. However it is not as sustainable as some of the other materials available."},

        {"BambooFlooring", "- {0}: You have choosen to use {0} for flooring in {1}. {0} is very sustainable and versatile flooring material. It is also naturally fire-resistant, upto 400°C."},

        {"Carpet Flooring", "- {0}: You have choosen to use {0} for flooring in {1}. {0} is not as fire-resistant as the other materials available for flooring. Specific {0} made of wool can be sustainable."},
            
        {"ConcreteFlooring", "- {0}: You have choosen to use {0} for flooring in {1}. {0} Similar to Concrete material for walls, it has very high fire-resistant capabilities. However production of it is not as sustainable as some of the other materials available for flooring."},

        {"CorkFlooring", "- {0}: You have choosen to use {0} for flooring in {1}. {0} is naturally fire-resistant. {0} is harvested from Oak trees without damaging the tree."},

        {"HardwoodFlooring", "- {0}: You have choosen to use {0} for flooring in {1}. {0} can be fire-resistant due to it's dense nature containing water."},

        {"LinoleumFlooring", "- {0}: You have choosen to use {0} for flooring in {1}. {0} is made from natural materials. The natural properties of {0} makes it fire-resistant, however not as much as most of the other materials for flooring."},

        {"RecycledGlassTilesFlooring", "- {0}: You have choosen to use {0} for flooring in {1}. {0} is very recyclable as it is made from recycled glass. Glass is also highly fire and heat resistant making it a good choice for kitchen or new fireplace."},

        {"ResinFlooring", "- {0}: You have choosen to use {0} for flooring in {1}. {0} can be sustainable when made from renewable materials. It also does not burn easily."},

        {"TilesFlooring", "- {0}: You have choosen to use {0} for flooring in {1}. {0} is hard to melt and burn. It can be sustainable when made from natural clay or materials."},

        {"WoodCarpetFlooring", "- {0}: You have choosen to use {0} for flooring in {1}. {0} is sustainable as it saves gas and electricity by keeping the house warm in winter and cold in summer. However it is not very fire-resistant."},
    };

    private Room GetRoomByHouseAndName(string houseTag, string roomName)
    {
        switch (houseTag)
        {
            case "House1LampPost":
                return GetRoomByNameHouse1(roomName);
            case "House2LampPost":
                return GetRoomByNameHouse2(roomName);
            case "House3LampPost":
                return GetRoomByNameHouse3(roomName);
            default:
                return null;
        }
    }

    private Room GetRoomByNameHouse1(string roomName)
    {
        switch (roomName)
        {
            case "LivingRoom": return livingRoomHouse1;
            case "DrawingRoom": return drawingRoomHouse1;
            case "GuestRoom": return guestRoomHouse1;
            case "Kitchen": return kitchenHouse1;
            case "Staircase": return staircaseHouse1;
            case "Bedroom1": return bedroom1House1;
            case "Bedroom2": return bedroom2House1;
            case "MasterBedroom": return masterBedroomHouse1;
            case "Lounge": return loungeHouse1;
            case "RoofTop": return roofTopHouse1;
            default: return null;
        }
    }

    private Room GetRoomByNameHouse2(string roomName)
    {
        switch (roomName)
        {
            case "LivingRoom": return livingRoomHouse2;
            case "DrawingRoom": return drawingRoomHouse2;
            case "GuestRoom": return guestRoomHouse2;
            case "Kitchen": return kitchenHouse2;
            case "Staircase": return staircaseHouse2;
            case "Bedroom1": return bedroom1House2;
            case "Bedroom2": return bedroom2House2;
            case "MasterBedroom": return masterBedroomHouse2;
            case "Lounge": return loungeHouse2;
            case "RoofTop": return roofTopHouse2;
            default: return null;
        }
    }


    private Room GetRoomByNameHouse3(string roomName)
    {
        switch (roomName)
        {
            case "LivingRoom": return livingRoomHouse3;
            case "DrawingRoom": return drawingRoomHouse3;
            case "GuestRoom": return guestRoomHouse3;
            case "Kitchen": return kitchenHouse3;
            case "Staircase": return staircaseHouse3;
            case "Bedroom1": return bedroom1House3;
            case "Bedroom2": return bedroom2House3;
            case "MasterBedroom": return masterBedroomHouse3;
            case "Lounge": return loungeHouse3;
            case "RoofTop": return roofTopHouse3;
            default: return null;
        }
    }


    // Applies wall materials to a specific room's walls and updates house scores.
    public void ApplyWallMaterial(string houseTag, string roomName, MaterialData materialData)
    {
        int houseIndex = GetHouseIndex(houseTag);
        Room room = GetRoomByHouseAndName(houseTag, roomName);
        if (room != null)
        {
            foreach (GameObject wall in room.walls)
            {
                wall.GetComponent<Renderer>().material = materialData.material;
            }
            string key = houseTag + roomName + "Wall";
            if (currentMaterials.ContainsKey(key))
            {
                HouseScoreManager.Instance.UpdateScores(houseIndex, materialData.fireSafetyScore - currentMaterials[key].fireSafetyScore, materialData.sustainabilityScore - currentMaterials[key].sustainabilityScore);
            }
            else
            {
                HouseScoreManager.Instance.UpdateScores(houseIndex, materialData.fireSafetyScore, materialData.sustainabilityScore);
            }
            currentMaterials[key] = materialData;
        }
    }

    // Applies floor materials to a specific room's floors and updates house scores.
    public void ApplyFloorMaterial(string houseTag, string roomName, MaterialData materialData)
    {
        int houseIndex = GetHouseIndex(houseTag);
        Room room = GetRoomByHouseAndName(houseTag, roomName);
        if (room != null)
        {
            foreach (GameObject floor in room.floors)
            {
                floor.GetComponent<Renderer>().material = materialData.material;
            }
            string key = houseTag + roomName + "Floor";
            if (currentMaterials.ContainsKey(key))
            {
                HouseScoreManager.Instance.UpdateScores(houseIndex, materialData.fireSafetyScore - currentMaterials[key].fireSafetyScore, materialData.sustainabilityScore - currentMaterials[key].sustainabilityScore);
            }
            else
            {
                HouseScoreManager.Instance.UpdateScores(houseIndex, materialData.fireSafetyScore, materialData.sustainabilityScore);
            }
            currentMaterials[key] = materialData;
        }
    }


    private int GetHouseIndex(string houseTag)
    {
        string numberStr = System.Text.RegularExpressions.Regex.Match(houseTag, @"\d+").Value;
        return int.Parse(numberStr) - 1;
    }


    // Helper method used in LoadGameState.cs to reset and apply scores based on material selection retrieved from the save file.
    public void ResetAndReapplyScores()
    {
        HouseScoreManager.Instance.ResetScores(); 

        foreach (var key in currentMaterials.Keys)
        {
            var materialData = currentMaterials[key];
            int houseIndex = GetHouseIndex(key);
            if (houseIndex >= 0 && houseIndex < 3)
            {
                HouseScoreManager.Instance.UpdateScores(houseIndex, materialData.fireSafetyScore, materialData.sustainabilityScore);
            }
        }
    }


    // Helper method used in LoadGameData.cs to reset the floor and wall materials before a game file is loaded.
    public void ClearAllMaterials()
    {
        var allRooms = new List<Room> {
        livingRoomHouse1, drawingRoomHouse1, guestRoomHouse1, kitchenHouse1, staircaseHouse1,
        bedroom1House1, bedroom2House1, masterBedroomHouse1, loungeHouse1, roofTopHouse1,
        livingRoomHouse2, drawingRoomHouse2, guestRoomHouse2, kitchenHouse2, staircaseHouse2,
        bedroom1House2, bedroom2House2, masterBedroomHouse2, loungeHouse2, roofTopHouse2,
        livingRoomHouse3, drawingRoomHouse3, guestRoomHouse3, kitchenHouse3, staircaseHouse3,
        bedroom1House3, bedroom2House3, masterBedroomHouse3, loungeHouse3, roofTopHouse3
    };

        foreach (var room in allRooms)
        {
            foreach (var wall in room.walls)
            {
                wall.GetComponent<Renderer>().material = defaultWallMaterialData.material;
            }
            foreach (var floor in room.floors)
            {
                floor.GetComponent<Renderer>().material = defaultFloorMaterialData.material;
            }
        }

        currentMaterials.Clear();
    }

    // Helper method to check if any wall or floor material has been changed for a house.
    public bool HasMaterialChanges(int houseIndex)
    {
        string houseTag = $"House{houseIndex + 1}LampPost";
        return currentMaterials.Keys.Any(key => key.StartsWith(houseTag));
    }

    // Helper method to check if all rooms floor and wall material has been changed for a specific house.
    public bool AllMaterialsChanged(int houseIndex)
    {
        Room[] rooms = GetRoomsByHouseIndex(houseIndex);
        foreach (var room in rooms)
        {
            if (room.walls.Concat(room.floors).Any(obj => obj.GetComponent<Renderer>().material.name == defaultWallMaterialData.material.name ||
                                                         obj.GetComponent<Renderer>().material.name == defaultFloorMaterialData.material.name))
            {
                return false;
            }
        }
        return true;
    }

    private Room[] GetRoomsByHouseIndex(int houseIndex)
    {
        switch (houseIndex)
        {
            case 0:
                return new Room[] { livingRoomHouse1, drawingRoomHouse1, guestRoomHouse1, kitchenHouse1, staircaseHouse1,
                                        bedroom1House1, bedroom2House1, masterBedroomHouse1, loungeHouse1, roofTopHouse1 };
            case 1:
                return new Room[] { livingRoomHouse2, drawingRoomHouse2, guestRoomHouse2, kitchenHouse2, staircaseHouse2,
                                        bedroom1House2, bedroom2House2, masterBedroomHouse2, loungeHouse2, roofTopHouse2 };
            case 2:
                return new Room[] { livingRoomHouse3, drawingRoomHouse3, guestRoomHouse3, kitchenHouse3, staircaseHouse3,
                                        bedroom1House3, bedroom2House3, masterBedroomHouse3, loungeHouse3, roofTopHouse3 };
            default: return null;
        }
    }

    public Dictionary<string, List<string>> GetConsolidatedMaterialEntries(int houseIndex)
    {
        Dictionary<string, List<string>> materialRooms = new Dictionary<string, List<string>>();
        string houseTag = $"House{houseIndex + 1}LampPost";

        foreach (var item in currentMaterials)
        {
            if (item.Key.StartsWith(houseTag))
            {
                string materialName = item.Value.materialName;
                string roomName = ExtractRoomNameFromKey(item.Key);

                if (!materialRooms.ContainsKey(materialName))
                {
                    materialRooms[materialName] = new List<string>();
                }

                if (!materialRooms[materialName].Contains(roomName))
                {
                    materialRooms[materialName].Add(roomName);
                }
            }
        }

        return materialRooms;
    }

    private string ExtractRoomNameFromKey(string key)
    {
        int index = key.IndexOf("LampPost") + "LampPost".Length;
        string roomPart = key.Substring(index);
        return roomPart.Replace("Wall", "").Replace("Floor", "");
    }

    public string GenerateMaterialFeedback(int houseIndex)
    {
        StringBuilder feedbackBuilder = new StringBuilder();
        var materialRooms = GetConsolidatedMaterialEntries(houseIndex);

        foreach (var entry in materialRooms)
        {
            if (feedbackTemplates.TryGetValue(entry.Key, out string template))
            {
                string roomsList = string.Join(", ", entry.Value);
                feedbackBuilder.AppendLine(string.Format(template, entry.Key, roomsList));
            }
        }

        return feedbackBuilder.ToString();
    }

}
