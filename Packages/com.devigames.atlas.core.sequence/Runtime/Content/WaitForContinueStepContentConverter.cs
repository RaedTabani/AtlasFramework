using System;

using DeviGames.Atlas.Core.Content.Models;
using DeviGames.Atlas.Core.Sequence.Interfaces;
using DeviGames.Atlas.Core.Sequence.Models;

namespace DeviGames.Atlas.Core.Sequence.Content
{
    public sealed class WaitForContinueStepContentConverter :
        ISequenceStepContentConverter
    {
        public string Type =>
            WaitForContinueStepDefinition.StepType;

        public SequenceStepDefinition Convert(
            SequenceStepContentData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(
                    nameof(data));
            }

            return new WaitForContinueStepDefinition();
        }
    }
}