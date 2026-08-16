using System;

using DeviGames.Atlas.Core.Content.Models;

using UnityEngine;

namespace DeviGames.Atlas.Core.Content.Serialization
{
    public sealed class ContentJsonParser
    {
        public ContentPackageData Parse(
            string json)
        {
            if (string.IsNullOrWhiteSpace(
                    json))
            {
                throw new ArgumentException(
                    "Content JSON cannot be empty.",
                    nameof(json));
            }

            ContentPackageData data =
                JsonUtility.FromJson<
                    ContentPackageData>(
                        json);

            if (data == null)
            {
                throw new InvalidOperationException(
                    "Failed to deserialize content package.");
            }

            return data;
        }
    }
}