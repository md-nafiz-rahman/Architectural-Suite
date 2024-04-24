using System;
using UnityEngine;

public class HouseScoreManager : MonoBehaviour
{
    public static HouseScoreManager Instance;

    private float[] fireSafetyScores; 
    private float[] sustainabilityScores;
    public event Action<int> OnScoresUpdated;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            int numHouses = 3;
            fireSafetyScores = new float[numHouses];
            sustainabilityScores = new float[numHouses];
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateScores(int houseIndex, float fireSafetyDelta, float sustainabilityDelta)
    {
        if (houseIndex < 0 || houseIndex >= fireSafetyScores.Length)
            return;

        fireSafetyScores[houseIndex] += fireSafetyDelta * 0.25f; ;
        sustainabilityScores[houseIndex] += sustainabilityDelta * 0.25f;

        OnScoresUpdated?.Invoke(houseIndex);

        Debug.Log($"Updated House {houseIndex + 1} Scores - Fire Safety: {fireSafetyScores[houseIndex]}, Sustainability: {sustainabilityScores[houseIndex]}");
    }

    public void ResetScores()
    {
        for (int i = 0; i < fireSafetyScores.Length; i++)
        {
            fireSafetyScores[i] = 0;
            sustainabilityScores[i] = 0;
            OnScoresUpdated?.Invoke(i);
        }
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
