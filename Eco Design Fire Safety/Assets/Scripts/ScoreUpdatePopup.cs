using UnityEngine;
using TMPro;

public class ScoreUpdatePopup : MonoBehaviour
{
    public GameObject popupObject;
    public TextMeshProUGUI scoreText;
    public float displayDuration = 2.0f;

    private void Start()
    {
        popupObject.SetActive(false);
    }

    public void ShowScoreUpdate(string message)
    {
        scoreText.text = message;
        popupObject.SetActive(true);
        Invoke("HidePopup", displayDuration);
    }

    private void HidePopup()
    {
        popupObject.SetActive(false);
    }
}
