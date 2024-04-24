using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public FurnitureScoreManager furnitureScoreManager;
    public HouseScoreManager houseScoreManager;
    public ScoreUpdatePopup scoreUpdatePopup;


    private float[] totalFireSafetyScores = new float[3];
    private float[] totalSustainabilityScores = new float[3];
    public static ScoreManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        furnitureScoreManager.OnScoresUpdated += (index) => CalculateTotalScores(index);
        houseScoreManager.OnScoresUpdated += (index) => CalculateTotalScores(index);
    }

    void OnDestroy()
    {
        furnitureScoreManager.OnScoresUpdated -= (index) => CalculateTotalScores(index);
        houseScoreManager.OnScoresUpdated -= (index) => CalculateTotalScores(index);
    }

    public float GetTotalFireSafetyScore(int houseIndex)
    {
        return totalFireSafetyScores[Mathf.Clamp(houseIndex, 0, totalFireSafetyScores.Length - 1)];
    }

    public float GetTotalSustainabilityScore(int houseIndex)
    {
        return totalSustainabilityScores[Mathf.Clamp(houseIndex, 0, totalSustainabilityScores.Length - 1)];
    }

    public void CalculateTotalScores(int houseIndex)
    {
        totalFireSafetyScores[houseIndex] = furnitureScoreManager.GetTotalFireSafetyScore(houseIndex) + houseScoreManager.GetTotalFireSafetyScore(houseIndex);
        totalSustainabilityScores[houseIndex] = furnitureScoreManager.GetTotalSustainabilityScore(houseIndex) + houseScoreManager.GetTotalSustainabilityScore(houseIndex);

        if (scoreUpdatePopup != null)
        {
            string message = $"House {houseIndex + 1} Scores Updated: Fire Safety: {totalFireSafetyScores[houseIndex]}, Sustainability: {totalSustainabilityScores[houseIndex]}";
            scoreUpdatePopup.ShowScoreUpdate(message);
        }
    }
}
