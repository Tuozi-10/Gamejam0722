using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityToolbarExtension {
	[InitializeOnLoad]
	public static class ToolbarExtension {
		private static int m_toolCount;
		public static readonly List<Action> LeftToolbarGUI = new List<Action>();
		public static readonly List<Action> RightToolbarGUI = new List<Action>();

		/// <summary>
		/// Method called when project load or when elements are reload
		/// </summary>
		static ToolbarExtension() {
			ToolbarCallback.OnToolbarGUILeft = GUILeft;
			ToolbarCallback.OnToolbarGUIRight = GUIRight;
		}

		/// <summary>
		/// Draw GUI Elements on the left of the middle toolbar buttons
		/// </summary>
		private static void GUILeft() {
			GUILayout.BeginHorizontal();
			foreach (var handler in LeftToolbarGUI) {
				handler();
			}
			GUILayout.EndHorizontal();
		}
		
		/// <summary>
		/// Draw GUI Elements on the right of the middle toolbar buttons
		/// </summary>
		private static void GUIRight() {
			GUILayout.BeginHorizontal();
			foreach (var handler in RightToolbarGUI) {
				handler();
			}
			GUILayout.EndHorizontal();
		}
	}
}