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
    [Space]
    [SerializeField] private float entityAppartionDuration = 1.5f;
    [Space]
    [SerializeField] private float playerMoveDuration = 1.5f;
    [Space]
    [SerializeField] private float fallDuration = 4f;
    [SerializeField] private int numberOfTurnBeforeChange = 3;
    
    [Header("--- RANDOM DICE THROW")]
    public bool setRandomDiceValue = true;
    [SerializeField] private int wallDiceValue = 0;
    [SerializeField] private int holeDiceValue = 0;

    private int actualLoopNumber;

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
        if(heightRandomness.x + heightRandomness.y != 0) yield return new WaitForSeconds(LevelCreationManager.instance.DestroyLevelDuration);

        randomWallDice = setRandomDiceValue ? Random.Range(1, 7) : wallDiceValue;
        randomHoleDice = setRandomDiceValue ? Random.Range(1, 7) : holeDiceValue;
        do {
            randomHoleDice = Random.Range(1, 7);
        } while (randomHoleDice == randomWallDice);
        
        UIManager.instance.SetDiceUIColor(randomWallDice, randomHoleDice);

        actualLoopNumber = 0;
        UIManager.instance.SetTextToTurnNeeded(actualLoopNumber);

        yield return new WaitForSeconds(.5f);
        StartCoroutine(ChangeHeightEvent(true));
    }

    #region Set Terrain Height

    private int randomWallDice = 0;
    public int RandomWallDice => randomWallDice;
    private int randomHoleDice = 0;
    public int RandomHoleDice => randomHoleDice;

    /// <summary>
    /// Launch the event which change the height of some Dice
    /// </summary>

    public void CheckIfChange(bool firstLaunch = false)
    {
        actualLoopNumber++;
        UIManager.instance.SetTextToTurnNeeded(actualLoopNumber);

        if (actualLoopNumber == numberOfTurnBeforeChange)
        {
            StartCoroutine(ChangeHeightEvent(firstLaunch));
            actualLoopNumber = 0;
            UIManager.instance.SetTextToTurnNeeded(actualLoopNumber);
            return;
        }
        
        if(firstLaunch) LevelManager.instance.GenerateEntities();
        else LevelManager.instance.GetNextEntity()?.StartTurn();
    }
    
    public IEnumerator ChangeHeightEvent(bool firstLaunch = false) {
        yield return new WaitForSeconds(moveHeightDuration);
        
        CreateWallAndHole(firstLaunch);
        yield return new WaitForSeconds(moveHeightDuration + 0.25f);

        if (firstLaunch) {
            StartCoroutine(SpawnPlayer());

            yield return new WaitForSeconds(1.5f + entityAppartionDuration);

            foreach (DiceTerrain dice in diceTerrainlsit) {
                if (dice.diceData.diceEffectState == DiceEffectState.Spawner) {
                   StartCoroutine(SpawnEnemy(dice));
                }
            }

            yield return new WaitForSeconds(entityAppartionDuration + .5f);
        }

        yield return new WaitForSeconds(1);
        
        for (int i = 0; i < 8; i++) {
            UIManager.instance.SetRandomDiceUIColor();
            yield return new WaitForSeconds(.05f);
        }
        
        randomWallDice = setRandomDiceValue ? Random.Range(1, 7) : wallDiceValue;
        randomHoleDice = setRandomDiceValue ? Random.Range(1, 7) : holeDiceValue;
        do {
            randomHoleDice = Random.Range(1, 7);
        } while (randomHoleDice == randomWallDice);
        
        UIManager.instance.SetDiceUIColor(randomWallDice, randomHoleDice);
        
        if(firstLaunch) LevelManager.instance.GenerateEntities();
        else LevelManager.instance.GetNextEntity()?.StartTurn();
    }

    /// <summary>
    /// Create walls and holes in the terrain
    /// </summary>
    private void CreateWallAndHole(bool firstLaunch) {
        foreach (DiceTerrain dice in diceTerrainlsit) {
            if (dice.diceData.diceState is DiceState.ForceHole or DiceState.ForceWall) continue;
            
            //CREATE WALL
            if (GetDiceWithSameValue(randomWallDice).Contains(dice) ) 
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
            //CREATE FLOOR
            else {
                SetDiceHeight(dice, dice.heightRandomness, moveHeightDuration);
                dice.diceData.diceState = DiceState.Walkable;
            }
            
            if(!firstLaunch) CheckEntityDice(dice);
        }
    }

    /// <summary>
    /// Spawn the player
    /// </summary>
    private IEnumerator SpawnPlayer() {
        enemyEntity.Clear();
        Character.instance.pos = GetStartDice() != null ? GetStartDice().pos : new Vector2Int(0, 0);
        Character.instance.transform.position = BaseAI.GetPosFromCoord(Character.instance.pos.x, Character.instance.pos.y);
        Character.instance.transform.localScale = new Vector3(0,0,0);
        Character.instance.transform.DOScale(1.25f, entityAppartionDuration - .15f).OnComplete(() => Character.instance.transform.DOScale(1f,.15f));
        
        yield return new WaitForSeconds(entityAppartionDuration);
        SetImpactAt(Character.instance.pos.x, Character.instance.pos.y, 7, .45f);
    }
    
    /// <summary>
    /// Spawn an enemy at a given position
    /// </summary>
    private IEnumerator SpawnEnemy(DiceTerrain dice) {
        GameObject enemy = Instantiate(enemyPrefab, BaseAI.GetPosFromCoord(dice.pos.x, dice.pos.y), Quaternion.identity, enemyParent);
        enemy.transform.localScale = new Vector3(0,0,0);
        enemy.transform.DOScale(1.25f, entityAppartionDuration - .15f).OnComplete(() => enemy.transform.DOScale(1f,.15f));
        enemy.GetComponent<BaseAI>().pos = dice.pos;
        enemy.GetComponent<BaseAI>().startPos = dice.pos;
        enemyEntity.Add(enemy.GetComponent<AbstractEntity>());

        yield return new WaitForSeconds(entityAppartionDuration);
        SetImpactAt(dice.pos.x, dice.pos.y, 4, .25f);
    }

    /// <summary>
    /// Check if an entity is on the dice
    /// </summary>
    private void CheckEntityDice(DiceTerrain dice) {
        for (var i = LevelManager.instance.Entities.Count - 1; i >= 0; i--) {
            AbstractEntity ent = LevelManager.instance.Entities[i];
            if (new Vector2Int(ent.pos.x, ent.pos.y) != dice.pos) continue;
            
            ent.transform.DOKill();
            switch (dice.diceData.diceState) {
                case DiceState.Hole:{
                    ent.transform.DOLocalMove(new Vector3(ent.transform.position.x, -10, ent.transform.position.z), fallDuration);
                    if (ent.pos == Character.instance.pos) StartCoroutine(StartPlayerDeath());
                    else
                    {
                        ent.transform.DOScale(1.25f, 0.25f).OnComplete(() => ent.transform.DOScale(0, 0.25f).OnComplete(
                            () =>
                            {
                                LevelManager.instance.RemoveEntity(LevelManager.instance.Entities.IndexOf(ent));
                            }));
                    }
                    break;
                }
                case DiceState.Wall:
                    ent.transform.DOLocalMove(new Vector3(ent.transform.position.x,  BaseAI.GetPosFromCoord(dice.diceData.dicePosX, dice.diceData.dicePosY).y + wallHeightValue + dice.heightRandomness, ent.transform.position.z), moveHeightDuration);
                    break;
            }
        }
    }
    
    /// <summary>
    /// Start the death of the player
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartPlayerDeath() {
        yield return new WaitForSeconds(fallDuration/2);
        UIManager.instance.Defeat();
        LevelManager.instance.ReloadLevel();
    }
    
    private float heightAddValue = 0;
    /// <summary>
    /// Add a little random on the height of each dice
    /// </summary>
    private void AddHeightRandomness() {
        foreach (DiceTerrain dice in diceTerrainlsit) {
            if (dice.diceData.diceState == DiceState.ForceHole) continue;
            heightAddValue = Random.Range(heightRandomness.x, heightRandomness.y);
            SetDiceHeight(dice, dice.diceData.diceState == DiceState.ForceWall? wallHeightValue + heightAddValue : heightAddValue, LevelCreationManager.instance.DestroyLevelDuration);
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
    public List<DiceTerrain> GetDiceWithSameValue(int value) {
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
    public bool[,] GetAvailableArray(bool onWall)
    {
        bool[,] ground = new bool[diceTerrainlsit.GetLength(0), diceTerrainlsit.GetLength(1)];

        for (int i = 0; i < diceTerrainlsit.GetLength(0); i++)
        {
            for (int j = 0; j < diceTerrainlsit.GetLength(1); j++)
            {
                ground[i, j] = diceTerrainlsit[i, j].diceData.diceState == DiceState.Walkable || onWall &&  diceTerrainlsit[i, j].diceData.isWall;

                if(LevelManager.instance is null || LevelManager.instance.Entities is null) continue;
                foreach (var entity in LevelManager.instance.Entities)
                {
                    if(entity is Character) continue;
                    if (entity.pos.x == i && entity.pos.y == j)
                        ground[i, j] = false;
                }
            }    
        }

        return ground;
    }
    
    #endregion DiceTerrainHelper


    public void SetImpactAt(int x, int y, int radius, float intensity)
    {
        foreach (var dice in diceTerrainlsit)
        {
            var dist = Vector2Int.Distance(dice.pos, new Vector2Int(x, y));
            if(dist < radius)
            {
                float ratio = dist / radius;
                dice.DoShakePivot(intensity * (1 - ratio), 0.1f + dist * 0.2f,dist * 0.05f);
            }
        }
    }
}