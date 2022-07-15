using UnityEngine;

[CreateAssetMenu(fileName = "Custom SO/New Level")]
public class LevelSO : ScriptableObject {
    [SerializeField] private Vector2 terrainSize = new Vector2(0, 0);
    
    
    
    public void CreateNewLevel(Vector2 terrainSize) {
        terrainSize = this.terrainSize;
    }
}
