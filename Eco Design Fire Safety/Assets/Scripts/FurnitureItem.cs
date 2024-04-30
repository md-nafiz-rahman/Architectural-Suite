using UnityEngine;

// Enum to categorise different types of furniture in the game.
public enum FurnitureCategory
{
    Bed,
    TableDesk,
    SofaChair,
    Cabinet,
    Decoration,
    Other
}

// Defines a scriptable object for furniture items that is used within the inventory system.
[CreateAssetMenu(fileName = "New FurnitureItem", menuName = "Inventory/FurnitureItem")]
public class FurnitureItem : ScriptableObject
{
    public string itemName; 
    public Sprite icon; 
    public GameObject prefab;
    public FurnitureCategory category;
    public MaterialData materialData;
}
