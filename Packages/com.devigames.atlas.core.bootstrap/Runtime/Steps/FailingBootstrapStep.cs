using System;
using System.Threading.Tasks;

namespace DeviGames.Atlas.Core.Bootstrap.Steps
{
    public sealed class FailingBootstrapStep : IBootstrapStep
    {
        public string Name => "Failing Step";

        public Task ExecuteAsync()
        {
            throw new InvalidOperationException("Bootstrap step failed.");
        }
    }
}