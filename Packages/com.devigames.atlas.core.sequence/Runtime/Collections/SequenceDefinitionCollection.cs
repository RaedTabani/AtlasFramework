using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Sequence.Models;
using DeviGames.Atlas.Core.Sequence.Interfaces;

namespace DeviGames.Atlas.Core.Sequence.Collections
{
    public sealed class SequenceDefinitionCollection :
        ISequenceDefinitionCollection
    {
        private readonly Dictionary<string, SequenceDefinition> _definitions =
            new(StringComparer.Ordinal);

        public IReadOnlyCollection<SequenceDefinition> Definitions =>
            _definitions.Values;

        public void Add(
            SequenceDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            if (string.IsNullOrWhiteSpace(definition.Id))
            {
                throw new ArgumentException(
                    "Sequence ID cannot be null or empty.",
                    nameof(definition));
            }

            if (!_definitions.TryAdd(
                definition.Id,
                definition))
            {
                throw new InvalidOperationException(
                    $"Sequence '{definition.Id}' is already registered.");
            }
        }

        public bool TryGet(
            string sequenceId,
            out SequenceDefinition definition)
        {
            if (string.IsNullOrWhiteSpace(sequenceId))
            {
                definition =
                    null;

                return false;
            }

            return _definitions.TryGetValue(
                sequenceId,
                out definition);
        }

        public SequenceDefinition Get(
            string sequenceId)
        {
            if (!TryGet(
                sequenceId,
                out SequenceDefinition definition))
            {
                throw new InvalidOperationException(
                    $"Sequence '{sequenceId}' is not registered.");
            }

            return definition;
        }
    }
}