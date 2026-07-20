using DeviGames.Atlas.Core.Execution.Interfaces;
using DeviGames.Atlas.Core.Execution.Models;
namespace DeviGames.Atlas.Core.Execution.Tests
{
    public sealed class TestUpdateSystem :IUpdatable
    {
        public int UpdateCount { get; private set; }

        public ExecutionContext LastContext { get; private set; }

        public void Update(ExecutionContext context)
        {
            UpdateCount++;
            LastContext = context;
        }
    }    
}
