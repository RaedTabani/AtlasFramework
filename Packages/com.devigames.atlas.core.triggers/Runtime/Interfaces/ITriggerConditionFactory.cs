using DeviGames.Atlas.Core.Triggers.Models;

namespace DeviGames.Atlas.Core.Triggers.Interfaces
{
    public interface ITriggerConditionFactory
    {
        string Type { get; }

        ITriggerCondition Create(
            ITriggerConditionDefinition definition,
            TriggerBuildContext context);
    }
}