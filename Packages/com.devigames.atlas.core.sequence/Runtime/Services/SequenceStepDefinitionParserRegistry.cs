using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Sequence.Interfaces;

namespace DeviGames.Atlas.Core.Sequence.Services
{
    public sealed class SequenceStepDefinitionParserRegistry
    {
        private readonly Dictionary<string, ISequenceStepDefinitionParser> _parsers =
            new(StringComparer.Ordinal);

        public void Register(
            ISequenceStepDefinitionParser parser)
        {
            if (parser == null)
            {
                throw new ArgumentNullException(
                    nameof(parser));
            }

            if (string.IsNullOrWhiteSpace(parser.Type))
            {
                throw new ArgumentException(
                    "Sequence step definition parser type cannot be null or empty.",
                    nameof(parser));
            }

            if (!_parsers.TryAdd(
                parser.Type,
                parser))
            {
                throw new InvalidOperationException(
                    $"Sequence step definition parser '{parser.Type}' is already registered.");
            }
        }

        public bool TryGet(
            string type,
            out ISequenceStepDefinitionParser parser)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                parser = null;
                return false;
            }

            return _parsers.TryGetValue(
                type,
                out parser);
        }

        public ISequenceStepDefinitionParser Get(
            string type)
        {
            if (!TryGet(
                type,
                out ISequenceStepDefinitionParser parser))
            {
                throw new InvalidOperationException(
                    $"Sequence step definition parser '{type}' is not registered.");
            }

            return parser;
        }
    }
}