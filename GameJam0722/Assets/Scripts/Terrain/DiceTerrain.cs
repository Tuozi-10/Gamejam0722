using UnityEngine;

public class DiceTerrain : MonoBehaviour {
    public DiceClass diceData;
}

[System.Serializable]
public class DiceClass {
    [Range(0, 5)] public int diceValue;
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