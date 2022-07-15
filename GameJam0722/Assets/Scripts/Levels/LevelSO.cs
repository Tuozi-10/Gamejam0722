using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Custom SO/New Level")]
public class LevelSO : ScriptableObject {
    [SerializeField] private Vector2 terrainSize = new Vector2(0, 0);
    [SerializeField] private List<DiceClass> diceClass = new List<DiceClass>();

    /// <summary>
    /// Set all data for the level
    /// </summary>
    /// <param name="terrainSize"></param>
    /// <param name="dicesClass"></param>
    public void SaveLevelData(Vector2 terrainSize, List<DiceClass> dicesClass) {
        this.terrainSize = terrainSize;
        this.diceClass = dicesClass;
    }
}
