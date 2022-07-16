using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom SO/Color Dice Data")]
public class DiceTerrainColorSO : ScriptableObject {
    [SerializeField] private Color dice01 = new Color();
    [SerializeField] private Color dice02 = new Color();
    [SerializeField] private Color dice03 = new Color();
    [SerializeField] private Color dice04 = new Color();
    [SerializeField] private Color dice05 = new Color();
}
