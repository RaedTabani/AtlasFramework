using System;

using DeviGames.Atlas.Core.Content.Models;
using DeviGames.Atlas.Core.Sequence.Interfaces;
using DeviGames.Atlas.Core.Sequence.Models;

namespace DeviGames.Atlas.Core.Sequence.Content
{
    public sealed class ShowTextStepContentConverter :
        ISequenceStepContentConverter
    {
        public string Type =>
            ShowTextStepDefinition.StepType;

        public SequenceStepDefinition Convert(
            SequenceStepContentData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(
                    nameof(data));
            }

            if (string.IsNullOrWhiteSpace(data.Text))
            {
                throw new InvalidOperationException(
                    "Show text sequence step requires non-empty text.");
            }

            return new ShowTextStepDefinition(
                data.Text);
        }
    }
}