using UnityEngine;

[CreateAssetMenu(fileName = "New MaterialData", menuName = "Material Data")]
public class MaterialData : ScriptableObject
{
    public string materialName;
    public Material material;
    public float fireSafetyScore;
    public float sustainabilityScore;

}
