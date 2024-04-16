using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public FurnitureScoreManager furnitureScoreManager;
    public HouseScoreManager houseScoreManager;

    private float[] totalFireSafetyScores = new float[3];
    private float[] totalSustainabilityScores = new float[3];

    void Start()
    {
        furnitureScoreManager.OnScoresUpdated += CalculateTotalScores;
        houseScoreManager.OnScoresUpdated += CalculateTotalScores;
    }

    void OnDestroy()
    {
        furnitureScoreManager.OnScoresUpdated -= CalculateTotalScores;
        houseScoreManager.OnScoresUpdated -= CalculateTotalScores;
    }

    public float GetTotalFireSafetyScore(int houseIndex)
    {
        return totalFireSafetyScores[Mathf.Clamp(houseIndex, 0, totalFireSafetyScores.Length - 1)];
    }

    public float GetTotalSustainabilityScore(int houseIndex)
    {
        return totalSustainabilityScores[Mathf.Clamp(houseIndex, 0, totalSustainabilityScores.Length - 1)];
    }

    private void CalculateTotalScores()
    {
        for (int i = 0; i < 3; i++)
        {
            totalFireSafetyScores[i] = furnitureScoreManager.GetTotalFireSafetyScore(i) + houseScoreManager.GetTotalFireSafetyScore(i);
            totalSustainabilityScores[i] = furnitureScoreManager.GetTotalSustainabilityScore(i) + houseScoreManager.GetTotalSustainabilityScore(i);
        }
    }
}
