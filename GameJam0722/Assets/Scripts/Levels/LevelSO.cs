using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[CreateAssetMenu(menuName = "Custom SO/New Level")]
public class LevelSO : ScriptableObject {
    [SerializeField] private Vector2 terrainSize = new Vector2(0, 0);
    public Vector2 TerrainSize => terrainSize;
    
    [Space]
    
    [SerializeField] private bool useCustomColorDice = true;
    public bool UseCustomColorDice => useCustomColorDice;
    
    [SerializeField, NonReorderable] private List<DiceClass> diceClass = new List<DiceClass>();
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

[System.Serializable]
public class Level {
    public (int x, int y) terrainSize;
    public bool useRandom;
    public List<DiceClass> DiceClass;

    public static Level CreateLevel(LevelSO level) {
        var newCreatedLevel = new Level {
            terrainSize = ((int) level.TerrainSize.x, (int)level.TerrainSize.y), 
            DiceClass = new (level.DiceClass),
            useRandom = level.UseCustomColorDice
        };
        return newCreatedLevel.DeepClone();
    }
}

public static class LevelCreationHelper 
{
    public static T DeepClone<T>(this T obj)
    {
        using (var ms = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;

            return (T) formatter.Deserialize(ms);
        }
    }
}
