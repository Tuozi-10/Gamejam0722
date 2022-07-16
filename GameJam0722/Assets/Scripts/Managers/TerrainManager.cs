using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Entities;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainManager : Singleton<TerrainManager> {
    
    public DiceTerrain[,] diceTerrainlsit;
    
    [SerializeField] private GameObject enemyPrefab = null;
    [SerializeField] private Transform enemyParent = null;
    [HideInInspector] public List<AbstractEntity> enemyEntity = new List<AbstractEntity>();

    [Header("--- HEIGHT VALUE")]
    [SerializeField] private Vector2 heightRandomness = new Vector2(-.25f,.25f);
    [SerializeField] private float wallHeightValue = 0;
    [SerializeField] private float holeHeightValue = 0;
    
    [Header("--- MOVE DURATION")]
    [SerializeField] private float moveHeightDuration = 1;
    
    [Header("--- RANDOM DICE THROW")]
    public bool setRandomDiceValue = true;
    [SerializeField] private int wallDiceValue = 0;
    [SerializeField] private int holeDiceValue = 0;
    
    /// <summary>
    /// Load all dice data
    /// </summary>
    /// <param name="levelSize"></param>
    /// <param name="diceList"></param>
    public IEnumerator InitTerrainCreation(Vector2 levelSize, List<DiceTerrain> diceList) {
        diceTerrainlsit = new DiceTerrain[(int) levelSize.x, (int) levelSize.y];
        foreach (DiceTerrain dice in diceList) {
            diceTerrainlsit[(int) (diceList.IndexOf(dice) / levelSize.x), (int) (diceList.IndexOf(dice) % levelSize.x)] = dice;
        }
        
        AddHeightRandomness();
        yield return new WaitForSeconds(LevelCreationManager.instance.DestroyLevelDuration);

        StartCoroutine(ChangeHeightEvent(true));
    }

    #region Set Terrain Height

    private int randomWallDice = 0;
    private int randomHoleDice = 0;
    /// <summary>
    /// Launch the event which change the height of some Dice
    /// </summary>
    public IEnumerator ChangeHeightEvent(bool firstLaunch = false) 
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
        
        randomWallDice = setRandomDiceValue ? Random.Range(1, 6) : wallDiceValue;
        randomHoleDice = setRandomDiceValue ? Random.Range(1, 6) : holeDiceValue;
        do {
            randomHoleDice = Random.Range(1, 6);
        } while (randomHoleDice == randomWallDice);
        
        yield return new WaitForSeconds(moveHeightDuration + moveHeightDuration / 8);
        
        foreach (DiceTerrain dice in diceTerrainlsit) 
        {
            if (dice.diceData.diceState == DiceState.ForceHole) SetDiceHeight(dice, wallHeightValue, moveHeightDuration);
            
            //CREATE WALL
            else if (GetDiceWithSameValue(randomWallDice).Contains(dice) ) 
            {
                SetDiceHeight(dice, wallHeightValue + dice.heightRandomness, moveHeightDuration);
                dice.diceData.diceState = DiceState.Wall;
            }
            //CREATE HOLE
            else if (GetDiceWithSameValue(randomHoleDice).Contains(dice) && (dice.diceData.diceEffectState == DiceEffectState.None)) 
            {
                SetDiceHeight(dice, holeHeightValue, moveHeightDuration);
                dice.diceData.diceState = DiceState.Hole;
            }
        }

        yield return new WaitForSeconds(moveHeightDuration + 0.25f);

        if (firstLaunch) 
        {
            enemyEntity.Clear();
            Character.instance.pos = GetStartDice() != null ? GetStartDice().pos : new Vector2Int(0, 0);
            Character.instance.transform.position = BaseAI.GetPosFromCoord(Character.instance.pos.x, Character.instance.pos.y);
        
            foreach (DiceTerrain dice in diceTerrainlsit) {
                if(dice.diceData.diceEffectState == DiceEffectState.Spawner) SpawnEnemy(dice);
            }
            
            LevelManager.instance.GenerateEntities();
        }
        else 
        {
            LevelManager.instance.GetNextEntity().StartTurn();
        }
    }
    
    /// <summary>
    /// Spawn an enemy at a given position
    /// </summary>
    private void SpawnEnemy(DiceTerrain dice) {
        GameObject enemy = Instantiate(enemyPrefab, BaseAI.GetPosFromCoord(dice.pos.x, dice.pos.y), Quaternion.identity, enemyParent);
        enemy.GetComponent<BaseAI>().pos = dice.pos;
        enemyEntity.Add(enemy.GetComponent<AbstractEntity>());
    }
    

    private float heightAddValue = 0;
    /// <summary>
    /// Add a little random on the height of each dice
    /// </summary>
    private void AddHeightRandomness() {
        foreach (DiceTerrain dice in diceTerrainlsit) {
            heightAddValue = Random.Range(heightRandomness.x, heightRandomness.y);
            SetDiceHeight(dice, heightAddValue, LevelCreationManager.instance.DestroyLevelDuration);
            dice.heightRandomness = heightAddValue;
        }
    }

    /// <summary>
    /// Set the height of a dice
    /// </summary>
    private void SetDiceHeight(DiceTerrain dice, float height, float duration) {
        dice.transform.DOKill();
        dice.transform.DOLocalMove(new Vector3(dice.transform.localPosition.x, height, dice.transform.localPosition.z), Random.Range(duration - duration / 8, duration + duration / 8));
    }

    #endregion Set Terrain Height
    
    #region DiceTerrainHelper
    
    /// <summary>
    /// Return a list of dice which get the same value as the parameter
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private List<DiceTerrain> GetDiceWithSameValue(int value) {
        List<DiceTerrain> diceValueList = new List<DiceTerrain>();
        foreach (DiceTerrain dice in diceTerrainlsit) {
            if(dice.diceData.diceValue == value) diceValueList.Add(dice);
        }
        return diceValueList;
    }

    /// <summary>
    /// Get the dice which is the start
    /// </summary>
    /// <returns></returns>
    private DiceTerrain GetStartDice() {
        foreach (DiceTerrain dice in diceTerrainlsit) {
            if (dice.diceData.diceEffectState == DiceEffectState.Start) return dice;
        }

        return null;
    }

    /// <summary>
    /// Return all available tiles in the list
    /// </summary>
    /// <returns></returns>
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