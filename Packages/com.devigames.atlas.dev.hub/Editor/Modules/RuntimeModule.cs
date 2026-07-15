using System;
using DeviGames.Atlas.Dev.Hub.Context;
using DeviGames.Atlas.Dev.Hub.Modules;
using UnityEditor;
using UnityEngine;

namespace DeviGames.Atlas.Dev.Hub.Editor.Modules
{
    public sealed class RuntimeModule : IDevModule
    {
        private Vector2 _scrollPosition;
        public string Id => "runtime";
        public string DisplayName => "Runtime";
        public int Order => 0;

        public void OnActivate(DevHubContext context)
        {
        }

        public void OnDeactivate(DevHubContext context)
        {
        }

        public void Draw(DevHubContext context)
        {
            _scrollPosition =
                EditorGUILayout.BeginScrollView(
                    _scrollPosition);

            DrawRuntimeStatus(context);
            EditorGUILayout.Space(8f);
            DrawRegisteredServices(context);

            EditorGUILayout.EndScrollView();
        }

        private static void DrawRuntimeStatus(
            DevHubContext context)
        {
            DrawSectionHeader("Runtime Status");

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField(
                "Container Initialized",
                context.Runtime.IsRunning ? "Yes" : "No");

            EditorGUILayout.LabelField(
                "Registered Service Count",
                context.Runtime.RegisteredTypes.Count.ToString());

            EditorGUILayout.EndVertical();
        }

        private static void DrawRegisteredServices(
            DevHubContext context)
        {
            DrawSectionHeader("Registered Services");

            if (context.Runtime.RegisteredTypes.Count == 0)
            {
                EditorGUILayout.HelpBox(
                    "No services are currently registered.",
                    MessageType.Info);

                return;
            }

            foreach (Type serviceType
                     in context.Runtime.RegisteredTypes)
            {
                EditorGUILayout.BeginVertical("box");

                EditorGUILayout.LabelField(
                    serviceType.Name,
                    EditorStyles.boldLabel);

                EditorGUILayout.LabelField(
                    serviceType.FullName,
                    EditorStyles.miniLabel);

                EditorGUILayout.EndVertical();
            }
        }

        private static void DrawSectionHeader(
            string title)
        {
            EditorGUILayout.LabelField(
                title,
                EditorStyles.boldLabel);
        }
    }
}