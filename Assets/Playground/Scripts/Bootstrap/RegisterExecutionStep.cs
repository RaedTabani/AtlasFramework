using System;
using System.Threading;
using System.Threading.Tasks;

using DeviGames.Atlas.Core.Bootstrap;
using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Execution.Services;
using DeviGames.Atlas.Core.Execution.Systems;
using DeviGames.Atlas.Core.Execution.Interfaces;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Unity.Execution.Installation;

namespace DeviGames.Playground.Bootstrap
{
    public sealed class RegisterExecutionStep :
        IBootstrapStep
    {
        public string Name =>
            nameof(RegisterExecutionStep);

        public Task ExecuteAsync(
            BootstrapContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(
                    nameof(context));
            }

            ServiceRegistry services =
                context.Services;

            var systemCollection =
                new SystemCollection();

            var executionService =
                new ExecutionService(
                    systemCollection);

            services.Register<ISystemCollection>(
                systemCollection);

            services.Register<IExecutionService>(
                executionService);

            var installer =
                new AtlasExecutionInstaller();

            installer.Install(
                executionService);

            return Task.CompletedTask;
        }
    }
}