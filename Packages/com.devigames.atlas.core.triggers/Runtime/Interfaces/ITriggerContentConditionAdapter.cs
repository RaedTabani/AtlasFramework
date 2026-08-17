using DeviGames.Atlas.Core.Content.Models;
using DeviGames.Atlas.Core.Triggers.Models;
using DeviGames.Atlas.Core.Triggers.Interfaces;

namespace DeviGames.Atlas.Core.Triggers.Interfaces
{
    public interface ITriggerContentConditionAdapter
    {
        string Type { get; }

        ITriggerConditionDefinition CreateDefinition(
            TriggerContentData data);
    }
}