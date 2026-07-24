using System.Threading.Tasks;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Bootstrap.Interfaces;
namespace DeviGames.Atlas.Core.Bootstrap.Steps
{
    public sealed class DummyBootstrapStep : IBootstrapStep
    {
        public string Name => "Dummy";

        public Task ExecuteAsync(BootstrapContext context)
        {
            return Task.CompletedTask;
        }
    }
}