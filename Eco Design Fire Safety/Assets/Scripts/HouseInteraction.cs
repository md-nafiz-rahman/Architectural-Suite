using UnityEngine;

public class HouseInteraction : MonoBehaviour
{
    public GameObject messagePanelHouse1;
    public GameObject messagePanelHouse2;
    public GameObject messagePanelHouse3;

    public GameObject customizationPanelHouse1;
    public GameObject customizationPanelHouse2;
    public GameObject customizationPanelHouse3;

    public GameObject groundFloorPanelHouse1;
    public GameObject firstFloorPanelHouse1;
    public GameObject groundFloorPanelHouse2;
    public GameObject firstFloorPanelHouse2;
    public GameObject groundFloorPanelHouse3;
    public GameObject firstFloorPanelHouse3;

    public GameObject livingRoomMaterialPanelHouse1;
    public GameObject drawingRoomMaterialPanelHouse1;
    public GameObject guestRoomMaterialPanelHouse1;
    public GameObject kitchenMaterialPanelHouse1;
    public GameObject bedroom1MaterialPanelHouse1;
    public GameObject bedroom2MaterialPanelHouse1;
    public GameObject masterBedroomMaterialPanelHouse1;
    public GameObject loungeMaterialPanelHouse1;

    public GameObject livingRoomMaterialPanelHouse2;
    public GameObject drawingRoomMaterialPanelHouse2;
    public GameObject guestRoomMaterialPanelHouse2;
    public GameObject kitchenMaterialPanelHouse2;
    public GameObject bedroom1MaterialPanelHouse2;
    public GameObject bedroom2MaterialPanelHouse2;
    public GameObject masterBedroomMaterialPanelHouse2;
    public GameObject loungeMaterialPanelHouse2;

    public GameObject livingRoomMaterialPanelHouse3;
    public GameObject drawingRoomMaterialPanelHouse3;
    public GameObject guestRoomMaterialPanelHouse3;
    public GameObject kitchenMaterialPanelHouse3;
    public GameObject bedroom1MaterialPanelHouse3;
    public GameObject bedroom2MaterialPanelHouse3;
    public GameObject masterBedroomMaterialPanelHouse3;
    public GameObject loungeMaterialPanelHouse3;

    public GameObject messageGroundFloorHouse1;
    public GameObject messageFirstFloorHouse1;
    public GameObject messageGroundFloorHouse2;
    public GameObject messageFirstFloorHouse2;
    public GameObject messageGroundFloorHouse3;
    public GameObject messageFirstFloorHouse3;

    private GameObject currentLampPost = null;
    private bool isCustomizationPanelOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ToggleCustomizationPanel();
        }
    }

    public void ShowRoomMaterialPanel(string houseTag, string room)
    {
        HideAllRoomMaterialPanels();
        HideSelectionMessage(houseTag);

        GameObject roomPanel = null;
        switch (houseTag)
        {
            case "House1LampPost":
                roomPanel = GetRoomPanelForHouse(room, 1);
                break;
            case "House2LampPost":
                roomPanel = GetRoomPanelForHouse(room, 2);
                break;
            case "House3LampPost":
                roomPanel = GetRoomPanelForHouse(room, 3);
                break;
        }

        if (roomPanel != null)
        {
            roomPanel.SetActive(true);
            isCustomizationPanelOpen = true;
        }
    }

    private void HideSelectionMessage(string houseTag)
    {
        switch (houseTag)
        {
            case "House1LampPost":
                messageGroundFloorHouse1.SetActive(false);
                messageFirstFloorHouse1.SetActive(false);
                break;
            case "House2LampPost":
                messageGroundFloorHouse2.SetActive(false);
                messageFirstFloorHouse2.SetActive(false);
                break;
            case "House3LampPost":
                messageGroundFloorHouse3.SetActive(false);
                messageFirstFloorHouse3.SetActive(false);
                break;
        }
    }

    private GameObject GetRoomPanelForHouse(string room, int houseNumber)
    {
        switch (room)
        {
            case "LivingRoom":
                return houseNumber == 1 ? livingRoomMaterialPanelHouse1 : (houseNumber == 2 ? livingRoomMaterialPanelHouse2 : livingRoomMaterialPanelHouse3);
            case "DrawingRoom":
                return houseNumber == 1 ? drawingRoomMaterialPanelHouse1 : (houseNumber == 2 ? drawingRoomMaterialPanelHouse2 : drawingRoomMaterialPanelHouse3);
            case "GuestRoom":
                return houseNumber == 1 ? guestRoomMaterialPanelHouse1 : (houseNumber == 2 ? guestRoomMaterialPanelHouse2 : guestRoomMaterialPanelHouse3);
            case "Kitchen":
                return houseNumber == 1 ? kitchenMaterialPanelHouse1 : (houseNumber == 2 ? kitchenMaterialPanelHouse2 : kitchenMaterialPanelHouse3);
            case "Bedroom1":
                return houseNumber == 1 ? bedroom1MaterialPanelHouse1 : (houseNumber == 2 ? bedroom1MaterialPanelHouse2 : bedroom1MaterialPanelHouse3);
            case "Bedroom2":
                return houseNumber == 1 ? bedroom2MaterialPanelHouse1 : (houseNumber == 2 ? bedroom2MaterialPanelHouse2 : bedroom2MaterialPanelHouse3);
            case "MasterBedroom":
                return houseNumber == 1 ? masterBedroomMaterialPanelHouse1 : (houseNumber == 2 ? masterBedroomMaterialPanelHouse2 : masterBedroomMaterialPanelHouse3);
            case "Lounge":
                return houseNumber == 1 ? loungeMaterialPanelHouse1 : (houseNumber == 2 ? loungeMaterialPanelHouse2 : loungeMaterialPanelHouse3);
        }

        return null;
    }



    private void HideAllRoomMaterialPanels()
    {
        livingRoomMaterialPanelHouse1.SetActive(false);
        drawingRoomMaterialPanelHouse1.SetActive(false);
        guestRoomMaterialPanelHouse1.SetActive(false);
        kitchenMaterialPanelHouse1.SetActive(false);
        bedroom1MaterialPanelHouse1.SetActive(false);
        bedroom2MaterialPanelHouse1.SetActive(false);
        masterBedroomMaterialPanelHouse1.SetActive(false);
        loungeMaterialPanelHouse1.SetActive(false);

        livingRoomMaterialPanelHouse2.SetActive(false);
        drawingRoomMaterialPanelHouse2.SetActive(false);
        guestRoomMaterialPanelHouse2.SetActive(false);
        kitchenMaterialPanelHouse2.SetActive(false);
        bedroom1MaterialPanelHouse2.SetActive(false);
        bedroom2MaterialPanelHouse2.SetActive(false);
        masterBedroomMaterialPanelHouse2.SetActive(false);
        loungeMaterialPanelHouse2.SetActive(false);

        livingRoomMaterialPanelHouse3.SetActive(false);
        drawingRoomMaterialPanelHouse3.SetActive(false);
        guestRoomMaterialPanelHouse3.SetActive(false);
        kitchenMaterialPanelHouse3.SetActive(false);
        bedroom1MaterialPanelHouse3.SetActive(false);
        bedroom2MaterialPanelHouse3.SetActive(false);
        masterBedroomMaterialPanelHouse3.SetActive(false);
        loungeMaterialPanelHouse3.SetActive(false);

    }


    private void ToggleCustomizationPanel()
    {
        if (currentLampPost != null && !isCustomizationPanelOpen)
        {
            ShowCustomizationPanel(currentLampPost.tag);
            FreezeGame(true);
            isCustomizationPanelOpen = true;
        }
        else if (IsAnyCustomizationPanelOpen())
        {
            HideAllPanels();
            HideAllWallFloorPanels(); 
            FreezeGame(false);
            isCustomizationPanelOpen = false;
        }
    }

    private bool IsAnyCustomizationPanelOpen()
    {
        return customizationPanelHouse1.activeSelf ||
               customizationPanelHouse2.activeSelf ||
               customizationPanelHouse3.activeSelf ||
               groundFloorPanelHouse1.activeSelf ||
               firstFloorPanelHouse1.activeSelf ||
               groundFloorPanelHouse2.activeSelf ||
               firstFloorPanelHouse2.activeSelf ||
               groundFloorPanelHouse3.activeSelf ||
               firstFloorPanelHouse3.activeSelf;
    }

    public void ShowGroundFloorPanel(string houseTag)
    {
        HideAllCustomizationPanels(); 
        ActivateFloorPanel(houseTag, true);
    }

    public void ShowFirstFloorPanel(string houseTag)
    {
        HideAllCustomizationPanels();
        ActivateFloorPanel(houseTag, false);
    }

    private void ActivateFloorPanel(string houseTag, bool isGroundFloor)
    {
        GameObject panelToShow = null;
        switch (houseTag)
        {
            case "House1LampPost":
                panelToShow = isGroundFloor ? groundFloorPanelHouse1 : firstFloorPanelHouse1;
                break;
            case "House2LampPost":
                panelToShow = isGroundFloor ? groundFloorPanelHouse2 : firstFloorPanelHouse2;
                break;
            case "House3LampPost":
                panelToShow = isGroundFloor ? groundFloorPanelHouse3 : firstFloorPanelHouse3;
                break;
        }

        if (panelToShow != null)
        {
            panelToShow.SetActive(true);
            isCustomizationPanelOpen = true;
        }
    }

    public void OnGoBackButtonClicked()
    {
        if (currentLampPost != null)
        {
            HideAllWallFloorPanels();
            ShowCustomizationPanel(currentLampPost.tag);
        }
    }

    private void HideAllWallFloorPanels()
    {
        groundFloorPanelHouse1.SetActive(false);
        firstFloorPanelHouse1.SetActive(false);
        groundFloorPanelHouse2.SetActive(false);
        firstFloorPanelHouse2.SetActive(false);
        groundFloorPanelHouse3.SetActive(false);
        firstFloorPanelHouse3.SetActive(false);
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

    public void OnLivingRoomButtonClickedHouse1()
    {
        ShowRoomMaterialPanel("House1LampPost", "LivingRoom");
    }

    public void OnDrawingRoomButtonClickedHouse1()
    {
        ShowRoomMaterialPanel("House1LampPost", "DrawingRoom");
    }

    public void OnGuestRoomButtonClickedHouse1()
    {
        ShowRoomMaterialPanel("House1LampPost", "GuestRoom");
    }

    public void OnKitchenButtonClickedHouse1()
    {
        ShowRoomMaterialPanel("House1LampPost", "Kitchen");
    }

    public void OnBedroom1ButtonClickedHouse1()
    {
        ShowRoomMaterialPanel("House1LampPost", "Bedroom1");
    }

    public void OnBedroom2ButtonClickedHouse1()
    {
        ShowRoomMaterialPanel("House1LampPost", "Bedroom2");
    }

    public void OnMasterBedroomButtonClickedHouse1()
    {
        ShowRoomMaterialPanel("House1LampPost", "MasterBedroom");
    }

    public void OnLoungeButtonClickedHouse1()
    {
        ShowRoomMaterialPanel("House1LampPost", "Lounge");
    }

    public void OnLivingRoomButtonClickedHouse2()
    {
        ShowRoomMaterialPanel("House2LampPost", "LivingRoom");
    }

    public void OnDrawingRoomButtonClickedHouse2()
    {
        ShowRoomMaterialPanel("House2LampPost", "DrawingRoom");
    }

    public void OnGuestRoomButtonClickedHouse2()
    {
        ShowRoomMaterialPanel("House2LampPost", "GuestRoom");
    }

    public void OnKitchenButtonClickedHouse2()
    {
        ShowRoomMaterialPanel("House2LampPost", "Kitchen");
    }

    public void OnBedroom1ButtonClickedHouse2()
    {
        ShowRoomMaterialPanel("House2LampPost", "Bedroom1");
    }

    public void OnBedroom2ButtonClickedHouse2()
    {
        ShowRoomMaterialPanel("House2LampPost", "Bedroom2");
    }

    public void OnMasterBedroomButtonClickedHouse2()
    {
        ShowRoomMaterialPanel("House2LampPost", "MasterBedroom");
    }

    public void OnLoungeButtonClickedHouse2()
    {
        ShowRoomMaterialPanel("House2LampPost", "Lounge");
    }

    public void OnLivingRoomButtonClickedHouse3()
    {
        ShowRoomMaterialPanel("House3LampPost", "LivingRoom");
    }

    public void OnDrawingRoomButtonClickedHouse3()
    {
        ShowRoomMaterialPanel("House3LampPost", "DrawingRoom");
    }

    public void OnGuestRoomButtonClickedHouse3()
    {
        ShowRoomMaterialPanel("House3LampPost", "GuestRoom");
    }

    public void OnKitchenButtonClickedHouse3()
    {
        ShowRoomMaterialPanel("House3LampPost", "Kitchen");
    }

    public void OnBedroom1ButtonClickedHouse3()
    {
        ShowRoomMaterialPanel("House3LampPost", "Bedroom1");
    }

    public void OnBedroom2ButtonClickedHouse3()
    {
        ShowRoomMaterialPanel("House3LampPost", "Bedroom2");
    }

    public void OnMasterBedroomButtonClickedHouse3()
    {
        ShowRoomMaterialPanel("House3LampPost", "MasterBedroom");
    }

    public void OnLoungeButtonClickedHouse3()
    {
        ShowRoomMaterialPanel("House3LampPost", "Lounge");
    }

}
