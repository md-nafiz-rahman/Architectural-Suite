using UnityEngine;
using TMPro;
using System.Text;
using System.IO;
using System.Collections;

public class FeedBackManager : MonoBehaviour
{
    public static FeedBackManager Instance;
    public TextMeshProUGUI feedbackText;
    public GameObject feedbackCanvas;
    public HouseInteraction houseInteraction;
    private int activeHouseIndex = -1;
    public GameObject exportFeedbackPanel;


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

    public void ShowFeedback(int houseIndex)
    {
        string feedback = GenerateFeedback(houseIndex);
        feedbackText.text = feedback;
        feedbackCanvas.SetActive(true);

        activeHouseIndex = houseIndex;
        houseInteraction.customizationPanelHouse1.SetActive(false);
        houseInteraction.customizationPanelHouse2.SetActive(false);
        houseInteraction.customizationPanelHouse3.SetActive(false);
    }

    public void GoBack()
    {
        feedbackCanvas.SetActive(false);
        switch (activeHouseIndex)
        {
            case 0:
                houseInteraction.customizationPanelHouse1.SetActive(true);
                break;
            case 1:
                houseInteraction.customizationPanelHouse2.SetActive(true);
                break;
            case 2:
                houseInteraction.customizationPanelHouse3.SetActive(true);
                break;
        }
        activeHouseIndex = -1;
    }

    public void ExportFeedback()
    {
        string fileName = "Feedback_House_" + (activeHouseIndex + 1) + ".txt";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        StringBuilder fileContent = new StringBuilder();

        fileContent.AppendLine("Eco-Design & Safety Architectural Suite");
        fileContent.AppendLine("Feedback Report");
        fileContent.AppendLine("Date and Time: " + System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
        fileContent.AppendLine("");
        fileContent.AppendLine("Feedback Details:");
        fileContent.AppendLine("");
        fileContent.AppendLine(feedbackText.text);

        try
        {
            File.WriteAllText(filePath, fileContent.ToString());
            StartCoroutine(ShowExportSuccessMessage("Feedback exported successfully to: " + filePath));
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to export feedback: " + ex.Message);
        }
    }

    IEnumerator ShowExportSuccessMessage(string message)
    {
        exportFeedbackPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
        exportFeedbackPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        exportFeedbackPanel.SetActive(false);
    }



    private string GenerateFeedback(int houseIndex)
    {
        StringBuilder feedbackBuilder = new StringBuilder();
        float fireSafetyScore = ScoreManager.Instance.GetTotalFireSafetyScore(houseIndex);
        float sustainabilityScore = ScoreManager.Instance.GetTotalSustainabilityScore(houseIndex);
        int furnitureCount = FurnitureScoreManager.Instance.houseFurnitures[houseIndex].Count;
        bool hasMaterialChanges = MaterialSelection.Instance.HasMaterialChanges(houseIndex);

        if (fireSafetyScore == 0 && sustainabilityScore == 0)
        {
            feedbackBuilder.AppendLine("No available feedback, need to interact with house and furniture first.");
        }

        if (fireSafetyScore ==  0)
        {
            feedbackBuilder.AppendLine("Make sure furniture placement does not obstruct safety exits and furniture is not placed close to fire hazards.");
        }

        if (sustainabilityScore == 0)
        {
            feedbackBuilder.AppendLine("Make sure to properly choose wall, floor and furniture materials to reflect good sustainability score.");
        }

        if (!hasMaterialChanges && furnitureCount == 0)
        {
            feedbackBuilder.AppendLine("To get better score, you need to change house materials or furniture materials.");
        }

        if (furnitureCount == 0)
        {
            feedbackBuilder.AppendLine("Please make sure you have selected furniture with furniture material to place in the house");
        }

        if (FurnitureScoreManager.Instance.CheckDoorObstruction(houseIndex))
        {
            feedbackBuilder.AppendLine("You have placed furniture that is obstructing safe fire exit.");
        }

        if (FurnitureScoreManager.Instance.CheckCloseProximityToFire(houseIndex))
        {
            feedbackBuilder.AppendLine("You have placed furniture too close to fire hazard.");
        }

        if (!MaterialSelection.Instance.AllMaterialsChanged(houseIndex))
        {
            feedbackBuilder.AppendLine("Changing all house wall and floor materials will reflect better feedback.");
        }

        if (furnitureCount > 25)
        {
            feedbackBuilder.AppendLine("Too many furniture items may affect score negatively.");
        }

        if (furnitureCount == 20 && (fireSafetyScore + sustainabilityScore) < 50)
        {
            feedbackBuilder.AppendLine("Try using more sustainable and fire safe materials to improve scores.");
        }

        if (fireSafetyScore + sustainabilityScore == 60)
        {
            feedbackBuilder.AppendLine("Good effort, but using more sustainable and fire safe materials could enhance scores further.");
        }

        if (fireSafetyScore + sustainabilityScore >= 70)
        {
            feedbackBuilder.AppendLine("Well done, your house is fire-safe and sustainable.");
        }

        if (feedbackBuilder.Length == 0)
        {
            feedbackBuilder.Append("For specific feedback, adjust your interiors more!");
        }

        return feedbackBuilder.ToString();
    }
}
