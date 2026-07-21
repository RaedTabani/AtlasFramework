using DeviGames.Atlas.Core.Triggers.Interfaces;

namespace DeviGames.Atlas.Core.Triggers.Test
{
    public sealed class BooleanConditionDefinition :
        ITriggerConditionDefinition
    {
        public const string ConditionType =
            "test.boolean";

        public string Type =>
            ConditionType;

        public bool Value { get; }

        public BooleanConditionDefinition(
            bool value)
        {
            Value = value;
        }
    }
}