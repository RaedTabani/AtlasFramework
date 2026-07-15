using System;
using System.Collections.Generic;
using DeviGames.Atlas.Core.Diagnostics.Models;
using DeviGames.Atlas.Core.Diagnostics.Services;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Dev.Hub.Models;
using DeviGames.Atlas.Dev.Hub.Services;
using DeviGames.Atlas.Core.Lifecycle.Interfaces;
using UnityEditor;
using UnityEngine;

namespace DeviGames.Atlas.Dev.Hub.Editor
{
    public sealed class AtlasDeveloperHubWindow : EditorWindow
    {
        private enum HubTab
        {
            Runtime,
            Gameplay,
            Events
        }

        private static readonly string[] TabLabels =
        {
            "Runtime",
            "Gameplay",
            "Events"
        };

        private HubTab _selectedTab;

        private Vector2 _runtimeScroll;
        private Vector2 _gameplayScroll;
        private Vector2 _eventsScroll;

        private string _eventFilter = string.Empty;
        private bool _newestEventsFirst = true;

        [MenuItem("DeviGames/Atlas/Developer Hub")]
        public static void Open()
        {
            AtlasDeveloperHubWindow window =
                GetWindow<AtlasDeveloperHubWindow>(
                    "Atlas Developer Hub");

            window.minSize = new Vector2(440f, 320f);
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged +=
                OnPlayModeStateChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -=
                OnPlayModeStateChanged;
        }

        private void OnGUI()
        {
            DrawHeader();

            if (!EditorApplication.isPlaying)
            {
                DrawNotPlayingMessage();
                return;
            }

            DrawToolbar();

            EditorGUILayout.Space(6f);

            switch (_selectedTab)
            {
                case HubTab.Runtime:
                    DrawRuntimeTab();
                    break;

                case HubTab.Gameplay:
                    DrawGameplayTab();
                    break;

                case HubTab.Events:
                    DrawEventsTab();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            Repaint();
        }

        private static void DrawHeader()
        {
            EditorGUILayout.Space(4f);

            EditorGUILayout.LabelField(
                "Atlas Developer Hub",
                EditorStyles.boldLabel);

            EditorGUILayout.LabelField(
                "Runtime inspection and diagnostics",
                EditorStyles.miniLabel);

            EditorGUILayout.Space(4f);
        }

        private static void DrawNotPlayingMessage()
        {
            EditorGUILayout.HelpBox(
                "Enter Play Mode to inspect the Atlas runtime.",
                MessageType.Info);
        }

        private void DrawToolbar()
        {
            _selectedTab = (HubTab)GUILayout.Toolbar(
                (int)_selectedTab,
                TabLabels);
        }

        private void DrawRuntimeTab()
        {
            _runtimeScroll =
                EditorGUILayout.BeginScrollView(_runtimeScroll);

            DrawRuntimeStatus();
            EditorGUILayout.Space(8f);
            DrawRegisteredServices();

            EditorGUILayout.EndScrollView();
        }

        private static void DrawRuntimeStatus()
        {
            DrawSectionHeader("Runtime Status");

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField(
                "Container Initialized",
                DeviGames.Atlas.Core.Services.Services.IsInitialized ? "Yes" : "No");

            EditorGUILayout.LabelField(
                "Registered Service Count",
                DeviGames.Atlas.Core.Services.Services.RegisteredTypes.Count.ToString());

            EditorGUILayout.EndVertical();
        }

        private static void DrawRegisteredServices()
        {
            DrawSectionHeader("Registered Services");

            IReadOnlyCollection<Type> registeredTypes =
                DeviGames.Atlas.Core.Services.Services.RegisteredTypes;

            if (registeredTypes.Count == 0)
            {
                EditorGUILayout.HelpBox(
                    "No services are currently registered.",
                    MessageType.Info);

                return;
            }

            foreach (Type serviceType in registeredTypes)
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

        private void DrawGameplayTab()
        {
            _gameplayScroll =
                EditorGUILayout.BeginScrollView(_gameplayScroll);

            if (!DeviGames.Atlas.Core.Services.Services.TryResolve(
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
                string.IsNullOrWhiteSpace(snapshot.CurrentMissionId)
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

            foreach (ObjectiveSnapshot objective in snapshot.Objectives)
            {
                EditorGUILayout.BeginVertical("box");

                EditorGUILayout.LabelField(
                    objective.ObjectiveId,
                    EditorStyles.boldLabel);

                EditorGUILayout.LabelField(
                    "Progress",
                    $"{objective.CurrentValue}/{objective.TargetValue}");

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
                    $"{objective.CurrentValue}/{objective.TargetValue}");

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
                foreach (string itemId in snapshot.InventoryItemIds)
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
                foreach (string missionId in snapshot.CompletedMissionIds)
                {
                    EditorGUILayout.LabelField(
                        "✓ " + missionId);
                }
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawEventsTab()
        {
            if (!DeviGames.Atlas.Core.Services.Services.TryResolve(
                    out EventHistoryService historyService))
            {
                EditorGUILayout.HelpBox(
                    "EventHistoryService is not registered.",
                    MessageType.Warning);

                return;
            }

            DrawEventControls(historyService);

            EditorGUILayout.Space(6f);

            _eventsScroll =
                EditorGUILayout.BeginScrollView(_eventsScroll);

            DrawEventRecords(historyService);

            EditorGUILayout.EndScrollView();
        }

        private void DrawEventControls(
            EventHistoryService historyService)
        {
            DrawSectionHeader("Event History");

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();

            bool paused = EditorGUILayout.ToggleLeft(
                "Pause Capture",
                historyService.IsPaused,
                GUILayout.Width(110f));

            if (paused != historyService.IsPaused)
            {
                historyService.IsPaused = paused;
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button(
                    "Clear",
                    GUILayout.Width(70f)))
            {
                historyService.Clear();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(4f);

            _eventFilter = EditorGUILayout.TextField(
                "Filter",
                _eventFilter);

            _newestEventsFirst = EditorGUILayout.Toggle(
                "Newest First",
                _newestEventsFirst);

            EditorGUILayout.LabelField(
                "Recorded Events",
                $"{historyService.Count}/{historyService.Capacity}");

            EditorGUILayout.EndVertical();
        }

        private void DrawEventRecords(
            EventHistoryService historyService)
        {
            IReadOnlyList<EventRecord> records =
                historyService.Records;

            if (records.Count == 0)
            {
                EditorGUILayout.HelpBox(
                    historyService.IsPaused
                        ? "Capture is paused and no events are recorded."
                        : "No events have been recorded yet.",
                    MessageType.Info);

                return;
            }

            if (_newestEventsFirst)
            {
                for (int index = records.Count - 1;
                     index >= 0;
                     index--)
                {
                    DrawEventRecord(records[index]);
                }
            }
            else
            {
                for (int index = 0;
                     index < records.Count;
                     index++)
                {
                    DrawEventRecord(records[index]);
                }
            }
        }

        private void DrawEventRecord(
            EventRecord record)
        {
            if (!MatchesFilter(record))
                return;

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(
                $"#{record.SequenceNumber}",
                EditorStyles.boldLabel,
                GUILayout.Width(65f));

            EditorGUILayout.LabelField(
                record.EventName,
                EditorStyles.boldLabel);

            GUILayout.FlexibleSpace();

            EditorGUILayout.LabelField(
                record.TimestampUtc.ToLocalTime()
                    .ToString("HH:mm:ss.fff"),
                EditorStyles.miniLabel,
                GUILayout.Width(90f));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField(
                record.EventType?.FullName ?? "Unknown type",
                EditorStyles.miniLabel);

            DrawKnownPayload(record.EventData);

            EditorGUILayout.EndVertical();
        }

        private bool MatchesFilter(
            EventRecord record)
        {
            if (string.IsNullOrWhiteSpace(_eventFilter))
                return true;

            return record.EventName.Contains(
                _eventFilter,
                StringComparison.OrdinalIgnoreCase);
        }

        private static void DrawKnownPayload(
            object eventData)
        {
            if (eventData == null)
            {
                EditorGUILayout.LabelField(
                    "Payload",
                    "Null");

                return;
            }

            Type eventType = eventData.GetType();

            EditorGUILayout.LabelField(
                "Payload Type",
                eventType.Name);

            EditorGUILayout.LabelField(
                "Payload",
                eventData.ToString());
        }

        private static void DrawSectionHeader(
            string title)
        {
            EditorGUILayout.LabelField(
                title,
                EditorStyles.boldLabel);
        }

        private void OnPlayModeStateChanged(
            PlayModeStateChange state)
        {
            Repaint();
        }
    }
}