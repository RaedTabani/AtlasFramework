using System;
using System.Collections.Generic;
using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Objectives.Definitions;
using DeviGames.Atlas.Core.Objectives.Events;
using DeviGames.Atlas.Core.Objectives.Models;


namespace DeviGames.Atlas.Core.Objectives.Services
{
    public sealed class ObjectiveService
    {
        private readonly Dictionary<string, ObjectiveState> _states = new();
        private readonly Dictionary<string, ObjectiveDefinition> _definitions = new();

        public IReadOnlyDictionary<string, ObjectiveState> States => _states;

        public void StartObjective(ObjectiveDefinition definition)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            if (string.IsNullOrWhiteSpace(definition.Id))
                throw new ArgumentException("Objective id cannot be empty.", nameof(definition));

            if (_states.ContainsKey(definition.Id))
                return;

            var state = new ObjectiveState(definition.Id, definition.TargetValue);
            state.Start();

            _states.Add(definition.Id, state);
            _definitions.Add(definition.Id, definition);

            EventBus.Publish(new ObjectiveStartedEvent(definition.Id));
        }

        public void AddProgress(string objectiveId, int amount)
        {
            if (string.IsNullOrWhiteSpace(objectiveId))
                return;

            if (amount <= 0)
                return;

            if (!_states.TryGetValue(objectiveId, out ObjectiveState state))
                return;

            if (state.IsCompleted)
                return;

            state.AddProgress(amount);

            EventBus.Publish(
                new ObjectiveProgressChangedEvent(
                    objectiveId,
                    state.CurrentValue,
                    state.TargetValue));

            if (state.IsCompleted)
            {
                EventBus.Publish(new ObjectiveCompletedEvent(objectiveId));
            }
        }

        public void ProcessSignal(ObjectiveSignal signal)
        {
            if (string.IsNullOrWhiteSpace(signal.Type))
                return;

            if (signal.Amount <= 0)
                return;

            foreach (KeyValuePair<string, ObjectiveState> pair in _states)
            {
                ObjectiveState state = pair.Value;

                if (state.IsCompleted)
                    continue;

                if (!_definitions.TryGetValue(pair.Key, out ObjectiveDefinition definition))
                    continue;

                if (definition.TriggerType != signal.Type)
                    continue;

                if (definition.TriggerTargetId != signal.TargetId)
                    continue;

                AddProgress(definition.Id, signal.Amount);
            }
        }

        public bool IsCompleted(string objectiveId)
        {
            return _states.TryGetValue(objectiveId, out ObjectiveState state)
                   && state.IsCompleted;
        }

        public void Reset()
        {
            _states.Clear();
            _definitions.Clear();
        }
    }
}