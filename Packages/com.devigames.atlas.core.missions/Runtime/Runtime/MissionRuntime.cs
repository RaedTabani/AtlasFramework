using System;
using System.Collections.Generic;
using DeviGames.Atlas.Core.Missions.Models;

namespace DeviGames.Atlas.Core.Missions.Runtime
{
    public sealed class MissionRuntime
    {
        private readonly HashSet<string>
            _completedObjectiveIds =
                new(StringComparer.Ordinal);

        public MissionDefinition Definition { get; }

        public string Id =>
            Definition.Id;

        public string DisplayName =>
            Definition.DisplayName;

        public string Description =>
            Definition.Description;

        public MissionState State { get; private set; }

        public bool IsCompleted =>
            State == MissionState.Completed;

        public int ObjectiveCount =>
            Definition.ObjectiveCount;

        public int CompletedObjectiveCount =>
            _completedObjectiveIds.Count;

        public float NormalizedProgress =>
            ObjectiveCount == 0
                ? 0f
                : (float)CompletedObjectiveCount /
                  ObjectiveCount;

        public MissionRuntime(
            MissionDefinition definition)
        {
            Definition =
                definition
                ?? throw new ArgumentNullException(
                    nameof(definition));

            State =
                MissionState.Active;
        }

        public MissionUpdateResult
            NotifyObjectiveCompleted(
                string objectiveId)
        {
            if (string.IsNullOrWhiteSpace(
                    objectiveId))
            {
                return MissionUpdateResult.None;
            }

            if (IsCompleted)
            {
                return MissionUpdateResult.None;
            }

            if (!ContainsObjective(
                    objectiveId))
            {
                return MissionUpdateResult.None;
            }

            if (!_completedObjectiveIds.Add(
                    objectiveId))
            {
                return MissionUpdateResult.None;
            }

            if (_completedObjectiveIds.Count >=
                Definition.ObjectiveCount)
            {
                State =
                    MissionState.Completed;

                return MissionUpdateResult.Completed;
            }

            return MissionUpdateResult.ObjectiveCompleted;
        }

        public bool ContainsObjective(
            string objectiveId)
        {
            if (string.IsNullOrWhiteSpace(
                    objectiveId))
            {
                return false;
            }

            IReadOnlyList<string> objectiveIds =
                Definition.ObjectiveIds;

            for (int index = 0;
                 index < objectiveIds.Count;
                 index++)
            {
                if (string.Equals(
                        objectiveIds[index],
                        objectiveId,
                        StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsObjectiveCompleted(
            string objectiveId)
        {
            if (string.IsNullOrWhiteSpace(
                    objectiveId))
            {
                return false;
            }

            return _completedObjectiveIds.Contains(
                objectiveId);
        }

        public void Reset()
        {
            _completedObjectiveIds.Clear();
            State = MissionState.Active;
        }
    }
}