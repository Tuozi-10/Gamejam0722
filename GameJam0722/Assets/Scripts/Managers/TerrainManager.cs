using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainManager : MonoBehaviour {
    //Tempo list for dice in scene (destroy this when level creator is ready)
    [SerializeField] private List<DiceTerrain> tempoCubeList = new List<DiceTerrain>();
    
    
    [SerializeField] private Vector2Int terrainSize = new Vector2Int(0, 0);
    private DiceTerrain[,] diceTerrainlsit;

    [SerializeField] private float heigthValue = 0;
    [Space] 
    public bool SetRandomDiceValue = true;
    [SerializeField] private int upperDiceValue = 0;


    private void Awake() {
        TempoFuncLoad();
        SetDiceHeight();
    }

    /// <summary>
    /// Destroy this when level creation is ready
    /// </summary>
    private void TempoFuncLoad() {
        diceTerrainlsit = new DiceTerrain[terrainSize.x, terrainSize.y];
        for (int x = 0; x < terrainSize.x; x++) {
            for (int y = 0; y < terrainSize.y; y++) {
                AddDiceToList(tempoCubeList[x * terrainSize.x + y], new Vector2Int(x, y));
            }
        }
        
        Debug.Log(diceTerrainlsit[1,2]);
    }

    #region SetTerrainAtStart
    private void SetDiceHeight() 
    {
        if(SetRandomDiceValue) upperDiceValue = Random.Range(1, 6);
        
        foreach (DiceTerrain dice in GetDiceWithSameValue(upperDiceValue)) 
        {
            if (dice.diceData.diceState != DiceState.Walkable || dice.diceData.diceEffectState != DiceEffectState.None) continue;
            dice.transform.DOKill();
            dice.transform.DOLocalMove(new Vector3(dice.transform.localPosition.x, heigthValue, dice.transform.localPosition.z), 1);
            dice.diceData.diceState = DiceState.Wall;
        }
    }
    #endregion SetTerrainAtStart
    
    
    #region GetTerrain
    /// <summary>
    /// Add the created DiceTerrain class to the array
    /// </summary>
    /// <param name="dice"></param>
    public void AddDiceToList(DiceTerrain dice, Vector2Int pos) => diceTerrainlsit[pos.x, pos.y] = dice;
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