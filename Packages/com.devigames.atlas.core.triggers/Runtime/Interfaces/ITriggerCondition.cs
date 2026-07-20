using DeviGames.Atlas.Core.Triggers.Models;
namespace DeviGames.Atlas.Core.Triggers.Interfaces
{
    public interface ITriggerCondition
    {
        bool Evaluate(
            TriggerContext context);
    }
}