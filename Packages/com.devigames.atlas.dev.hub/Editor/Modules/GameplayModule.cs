using DeviGames.Atlas.Dev.Hub.Context;
using DeviGames.Atlas.Dev.Hub.Models;
using DeviGames.Atlas.Dev.Hub.Modules;
using DeviGames.Atlas.Dev.Hub.Services;
using UnityEditor;
using UnityEngine;

namespace DeviGames.Atlas.Dev.Hub.Editor.Modules
{
    public sealed class GameplayModule : IDevModule
    {
        private Vector2 _scrollPosition;

        public string Id => "gameplay";
        public string DisplayName => "Gameplay";
        public int Order => 100;

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

            if (!context.Runtime.TryResolve(
                    out DevHubSnapshotService snapshotService))
            {
                EditorGUILayout.HelpBox(
                    "DevHubSnapshotService is not registered.",
                    MessageType.Warning);

                EditorGUILayout.EndScrollView();
                return;
            }

            DevHubSnapshot snapshot =
                snapshotService.CreateSnapshot();

            DrawMission(snapshot);
            DrawObjectives(snapshot);
            DrawInventory(snapshot);
            DrawProgress(snapshot);

            EditorGUILayout.EndScrollView();
        }

        private static void DrawMission(
            DevHubSnapshot snapshot)
        {
            DrawSectionHeader("Mission");

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField(
                "Active",
                snapshot.HasActiveMission ? "Yes" : "No");

            EditorGUILayout.LabelField(
                "Current Mission",
                string.IsNullOrWhiteSpace(
                    snapshot.CurrentMissionId)
                    ? "None"
                    : snapshot.CurrentMissionId);

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(8f);
        }

        private static void DrawObjectives(
            DevHubSnapshot snapshot)
        {
            DrawSectionHeader("Objectives");

            if (snapshot.Objectives.Count == 0)
            {
                EditorGUILayout.HelpBox(
                    "No objective state is available.",
                    MessageType.Info);
            }

            foreach (ObjectiveSnapshot objective
                     in snapshot.Objectives)
            {
                EditorGUILayout.BeginVertical("box");

                EditorGUILayout.LabelField(
                    objective.ObjectiveId,
                    EditorStyles.boldLabel);

                EditorGUILayout.LabelField(
                    "Completed",
                    objective.IsCompleted ? "Yes" : "No");

                float progress =
                    objective.TargetValue <= 0
                        ? 0f
                        : Mathf.Clamp01(
                            (float)objective.CurrentValue /
                            objective.TargetValue);

                Rect progressRect =
                    GUILayoutUtility.GetRect(
                        18f,
                        18f,
                        "TextField");

                EditorGUI.ProgressBar(
                    progressRect,
                    progress,
                    $"{objective.CurrentValue}/" +
                    $"{objective.TargetValue}");

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.Space(8f);
        }

        private static void DrawInventory(
            DevHubSnapshot snapshot)
        {
            DrawSectionHeader("Inventory");

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField(
                "Item Count",
                snapshot.InventoryItemIds.Count.ToString());

            if (snapshot.InventoryItemIds.Count == 0)
            {
                EditorGUILayout.LabelField(
                    "Inventory is empty.",
                    EditorStyles.miniLabel);
            }
            else
            {
                foreach (string itemId
                         in snapshot.InventoryItemIds)
                {
                    EditorGUILayout.LabelField(
                        "• " + itemId);
                }
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(8f);
        }

        private static void DrawProgress(
            DevHubSnapshot snapshot)
        {
            DrawSectionHeader("Completed Missions");

            EditorGUILayout.BeginVertical("box");

            if (snapshot.CompletedMissionIds.Count == 0)
            {
                EditorGUILayout.LabelField(
                    "No completed missions.",
                    EditorStyles.miniLabel);
            }
            else
            {
                foreach (string missionId
                         in snapshot.CompletedMissionIds)
                {
                    EditorGUILayout.LabelField(
                        "✓ " + missionId);
                }
            }

            EditorGUILayout.EndVertical();
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