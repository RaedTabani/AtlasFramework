using DeviGames.Atlas.Core.Missions.Models;
using DeviGames.Atlas.Core.Missions.Runtime;

namespace DeviGames.Atlas.Core.Missions.Interfaces
{
    public interface IMissionFactory
    {
        MissionRuntime Create(
            MissionDefinition definition);
    }
}