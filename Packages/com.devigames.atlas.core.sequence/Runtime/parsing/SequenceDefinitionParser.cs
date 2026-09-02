using System;

using DeviGames.Atlas.Core.Sequence.Models;
using DeviGames.Atlas.Core.Sequence.Interfaces;
using DeviGames.Atlas.Core.Sequence.Services;

using Newtonsoft.Json.Linq;

namespace DeviGames.Atlas.Core.Sequence.Parsing
{
    public sealed class SequenceDefinitionParser
    {
        private readonly SequenceStepDefinitionParserRegistry _stepParserRegistry;

        public SequenceDefinitionParser(
            SequenceStepDefinitionParserRegistry stepParserRegistry)
        {
            _stepParserRegistry = stepParserRegistry ?? throw new ArgumentNullException(nameof(stepParserRegistry));
        }

        public SequenceDefinition Parse(
            JObject json)
        {
            if (json == null)
            {
                throw new ArgumentNullException(
                    nameof(json));
            }

            string id =
                json.Value<string>(
                    "Id");

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new InvalidOperationException(
                    "Sequence requires a non-empty 'Id'.");
            }

            var definition =
                new SequenceDefinition(
                    id);

            JToken stepsToken =
                json["Steps"];

            if (stepsToken == null)
            {
                return definition;
            }

            if (stepsToken is not JArray stepsArray)
            {
                throw new InvalidOperationException(
                    $"Sequence '{id}' property 'Steps' must be an array.");
            }

            foreach (JToken stepToken in stepsArray)
            {
                if (stepToken is not JObject stepObject)
                {
                    throw new InvalidOperationException(
                        $"Sequence '{id}' contains an invalid step.");
                }

                string type =
                    stepObject.Value<string>(
                        "Type");

                if (string.IsNullOrWhiteSpace(type))
                {
                    throw new InvalidOperationException(
                        $"Sequence '{id}' contains a step without a valid 'Type'.");
                }

                ISequenceStepDefinitionParser parser =
                    _stepParserRegistry.Get(
                        type);

                SequenceStepDefinition stepDefinition =
                    parser.Parse(
                        stepObject);

                if (stepDefinition == null)
                {
                    throw new InvalidOperationException(
                        $"Sequence step definition parser '{type}' returned null.");
                }

                definition.Steps.Add(
                    stepDefinition);
            }

            return definition;
        }
    }
}