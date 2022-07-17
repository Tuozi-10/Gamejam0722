using System;
using DG.Tweening;
using UnityEngine;

public class DiceTerrain : MonoBehaviour {
    public DiceClass diceData;
    
    public Vector2Int pos => new Vector2Int(diceData.dicePos.x, diceData.dicePos.y);
    public float heightRandomness = 0;


    [SerializeField] private MeshRenderer objectMesh = null;
    public MeshRenderer ObjectMesh => objectMesh;
    [SerializeField] private DiceTerrainMaterialSO materialData = null;
    [SerializeField] private MeshRenderer diceRend = null;

    [SerializeField] public Transform pivot;

    public GameObject collectible;

    /// <summary>
    /// Initialize dice data value
    /// </summary>
    /// <param name="dicePos"></param>
    /// <param name="diceState"></param>
    public void InitDice(Vector2Int dicePos, DiceState diceState = DiceState.Walkable) 
    {
        diceData.dicePosX = dicePos.x;
        diceData.dicePosY = dicePos.y;
        diceData.diceState = diceState;
    }

    private void InitEffects()
    {
        if (diceData.diceEffectState == DiceEffectState.Collectible)
        {
            collectible = Instantiate(TerrainManager.instance.Collectible, pivot).gameObject;
            collectible.transform.localPosition = new Vector3(0, 0.78f, 0);
        }

    }

    /// <summary>
    /// Update the dice when variables are changed
    /// </summary>
    public void UpdateDiceData()
    {
        diceRend.sharedMaterial = materialData.DiceMaterialData[diceData.diceValue];
        if (diceData.diceState is DiceState.ForceHole or DiceState.ForceWall || diceData.diceEffectState is DiceEffectState.Spawner or DiceEffectState.End or DiceEffectState.Start) diceData.diceValue = 0;
      
        InitEffects();
    }

    public void UpdateDiceInEditor() {
        UpdateDiceData();
        
        transform.localPosition = diceData.diceState switch {
            DiceState.Wall => new Vector3(transform.localPosition.x, .25f, transform.localPosition.z),
            DiceState.ForceWall => new Vector3(transform.localPosition.x, .25f, transform.localPosition.z),
            DiceState.Hole => new Vector3(transform.localPosition.x, -.25f, transform.localPosition.z),
            DiceState.ForceHole => new Vector3(transform.localPosition.x, -1f, transform.localPosition.z),
            DiceState.ForceBeach => new Vector3(transform.localPosition.x, -.55f, transform.localPosition.z),
            _ => new Vector3(transform.localPosition.x, 0, transform.localPosition.z)
        };
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
    [Range(0, 6)] public int diceValue;
    public (int x, int y) dicePos => (dicePosX, dicePosY);
    public int dicePosX;
    public int dicePosY;
    public DiceState diceState = DiceState.Walkable;
    public DiceEffectState diceEffectState = DiceEffectState.None;
    public bool isWall => diceState is DiceState.Wall or DiceState.ForceWall;
}

/// <summary>
/// Main Dice State
/// </summary>
public enum DiceState {
    Walkable,
    Wall,
    Hole,
    ForceWall,
    ForceHole,
    ForceBeach
}

/// <summary>
/// Secondary State for dice
/// </summary>
public enum DiceEffectState {
    None,
    Start,
    End,
    Effect,
    Spawner,
    Collectible
}