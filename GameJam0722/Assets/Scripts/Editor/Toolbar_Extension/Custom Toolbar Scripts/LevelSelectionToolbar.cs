using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolbarExtension;

[InitializeOnLoad]
public class LevelSelectionToolbar {
    private static List<string> levelPossibilities = new List<string>();
    private static int levelIndex => PlayerPrefs.GetInt("SceneDirectoryIndex", 0);
    private static GUIContent buttonContent => new(Resources.Load<Texture>("OpenIcon"), "Open Scene In Editor");

    static LevelSelectionToolbar() {
        ToolbarExtension.RightToolbarGUI.Add(LevelSelectionDrawer);
        
        EditorApplication.playModeStateChanged -= CheckActivScene;
        EditorApplication.playModeStateChanged += CheckActivScene;
    }

    /// <summary>
    /// Draw the Dropdown for the level selection
    /// </summary>
    private static void LevelSelectionDrawer() {
        levelPossibilities.Clear();

        EditorBuildSettingsScene[] filePath = EditorBuildSettings.scenes;
        foreach (EditorBuildSettingsScene file in filePath) levelPossibilities.Add(Path.GetFileName(file.path).Split(".")[0]);

        if (levelPossibilities.Count == 0) return;

        int value = EditorGUILayout.Popup(levelIndex, levelPossibilities.ToArray());
        PlayerPrefs.SetInt("SceneDirectoryIndex", value);

        GUI.enabled = (EditorSceneManager.GetActiveScene().name != EditorSceneManager.GetSceneByBuildIndex(levelIndex).name);
        GUIStyle buttonStyle = new(GUI.skin.button) {padding = new RectOffset(4, 4, 4, 4)};
        if (GUILayout.Button(buttonContent, buttonStyle, GUILayout.Width(18), GUILayout.Height(18))) EditorSceneManager.OpenScene(EditorBuildSettings.scenes[levelIndex].path);
        GUI.enabled = true;
        
        GUILayout.FlexibleSpace();
    }

    /// <summary>
    /// Check if the active scene is the scene to be load
    /// </summary>
    /// <param name="obj"></param>
    private static void CheckActivScene(PlayModeStateChange obj) {
        if (obj != PlayModeStateChange.EnteredPlayMode) return;
        if (EditorSceneManager.GetActiveScene().name != EditorSceneManager.GetSceneByBuildIndex(levelIndex).name) EditorSceneManager.LoadScene(levelIndex);
    }
}