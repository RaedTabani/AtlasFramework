using System.Threading.Tasks;

namespace DeviGames.Atlas.Core.Bootstrap.Steps
{
    public sealed class DummyBootstrapStep : IBootstrapStep
    {
        public string Name => "Dummy";

        public Task ExecuteAsync()
        {
            return Task.CompletedTask;
        }
    }
}