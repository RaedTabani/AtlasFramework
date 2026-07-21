using System;
using DeviGames.Atlas.Core.Triggers.Interfaces;
using DeviGames.Atlas.Core.Triggers.Factories;
using DeviGames.Atlas.Core.Triggers.Models;

namespace DeviGames.Atlas.Core.Triggers.Test
{
    public sealed class BooleanConditionFactory :
        ITriggerConditionFactory
    {
        public string Type =>
            BooleanConditionDefinition.ConditionType;

        public ITriggerCondition Create(
            ITriggerConditionDefinition definition,
            TriggerBuildContext context)
        {
            if (definition is not BooleanConditionDefinition
                booleanDefinition)
            {
                throw new ArgumentException(
                    $"Expected {nameof(BooleanConditionDefinition)}.",
                    nameof(definition));
            }

            return new BooleanCondition(
                booleanDefinition.Value);
        }
    }
}