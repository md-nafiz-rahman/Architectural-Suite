
// CloseToFirePopup.cs is responsible for automatically displaying and hiding popup when furniture is placed close to fire hazards.

using UnityEngine;
public class CloseToFirePopup : MonoBehaviour
{
    public GameObject popupObject;

    // Ensure popup hidden at start.
    void Start()
    {
        if (popupObject != null)
        {
            popupObject.SetActive(false);
        }
    }

    // Activate the popup for 2 seconds.
    public void ShowPopup()
    {
        if (popupObject != null)
        {
            popupObject.SetActive(true);
            Invoke("HidePopup", 2.0f);
        }
    }

    // Hide the popup.
    public void HidePopup()
    {
        if (popupObject != null)
        {
            popupObject.SetActive(false);
        }
    }
}
