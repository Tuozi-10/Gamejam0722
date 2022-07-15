using System.Collections.Generic;
using UnityEngine;

public class LevelCreationManager : MonoBehaviour {
    public static LevelCreationManager instance;
    
    private void Awake()
    {
        if (instance is not null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }
    
    
    
    
    
    
    private List<DiceTerrain> diceList = new List<DiceTerrain>();

    [SerializeField] private LevelSO loadLevelSO = null;
    public LevelSO LoadLevelSO => loadLevelSO;

    [SerializeField] private Vector2 levelSize = new Vector2(1, 1);
    [SerializeField] private GameObject dicePrefab = null;
    [SerializeField] private float diceSize = 1.5f;



    public void LoadLevel() {
        
    }
    
    
    
    
    
    
    /// <summary>
    /// Load a level based on a scriptableObject
    /// </summary>
    public void LoadLevelScriptable() {
        
    }

    /// <summary>
    /// Save the actual state of the level in a new SO
    /// </summary>
    public void SaveActualLevel() {
        
    }

    /// <summary>
    /// Generate a new level
    /// </summary>
    public void GenerateNewLevel() {
        foreach (DiceTerrain dice in diceList) {
            Destroy(dice.gameObject);
        }
        diceList.Clear();
        
        for (int x = 0; x < levelSize.x; x++) {
            for (int y = 0; y < levelSize.y; y++) {
                
            }
        }
    }
}
