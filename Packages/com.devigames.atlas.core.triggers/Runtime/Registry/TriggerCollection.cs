using System;
using System.Collections.Generic;
using DeviGames.Atlas.Core.Triggers.Runtime;
using DeviGames.Atlas.Core.Triggers.Interfaces;

namespace DeviGames.Atlas.Core.Triggers.Registry
{
    public sealed class TriggerCollection :
        ITriggerCollection
    {
        private readonly List<TriggerRuntime> _triggers =
            new();

        public IReadOnlyList<TriggerRuntime> Triggers =>
            _triggers;

        public int Count =>
            _triggers.Count;

        public void Add(
            TriggerRuntime trigger)
        {
            if (trigger == null)
            {
                throw new ArgumentNullException(
                    nameof(trigger));
            }

            string triggerId =
                trigger.Definition.Id;

            if (TryGet(
                    triggerId,
                    out _))
            {
                throw new InvalidOperationException(
                    $"A trigger with the ID '{triggerId}' is already registered.");
            }

            _triggers.Add(trigger);
        }

        public bool Remove(
            TriggerRuntime trigger)
        {
            if (trigger == null)
                return false;

            int index =
                IndexOfReference(trigger);

            if (index < 0)
                return false;

            _triggers.RemoveAt(index);

            return true;
        }

        public bool Contains(
            TriggerRuntime trigger)
        {
            return trigger != null &&
                   IndexOfReference(trigger) >= 0;
        }

        public bool TryGet(
            string id,
            out TriggerRuntime trigger)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                trigger = null;

                return false;
            }

            for (int index = 0;
                 index < _triggers.Count;
                 index++)
            {
                TriggerRuntime candidate =
                    _triggers[index];

                if (!string.Equals(
                        candidate.Definition.Id,
                        id,
                        StringComparison.Ordinal))
                {
                    continue;
                }

                trigger = candidate;

                return true;
            }

            trigger = null;

            return false;
        }

        public void Clear()
        {
            _triggers.Clear();
        }

        private int IndexOfReference(
            TriggerRuntime trigger)
        {
            for (int index = 0;
                 index < _triggers.Count;
                 index++)
            {
                if (ReferenceEquals(
                        _triggers[index],
                        trigger))
                {
                    return index;
                }
            }

            return -1;
        }
    }
}