using UnityEngine;

public class LevelCreationManager : MonoBehaviour {
   
    
    [SerializeField] private LevelSO loadLevelSO = null;
    public LevelSO LoadLevelSO => loadLevelSO;

    [SerializeField] private Vector2 levelSize = new Vector2(1, 1);
    [SerializeField] private GameObject dicePrefab = null;
    

    /// <summary>
    /// Load a level based on a scriptableObject
    /// </summary>
    public void LoadLevel() {
        
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
        
    }
}
