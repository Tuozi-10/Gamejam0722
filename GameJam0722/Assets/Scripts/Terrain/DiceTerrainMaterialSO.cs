using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom SO/Color Dice Data")]
public class DiceTerrainMaterialSO : ScriptableObject {
    [SerializeField, NonReorderable] private List<Material> diceMaterialData = new List<Material>(5);
    public List<Material> DiceMaterialData => diceMaterialData;
}
