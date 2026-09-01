using System;

using DeviGames.Atlas.Core.Sequence.Models;
using DeviGames.Atlas.Core.Sequence.Interfaces;
using DeviGames.Atlas.Core.Sequence.Steps;

namespace DeviGames.Atlas.Core.Sequence.Services
{
    public sealed class ShowTextStepFactory :
        ISequenceStepFactory
    {
        private readonly ISequenceTextPresenter _presenter;

        public string Type =>
            ShowTextStepDefinition.StepType;

        public ShowTextStepFactory(
            ISequenceTextPresenter presenter)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }

        public ISequenceStep Create(
            SequenceStepDefinition definition)
        {
            if (definition is not ShowTextStepDefinition showTextDefinition)
            {
                throw new ArgumentException(
                    $"Expected {nameof(ShowTextStepDefinition)}.",
                    nameof(definition));
            }

            return new ShowTextStep(
                showTextDefinition.Text,
                _presenter);
        }
    }
}