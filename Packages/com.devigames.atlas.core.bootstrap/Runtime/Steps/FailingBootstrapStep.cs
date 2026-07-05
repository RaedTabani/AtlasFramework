using System;
using System.Threading.Tasks;
using DeviGames.Atlas.Core.Bootstrap.Context;
namespace DeviGames.Atlas.Core.Bootstrap.Steps
{
    public sealed class FailingBootstrapStep : IBootstrapStep
    {
        public string Name => "Failing Step";

        public Task ExecuteAsync(BootstrapContext context)
        {
            throw new InvalidOperationException("Bootstrap step failed.");
        }
    }
}