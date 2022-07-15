using UnityEngine;

public class DiceTerrain : MonoBehaviour {
    [SerializeField, Range(0,5)] private int diceValue = 1;
    public int DiceValue => diceValue;
    
    
    [SerializeField] private DiceState diceState = DiceState.Walkable;
    public DiceState DiceState {
        get => diceState;
        set => diceState = value;
    }

    
    [SerializeField] private DiceEffectState diceEffectState = DiceEffectState.None;

    public DiceEffectState DiceEffectState {
        get => diceEffectState;
        set => diceEffectState = value;
    }
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