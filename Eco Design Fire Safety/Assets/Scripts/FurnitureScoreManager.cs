using UnityEngine;

public class FurnitureScoreManager : MonoBehaviour
{
    private float[] fireSafetyScores;
    private float[] sustainabilityScores;
    private int[] furnitureCounts;

    public static FurnitureScoreManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            fireSafetyScores = new float[3];
            sustainabilityScores = new float[3];
            furnitureCounts = new int[3];
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddFurniturePlacement(int houseIndex, MaterialData materialData)
    {
        if (houseIndex < 0 || houseIndex >= furnitureCounts.Length)
            return;

        if (materialData != null)
        {
            furnitureCounts[houseIndex]++;

            float weightedScore;
            if (furnitureCounts[houseIndex] <= 10)
            {
                weightedScore = 5.0f;  // Max weighted score per furniture if <= 10 pieces
            }
            else
            {
                weightedScore = 50.0f / furnitureCounts[houseIndex];  // Adjust weighted score if > 10 pieces
            }

            // Scale the actual scores to a maximum of 10
            float scaledFireSafetyScore = materialData.fireSafetyScore / 10.0f;
            float scaledSustainabilityScore = materialData.sustainabilityScore / 10.0f;

            // Multiply by weightedScore to adjust according to the number of furniture pieces
            fireSafetyScores[houseIndex] += scaledFireSafetyScore * weightedScore;
            sustainabilityScores[houseIndex] += scaledSustainabilityScore * weightedScore;
        }

        Debug.Log($"House {houseIndex + 1} - Furniture count: {furnitureCounts[houseIndex]}, " +
                  $"Total Fire Safety Score: {fireSafetyScores[houseIndex]}, " +
                  $"Total Sustainability Score: {sustainabilityScores[houseIndex]}");
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
}
