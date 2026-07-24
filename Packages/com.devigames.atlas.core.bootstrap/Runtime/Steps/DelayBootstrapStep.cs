using System.Threading.Tasks;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Bootstrap.Interfaces;
namespace DeviGames.Atlas.Core.Bootstrap.Steps
{
    public sealed class DelayBootstrapStep : IBootstrapStep
    {
        private readonly string _name;
        private readonly int _milliseconds;

        public string Name => _name;

        public DelayBootstrapStep(string name, int milliseconds)
        {
            _name = name;
            _milliseconds = milliseconds;
        }

        public async Task ExecuteAsync(BootstrapContext context)
        {
            await Task.Delay(_milliseconds);
        }
    }
}