using DeviGames.Atlas.Core.Triggers.Models;
using DeviGames.Atlas.Core.Triggers.Runtime;

namespace DeviGames.Atlas.Core.Triggers.Interfaces
{
    public interface ITriggerFactory
    {
        TriggerRuntime Create(
            TriggerDefinition definition);
    }
}