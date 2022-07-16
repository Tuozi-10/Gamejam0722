using DG.Tweening;
using UnityEngine;

public class DiceTerrain : MonoBehaviour {
    public DiceClass diceData;
    
    public Vector2Int pos => new Vector2Int(diceData.dicePos.x, diceData.dicePos.y);
    public float heightRandomness = 0;
    
    [SerializeField] private DiceTerrainMaterialSO materialData = null;
    [SerializeField] private MeshRenderer diceRend = null;

    [SerializeField] private Transform pivot;

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
    public void UpdateDiceData() {
        diceRend.sharedMaterial = materialData.DiceMaterialData[diceData.diceValue];
        if (diceData.diceValue == 0) diceData.diceEffectState = DiceEffectState.Spawner;
    }

    public void DoShakePivot(float intensity, float duration, float delay)
    {
        //pivot.DOKill();
        pivot.transform.DOLocalMoveY(-intensity, duration).SetDelay(delay).OnComplete(()=>pivot.transform.DOLocalMoveY(0, duration * 0.5f));
    }
    
    /// <summary>
    /// When changes are made to the variable
    /// </summary>
    private void OnValidate() => UpdateDiceData();
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

/// <summary>
/// Main Dice State
/// </summary>
public enum DiceState {
    Walkable,
    Wall,
    Hole,
    ForceHole
}

/// <summary>
/// Secondary State for dice
/// </summary>
public enum DiceEffectState {
    None,
    Start,
    End,
    Effect,
    Spawner
}