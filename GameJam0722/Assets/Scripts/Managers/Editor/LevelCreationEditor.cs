using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelCreationManager))]
public class LevelCreationEditor : Editor {
    private LevelCreationManager script;
    
    public override void OnInspectorGUI() {
        serializedObject.Update();
        if (script == null) script = (LevelCreationManager) target;

        using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
            GUILayout.Label("Level Generation Data".ToUpper());
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dicePrefab"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("diceParent"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("diceParentEditor"));
            GUILayout.Space(4);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("destroyLevelDuration"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("spawnHeight"));
        }
        
        GUILayout.Space(8);
        
        using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
            GUILayout.Label("Generate new Level".ToUpper());
            EditorGUILayout.PropertyField(serializedObject.FindProperty("levelSizeGeneration"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("diceSpaceSize"));

            using (new GUILayout.HorizontalScope()) {
                if (GUILayout.Button("Generate New level")) script.GenerateNewLevel(script.LevelSizeGeneration, Application.isPlaying);
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Delete Level")) script.DestroyImmediateLevel();
                GUI.backgroundColor = Color.white;
            }
        }
        
        GUILayout.Space(8);
        
        GUI.backgroundColor = Color.green;
        using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
            GUILayout.Label("Load level".ToUpper());
            EditorGUILayout.PropertyField(serializedObject.FindProperty("loadLevelSO"));
            
            if(script.LoadLevelSO == null) EditorGUILayout.HelpBox("Need a scriptable Object to be able to load a new level", MessageType.Error);
            
            GUI.enabled = script.LoadLevelSO != null;
            if (GUILayout.Button("Load level")) ((LevelCreationManager)target).LoadLevel(script.LoadLevelSO);
            GUI.enabled = true;
        }
        GUI.backgroundColor = Color.white;

        GUILayout.Space(8);
        
        GUI.backgroundColor = Color.cyan;
        using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
            GUILayout.Label("Save actual level".ToUpper());
            EditorGUILayout.PropertyField(serializedObject.FindProperty("folderPath"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("levelName"));
            
            if (GUILayout.Button( AssetDatabase.LoadAssetAtPath<LevelSO>($"{script.FolderPath + script.LevelName}.asset") == null? "Save Level as SO" : "Update Level SO")) script.SaveActualLevel();
        }
        GUI.backgroundColor = Color.white;
        
        serializedObject.ApplyModifiedProperties();
    }
}
