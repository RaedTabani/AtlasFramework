using DeviGames.Atlas.Core.Triggers.Conditions;
using DeviGames.Atlas.Core.Triggers.Interfaces;

namespace DeviGames.Atlas.Core.Triggers.Models
{
    public sealed class TriggerDefinition
    {
        public string Id { get; }

        public bool Repeatable { get; }

        public ITriggerCondition Condition { get; }

        public TriggerDefinition(
            string id,
            ITriggerCondition condition,
            bool repeatable = false)
        {
            Id = id;
            Condition = condition;
            Repeatable = repeatable;
        }
    }
}