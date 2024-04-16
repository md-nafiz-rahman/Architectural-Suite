using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public FurnitureScoreManager furnitureScoreManager;
    public HouseScoreManager houseScoreManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            CalculateTotalScores();
        }
    }

    void CalculateTotalScores()
    {
        for (int i = 0; i < 3; i++)
        {
            float houseFireSafety = furnitureScoreManager.GetTotalFireSafetyScore(i) + houseScoreManager.GetTotalFireSafetyScore(i);
            float houseSustainability = furnitureScoreManager.GetTotalSustainabilityScore(i) + houseScoreManager.GetTotalSustainabilityScore(i);

            Debug.Log($"House {i + 1} Fire Safety Score: {houseFireSafety}");
            Debug.Log($"House {i + 1} Sustainability Score: {houseSustainability}");
        }
    }
}

