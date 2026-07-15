using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Dev.Hub.Models;
using DeviGames.Atlas.Dev.Hub.Services;
using UnityEditor;
using UnityEngine;
using System;

namespace DeviGames.Atlas.Dev.Hub.Editor
{
    public sealed class AtlasDeveloperHubWindow : EditorWindow
    {
        private Vector2 _scrollPosition;

        [MenuItem("DeviGames/Atlas/Developer Hub")]
        public static void Open()
        {
            GetWindow<AtlasDeveloperHubWindow>(
                "Atlas Developer Hub");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField(
                "Atlas Developer Hub",
                EditorStyles.boldLabel);

            EditorGUILayout.Space();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox(
                    "Enter Play Mode to inspect Atlas services.",
                    MessageType.Info);

                return;
            }

            DrawRegisteredServices();

            DrawInitializedServices();
            
            if (!DeviGames.Atlas.Core.Services.Services.TryResolve(
                    out DevHubSnapshotService snapshotService))
            {
                EditorGUILayout.HelpBox(
                    "DevHubSnapshotService is not registered.",
                    MessageType.Warning);

                return;
            }

            DevHubSnapshot snapshot =
                snapshotService.CreateSnapshot();

            _scrollPosition =
                EditorGUILayout.BeginScrollView(_scrollPosition);

            DrawMission(snapshot);
            DrawObjectives(snapshot);
            DrawInventory(snapshot);
            DrawProgress(snapshot);

            EditorGUILayout.EndScrollView();

            Repaint();
        }

        private static void DrawRegisteredServices()
        {
            EditorGUILayout.LabelField(
                "Registered Services",
                EditorStyles.boldLabel);

            foreach (Type serviceType in DeviGames.Atlas.Core.Services.Services.RegisteredTypes)
            {
                EditorGUILayout.LabelField(serviceType.Name);
            }

            EditorGUILayout.LabelField(
                "Container Initialized",
                DeviGames.Atlas.Core.Services.Services.IsInitialized.ToString());
        }

        private static void DrawInitializedServices()
        {
            EditorGUILayout.LabelField(
                "Initialized Services",
                EditorStyles.boldLabel);

            foreach (object service in DeviGames.Atlas.Core.Services.Services.InitializedServices)
            {
                EditorGUILayout.LabelField(service.GetType().Name);
            }
        }
        private static void DrawMission(
            DevHubSnapshot snapshot)
        {
            EditorGUILayout.LabelField(
                "Mission",
                EditorStyles.boldLabel);

            EditorGUILayout.LabelField(
                "Active",
                snapshot.HasActiveMission.ToString());

            EditorGUILayout.LabelField(
                "Current Mission",
                string.IsNullOrEmpty(snapshot.CurrentMissionId)
                    ? "None"
                    : snapshot.CurrentMissionId);

            EditorGUILayout.Space();
        }

        private static void DrawObjectives(
            DevHubSnapshot snapshot)
        {
            EditorGUILayout.LabelField(
                "Objectives",
                EditorStyles.boldLabel);

            if (snapshot.Objectives.Count == 0)
            {
                EditorGUILayout.LabelField(
                    "No active objectives.");
            }

            foreach (ObjectiveSnapshot objective in snapshot.Objectives)
            {
                EditorGUILayout.BeginVertical("box");

                EditorGUILayout.LabelField(
                    objective.ObjectiveId);

                EditorGUILayout.LabelField(
                    "Progress",
                    $"{objective.CurrentValue}/{objective.TargetValue}");

                EditorGUILayout.LabelField(
                    "Completed",
                    objective.IsCompleted.ToString());

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.Space();
        }

        private static void DrawInventory(
            DevHubSnapshot snapshot)
        {
            EditorGUILayout.LabelField(
                "Inventory",
                EditorStyles.boldLabel);

            if (snapshot.InventoryItemIds.Count == 0)
            {
                EditorGUILayout.LabelField(
                    "Inventory is empty.");
            }

            foreach (string itemId in snapshot.InventoryItemIds)
            {
                EditorGUILayout.LabelField(itemId);
            }

            EditorGUILayout.Space();
        }

        private static void DrawProgress(
            DevHubSnapshot snapshot)
        {
            EditorGUILayout.LabelField(
                "Completed Missions",
                EditorStyles.boldLabel);

            if (snapshot.CompletedMissionIds.Count == 0)
            {
                EditorGUILayout.LabelField(
                    "No completed missions.");
            }

            foreach (string missionId in snapshot.CompletedMissionIds)
            {
                EditorGUILayout.LabelField(missionId);
            }
        }
    }
}