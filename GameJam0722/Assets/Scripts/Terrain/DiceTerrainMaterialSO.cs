using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom SO/Color Dice Data")]
public class DiceTerrainMaterialSO : ScriptableObject {
    [SerializeField] private List<Material> diceMaterialData = new List<Material>(6);
    public List<Material> DiceMaterialData => diceMaterialData;
    
    
    [SerializeField] private List<Color> diceColorLightData = new List<Color>(6);
    public List<Color> DiceColorLightData => diceColorLightData;
}
