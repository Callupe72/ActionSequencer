using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace GA.Window
{
    using Elements;
    using System;

    enum AllGameActions
    {
        Wait,
        Move,
    }

    public class GAGraphView : GraphView
    {
        public GAGraphView()
        {
            AddManipulators();

            AddGridBackground();

            AddStyles();
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if(startPort == port)
                {
                    return;
                }

                if(startPort.node == port.node)
                {
                    return;
                }

                if(startPort.direction == port.direction)
                {
                    return;
                }

                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        GANode CreateNode(Vector2 positionToSpawnNode, MonoScript ga)
        {
            GANode node = new GANode();
            AddElement(node);
            node.Initialize(positionToSpawnNode, ga);
            node.Draw();

            Debug.Log(ga.name);

            return node;
        }

        void AddManipulators()
        {

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            foreach (var item in GetAllGameActionsInProject())
            {
                this.AddManipulator(CreateNodeContextualMenu(item));
            }

            this.AddManipulator(CreateGroupeContextualMenu());
        }

        private IManipulator CreateGroupeContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator
            (
                menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => AddElement(CreateGroup("GA Group", actionEvent.eventInfo.localMousePosition)))
            );

            return contextualMenuManipulator;
        }

        Group CreateGroup(string title, Vector2 localMousePosition)
        {
            Group group = new Group()
            {
                title = title,
            };

            group.SetPosition(new Rect(localMousePosition, Vector2.zero));

            return group;
        }

        MonoScript[] GetAllGameActionsInProject()
        {
            string sAssetFolderPath = "Assets/Script";
            string[] aux = sAssetFolderPath.Split(new char[] { '/' });
            string onlyFolderPath = aux[0] + "/" + aux[1] + "/";

            string[] aFilePaths = Directory.GetFiles(onlyFolderPath);

            List<MonoScript> actions = new List<MonoScript>();
            foreach (string sFilePath in aFilePaths)
            {
                if (sFilePath.Contains("GameAction") && Path.GetExtension(sFilePath) == ".cs" && !sFilePath.Equals("AGameAction"))
                {
                    MonoScript objAsset = (MonoScript)AssetDatabase.LoadAssetAtPath(sFilePath, typeof(MonoScript));
                    actions.Add(objAsset);
                }
            }

            return actions.ToArray();
        }


        private IManipulator CreateNodeContextualMenu(MonoScript ga)
        {
            string name = ga.name;

            name = name.Replace("GameAction", "");

            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator
            (
                menuEvent => menuEvent.menu.AppendAction("Add node/" + name, actionEvent => AddElement(CreateNode(actionEvent.eventInfo.localMousePosition, ga)))
            );

            return contextualMenuManipulator;
        }

        void AddStyles()
        {
            StyleSheet stylesheet = (StyleSheet)EditorGUIUtility.Load("GA/GAStylesheet.uss");
            styleSheets.Add(stylesheet);
        }

        void AddGridBackground()
        {
            GridBackground gb = new GridBackground();

            gb.StretchToParentSize();

            Insert(0, gb);
        }
    }
}

