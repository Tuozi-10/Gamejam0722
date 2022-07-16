using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace UnityToolbarExtension {
    public static class ToolbarCallback {
        private static Type toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
        private static ScriptableObject currentToolbar;

        /// <summary>
        /// Callback for toolbar OnGUI method.
        /// </summary>
        public static Action OnToolbarGUILeft;
        public static Action OnToolbarGUIRight;

        static ToolbarCallback() {
            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;
        }

        /// <summary>
        /// Method called onUpdate
        /// </summary>
        private static void OnUpdate() {
            if (currentToolbar != null) return;
            
            //Get toolbar
            Object[] toolbars = Resources.FindObjectsOfTypeAll(toolbarType);
            currentToolbar = toolbars.Length > 0 ? (ScriptableObject) toolbars[0] : null;
            if (currentToolbar == null) return;

            FieldInfo root = currentToolbar.GetType().GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);
            object rawRoot = root?.GetValue(currentToolbar);
            VisualElement mRoot = rawRoot as VisualElement;
            RegisterCallback(OnToolbarGUILeft, mRoot.Q("ToolbarZoneLeftAlign"));
            RegisterCallback(OnToolbarGUIRight, mRoot.Q("ToolbarZoneRightAlign"));
        }

        /// <summary>
        /// Draw the elements inside the Visual Element
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="rootQ"></param>
        private static void RegisterCallback(Action cb, VisualElement rootQ) {
            VisualElement parent = new VisualElement() { style = { flexGrow = 1, flexDirection = FlexDirection.Row, alignItems = Align.Center} };
            IMGUIContainer container = new IMGUIContainer();
            container.style.flexGrow = 1;
            container.onGUIHandler += () => { cb?.Invoke(); };

            parent.Add(container);
            rootQ.Add(parent);
        }
    }
}