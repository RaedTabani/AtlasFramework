using DeviGames.Atlas.Core.Execution.Models;

namespace DeviGames.Atlas.Core.Execution.Interfaces
{
    public interface IFixedUpdatable
    {
        void FixedUpdate(ExecutionContext context);
    }
}