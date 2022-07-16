using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainManager : Singleton<TerrainManager> {
    
    public DiceTerrain[,] diceTerrainlsit;
    [Header("--- HEIGHT VALUE")]
    [SerializeField] private Vector2 heightRandomness = new Vector2(-.25f,.25f);
    [SerializeField] private float wallHeightValue = 0;
    [SerializeField] private float holeHeightValue = 0;
    
    [Header("--- MOVE DURATION")]
    [SerializeField] private float moveHeightRandomnessDuration = 1;
    [SerializeField] private float moveHeightDuration = 1;
    
    [Header("--- RANDOM DICE THROW")]
    public bool setRandomDiceValue = true;
    [SerializeField] private int upperDiceValue = 0;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.LeftAlt)) StartCoroutine(ChangeHeightEvent());
    }
    
    /// <summary>
    /// Load all dice data
    /// </summary>
    /// <param name="levelSize"></param>
    /// <param name="diceList"></param>
    public void InitTerrainCreation(Vector2 levelSize, List<DiceTerrain> diceList) {
        diceTerrainlsit = new DiceTerrain[(int) levelSize.x, (int) levelSize.y];
        foreach (DiceTerrain dice in diceList) {
            diceTerrainlsit[(int) (diceList.IndexOf(dice) / levelSize.x), (int) (diceList.IndexOf(dice) % levelSize.x)] = dice;
        }
        
        AddHeightRandomness();
        StartCoroutine(ChangeHeightEvent(true));
    }
    
    
    #region Set Terrain Height

    private int randomDice = 0;
    /// <summary>
    /// Launch the event which change the height of some Dice
    /// </summary>
    private IEnumerator ChangeHeightEvent(bool firstLaunch = false) 
    {
        if (!firstLaunch) 
        {
            foreach (DiceTerrain dice in diceTerrainlsit) 
            {
                if (dice.diceData.diceState != DiceState.ForceHole) {
                    SetDiceHeight(dice, dice.heightRandomness, moveHeightDuration);
                    dice.diceData.diceState = DiceState.Walkable;
                }
            }

            yield return new WaitForSeconds(moveHeightDuration + moveHeightDuration / 8);
        }
        
        randomDice = setRandomDiceValue ? Random.Range(1, 6) : upperDiceValue;
        
        yield return new WaitForSeconds(moveHeightDuration + moveHeightDuration / 8);
        
        foreach (DiceTerrain dice in diceTerrainlsit) 
        {
            if (dice.diceData.diceState == DiceState.ForceHole)
            {
                SetDiceHeight(dice, wallHeightValue, moveHeightDuration);
            }
            else if (GetDiceWithSameValue(randomDice).Contains(dice) && (dice.diceData.diceEffectState == DiceEffectState.None)) 
            {
                SetDiceHeight(dice, wallHeightValue, moveHeightDuration + dice.heightRandomness);
                dice.diceData.diceState = DiceState.Wall;
            }
        }
    }

    private float heightAddValue = 0;
    /// <summary>
    /// Add a little random on the height of each dice
    /// </summary>
    private void AddHeightRandomness() {
        foreach (DiceTerrain dice in diceTerrainlsit) {
            heightAddValue = Random.Range(heightRandomness.x, heightRandomness.y);
            SetDiceHeight(dice, heightAddValue, moveHeightRandomnessDuration);
            dice.heightRandomness = heightAddValue;
        }
    }

    /// <summary>
    /// Set the height of a dice
    /// </summary>
    private void SetDiceHeight(DiceTerrain dice, float height, float duration) {
        dice.transform.DOKill();
        dice.transform.DOLocalMove(new Vector3(dice.transform.localPosition.x, height, dice.transform.localPosition.z), Random.Range(moveHeightDuration - moveHeightDuration / 8, moveHeightDuration + moveHeightDuration / 8));
    }

    #endregion Set Terrain Height
    
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

    public bool[,] GetAvailableArray()
    {
        bool[,] ground = new bool[diceTerrainlsit.GetLength(0), diceTerrainlsit.GetLength(1)];

        for (int i = 0; i < diceTerrainlsit.GetLength(0); i++)
        {
            for (int j = 0; j < diceTerrainlsit.GetLength(1); j++)
            {
                ground[i, j] = diceTerrainlsit[i, j].diceData.diceState == DiceState.Walkable;
                // TODO CHECK ENEMY PRESENCE
            }    
        }

        return ground;
    }
    
    #endregion DiceTerrainHelper
}