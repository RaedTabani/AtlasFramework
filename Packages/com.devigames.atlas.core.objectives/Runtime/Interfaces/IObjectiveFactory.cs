using DeviGames.Atlas.Core.Objectives.Models;
using DeviGames.Atlas.Core.Objectives.Runtime;

namespace DeviGames.Atlas.Core.Objectives.Interfaces
{
    public interface IObjectiveFactory
    {
        ObjectiveRuntime Create(
            ObjectiveDefinition definition);
    }
}