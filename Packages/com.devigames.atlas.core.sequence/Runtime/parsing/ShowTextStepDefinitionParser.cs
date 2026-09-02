using System;

using DeviGames.Atlas.Core.Sequence.Models;
using DeviGames.Atlas.Core.Sequence.Interfaces;

using Newtonsoft.Json.Linq;

namespace DeviGames.Atlas.Core.Sequence.Parsing
{
    public sealed class ShowTextStepDefinitionParser :
        ISequenceStepDefinitionParser
    {
        public string Type =>
            ShowTextStepDefinition.StepType;

        public SequenceStepDefinition Parse(
            JObject json)
        {
            if (json == null)
            {
                throw new ArgumentNullException(
                    nameof(json));
            }

            string text =
                json.Value<string>(
                    "Text");

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new InvalidOperationException(
                    "Show text sequence step requires a non-empty 'Text' value.");
            }

            return new ShowTextStepDefinition(
                text);
        }
    }
}