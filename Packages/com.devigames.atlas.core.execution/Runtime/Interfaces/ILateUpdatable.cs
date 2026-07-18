using DeviGames.Atlas.Core.Execution.Models;

namespace DeviGames.Atlas.Core.Execution.Interfaces
{
    public interface ILateUpdatable
    {
        void LateUpdate(ExecutionContext context);
    }
}