using UnityEngine;


public enum FurnitureCategory
{
    Bed,
    TableDesk,
    SofaChair,
    Cabinet,
    Decoration,
    Other
}

[CreateAssetMenu(fileName = "New FurnitureItem", menuName = "Inventory/FurnitureItem")]
public class FurnitureItem : ScriptableObject
{
    public string itemName; 
    public Sprite icon; 
    public GameObject prefab;
    public FurnitureCategory category; 
}
