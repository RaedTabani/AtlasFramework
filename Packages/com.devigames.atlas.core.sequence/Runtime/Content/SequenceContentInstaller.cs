using System;

using DeviGames.Atlas.Core.Content.Interfaces;
using DeviGames.Atlas.Core.Content.Models;
using DeviGames.Atlas.Core.Sequence.Collections;
using DeviGames.Atlas.Core.Sequence.Interfaces;
using DeviGames.Atlas.Core.Sequence.Models;

namespace DeviGames.Atlas.Core.Sequence.Content
{
    public sealed class SequenceContentInstaller :
        IContentPackageConsumer
    {
        private readonly SequenceDefinitionCollection _sequenceCollection;
        private readonly SequenceStepContentConverterRegistry _converterRegistry;

        public int Order =>
            150;

        public SequenceContentInstaller(
            SequenceDefinitionCollection sequenceCollection,
            SequenceStepContentConverterRegistry converterRegistry)
        {
            _sequenceCollection = sequenceCollection ?? throw new ArgumentNullException(nameof(sequenceCollection));
            _converterRegistry = converterRegistry ?? throw new ArgumentNullException(nameof(converterRegistry));
        }

        public void Install(
            ContentPackageData package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(
                    nameof(package));
            }

            foreach (SequenceContentData data in package.Sequences)
            {
                InstallSequence(
                    data);
            }
        }

        private void InstallSequence(
            SequenceContentData data)
        {
            if (data == null)
            {
                throw new InvalidOperationException(
                    "Content package contains a null sequence.");
            }

            if (string.IsNullOrWhiteSpace(data.Id))
            {
                throw new InvalidOperationException(
                    "Sequence content requires a non-empty ID.");
            }

            var definition =
                new SequenceDefinition(
                    data.Id);

            SequenceStepContentData[] steps =
                data.Steps ?? Array.Empty<SequenceStepContentData>();

            foreach (SequenceStepContentData stepData in steps)
            {
                if (stepData == null)
                {
                    throw new InvalidOperationException(
                        $"Sequence '{data.Id}' contains a null step.");
                }

                if (string.IsNullOrWhiteSpace(stepData.Type))
                {
                    throw new InvalidOperationException(
                        $"Sequence '{data.Id}' contains a step without a valid type.");
                }

                ISequenceStepContentConverter converter =
                    _converterRegistry.Get(
                        stepData.Type);

                SequenceStepDefinition stepDefinition =
                    converter.Convert(
                        stepData);

                if (stepDefinition == null)
                {
                    throw new InvalidOperationException(
                        $"Sequence step content converter '{stepData.Type}' returned null.");
                }

                definition.Steps.Add(
                    stepDefinition);
            }

            _sequenceCollection.Add(
                definition);
        }
    }
}