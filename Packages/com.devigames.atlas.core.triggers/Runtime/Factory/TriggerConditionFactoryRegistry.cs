using System;
using System.Collections.Generic;
using DeviGames.Atlas.Core.Triggers.Interfaces;
namespace DeviGames.Atlas.Core.Triggers.Factories
{
    public sealed class TriggerConditionFactoryRegistry :
        ITriggerConditionFactoryRegistry
    {
        private readonly Dictionary<
            string,
            ITriggerConditionFactory> _factories;

        public IReadOnlyCollection<string> Types =>
            _factories.Keys;

        public int Count =>
            _factories.Count;

        public TriggerConditionFactoryRegistry()
        {
            _factories =
                new Dictionary<
                    string,
                    ITriggerConditionFactory>(
                    StringComparer.Ordinal);
        }

        public void Register(
            ITriggerConditionFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(
                    nameof(factory));
            }

            if (string.IsNullOrWhiteSpace(
                    factory.Type))
            {
                throw new ArgumentException(
                    "Trigger condition type cannot be empty.",
                    nameof(factory));
            }

            if (!_factories.TryAdd(
                    factory.Type,
                    factory))
            {
                throw new InvalidOperationException(
                    $"A trigger condition factory with type " +
                    $"'{factory.Type}' is already registered.");
            }
        }

        public bool Unregister(
            string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return false;

            return _factories.Remove(type);
        }

        public bool Contains(
            string type)
        {
            return !string.IsNullOrWhiteSpace(type)
                   && _factories.ContainsKey(type);
        }

        public bool TryGet(
            string type,
            out ITriggerConditionFactory factory)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                factory = null;
                return false;
            }

            return _factories.TryGetValue(
                type,
                out factory);
        }

        public ITriggerConditionFactory Get(
            string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException(
                    "Trigger condition type cannot be empty.",
                    nameof(type));
            }

            if (!_factories.TryGetValue(
                    type,
                    out ITriggerConditionFactory factory))
            {
                throw new KeyNotFoundException(
                    $"No trigger condition factory is registered " +
                    $"for type '{type}'.");
            }

            return factory;
        }

        public void Clear()
        {
            _factories.Clear();
        }
    }
}