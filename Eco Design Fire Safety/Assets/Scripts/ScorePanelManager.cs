
// ScorePanelManager.cs is responsible for managing the UI panels that display the fire-safety and sustainability scores for each house.

using UnityEngine;
using TMPro;

public class ScorePanelManager : MonoBehaviour
{

    public GameObject[] fireSafetyPanels;
    public GameObject[] sustainabilityPanels;
    public TMP_Text[] fireSafetyTotalScores;
    public TMP_Text[] fireSafetyFurnitureScores;
    public TMP_Text[] fireSafetyHouseScores;
    public TMP_Text[] sustainabilityTotalScores;
    public TMP_Text[] sustainabilityFurnitureScores;
    public TMP_Text[] sustainabilityHouseScores;
    public GameObject[] houseCustomizationPanels;

    public FurnitureScoreManager furnitureScoreManager;
    public HouseScoreManager houseScoreManager;
    public ScoreManager scoreManager;

    public void ShowFireSafetyPanel(int houseIndex)
    {
        SetScores(houseIndex, true);
        houseCustomizationPanels[houseIndex].SetActive(false);

    }

    public void ShowSustainabilityPanel(int houseIndex)
    {
        SetScores(houseIndex, false);
        houseCustomizationPanels[houseIndex].SetActive(false);

    }

    public void GoBackToCustomizationPanel(int houseIndex)
    {
        CloseAllPanels();
        FreezeGame(true);
        houseCustomizationPanels[houseIndex].SetActive(true);
    }

    // Sets scores in the UI for fire safety or sustainability for a specific house.
    private void SetScores(int houseIndex, bool isFireSafety)
    {
        CloseAllPanels();

        float furnitureScore, houseScore, totalScore;

        if (isFireSafety)
        {
            furnitureScore = furnitureScoreManager.GetTotalFireSafetyScore(houseIndex);
            houseScore = houseScoreManager.GetTotalFireSafetyScore(houseIndex);
            totalScore = scoreManager.GetTotalFireSafetyScore(houseIndex);
            fireSafetyTotalScores[houseIndex].text = $"{totalScore}";
            fireSafetyFurnitureScores[houseIndex].text = $"{furnitureScore}";
            fireSafetyHouseScores[houseIndex].text = $"{houseScore}";
            fireSafetyPanels[houseIndex].SetActive(true);
        }
        else
        {
            furnitureScore = furnitureScoreManager.GetTotalSustainabilityScore(houseIndex);
            houseScore = houseScoreManager.GetTotalSustainabilityScore(houseIndex);
            totalScore = scoreManager.GetTotalSustainabilityScore(houseIndex);
            sustainabilityTotalScores[houseIndex].text = $"{totalScore}";
            sustainabilityFurnitureScores[houseIndex].text = $"{furnitureScore}";
            sustainabilityHouseScores[houseIndex].text = $"{houseScore}";
            sustainabilityPanels[houseIndex].SetActive(true);
        }

        FreezeGame(true);
    }



    private void CloseAllPanels()
    {
        foreach (var panel in fireSafetyPanels)
            panel.SetActive(false);

        foreach (var panel in sustainabilityPanels)
            panel.SetActive(false);

        FreezeGame(false);
    }

    private void FreezeGame(bool freeze)
    {
        Time.timeScale = freeze ? 0 : 1;
        Cursor.lockState = freeze ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = freeze;
    }
}
