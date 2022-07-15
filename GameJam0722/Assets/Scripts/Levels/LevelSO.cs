using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom SO/New Level")]
public class LevelSO : ScriptableObject {
    [SerializeField] private Vector2 terrainSize = new Vector2(0, 0);
    public Vector2 TerrainSize => terrainSize;
    [SerializeField] private List<DiceClass> diceClass = new List<DiceClass>();
    public List<DiceClass> DiceClass => diceClass;


    /// <summary>
    /// Set all data for the level
    /// </summary>
    /// <param name="levelSize"></param>
    /// <param name="dicesClass"></param>
    public void SaveLevelData(Vector2 levelSize, List<DiceClass> dicesClass) {
        this.terrainSize = levelSize;
        this.diceClass = dicesClass;
    }
}
