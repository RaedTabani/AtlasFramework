using DeviGames.Atlas.Core.Diagnostics.Save;
using DeviGames.Atlas.Dev.Hub.Context;
using DeviGames.Atlas.Dev.Hub.Modules;
using UnityEditor;

namespace DeviGames.Atlas.Dev.Hub.Editor.Modules
{
    public sealed class SaveModule : IDevModule
    {
        public string Id => "save";

        public string DisplayName => "Save";

        public int Order => 300;

        public void OnActivate(
            DevHubContext context)
        {
        }

        public void OnDeactivate(
            DevHubContext context)
        {
        }

        public void Draw(
            DevHubContext context)
        {
            EditorGUILayout.LabelField(
                "Save Inspector",
                EditorStyles.boldLabel);

            EditorGUILayout.Space();

            if (!context.Runtime.TryResolve(
                    out ISaveDiagnosticsService diagnostics))
            {
                EditorGUILayout.HelpBox(
                    "Save diagnostics service is not registered.",
                    MessageType.Warning);

                return;
            }

            EditorGUILayout.HelpBox(
                "Save diagnostics connected successfully.",
                MessageType.Info);

            EditorGUILayout.LabelField(
                "Save Count",
                diagnostics.GetSaveFiles()
                    .Count
                    .ToString());
        }
    }
}