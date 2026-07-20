using DeviGames.Atlas.Core.Execution.Interfaces;
using DeviGames.Atlas.Core.Execution.Models;
namespace DeviGames.Atlas.Core.Execution.Tests
{
    public sealed class TestMultiPhaseSystem :
        IUpdatable,
        IFixedUpdatable,
        ILateUpdatable
    {
        public int UpdateCount { get; private set; }

        public int FixedUpdateCount { get; private set; }

        public int LateUpdateCount { get; private set; }

        public void Update(
            ExecutionContext context)
        {
            UpdateCount++;
        }

        public void FixedUpdate(
            ExecutionContext context)
        {
            FixedUpdateCount++;
        }

        public void LateUpdate(
            ExecutionContext context)
        {
            LateUpdateCount++;
        }
    }   
}
