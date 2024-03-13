using UnityEngine;


public enum FurnitureCategory
{
    Bed,
    Table,
}

[CreateAssetMenu(fileName = "New FurnitureItem", menuName = "Inventory/FurnitureItem")]
public class FurnitureItem : ScriptableObject
{
    public string itemName; 
    public Sprite icon; 
    public GameObject prefab;
    public FurnitureCategory category; 
}
