using System.Collections.Generic;
using UnityEngine;

public class MaterialSelection : MonoBehaviour
{
    [System.Serializable]

    public class Room
    {
        public List<GameObject> walls;
        public List<GameObject> floors;

    }

    private Dictionary<string, MaterialData> currentMaterials = new Dictionary<string, MaterialData>();

    public Room livingRoomHouse1, drawingRoomHouse1, guestRoomHouse1, kitchenHouse1, staircaseHouse1,
                bedroom1House1, bedroom2House1, masterBedroomHouse1, loungeHouse1, roofTopHouse1;

    public Room livingRoomHouse2, drawingRoomHouse2, guestRoomHouse2, kitchenHouse2, staircaseHouse2,
                bedroom1House2, bedroom2House2, masterBedroomHouse2, loungeHouse2, roofTopHouse2;

    public Room livingRoomHouse3, drawingRoomHouse3, guestRoomHouse3, kitchenHouse3, staircaseHouse3,
                bedroom1House3, bedroom2House3, masterBedroomHouse3, loungeHouse3, roofTopHouse3;

    public void ApplyMaterial(string houseTag, string roomName, Material wallMaterial, Material floorMaterial)
    {
        Room room = GetRoomByHouseAndName(houseTag, roomName);
        if (room != null)
        {
            foreach (GameObject wall in room.walls)
            {
                wall.GetComponent<Renderer>().material = wallMaterial;
            }
            foreach (GameObject floor in room.floors)
            {
                floor.GetComponent<Renderer>().material = floorMaterial;
            }
        }
    }

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
        Room room = GetRoomByHouseAndName(houseTag, roomName);
        if (room != null)
        {
            string key = houseTag + roomName + "Wall";
            UpdateScoresForRoom(key, materialData);

            foreach (GameObject wall in room.walls)
                wall.GetComponent<Renderer>().material = materialData.material;
        }
    }

    public void ApplyFloorMaterial(string houseTag, string roomName, MaterialData materialData)
    {
        Room room = GetRoomByHouseAndName(houseTag, roomName);
        if (room != null)
        {
            string key = houseTag + roomName + "Floor";
            UpdateScoresForRoom(key, materialData);

            foreach (GameObject floor in room.floors)
                floor.GetComponent<Renderer>().material = materialData.material;
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


}
