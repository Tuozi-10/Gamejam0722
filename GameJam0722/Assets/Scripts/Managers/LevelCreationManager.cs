using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelCreationManager : Singleton<LevelCreationManager> {
    [SerializeField] private Vector2 actualLevelSize = new Vector2(0,0);
    
    //LEVEL GENERATION
    [SerializeField] private Vector2 levelSizeGeneration = new Vector2(1, 1);
    public Vector2 LevelSizeGeneration => levelSizeGeneration;
    [SerializeField] private GameObject dicePrefab = null;
    [SerializeField] private Transform diceParent = null;
    [SerializeField] private Transform diceParentEditor = null;
    [SerializeField] private float diceSize = 1.5f;

    //LOAD CUSTOM LEVEL
    [SerializeField] private LevelSO loadLevelSO = null;
    public LevelSO LoadLevelSO => loadLevelSO;

    //SAVE ACTUAL LEVEL
    [SerializeField] private string folderPath = "Assets/Scriptable Object/Levels/";
    public string FolderPath => folderPath;
    [SerializeField] private string levelName = "New Level";
    public string LevelName => levelName;
    
    private void Start() 
    {
        if(loadLevelSO != null) LoadLevel(loadLevelSO);
    }

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
            dice.UpdateDiceValue();
        }

        if (isInGame) TerrainManager.instance.InitTerrainCreation(new Vector2Int(levelClass.terrainSize.x, levelClass.terrainSize.y), diceTerrainList);
    }

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
    
    private float xPosMax = 0;
    private float yPosMax = 0;
    /// <summary>
    /// Generate a new level based on the size of the terrain
    /// </summary>
    public List<DiceTerrain> GenerateNewLevel(Vector2 custLevelSize, bool isInGame) 
    {
        actualLevelSize = custLevelSize;

        foreach (DiceTerrain dice in diceParentEditor.GetComponentsInChildren<DiceTerrain>()) 
        {
            if (dice != null && dice.gameObject != null) DestroyImmediate(dice.gameObject);
        }

        xPosMax = 0;
        yPosMax = 0;
        diceParent.transform.position = Vector3.zero;
        List<DiceTerrain> newCreatedDice = new List<DiceTerrain>();
        for (int x = 0; x < custLevelSize.x; x++) 
        {
            for (int y = 0; y < custLevelSize.y; y++) 
            {
                GameObject dice = Instantiate(dicePrefab, new Vector3(x * diceSize, 0, y * diceSize), Quaternion.identity, isInGame? diceParent : diceParentEditor);
                dice.GetComponent<DiceTerrain>().InitDice(new Vector2Int(x, y));
                newCreatedDice.Add(dice.GetComponent<DiceTerrain>());

                if (xPosMax <= x * diceSize) xPosMax = x * diceSize;
                if (yPosMax <= y * diceSize) yPosMax = y * diceSize;
            }
        }
        diceParent.transform.position = new Vector3(-xPosMax / 2f, 0, -yPosMax / 2f);

        return newCreatedDice;
    }
}