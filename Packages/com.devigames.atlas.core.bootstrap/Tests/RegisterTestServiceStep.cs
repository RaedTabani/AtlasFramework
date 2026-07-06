using System.Threading.Tasks;
using DeviGames.Atlas.Core.Bootstrap.Context;
using DeviGames.Atlas.Core.Bootstrap.Steps;

namespace DeviGames.Atlas.Core.Bootstrap.Tests
{
    internal sealed class RegisterTestServiceStep : IBootstrapStep
    {
        public string Name => "Register Test Service";

        public Task ExecuteAsync(BootstrapContext context)
        {
            context.Services.Register(new TestService());

            return Task.CompletedTask;
        }
    }

    internal sealed class TestService
    {
    }
}