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

