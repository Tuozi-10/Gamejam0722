using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[CustomEditor(typeof(TerrainDrawer))]
public class TerrainDrawerEditor : Editor {
    private static TerrainDrawer script;
    
    public override void OnInspectorGUI() {
        if (script == null) script = (TerrainDrawer) target;
        serializedObject.Update();
        
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("isDrawing"));
        if (EditorGUI.EndChangeCheck()) {
            SceneView.duringSceneGui -= DrawInScene;
            if(serializedObject.FindProperty("isDrawing").boolValue) SceneView.duringSceneGui += DrawInScene;
        } 
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("diceData"));

        serializedObject.ApplyModifiedProperties();
    }


    private static DiceTerrain groundUnderMouse = null;
    private void DrawInScene(SceneView sceneView) {
        if(Event.current.type == EventType.MouseMove) sceneView.Repaint();
        
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        if (Physics.Raycast(mouseRay, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Ground"))) {
            groundUnderMouse = hit.collider.gameObject.GetComponent<DiceTerrain>();
        }
    }

    [MenuItem("Tools/LD/Create Object At World Pos _a")]
    private static void CreateObject() {
        if (groundUnderMouse == null) return;

        groundUnderMouse.diceData.diceValue = script.DiceData.diceValue;
        groundUnderMouse.diceData.diceState = script.DiceData.diceState;
        groundUnderMouse.diceData.diceEffectState = script.DiceData.diceEffectState;
        groundUnderMouse.UpdateDiceInEditor();
    }
}
