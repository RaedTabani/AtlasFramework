using UnityEditor;
using UnityEngine;

namespace DeviGames.Atlas.Dev.Hub.Editor.UI
{
    internal static class DevGuiStyles
    {
        private static GUIStyle _sectionHeader;

        public static GUIStyle SectionHeader =>
            _sectionHeader ??= CreateSectionHeader();

        private static GUIStyle CreateSectionHeader()
        {
            return new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 13,
                margin = new RectOffset(0, 0, 8, 4)
            };
        }

        public static void Section(string title)
        {
            EditorGUILayout.LabelField(
                title,
                DevGuiStyles.SectionHeader);
        }

        public static void Box(
            System.Action draw)
        {
            EditorGUILayout.BeginVertical("box");

            draw?.Invoke();

            EditorGUILayout.EndVertical();
        }

        public static void Info(string message)
        {
            EditorGUILayout.HelpBox(
                message,
                MessageType.Info);
        }

        public static void Warning(string message)
        {
            EditorGUILayout.HelpBox(
                message,
                MessageType.Warning);
        }

        public static void Error(string message)
        {
            EditorGUILayout.HelpBox(
                message,
                MessageType.Error);
        }

        public static void Metadata(
            string label,
            string value)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(
                label,
                GUILayout.Width(100));

            EditorGUILayout.LabelField(value);

            EditorGUILayout.EndHorizontal();
        }

        public static void Status(
            string label,
            bool value)
        {
            Metadata(
                label,
                value ? "Yes" : "No");
        }

        public static Vector2 ScrollView(
            Vector2 scrollPosition,
            System.Action draw)
        {
            scrollPosition =
                EditorGUILayout.BeginScrollView(
                    scrollPosition);

            draw?.Invoke();

            EditorGUILayout.EndScrollView();

            return scrollPosition;
        }
    }
}