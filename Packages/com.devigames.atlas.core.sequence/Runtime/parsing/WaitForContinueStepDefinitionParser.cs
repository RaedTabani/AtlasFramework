using System;

using DeviGames.Atlas.Core.Sequence.Models;
using DeviGames.Atlas.Core.Sequence.Interfaces;

using Newtonsoft.Json.Linq;

namespace DeviGames.Atlas.Core.Sequence.Parsing
{
    public sealed class WaitForContinueStepDefinitionParser :
        ISequenceStepDefinitionParser
    {
        public string Type =>
            WaitForContinueStepDefinition.StepType;

        public SequenceStepDefinition Parse(
            JObject json)
        {
            if (json == null)
            {
                throw new ArgumentNullException(
                    nameof(json));
            }

            return new WaitForContinueStepDefinition();
        }
    }
}