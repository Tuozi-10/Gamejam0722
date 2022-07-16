using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class LevelCreationManager : Singleton<LevelCreationManager> {
    [SerializeField] private Vector2 actualLevelSize = new Vector2(0,0);
    
    //LEVEL GENERATION DATA
    [SerializeField] private GameObject dicePrefab = null;
    [SerializeField] private Transform diceParent = null;
    [SerializeField] private Transform diceParentEditor = null;
    [SerializeField, Range(0, 15)] private float destroyLevelDuration = 5;
    public float DestroyLevelDuration => destroyLevelDuration;
    [SerializeField, Range(-25, -10)] private float spawnHeight = -20;

    //LEVEL GENERATION
    [SerializeField] private Vector2 levelSizeGeneration = new Vector2(1, 1);
    public Vector2 LevelSizeGeneration => levelSizeGeneration;
    [SerializeField, Range(1, 2)] private float diceSpaceSize = 1.5f;

    //LOAD CUSTOM LEVEL
    [SerializeField] private LevelSO loadLevelSO = null;
    public LevelSO LoadLevelSO => loadLevelSO;

    
    //SAVE ACTUAL LEVEL
    [SerializeField] private string folderPath = "Assets/Scriptable Object/Levels/";
    public string FolderPath => folderPath;
    [SerializeField] private string levelName = "New Level";
    public string LevelName => levelName;

    #region Generate Level
    
    /// <summary>
    /// Load level data based on the scriptable object inside parameter
    /// </summary>
    /// <param name="level"></param>
    public void LoadLevel(LevelSO level) {
        bool isInGame = Application.isPlaying;
        
        Level levelClass = Level.CreateLevel(level);
        List<DiceTerrain> diceTerrainList = GenerateNewLevel(new Vector2Int(levelClass.terrainSize.x, levelClass.terrainSize.y), isInGame);

        foreach (DiceTerrain dice in diceTerrainList) {
            dice.diceData = levelClass.DiceClass[diceTerrainList.IndexOf(dice)];
            if(!isInGame) dice.UpdateDiceInEditor();
            else dice.UpdateDiceData();
        }

        if (isInGame) StartCoroutine(TerrainManager.instance.InitTerrainCreation(new Vector2Int(levelClass.terrainSize.x, levelClass.terrainSize.y), diceTerrainList));
    }
    
    private float xPosMax = 0;
    private float yPosMax = 0;
    /// <summary>
    /// Generate a new level based on the size of the terrain
    /// </summary>
    public List<DiceTerrain> GenerateNewLevel(Vector2 custLevelSize, bool isInGame) 
    {
        actualLevelSize = custLevelSize;
        
        DestroyImmediateLevel();

        xPosMax = 0;
        yPosMax = 0;
        if(isInGame) diceParent.transform.position = Vector3.zero;
        else diceParentEditor.transform.position = Vector3.zero;
        
        List<DiceTerrain> newCreatedDice = new List<DiceTerrain>();
        for (int x = 0; x < custLevelSize.x; x++) 
        {
            for (int y = 0; y < custLevelSize.y; y++) 
            {
                GameObject dice = Instantiate(dicePrefab, new Vector3(x * diceSpaceSize, isInGame? spawnHeight : 0, y * diceSpaceSize), Quaternion.identity, isInGame? diceParent : diceParentEditor);
                dice.GetComponent<DiceTerrain>().InitDice(new Vector2Int(x, y));
                newCreatedDice.Add(dice.GetComponent<DiceTerrain>());

                if (xPosMax <= x * diceSpaceSize) xPosMax = x * diceSpaceSize;
                if (yPosMax <= y * diceSpaceSize) yPosMax = y * diceSpaceSize;
            }
        }
        
        if(isInGame) diceParent.transform.position = new Vector3(-xPosMax / 2f, 0, -yPosMax / 2f);
        else diceParentEditor.transform.position = new Vector3(-xPosMax / 2f, 0, -yPosMax / 2f);

        return newCreatedDice;
    }

    /// <summary>
    /// Destroy the level
    /// </summary>
    public IEnumerator DestroyActuallevel(LevelSO newLevel) {
        foreach (DiceTerrain dice in diceParent.GetComponentsInChildren<DiceTerrain>()) {
            if (dice != null && dice.gameObject != null) {
                dice.transform.DOKill();
                dice.transform.DOLocalMove(new Vector3(dice.transform.localPosition.x, spawnHeight, dice.transform.localPosition.z), destroyLevelDuration);
                Destroy(dice.gameObject, destroyLevelDuration);
            }
        }

        yield return new WaitForSeconds(destroyLevelDuration);
        LoadLevel(newLevel);
    }

    /// <summary>
    /// Destroy the level immediatly
    /// </summary>
    public void DestroyImmediateLevel() {
        foreach (Transform child in diceParentEditor.GetComponentsInChildren<Transform>()) {
            if(diceParentEditor.transform != child && child != null) DestroyImmediate(child.gameObject);
        }
    }
    
    #endregion Generate Level

    /// <summary>
    /// Save the actual state of the level in a new SO
    /// </summary>
    public void SaveActualLevel() {
        List<DiceClass> diceDataList = new List<DiceClass>();
        foreach (DiceTerrain dice in diceParentEditor.GetComponentsInChildren<DiceTerrain>()) 
        {
            diceDataList.Add(dice.diceData);
        }
        
        LevelSO newLevelSO = AssetDatabase.LoadAssetAtPath<LevelSO>($"{folderPath + levelName}.asset");

        if (newLevelSO == null) {
            newLevelSO = ScriptableObject.CreateInstance<LevelSO>();
            AssetDatabase.CreateAsset(newLevelSO, $"{folderPath + levelName}.asset");
        }
        
        newLevelSO.SaveLevelData(actualLevelSize, diceDataList);
        EditorUtility.SetDirty(newLevelSO);
        AssetDatabase.Refresh();
    }
}