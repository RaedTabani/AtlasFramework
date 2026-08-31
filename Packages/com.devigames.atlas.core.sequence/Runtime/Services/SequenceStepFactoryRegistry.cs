using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Sequence.Interfaces;

namespace DeviGames.Atlas.Core.Sequence.Services
{
    public sealed class SequenceStepFactoryRegistry
    {
        private readonly Dictionary<string, ISequenceStepFactory> _factories =
            new(StringComparer.Ordinal);

        public void Register(
            ISequenceStepFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(
                    nameof(factory));
            }

            if (string.IsNullOrWhiteSpace(factory.Type))
            {
                throw new ArgumentException(
                    "Sequence step factory type cannot be null or empty.",
                    nameof(factory));
            }

            if (!_factories.TryAdd(
                factory.Type,
                factory))
            {
                throw new InvalidOperationException(
                    $"Sequence step factory '{factory.Type}' is already registered.");
            }
        }

        public bool TryGet(
            string type,
            out ISequenceStepFactory factory)
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

        public ISequenceStepFactory Get(
            string type)
        {
            if (!TryGet(
                type,
                out ISequenceStepFactory factory))
            {
                throw new InvalidOperationException(
                    $"Sequence step factory '{type}' is not registered.");
            }

            return factory;
        }
    }
}