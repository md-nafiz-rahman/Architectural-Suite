using UnityEngine;

public class HouseInteraction : MonoBehaviour
{
    public GameObject messagePanelHouse1;
    public GameObject messagePanelHouse2;
    public GameObject messagePanelHouse3;

    public GameObject customizationPanelHouse1;
    public GameObject customizationPanelHouse2;
    public GameObject customizationPanelHouse3;

    private GameObject currentLampPost = null;
    private bool isCustomizationPanelOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (currentLampPost != null && !isCustomizationPanelOpen)
            {
                ShowCustomizationPanel(currentLampPost.tag);
                FreezeGame(true);
                isCustomizationPanelOpen = true; 

            }
            else if (isCustomizationPanelOpen)
            {
                HideAllPanels();
                FreezeGame(false);
                isCustomizationPanelOpen = false; 
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("House1LampPost") || other.CompareTag("House2LampPost") || other.CompareTag("House3LampPost"))
        {
            currentLampPost = other.gameObject;
            if (!isCustomizationPanelOpen)
            { 
                ShowMessagePanel(currentLampPost.tag);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentLampPost != null && other.gameObject == currentLampPost)
        {
            currentLampPost = null;
            if (!isCustomizationPanelOpen)
            { 
                HideAllPanels(); 
            }
        }
    }

    private void ShowMessagePanel(string tag)
    {
        HideAllCustomizationPanels();

        switch (tag)
        {
            case "House1LampPost":
                messagePanelHouse1.SetActive(true);
                break;
            case "House2LampPost":
                messagePanelHouse2.SetActive(true);
                break;
            case "House3LampPost":
                messagePanelHouse3.SetActive(true);
                break;
        }
    }

    private void ShowCustomizationPanel(string tag)
    {
        HideAllPanels();
        isCustomizationPanelOpen = true;

        switch (tag)
        {
            case "House1LampPost":
                customizationPanelHouse1.SetActive(true);
                break;
            case "House2LampPost":
                customizationPanelHouse2.SetActive(true);
                break;
            case "House3LampPost":
                customizationPanelHouse3.SetActive(true);
                break;
        }
    }

    private void HideAllPanels()
    {
        messagePanelHouse1.SetActive(false);
        messagePanelHouse2.SetActive(false);
        messagePanelHouse3.SetActive(false);

        HideAllCustomizationPanels();
    }

    private void HideAllCustomizationPanels()
    {
        customizationPanelHouse1.SetActive(false);
        customizationPanelHouse2.SetActive(false);
        customizationPanelHouse3.SetActive(false);
    }

    private void FreezeGame(bool freeze)
    {
        Time.timeScale = freeze ? 0 : 1;
        Cursor.lockState = freeze ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = freeze;
    }
}
