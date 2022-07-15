using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainManager : Singleton<TerrainManager> {
    
    private DiceTerrain[,] diceTerrainlsit;

    [SerializeField] private float wallHeightValue = 0;
    [SerializeField] private float holeHeightValue = 0;
    [Space] 
    public bool SetRandomDiceValue = true;
    [SerializeField] private int upperDiceValue = 0;
    
    #region SetTerrainAtStart
    private void CheckDiceForChangingHeight() 
    {
        foreach (DiceTerrain dice in diceTerrainlsit) 
        {
            if(dice.diceData.diceState == DiceState.Wall) SetDiceHeight(dice, wallHeightValue);
            else if(dice.diceData.diceState == DiceState.Wall) SetDiceHeight(dice, holeHeightValue);
        }
        
        if(SetRandomDiceValue) upperDiceValue = Random.Range(1, 6);
        foreach (DiceTerrain dice in GetDiceWithSameValue(upperDiceValue)) 
        {
            if (dice.diceData.diceState != DiceState.Walkable || dice.diceData.diceEffectState != DiceEffectState.None) continue;
            SetDiceHeight(dice, wallHeightValue);
        }
    }

    /// <summary>
    /// Set height of a dice
    /// </summary>
    private void SetDiceHeight(DiceTerrain dice, float height) {
        dice.transform.DOKill();
        dice.transform.DOLocalMove(new Vector3(dice.transform.localPosition.x, height, dice.transform.localPosition.z), 1);
        dice.diceData.diceState = DiceState.Wall;
    }

    #endregion SetTerrainAtStart
    
    
    #region GetTerrain
    /// <summary>
    /// Load all dice data
    /// </summary>
    /// <param name="levelSize"></param>
    /// <param name="diceList"></param>
    public void StartTerrainCreation(Vector2 levelSize, List<DiceTerrain> diceList) {
        diceTerrainlsit = new DiceTerrain[(int) levelSize.x, (int) levelSize.y];
        foreach (DiceTerrain dice in diceList) {
            diceTerrainlsit[(int) (diceList.IndexOf(dice) / levelSize.x), (int) (diceList.IndexOf(dice) % levelSize.x)] = dice;
        }
        
        CheckDiceForChangingHeight();
    }
    #endregion GetTerrain
    
    
    #region DiceTerrainHelper
    /// <summary>
    /// Return a list of dice which get the same value as the parameter
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public List<DiceTerrain> GetDiceWithSameValue(int value) {
        List<DiceTerrain> diceValueList = new List<DiceTerrain>();
        foreach (DiceTerrain dice in diceTerrainlsit) {
            if(dice.diceData.diceValue == value) diceValueList.Add(dice);
        }
        return diceValueList;
    }
    #endregion DiceTerrainHelper
}