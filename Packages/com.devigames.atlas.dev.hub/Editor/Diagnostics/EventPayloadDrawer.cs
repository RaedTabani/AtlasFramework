using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DeviGames.Atlas.Dev.Hub.Editor.Diagnostics
{
    internal static class EventPayloadDrawer
    {
        private const int MaximumDepth = 2;
        private const int MaximumCollectionItems = 20;

        public static void Draw(object payload)
        {
            if (payload == null)
            {
                EditorGUILayout.LabelField("Payload", "Null");
                return;
            }

            DrawValue(
                "Payload",
                payload,
                payload.GetType(),
                0);
        }

        private static void DrawValue(
            string label,
            object value,
            Type declaredType,
            int depth)
        {
            if (value == null)
            {
                EditorGUILayout.LabelField(label, "Null");
                return;
            }

            Type runtimeType = value.GetType();

            if (IsSimpleType(runtimeType))
            {
                EditorGUILayout.LabelField(
                    label,
                    FormatSimpleValue(value));

                return;
            }

            if (value is UnityEngine.Object unityObject)
            {
                EditorGUILayout.ObjectField(
                    label,
                    unityObject,
                    unityObject.GetType(),
                    true);

                return;
            }

            if (depth >= MaximumDepth)
            {
                EditorGUILayout.LabelField(
                    label,
                    runtimeType.Name);

                return;
            }

            if (value is IEnumerable enumerable &&
                value is not string)
            {
                DrawCollection(
                    label,
                    enumerable,
                    runtimeType,
                    depth);

                return;
            }

            DrawObject(
                label,
                value,
                runtimeType,
                depth);
        }

        private static void DrawObject(
            string label,
            object value,
            Type type,
            int depth)
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField(
                label,
                type.Name,
                EditorStyles.boldLabel);

            PropertyInfo[] properties =
                type.GetProperties(
                    BindingFlags.Instance |
                    BindingFlags.Public);

            FieldInfo[] fields =
                type.GetFields(
                    BindingFlags.Instance |
                    BindingFlags.Public);

            bool drewMember = false;

            foreach (PropertyInfo property in properties)
            {
                if (!property.CanRead ||
                    property.GetIndexParameters().Length > 0)
                {
                    continue;
                }

                object propertyValue;

                try
                {
                    propertyValue = property.GetValue(value);
                }
                catch (Exception)
                {
                    EditorGUILayout.LabelField(
                        property.Name,
                        "<unavailable>");

                    continue;
                }

                DrawValue(
                    property.Name,
                    propertyValue,
                    property.PropertyType,
                    depth + 1);

                drewMember = true;
            }

            foreach (FieldInfo field in fields)
            {
                object fieldValue;

                try
                {
                    fieldValue = field.GetValue(value);
                }
                catch (Exception)
                {
                    EditorGUILayout.LabelField(
                        field.Name,
                        "<unavailable>");

                    continue;
                }

                DrawValue(
                    field.Name,
                    fieldValue,
                    field.FieldType,
                    depth + 1);

                drewMember = true;
            }

            if (!drewMember)
            {
                EditorGUILayout.LabelField(
                    "Value",
                    value.ToString());
            }

            EditorGUILayout.EndVertical();
        }

        private static void DrawCollection(
            string label,
            IEnumerable collection,
            Type collectionType,
            int depth)
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField(
                label,
                collectionType.Name,
                EditorStyles.boldLabel);

            int index = 0;

            foreach (object item in collection)
            {
                if (index >= MaximumCollectionItems)
                {
                    EditorGUILayout.LabelField(
                        $"Only the first {MaximumCollectionItems} items are shown.");

                    break;
                }

                DrawValue(
                    $"[{index}]",
                    item,
                    item?.GetType() ?? typeof(object),
                    depth + 1);

                index++;
            }

            if (index == 0)
            {
                EditorGUILayout.LabelField("Collection is empty.");
            }

            EditorGUILayout.EndVertical();
        }

        private static bool IsSimpleType(Type type)
        {
            Type underlying =
                Nullable.GetUnderlyingType(type) ?? type;

            return underlying.IsPrimitive ||
                   underlying.IsEnum ||
                   underlying == typeof(string) ||
                   underlying == typeof(decimal) ||
                   underlying == typeof(DateTime) ||
                   underlying == typeof(Guid) ||
                   underlying == typeof(TimeSpan);
        }

        private static string FormatSimpleValue(object value)
        {
            return value switch
            {
                DateTime dateTime =>
                    dateTime.ToString("O"),

                bool boolean =>
                    boolean ? "True" : "False",

                _ =>
                    value.ToString()
            };
        }
    }
}