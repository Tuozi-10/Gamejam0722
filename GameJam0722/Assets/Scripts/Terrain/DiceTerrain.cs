using UnityEngine;

public class DiceTerrain : MonoBehaviour {
    public DiceClass diceData;

    //CENTER // UP RIGHT // UP LEFT // BOTTOM RIGHT // BOTTOM LEFT
    [SerializeField] private GameObject[] diceCubeGam = new GameObject[5];

    /// <summary>
    /// Initialize dice data value
    /// </summary>
    /// <param name="dicePos"></param>
    /// <param name="diceState"></param>
    public void InitDice(Vector2Int dicePos, DiceState diceState = DiceState.Walkable) {
        diceData.dicePos = (dicePos.x, dicePos.y);
        diceData.diceState = diceState;
    }
    
    
    public void UpdateDiceValue() {
        return;
        diceCubeGam[0].SetActive(diceData.diceValue % 2 == 1);
        diceCubeGam[1].SetActive(diceData.diceValue >= 4);
        diceCubeGam[2].SetActive(diceData.diceValue >= 2);
        diceCubeGam[3].SetActive(diceData.diceValue >= 2);
        diceCubeGam[4].SetActive(diceData.diceValue >= 4);
    }
    
    /// <summary>
    /// When changes are made to the variable
    /// </summary>
    private void OnValidate() => UpdateDiceValue();
}

[System.Serializable]
public class DiceClass {
    [Range(0, 5)] public int diceValue;
    public (int x, int y) dicePos;
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