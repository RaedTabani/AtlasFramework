using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Triggers.Interfaces;

namespace DeviGames.Atlas.Core.Triggers.Content
{
    public sealed class TriggerContentConditionAdapterRegistry
    {
        private readonly Dictionary<
            string,
            ITriggerContentConditionAdapter>
            _adapters =
                new(
                    StringComparer.Ordinal);

        public void Register(
            ITriggerContentConditionAdapter adapter)
        {
            if (adapter == null)
            {
                throw new ArgumentNullException(
                    nameof(adapter));
            }

            if (string.IsNullOrWhiteSpace(
                    adapter.Type))
            {
                throw new ArgumentException(
                    "Trigger content condition adapter " +
                    "must have a type.",
                    nameof(adapter));
            }

            if (!_adapters.TryAdd(
                    adapter.Type,
                    adapter))
            {
                throw new InvalidOperationException(
                    $"Trigger content condition adapter " +
                    $"'{adapter.Type}' is already registered.");
            }
        }

        public ITriggerContentConditionAdapter Resolve(
            string type)
        {
            if (string.IsNullOrWhiteSpace(
                    type))
            {
                throw new ArgumentException(
                    "Trigger condition type cannot be empty.",
                    nameof(type));
            }

            if (!_adapters.TryGetValue(
                    type,
                    out ITriggerContentConditionAdapter
                        adapter))
            {
                throw new InvalidOperationException(
                    $"No trigger content condition adapter " +
                    $"is registered for '{type}'.");
            }

            return adapter;
        }

        public bool Contains(
            string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                return false;
            }

            return _adapters.ContainsKey(
                type);
        }
    }
}