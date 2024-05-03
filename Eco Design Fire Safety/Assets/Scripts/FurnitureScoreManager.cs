
// FurnitureScoreManager.cs is responsible for calculating fire-safety and sustainability score based on furniture placement and material selection within specific house.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FurnitureScoreManager : MonoBehaviour
{
    [System.Serializable]
    public class MaterialDataEntry
    {
        public string key;
        public MaterialData value;
    }

    public DoorObstructionPopup doorObstructionPopup;
    public CloseToFirePopup closeToFirePopup;
    public List<MaterialDataEntry> materialEntries = new List<MaterialDataEntry>();
    private Dictionary<string, MaterialData> allMaterials = new Dictionary<string, MaterialData>();

    public List<Furniture>[] houseFurnitures;
    private float[] fireSafetyScores;
    private float[] sustainabilityScores;

    public static FurnitureScoreManager Instance;
    private List<Furniture>[] obstructedFurnitures;
    private List<Furniture>[] closeToFireFurnitures;
    public RenewablePopup renewablePopup;
    public event Action<int> OnScoresUpdated;

    // Initializes the furniture score manager setup.
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            int numHouses = 3;
            houseFurnitures = new List<Furniture>[numHouses];
            obstructedFurnitures = new List<Furniture>[numHouses];
            closeToFireFurnitures = new List<Furniture>[numHouses];
            fireSafetyScores = new float[numHouses];
            sustainabilityScores = new float[numHouses];
            for (int i = 0; i < numHouses; i++)
            {
                houseFurnitures[i] = new List<Furniture>();
                obstructedFurnitures[i] = new List<Furniture>();
                closeToFireFurnitures[i] = new List<Furniture>();
            }

            foreach (var entry in materialEntries)
            {
                allMaterials.Add(entry.key, entry.value);
            }
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Adds a furniture to a list for a specific house and call to recalculate scores.
    public void AddFurniturePlacement(int houseIndex, Furniture furniture)
    {
        if (furniture == null || houseIndex < 0 || houseIndex >= houseFurnitures.Length)
        {
            Debug.LogError($"Invalid furniture addition attempt: furniture is null or houseIndex out of range for furniture: {furniture?.name ?? "Unknown"}.");
            return;
        }
        if (furniture.transform.position == Vector3.zero)
        {
            Debug.LogWarning($"Skipping furniture at position zero: {furniture.name}");
            return;
        }
        if (furniture.materialData == null)
        {
            Debug.LogError($"Missing material data for furniture: {furniture.name}.");
            return;
        }

        houseFurnitures[houseIndex].Add(furniture);
        RecalculateScores(houseIndex);
    }


    // Removes a furniture from a specific house and call to recalculate scores.
    public void RemoveFurniturePlacement(int houseIndex, Furniture furniture)
    {
        if (houseIndex < 0 || houseIndex >= fireSafetyScores.Length)
        {
            Debug.LogError("Invalid house index for removal.");
            return;
        }

        if (houseFurnitures[houseIndex].Contains(furniture))
        {
            houseFurnitures[houseIndex].Remove(furniture);
            RecalculateScores(houseIndex);
        }
        else
        {
            Debug.LogError("Attempted to remove furniture not found in the current house.");
        }
    }

    // Overloaded version to remove furniture without specifying house index, used in LoadGameData to reset number of furniture placed in a house before loading new game data.
    public void RemoveFurniturePlacement(Furniture furniture)
    {
        for (int i = 0; i < houseFurnitures.Length; i++)
        {
            if (houseFurnitures[i].Contains(furniture))
            {
                houseFurnitures[i].Remove(furniture);
                RecalculateScores(i);
                break;
            }
        }
    }


    // Calculate the fire safety and sustainability scores for a house specific house based on furniture placement and material selection.
    private void RecalculateScores(int houseIndex)
    {
        fireSafetyScores[houseIndex] = 0;
        sustainabilityScores[houseIndex] = 0;
        int totalItems = houseFurnitures[houseIndex].Count;

        for (int i = houseFurnitures[houseIndex].Count - 1; i >= 0; i--)
        {
            var furniture = houseFurnitures[houseIndex][i];
            if (furniture == null || furniture.materialData == null)
            {
                Debug.LogError("Material data missing for furniture: " + (furniture != null ? furniture.name : "Unknown furniture"));
                houseFurnitures[houseIndex].RemoveAt(i);
                continue;
            }

            float weightedScore = CalculateWeightedScore(totalItems);  // Calculate weighted score for the furniture based on total number of furniture user placed inside a house.

            // Update fire safety and sustainability scores for each furniture's material data's weighted score for fire-safety and sustainability.
            fireSafetyScores[houseIndex] += (furniture.materialData.fireSafetyScore / 10.0f) * weightedScore;
            sustainabilityScores[houseIndex] += (furniture.materialData.sustainabilityScore / 10.0f) * weightedScore;

            // Check if furniture is in the rooftop zone and is a solar panel; add 5 to sustainability score.
            if (IsInRooftopZone(furniture.gameObject, houseIndex) && furniture.furnitureItem != null && furniture.furnitureItem.itemName == "SolarPanel")
            {
                sustainabilityScores[houseIndex] += 5;
                renewablePopup.ShowPopup();

            }

            // Check if furniture is in door obstruction zone, deduct 5 from fire-safety score and add the furniture to list of obstructing furniture, display popup for door obstruction.
            if (IsInDoorObstructionZone(furniture.gameObject, houseIndex))
            {
                fireSafetyScores[houseIndex] -= 5;
                if (!obstructedFurnitures[houseIndex].Contains(furniture))
                {
                    obstructedFurnitures[houseIndex].Add(furniture);
                    doorObstructionPopup.ShowPopup();
                }
            }

            // Check if furniture is close to fire hazard, deduct 5 from fire-safety score and add the furniture to list of furniture close to fire hazards, display popup for fire hazard.
            if (IsCloseToFire(furniture.gameObject, houseIndex))
            {
                fireSafetyScores[houseIndex] -= 5;
                if (!closeToFireFurnitures[houseIndex].Contains(furniture))
                {
                    closeToFireFurnitures[houseIndex].Add(furniture);
                    closeToFirePopup.ShowPopup();
                }
            }
        }

        obstructedFurnitures[houseIndex].RemoveAll(furniture => !IsInDoorObstructionZone(furniture.gameObject, houseIndex));
        closeToFireFurnitures[houseIndex].RemoveAll(furniture => !IsCloseToFire(furniture.gameObject, houseIndex));

        if (fireSafetyScores[houseIndex] < 0)
        {
            fireSafetyScores[houseIndex] = 0;
        }

        fireSafetyScores[houseIndex] = Mathf.Min(fireSafetyScores[houseIndex], 50.0f);
        sustainabilityScores[houseIndex] = Mathf.Min(sustainabilityScores[houseIndex], 50.0f);
        OnScoresUpdated?.Invoke(houseIndex);

        Debug.Log($"Updated Scores - House {houseIndex + 1}: Fire Safety: {fireSafetyScores[houseIndex]}, Sustainability: {sustainabilityScores[houseIndex]}");
    }



    // Checks if furniture is in a door obstruction zone.
    private bool IsInDoorObstructionZone(GameObject furniture, int houseIndex)
    {
        Collider[] hitColliders = Physics.OverlapSphere(furniture.transform.position, 0.1f);
        string obstructionTag = $"DoorObstruction{houseIndex + 1}";
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(obstructionTag))
                return true;
        }
        return false;
    }

    // Checks if furniture is too close to a fire source.
    private bool IsCloseToFire(GameObject furniture, int houseIndex)
    {
        Collider[] hitColliders = Physics.OverlapSphere(furniture.transform.position, 0.1f);
        string obstructionTag = $"CloseToFire{houseIndex + 1}";
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(obstructionTag))
                return true;
        }
        return false;
    }

    // Checks if furniture is placed on a rooftop.
    private bool IsInRooftopZone(GameObject furniture, int houseIndex)
    {
        Collider[] hitColliders = Physics.OverlapSphere(furniture.transform.position, 0.1f);
        string rooftopTag = $"Rooftop{houseIndex + 1}";
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(rooftopTag))
                return true;
        }
        return false;
    }


    // Calculates weighted score for a piece of furniture based on its count.
    private float CalculateWeightedScore(int count)
    {
        return count > 10 ? 50.0f / count : 5.0f;
    }



    public float GetTotalFireSafetyScore(int houseIndex)
    {
        if (houseIndex < 0 || houseIndex >= fireSafetyScores.Length)
            return 0;
        return fireSafetyScores[houseIndex];
    }

    public float GetTotalSustainabilityScore(int houseIndex)
    {
        if (houseIndex < 0 || houseIndex >= sustainabilityScores.Length)
            return 0;
        return sustainabilityScores[houseIndex];
    }

    // Utility class to compare material data.
    private class MaterialDataComparer : IEqualityComparer<MaterialData>
    {
        public bool Equals(MaterialData x, MaterialData y)
        {
            return x.materialName == y.materialName;
        }

        public int GetHashCode(MaterialData obj)
        {
            return obj.materialName.GetHashCode();
        }
    }

    public bool CheckDoorObstruction(int houseIndex)
    {
        return obstructedFurnitures[houseIndex].Any();
    }

    public bool CheckCloseProximityToFire(int houseIndex)
    {
        return closeToFireFurnitures[houseIndex].Any();
    }


}
