using DeviGames.Atlas.Core.Triggers.Context;
namespace DeviGames.Atlas.Core.Triggers.Conditions
{
    public interface ITriggerCondition
    {
        bool Evaluate(
            TriggerContext context);
    }
}