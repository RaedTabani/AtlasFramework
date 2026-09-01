using System;

using DeviGames.Atlas.Core.Sequence.Models;
using DeviGames.Atlas.Core.Sequence.Interfaces;
using DeviGames.Atlas.Core.Sequence.Steps;

namespace DeviGames.Atlas.Core.Sequence.Factories
{
    public sealed class WaitForContinueStepFactory :
        ISequenceStepFactory
    {
        public string Type =>
            WaitForContinueStepDefinition.StepType;

        public ISequenceStep Create(
            SequenceStepDefinition definition)
        {
            if (definition is not WaitForContinueStepDefinition)
            {
                throw new ArgumentException(
                    $"Expected {nameof(WaitForContinueStepDefinition)}.",
                    nameof(definition));
            }

            return new WaitForContinueStep();
        }
    }
}