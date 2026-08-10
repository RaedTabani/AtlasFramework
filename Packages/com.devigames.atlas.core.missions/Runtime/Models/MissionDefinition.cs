using System;
using System.Collections.Generic;

namespace DeviGames.Atlas.Core.Missions.Models
{
    public sealed class MissionDefinition
    {
        private readonly string[] _objectiveIds;

        public string Id { get; }

        public string DisplayName { get; }

        public string Description { get; }

        public IReadOnlyList<string> ObjectiveIds =>
            _objectiveIds;

        public int ObjectiveCount =>
            _objectiveIds.Length;

        public MissionDefinition(
            string id,
            string displayName,
            string description,
            IEnumerable<string> objectiveIds)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException(
                    "Mission ID cannot be empty.",
                    nameof(id));
            }

            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException(
                    "Mission display name cannot be empty.",
                    nameof(displayName));
            }

            if (objectiveIds == null)
            {
                throw new ArgumentNullException(
                    nameof(objectiveIds));
            }

            _objectiveIds =
                ValidateAndCopyObjectiveIds(
                    objectiveIds);

            Id = id;
            DisplayName = displayName;
            Description =
                description ?? string.Empty;
        }

        private static string[] ValidateAndCopyObjectiveIds(
            IEnumerable<string> objectiveIds)
        {
            var ids =
                new List<string>();

            var uniqueIds =
                new HashSet<string>(
                    StringComparer.Ordinal);

            foreach (string objectiveId
                     in objectiveIds)
            {
                if (string.IsNullOrWhiteSpace(
                        objectiveId))
                {
                    throw new ArgumentException(
                        "Mission objective IDs cannot be empty.",
                        nameof(objectiveIds));
                }

                if (!uniqueIds.Add(
                        objectiveId))
                {
                    throw new ArgumentException(
                        $"Duplicate objective ID " +
                        $"'{objectiveId}' found in mission.",
                        nameof(objectiveIds));
                }

                ids.Add(
                    objectiveId);
            }

            if (ids.Count == 0)
            {
                throw new ArgumentException(
                    "A mission must contain at least one objective.",
                    nameof(objectiveIds));
            }

            return ids.ToArray();
        }
    }
}