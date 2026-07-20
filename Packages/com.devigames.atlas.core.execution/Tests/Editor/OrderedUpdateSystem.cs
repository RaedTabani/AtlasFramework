using DeviGames.Atlas.Core.Execution.Interfaces;
using DeviGames.Atlas.Core.Execution.Models;
using System.Collections.Generic;

namespace DeviGames.Atlas.Core.Execution.Tests
{
    public sealed class OrderedUpdateSystem :IUpdatable
    {
        private readonly string _name;

        private readonly List<string> _calls;

        public OrderedUpdateSystem(
            string name,
            List<string> calls)
        {
            _name = name;
            _calls = calls;
        }

        public void Update(
            ExecutionContext context)
        {
            _calls.Add(_name);
        }
    }   
}
