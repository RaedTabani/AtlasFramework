using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Sequence.Models;
using DeviGames.Atlas.Core.Sequence.Interfaces;

namespace DeviGames.Atlas.Core.Sequence.Services
{
    public sealed class SequenceFactory :
        ISequenceFactory
    {
        private readonly SequenceStepFactoryRegistry _stepFactoryRegistry;

        public SequenceFactory(
            SequenceStepFactoryRegistry stepFactoryRegistry)
        {
            _stepFactoryRegistry = stepFactoryRegistry ?? throw new ArgumentNullException(nameof(stepFactoryRegistry));
        }

        public SequenceRuntime Create(
            SequenceDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            var steps =
                new List<ISequenceStep>();

            foreach (SequenceStepDefinition stepDefinition in
                definition.Steps)
            {
                if (stepDefinition == null)
                {
                    throw new InvalidOperationException(
                        $"Sequence '{definition.Id}' contains a null step definition.");
                }

                ISequenceStepFactory factory =
                    _stepFactoryRegistry.Get(
                        stepDefinition.Type);

                ISequenceStep step =
                    factory.Create(
                        stepDefinition);

                if (step == null)
                {
                    throw new InvalidOperationException(
                        $"Sequence step factory '{factory.Type}' returned null.");
                }

                steps.Add(
                    step);
            }

            return new SequenceRuntime(
                definition,
                steps);
        }
    }
}