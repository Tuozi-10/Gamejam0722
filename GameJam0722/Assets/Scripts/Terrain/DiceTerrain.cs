using UnityEngine;

public class DiceTerrain : MonoBehaviour {
    public DiceClass diceData;
    public Vector2Int pos =>new Vector2Int(diceData.dicePos.x, diceData.dicePos.y);

    [SerializeField] private DiceTerrainMaterialSO materialData = null;
    [SerializeField] private MeshRenderer diceRend = null;
    
    //CENTER // UP RIGHT // UP LEFT // BOTTOM RIGHT // BOTTOM LEFT
    [SerializeField] private GameObject[] diceCubeGam = new GameObject[5];

    /// <summary>
    /// Initialize dice data value
    /// </summary>
    /// <param name="dicePos"></param>
    /// <param name="diceState"></param>
    public void InitDice(Vector2Int dicePos, DiceState diceState = DiceState.Walkable) {
        diceData.dicePosX = dicePos.x;
        diceData.dicePosY = dicePos.y;
        diceData.diceState = diceState;
    }

    /// <summary>
    /// Update the dice when variables are changed
    /// </summary>
    public void UpdateDiceValue() => diceRend.sharedMaterial = materialData.DiceMaterialData[diceData.diceValue - 1];
    
    /// <summary>
    /// When changes are made to the variable
    /// </summary>
    private void OnValidate() => UpdateDiceValue();
}

[System.Serializable]
public class DiceClass {
    [Range(0, 5)] public int diceValue;
    public (int x, int y) dicePos => (dicePosX, dicePosY);
    public int dicePosX;
    public int dicePosY;
    public DiceState diceState = DiceState.Walkable;
    public DiceEffectState diceEffectState = DiceEffectState.None;
}

public enum DiceState {
    Walkable,
    Wall,
    Hole
}

public enum DiceEffectState {
    None,
    Start,
    End,
    Effect
}