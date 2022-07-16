using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelCreationManager : Singleton<LevelCreationManager> {

    private List<DiceTerrain> diceList = new();
    public List<DiceTerrain> DiceList => diceList;
    [SerializeField] private Vector2 actualLevelSize = new Vector2(0,0);
    
    //LEVEL GENERATION
    [SerializeField] private Vector2 levelSizeGeneration = new Vector2(1, 1);
    public Vector2 LevelSizeGeneration => levelSizeGeneration;
    [SerializeField] private GameObject dicePrefab = null;
    [SerializeField] private Transform diceParent = null;
    [SerializeField] private float diceSize = 1.5f;

    //LOAD CUSTOM LEVEL
    [SerializeField] private LevelSO loadLevelSO = null;
    public LevelSO LoadLevelSO => loadLevelSO;

    //SAVE ACTUAL LEVEL
    [SerializeField] private string folderPath = "Assets/Levels";
    [SerializeField] private string levelName = "New Level";

    private void Start() => LoadLevel(loadLevelSO, true);

    /// <summary>
    /// Load level data based on the scriptable object inside parameter
    /// </summary>
    /// <param name="level"></param>
    public void LoadLevel(LevelSO level, bool isInGame = false) {
        if (isInGame) 
        {
            foreach (DiceTerrain dice in diceList) 
            {
                Destroy(dice.gameObject);
            }
        }
        GenerateNewLevel(loadLevelSO.TerrainSize);

        foreach (DiceTerrain dice in diceList) 
        {
            dice.diceData = loadLevelSO.DiceClass[diceList.IndexOf(dice)];
            dice.UpdateDiceValue();
        }
        if (isInGame) TerrainManager.instance.StartTerrainCreation(level.TerrainSize, diceList);
    }

    /// <summary>
    /// Save the actual state of the level in a new SO
    /// </summary>
    public void SaveActualLevel() {
        LevelSO newLevelSO = ScriptableObject.CreateInstance<LevelSO>();

        List<DiceClass> diceDataList = new List<DiceClass>();
        foreach (DiceTerrain dice in diceList) 
        {
            diceDataList.Add(dice.diceData);
        }
        
        newLevelSO.SaveLevelData(actualLevelSize, diceDataList);
        AssetDatabase.CreateAsset(newLevelSO, $"{folderPath + levelName}.asset");
    }
    
    private float xPosMax = 0;
    private float yPosMax = 0;
    /// <summary>
    /// Generate a new level based on the size of the terrain
    /// </summary>
    public void GenerateNewLevel(Vector2 custLevelSize) {
        actualLevelSize = custLevelSize;
        
        foreach (DiceTerrain dice in diceList) 
        {
            if (dice != null && dice.gameObject != null) 
            {
                DestroyImmediate(dice.gameObject);
            }
        }
        diceList.Clear();
        
        xPosMax = 0;
        yPosMax = 0;
        diceParent.transform.position = Vector3.zero;
        for (int x = 0; x < custLevelSize.x; x++) 
        {
            for (int y = 0; y < custLevelSize.y; y++) 
            {
                GameObject dice = Instantiate(dicePrefab, new Vector3(x * diceSize, 0, y * diceSize), Quaternion.identity, diceParent);
                diceList.Add(dice.GetComponent<DiceTerrain>());
                
                if (xPosMax <= x * diceSize) xPosMax = x * diceSize;
                if (yPosMax <= y * diceSize) yPosMax = y * diceSize;
            }
        }
        diceParent.transform.position = new Vector3(-xPosMax / 2f, 0, -yPosMax / 2f);
    }
}
