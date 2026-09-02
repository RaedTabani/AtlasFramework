using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Sequence.Interfaces;

namespace DeviGames.Atlas.Core.Sequence.Content
{
    public sealed class SequenceStepContentConverterRegistry
    {
        private readonly Dictionary<string, ISequenceStepContentConverter> _converters =
            new(StringComparer.Ordinal);

        public void Register(
            ISequenceStepContentConverter converter)
        {
            if (converter == null)
            {
                throw new ArgumentNullException(
                    nameof(converter));
            }

            if (string.IsNullOrWhiteSpace(converter.Type))
            {
                throw new ArgumentException(
                    "Sequence step content converter type cannot be null or empty.",
                    nameof(converter));
            }

            if (!_converters.TryAdd(
                converter.Type,
                converter))
            {
                throw new InvalidOperationException(
                    $"Sequence step content converter '{converter.Type}' is already registered.");
            }
        }

        public ISequenceStepContentConverter Get(
            string type)
        {
            if (string.IsNullOrWhiteSpace(type) ||
                !_converters.TryGetValue(
                    type,
                    out ISequenceStepContentConverter converter))
            {
                throw new InvalidOperationException(
                    $"Sequence step content converter '{type}' is not registered.");
            }

            return converter;
        }
    }
}