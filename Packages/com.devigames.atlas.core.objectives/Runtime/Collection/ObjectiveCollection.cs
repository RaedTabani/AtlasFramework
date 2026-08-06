using System;
using System.Collections.Generic;
using DeviGames.Atlas.Core.Objectives.Interfaces;
using DeviGames.Atlas.Core.Objectives.Runtime;

namespace DeviGames.Atlas.Core.Objectives.Collections
{
    public sealed class ObjectiveCollection :
        IObjectiveCollection
    {
        private readonly List<ObjectiveRuntime> _objectives =
            new();

        private readonly Dictionary<string, ObjectiveRuntime>
            _objectivesById =
                new(StringComparer.Ordinal);

        public IReadOnlyList<ObjectiveRuntime> Objectives =>
            _objectives;

        public int Count =>
            _objectives.Count;

        public void Add(
            ObjectiveRuntime objective)
        {
            if (objective == null)
            {
                throw new ArgumentNullException(
                    nameof(objective));
            }

            string objectiveId =
                objective.Definition.Id;

            if (_objectivesById.ContainsKey(
                    objectiveId))
            {
                throw new InvalidOperationException(
                    $"An objective with ID '{objectiveId}' " +
                    "already exists.");
            }

            _objectives.Add(
                objective);

            _objectivesById.Add(
                objectiveId,
                objective);
        }

        public bool Remove(
            ObjectiveRuntime objective)
        {
            if (objective == null)
                return false;

            string objectiveId =
                objective.Definition.Id;

            if (!_objectivesById.TryGetValue(
                    objectiveId,
                    out ObjectiveRuntime existing))
            {
                return false;
            }

            if (!ReferenceEquals(
                    existing,
                    objective))
            {
                return false;
            }

            _objectivesById.Remove(
                objectiveId);

            return RemoveReference(
                objective);
        }

        public bool Contains(
            ObjectiveRuntime objective)
        {
            if (objective == null)
                return false;

            return _objectivesById.TryGetValue(
                       objective.Definition.Id,
                       out ObjectiveRuntime existing)
                   && ReferenceEquals(
                       existing,
                       objective);
        }

        public bool TryGet(
            string objectiveId,
            out ObjectiveRuntime objective)
        {
            if (string.IsNullOrWhiteSpace(
                    objectiveId))
            {
                objective = null;
                return false;
            }

            return _objectivesById.TryGetValue(
                objectiveId,
                out objective);
        }

        public ObjectiveRuntime Get(
            string objectiveId)
        {
            if (string.IsNullOrWhiteSpace(
                    objectiveId))
            {
                throw new ArgumentException(
                    "Objective ID cannot be empty.",
                    nameof(objectiveId));
            }

            if (!_objectivesById.TryGetValue(
                    objectiveId,
                    out ObjectiveRuntime objective))
            {
                throw new KeyNotFoundException(
                    $"Objective '{objectiveId}' does not exist.");
            }

            return objective;
        }

        public void Clear()
        {
            _objectives.Clear();
            _objectivesById.Clear();
        }

        private bool RemoveReference(
            ObjectiveRuntime objective)
        {
            for (int index = 0;
                 index < _objectives.Count;
                 index++)
            {
                if (!ReferenceEquals(
                        _objectives[index],
                        objective))
                {
                    continue;
                }

                _objectives.RemoveAt(
                    index);

                return true;
            }

            return false;
        }
    }
}