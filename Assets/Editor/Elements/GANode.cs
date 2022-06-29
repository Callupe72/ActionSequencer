using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace GA.Elements
{
    public class GANode : Node
    {
        public Color GAColor = Color.red;

        MonoScript gaMonoScript;

        public virtual void Initialize(Vector2 positionToSpawn, MonoScript gameAction)
        {
            SetPosition(new Rect(positionToSpawn, Vector2.zero));
            gaMonoScript = gameAction;

            mainContainer.AddToClassList("ga-node_main-container");
            extensionContainer.AddToClassList("ga-node_extension-container");
        }

        public virtual void Draw()
        {
            TextField gaNameTextField = new TextField()
            {
                value = gaMonoScript.name,
            };



            titleContainer.Insert(0, gaNameTextField);

            VisualElement customDataContainer = new VisualElement();

            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));

            inputPort.portName = "In";
            inputContainer.Add(inputPort);
            RefreshExpandedState();

            Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));

            outputPort.portName = "Out";
            outputContainer.Add(outputPort);
            RefreshExpandedState();




            Foldout textFoldout = new Foldout()
            {
                text = "GA Text"
            };

            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);

            var fields = gaMonoScript.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            Debug.Log(fields.Length);
        }
    }

}