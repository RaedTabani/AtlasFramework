using System;
using DeviGames.Atlas.Core.Triggers.Interfaces;
using DeviGames.Atlas.Core.Triggers.Models;
using DeviGames.Atlas.Core.Triggers.Runtime;

namespace DeviGames.Atlas.Core.Triggers.Factories
{
    public sealed class TriggerFactory :
        ITriggerFactory
    {
        private readonly ITriggerConditionFactoryRegistry
            _conditionFactories;

        private readonly TriggerBuildContext
            _buildContext;

        public TriggerFactory(
            ITriggerConditionFactoryRegistry conditionFactories,
            TriggerBuildContext buildContext)
        {
            _conditionFactories =
                conditionFactories
                ?? throw new ArgumentNullException(
                    nameof(conditionFactories));

            _buildContext =
                buildContext
                ?? throw new ArgumentNullException(
                    nameof(buildContext));
        }

        public TriggerRuntime Create(
            TriggerDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            ITriggerConditionDefinition conditionDefinition =
                definition.Condition;

            if (string.IsNullOrWhiteSpace(
                    conditionDefinition.Type))
            {
                throw new InvalidOperationException(
                    $"Trigger '{definition.Id}' has an empty " +
                    $"condition type.");
            }

            ITriggerConditionFactory conditionFactory =
                _conditionFactories.Get(
                    conditionDefinition.Type);

            ITriggerCondition condition =
                conditionFactory.Create(
                    conditionDefinition,
                    _buildContext);

            if (condition == null)
            {
                throw new InvalidOperationException(
                    $"Trigger condition factory " +
                    $"'{conditionFactory.Type}' returned null " +
                    $"while building trigger '{definition.Id}'.");
            }

            return new TriggerRuntime(
                definition,
                condition);
        }
    }
}