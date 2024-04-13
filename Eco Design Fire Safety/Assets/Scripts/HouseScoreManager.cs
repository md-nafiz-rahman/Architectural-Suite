using UnityEngine;

public class HouseScoreManager : MonoBehaviour
{
    public static HouseScoreManager Instance;

    private float[,] fireSafetyScores; // Stores fire-safety scores for each house
    private float[,] sustainabilityScores; // Stores sustainability scores for each house

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // For 3 houses and 2 score types (fire-safety and sustainability)
            fireSafetyScores = new float[3, 2];
            sustainabilityScores = new float[3, 2];
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update scores for a specific house and material
    public void UpdateScores(int houseIndex, float fireSafetyScore, float sustainabilityScore)
    {
        // Each score contributes up to 2.5 points towards the 50 points allocated for materials
        fireSafetyScores[houseIndex, 0] += fireSafetyScore * 0.25f; // 10 * 0.25 = 2.5 max per material
        fireSafetyScores[houseIndex, 1] += 1; 

        sustainabilityScores[houseIndex, 0] += sustainabilityScore * 0.25f; // 10 * 0.25 = 2.5 max per material
        sustainabilityScores[houseIndex, 1] += 1; 

        Debug.Log($"Updated House {houseIndex + 1} Scores - Fire Safety: {fireSafetyScores[houseIndex, 0]}, Sustainability: {sustainabilityScores[houseIndex, 0]}");
    }

    public float GetAverageFireSafetyScore(int houseIndex)
    {
        if (fireSafetyScores[houseIndex, 1] == 0) return 0;
        return fireSafetyScores[houseIndex, 0];
    }

    public float GetAverageSustainabilityScore(int houseIndex)
    {
        if (sustainabilityScores[houseIndex, 1] == 0) return 0;
        return sustainabilityScores[houseIndex, 0];
    }

}
