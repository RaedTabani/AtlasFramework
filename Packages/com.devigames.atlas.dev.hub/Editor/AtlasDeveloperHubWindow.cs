using System;
using DeviGames.Atlas.Dev.Hub.Context;
using DeviGames.Atlas.Dev.Hub.Editor.Bootstrap;
using DeviGames.Atlas.Dev.Hub.Modules;
using UnityEditor;
using UnityEngine;

namespace DeviGames.Atlas.Dev.Hub.Editor
{
    public sealed class AtlasDeveloperHubWindow :
        EditorWindow
    {
        private DevModuleRegistry _registry;
        private DevHubContext _context;

        private int _selectedModuleIndex;

        [MenuItem("DeviGames/Atlas/Developer Hub")]
        public static void Open()
        {
            AtlasDeveloperHubWindow window =
                GetWindow<AtlasDeveloperHubWindow>(
                    "Atlas Developer Hub");

            window.minSize =
                new Vector2(440f, 320f);
        }

        private void OnEnable()
        {
            RebuildModules();

            EditorApplication.playModeStateChanged +=
                OnPlayModeStateChanged;

            ActivateSelectedModule();
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -=
                OnPlayModeStateChanged;

            DeactivateSelectedModule();
        }
        private void RebuildModules()
        {
            _registry = new DevModuleRegistry();
            _context = new DevHubContext();

            DeveloperHubBootstrap.RegisterModules(
                _registry);

            EnsureSelectedIndexIsValid();
        }
        private void OnGUI()
        {
            EnsureModulesExist();

            DrawHeader();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox(
                    "Enter Play Mode to inspect the Atlas runtime.",
                    MessageType.Info);

                return;
            }

            if (_registry == null || _registry.Count == 0)
            {
                EditorGUILayout.HelpBox(
                    "No Developer Hub modules are registered.",
                    MessageType.Warning);

                if (GUILayout.Button("Rebuild Modules"))
                {
                    RebuildModules();
                }

                return;
            }

            EnsureSelectedIndexIsValid();
            DrawModuleToolbar();

            EditorGUILayout.Space(6f);

            IDevModule selectedModule =
                _registry.Modules[_selectedModuleIndex];

            selectedModule.Draw(_context);

            Repaint();
        }

        private void EnsureModulesExist()
        {
            if (_registry == null ||
                _context == null ||
                _registry.Count == 0)
            {
                RebuildModules();
            }
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

        private void DrawModuleToolbar()
        {
            string[] labels =
                new string[_registry.Count];

            for (int index = 0;
                 index < _registry.Count;
                 index++)
            {
                labels[index] =
                    _registry.Modules[index]
                        .DisplayName;
            }

            int newIndex = GUILayout.Toolbar(
                _selectedModuleIndex,
                labels);

            if (newIndex == _selectedModuleIndex)
                return;

            IDevModule oldModule =
                _registry.Modules[
                    _selectedModuleIndex];

            oldModule.OnDeactivate(_context);

            _selectedModuleIndex = newIndex;

            IDevModule newModule =
                _registry.Modules[
                    _selectedModuleIndex];

            newModule.OnActivate(_context);
        }

        private void ActivateSelectedModule()
        {
            if (_registry.Count == 0)
                return;

            EnsureSelectedIndexIsValid();

            _registry.Modules[
                    _selectedModuleIndex]
                .OnActivate(_context);
        }

        private void DeactivateSelectedModule()
        {
            if (_registry.Count == 0)
                return;

            EnsureSelectedIndexIsValid();

            _registry.Modules[
                    _selectedModuleIndex]
                .OnDeactivate(_context);
        }

        private void EnsureSelectedIndexIsValid()
        {
            if (_registry.Count == 0)
            {
                _selectedModuleIndex = 0;
                return;
            }

            _selectedModuleIndex =
                Mathf.Clamp(
                    _selectedModuleIndex,
                    0,
                    _registry.Count - 1);
        }

        private void OnPlayModeStateChanged(
            PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode ||
                state == PlayModeStateChange.EnteredEditMode)
            {
                RebuildModules();
            }

            Repaint();
        }
    }
}