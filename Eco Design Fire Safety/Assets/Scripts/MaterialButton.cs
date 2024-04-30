
// MaterialButton.cs is responsible for setting up the UI button that selects the material for the floor or wall of a specific room in a specific house.
// Each material button is linked to a specific material for the house wall and floor.

using UnityEngine;
using UnityEngine.UI;

public class MaterialButton : MonoBehaviour
{
    public HouseInteraction houseInteraction;
    public int materialIndex;
    public bool isWallMaterial;
    public string roomIdentifier; 

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
            houseInteraction.OnMaterialSelected(roomIdentifier, materialIndex, isWallMaterial));
    }
}

