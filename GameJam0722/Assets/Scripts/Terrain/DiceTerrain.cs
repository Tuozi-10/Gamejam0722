using UnityEngine;

public class DiceTerrain : MonoBehaviour {
    public DiceClass diceData;
    public Vector2Int pos => new Vector2Int(diceData.dicePos.x, diceData.dicePos.y);
    public float heightRandomness = 0;
    
    [SerializeField] private DiceTerrainMaterialSO materialData = null;
    [SerializeField] private MeshRenderer diceRend = null;

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
    public void UpdateDiceValue() => diceRend.sharedMaterial = materialData.DiceMaterialData[diceData.diceValue];

    /// <summary>
    /// When changes are made to the variable
    /// </summary>
    private void OnValidate() {
        UpdateDiceValue();
        if (diceData.diceValue == 0) diceData.diceEffectState = DiceEffectState.Spawner;
        if (diceData.diceState == DiceState.ForceHole && diceData.diceEffectState == DiceEffectState.Spawner) diceData.diceState = DiceState.Walkable;
    }
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
    Hole,
    ForceHole
}

public enum DiceEffectState {
    None,
    Start,
    End,
    Effect,
    Spawner
}