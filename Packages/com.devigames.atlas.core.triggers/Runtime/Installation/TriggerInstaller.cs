using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Triggers.Factories;
using DeviGames.Atlas.Core.Triggers.Interfaces;
using DeviGames.Atlas.Core.Triggers.Models;
using DeviGames.Atlas.Core.Triggers.Registry;
using DeviGames.Atlas.Core.Triggers.Systems;
using DeviGames.Atlas.Core.Execution.Interfaces;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Core.Services.Interfaces;

namespace DeviGames.Atlas.Core.Triggers.Installation
{
    public sealed class TriggerInstaller :
        IAtlasInstaller
    {
        public void Install(AtlasInstallationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(
                    nameof(context));
            }

            ServiceRegistry services = context.Services;

            EnsureNotInstalled(services);

            ISystemCollection systems = context.Resolve<ISystemCollection>();

            var triggerCollection =
                new TriggerCollection();

            var triggerContext =
                new TriggerContext(
                    services);

            var triggerBuildContext =
                new TriggerBuildContext(
                    services);

            var conditionFactoryRegistry =
                new TriggerConditionFactoryRegistry();

            var triggerFactory =
                new TriggerFactory(
                    conditionFactoryRegistry,
                    triggerBuildContext);

            var triggerRunner =
                new TriggerRunner(
                    triggerCollection,
                    triggerContext);

            services.Register<ITriggerCollection>(
                triggerCollection);

            services.Register<
                ITriggerConditionFactoryRegistry>(
                conditionFactoryRegistry);

            services.Register<ITriggerFactory>(
                triggerFactory);

            systems.Add(
                triggerRunner);

        }

        private static void EnsureNotInstalled(
            ServiceRegistry services)
        {
            if (services.TryResolve<
                    ITriggerCollection>(
                    out _))
            {
                throw new InvalidOperationException(
                    "Atlas trigger collection has already been installed.");
            }

            if (services.TryResolve<
                    ITriggerFactory>(
                    out _))
            {
                throw new InvalidOperationException(
                    "Atlas trigger factory has already been installed.");
            }

            if (services.TryResolve<ITriggerConditionFactoryRegistry>(
                    out _))
            {
                throw new InvalidOperationException(
                    "Atlas TriggerConditionFactoryRegistry has already been installed.");
            }

        }
    }
}