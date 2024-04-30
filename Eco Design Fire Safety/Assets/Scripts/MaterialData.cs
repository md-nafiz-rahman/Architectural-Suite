
// MaterialData.cs defines a scriptable object to hold data about materials, this data includes material name, physical material, fire-safety score and sustainability score.

using UnityEngine;

[CreateAssetMenu(fileName = "New MaterialData", menuName = "Material Data")]
public class MaterialData : ScriptableObject
{
    public string materialName;
    public Material material;
    public float fireSafetyScore;
    public float sustainabilityScore;

}
