using DeviGames.Atlas.Core.Triggers.Conditions;
namespace DeviGames.Atlas.Core.Triggers.Definition
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