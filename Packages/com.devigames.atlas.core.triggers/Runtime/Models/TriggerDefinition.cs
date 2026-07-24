using DeviGames.Atlas.Core.Triggers.Conditions;
using DeviGames.Atlas.Core.Triggers.Interfaces;
using System;

namespace DeviGames.Atlas.Core.Triggers.Models
{
    public sealed class TriggerDefinition
    {
        public string Id { get; }

        public bool Repeatable { get; }

        public ITriggerConditionDefinition Condition { get; }

        public TriggerDefinition(
            string id,
            bool repeatable ,
            ITriggerConditionDefinition condition)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException(
                    "Trigger ID cannot be empty.",
                    nameof(id));
            }

            Id = id;
            Repeatable = repeatable;

            Condition =
                condition
                ?? throw new ArgumentNullException(
                    nameof(condition));
        }
    }
}