using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Execution.Services;
using DeviGames.Atlas.Core.Execution.Systems;
using DeviGames.Atlas.Core.Execution.Interfaces;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Core.Services.Interfaces;
using DeviGames.Atlas.Unity.Execution.Runtime;


namespace DeviGames.Atlas.Unity.Execution.Installation
{
    public sealed class ExecutionInstaller :
        IAtlasInstaller
    {
        public void Install(
            AtlasInstallationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(
                    nameof(context));
            }

            ServiceRegistry services =
                context.Services;

            EnsureNotInstalled(
                services);

            var systemCollection =
                new SystemCollection();

            var executionService =
                new ExecutionService(
                    systemCollection);

            services.Register<ISystemCollection>(
                systemCollection);

            services.Register<IExecutionService>(
                executionService);

            var bridgeInstaller =
                new AtlasExecutionInstaller();

            AtlasExecutionBehaviour behaviour =
                bridgeInstaller.Install(
                    executionService);

            services.Register(
                behaviour);
        }

        private static void EnsureNotInstalled(
            ServiceRegistry services)
        {
            if (services.TryResolve<
                    IExecutionService>(
                    out _))
            {
                throw new InvalidOperationException(
                    "Atlas execution has already been installed.");
            }

            if (services.TryResolve<
                    ISystemCollection>(
                    out _))
            {
                throw new InvalidOperationException(
                    "Atlas system collection has already been installed.");
            }
        }
    }
}