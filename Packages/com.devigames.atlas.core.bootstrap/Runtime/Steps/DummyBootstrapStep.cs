using System.Threading.Tasks;
using DeviGames.Atlas.Core.Bootstrap.Context;
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