using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GA.Window
{
    public class GAWindow : EditorWindow
    {
        [MenuItem("Tools/GA/Graph")]

        public static void Open()
        {
            GetWindow<GAWindow>("GA Graph");
        }

        void OnEnable()
        {
            AddGraphView();
            AddStyles();
        }

        void AddGraphView()
        {
            GAGraphView gv = new GAGraphView();

            gv.StretchToParentSize();

            rootVisualElement.Add(gv);
        }

        void AddStyles()
        {
            StyleSheet stylesheet = (StyleSheet)EditorGUIUtility.Load("GA/GAVariables.uss");
            rootVisualElement.styleSheets.Add(stylesheet);
        }
    }
}
