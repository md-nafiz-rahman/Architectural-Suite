using UnityEngine;

// RenewablePopup.cs is responsible for automatically displaying and hiding popup when placing renewable energy sources (Solar Panel).
public class RenewablePopup : MonoBehaviour
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
