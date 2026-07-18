using DeviGames.Atlas.Core.Execution.Models;

namespace DeviGames.Atlas.Core.Execution.Interfaces
{
    public interface IExecutionService
    {
        void Update(
            ExecutionContext context);

        void FixedUpdate(
            ExecutionContext context);

        void LateUpdate(
            ExecutionContext context);
    }
}