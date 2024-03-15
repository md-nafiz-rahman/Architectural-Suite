using UnityEngine;

public class HouseInteraction : MonoBehaviour
{
    public GameObject panelHouse1; 
    public GameObject panelHouse2; 
    public GameObject panelHouse3; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("House1LampPost"))
        {
            ShowPanel(panelHouse1);
        }
        else if (other.CompareTag("House2LampPost"))
        {
            ShowPanel(panelHouse2);
        }
        else if (other.CompareTag("House3LampPost"))
        {
            ShowPanel(panelHouse3);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("House1LampPost") || other.CompareTag("House2LampPost") || other.CompareTag("House3LampPost"))
        {
            HideAllPanels();
        }
    }

    private void ShowPanel(GameObject panelToShow)
    {
        HideAllPanels();
        panelToShow.SetActive(true);
    }

    private void HideAllPanels()
    {
        panelHouse1.SetActive(false);
        panelHouse2.SetActive(false);
        panelHouse3.SetActive(false);
    }
}
