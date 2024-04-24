using System.Collections.Generic;
using System.Linq;
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


    private void UpdateScoresForRoom(string key, MaterialData newMaterial)
    {
        if (currentMaterials.ContainsKey(key))
        {
            var oldMaterial = currentMaterials[key];
            HouseScoreManager.Instance.UpdateScores(
                GetHouseIndex(key.Substring(0, 10)),
                newMaterial.fireSafetyScore - oldMaterial.fireSafetyScore,
                newMaterial.sustainabilityScore - oldMaterial.sustainabilityScore
            );
        }
        else
        {
            HouseScoreManager.Instance.UpdateScores(
                GetHouseIndex(key.Substring(0, 10)),
                newMaterial.fireSafetyScore,
                newMaterial.sustainabilityScore
            );
        }
        currentMaterials[key] = newMaterial; 
    }

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

    public bool HasMaterialChanges(int houseIndex)
    {
        string houseTag = $"House{houseIndex + 1}LampPost";
        return currentMaterials.Keys.Any(key => key.StartsWith(houseTag));
    }

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



}
