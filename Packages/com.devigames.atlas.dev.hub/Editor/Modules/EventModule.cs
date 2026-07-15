using System;
using System.Collections.Generic;
using DeviGames.Atlas.Core.Diagnostics.Models;
using DeviGames.Atlas.Core.Diagnostics.Services;
using DeviGames.Atlas.Dev.Hub.Context;
using DeviGames.Atlas.Dev.Hub.Editor.Diagnostics;
using DeviGames.Atlas.Dev.Hub.Modules;
using UnityEditor;
using UnityEngine;

namespace DeviGames.Atlas.Dev.Hub.Editor.Modules
{
    public sealed class EventsModule : IDevModule
    {
        private readonly HashSet<long>
            _expandedEventRecords = new();

        private Vector2 _scrollPosition;
        private string _filter = string.Empty;
        private bool _newestFirst = true;
        private bool _freezeView;

        private List<EventRecord> _frozenRecords;

        public string Id => "events";
        public string DisplayName => "Events";
        public int Order => 200;

        public void OnActivate(DevHubContext context)
        {
        }

        public void OnDeactivate(DevHubContext context)
        {
        }

        public void Draw(DevHubContext context)
        {
            if (!context.Runtime.TryResolve(
                    out EventHistoryService historyService))
            {
                EditorGUILayout.HelpBox(
                    "EventHistoryService is not registered.",
                    MessageType.Warning);

                return;
            }

            DrawControls(historyService);

            EditorGUILayout.Space(6f);

            IReadOnlyList<EventRecord> visibleRecords =
                GetVisibleRecords(historyService);

            _scrollPosition =
                EditorGUILayout.BeginScrollView(
                    _scrollPosition);

            DrawRecords(
                visibleRecords,
                historyService.IsPaused);

            EditorGUILayout.EndScrollView();
        }

        private void DrawControls(
            EventHistoryService historyService)
        {
            EditorGUILayout.LabelField(
                "Event History",
                EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();

            bool paused =
                EditorGUILayout.ToggleLeft(
                    "Pause Capture",
                    historyService.IsPaused,
                    GUILayout.Width(110f));

            if (paused != historyService.IsPaused)
            {
                historyService.IsPaused = paused;
            }

            bool freeze =
                EditorGUILayout.ToggleLeft(
                    "Freeze View",
                    _freezeView,
                    GUILayout.Width(100f));

            SetFreezeView(
                freeze,
                historyService);

            GUILayout.FlexibleSpace();

            if (GUILayout.Button(
                    "Clear",
                    GUILayout.Width(70f)))
            {
                historyService.Clear();
                _expandedEventRecords.Clear();

                if (_freezeView)
                {
                    _frozenRecords =
                        new List<EventRecord>();
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(4f);

            _filter = EditorGUILayout.TextField(
                "Filter",
                _filter);

            _newestFirst = EditorGUILayout.Toggle(
                "Newest First",
                _newestFirst);

            EditorGUILayout.LabelField(
                "Recorded Events",
                $"{historyService.Count}/" +
                $"{historyService.Capacity}");

            EditorGUILayout.EndVertical();
        }

        private IReadOnlyList<EventRecord>
            GetVisibleRecords(
                EventHistoryService historyService)
        {
            if (!_freezeView)
                return historyService.Records;

            _frozenRecords ??=
                new List<EventRecord>(
                    historyService.Records);

            return _frozenRecords;
        }

        private void SetFreezeView(
            bool freeze,
            EventHistoryService historyService)
        {
            if (freeze == _freezeView)
                return;

            _freezeView = freeze;

            _frozenRecords = freeze
                ? new List<EventRecord>(
                    historyService.Records)
                : null;
        }

        private void DrawRecords(
            IReadOnlyList<EventRecord> records,
            bool capturePaused)
        {
            if (records.Count == 0)
            {
                EditorGUILayout.HelpBox(
                    capturePaused
                        ? "Capture is paused and no events are recorded."
                        : "No events have been recorded yet.",
                    MessageType.Info);

                return;
            }

            if (_newestFirst)
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

            bool isExpanded =
                _expandedEventRecords.Contains(
                    record.SequenceNumber);

            bool expanded =
                EditorGUILayout.Foldout(
                    isExpanded,
                    $"#{record.SequenceNumber}  " +
                    $"{record.EventName}",
                    true);

            if (expanded != isExpanded)
            {
                if (expanded)
                {
                    _expandedEventRecords.Add(
                        record.SequenceNumber);
                }
                else
                {
                    _expandedEventRecords.Remove(
                        record.SequenceNumber);
                }
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button(
                    "Copy",
                    EditorStyles.miniButton,
                    GUILayout.Width(45f)))
            {
                EditorGUIUtility.systemCopyBuffer =
                    CreateEventSummary(record);
            }

            EditorGUILayout.LabelField(
                record.TimestampUtc
                    .ToLocalTime()
                    .ToString("HH:mm:ss.fff"),
                EditorStyles.miniLabel,
                GUILayout.Width(90f));

            EditorGUILayout.EndHorizontal();

            if (expanded)
            {
                EditorGUILayout.LabelField(
                    "Event Type",
                    record.EventType?.FullName ??
                    "Unknown type");

                EventPayloadDrawer.Draw(
                    record.EventData);
            }

            EditorGUILayout.EndVertical();
        }

        private bool MatchesFilter(
            EventRecord record)
        {
            if (string.IsNullOrWhiteSpace(_filter))
                return true;

            return record.EventName.Contains(
                _filter,
                StringComparison.OrdinalIgnoreCase);
        }

        private static string CreateEventSummary(
            EventRecord record)
        {
            string payload =
                record.EventData?.ToString() ?? "Null";

            return
                $"#{record.SequenceNumber} " +
                $"{record.TimestampUtc:O} " +
                $"{record.EventName}\n" +
                $"Type: {record.EventType?.FullName}\n" +
                $"Payload: {payload}";
        }
    }
}