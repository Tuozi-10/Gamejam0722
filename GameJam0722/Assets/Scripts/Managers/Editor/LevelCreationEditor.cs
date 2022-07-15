using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelCreationManager))]
public class LevelCreationEditor : Editor {
    private LevelCreationManager script;
    
    public override void OnInspectorGUI() {
        serializedObject.Update();
        if (script == null) script = (LevelCreationManager) target;
        
        using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
            GUILayout.Label("Generate new Level".ToUpper());
            EditorGUILayout.PropertyField(serializedObject.FindProperty("levelSize"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dicePrefab"));
            
            if (GUILayout.Button("Generate New level")) script.GenerateNewLevel();
        }
        
        GUILayout.Space(16);
        
        GUI.backgroundColor = Color.green;
        using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
            GUILayout.Label("Load level".ToUpper());
            EditorGUILayout.PropertyField(serializedObject.FindProperty("loadLevelSO"));
            
            if(script.LoadLevelSO == null) EditorGUILayout.HelpBox("Need a scriptable Object to be able to load a new level", MessageType.Error);
            
            GUI.enabled = script.LoadLevelSO != null;
            if (GUILayout.Button("Load level")) ((LevelCreationManager)target).LoadLevel();
            GUI.enabled = true;
        }
        GUI.backgroundColor = Color.white;
        
        
        

        
        GUILayout.Space(8);
        
        GUI.backgroundColor = Color.cyan;
        using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
            GUILayout.Label("Save actual level".ToUpper());
            EditorGUILayout.PropertyField(serializedObject.FindProperty("levelSize"));
            
            if (GUILayout.Button("Generate New level")) script.SaveActualLevel();
        }
        GUI.backgroundColor = Color.white;
        
        serializedObject.ApplyModifiedProperties();
    }
}
