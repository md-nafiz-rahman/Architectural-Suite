using UnityEngine;

public class DoorObstructionPopup : MonoBehaviour
{
    public GameObject popupObject;

    void Start()
    {
        if (popupObject != null)
        {
            popupObject.SetActive(false);
        }
    }

    public void ShowPopup()
    {
        if (popupObject != null)
        {
            popupObject.SetActive(true);
            Invoke("HidePopup", 2.0f); 
        }
    }

    public void HidePopup()
    {
        if (popupObject != null)
        {
            popupObject.SetActive(false);
        }
    }

}
