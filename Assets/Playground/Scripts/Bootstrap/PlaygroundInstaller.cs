using System;
using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Core.Triggers.Models;
using DeviGames.Atlas.Core.Triggers.Runtime;
using DeviGames.Atlas.Core.Triggers.Interfaces;
using DeviGames.Atlas.Gameplay.Inventory.Triggers;


namespace DeviGames.Playground.Bootstrap
{
    public class PlaygroundInstaller : IAtlasInstaller
    {
        public void Install(AtlasInstallationContext context)
        {
            if(context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            ServiceRegistry services = context.Services;
            
            EnsureNotInstalled(services);
            InstallTriggers(services);
        }

        private static void InstallTriggers(ServiceRegistry services)
        {
            var definition = new TriggerDefinition(
                    id: "playground.inventory.collect-three-keys",
                    repeatable: false,
                    condition:
                        new InventoryQuantityConditionDefinition(
                            itemId: "key",
                            requiredQuantity: 3));

            TriggerRuntime runtime = services.Resolve<ITriggerFactory>().Create(definition);
            
            services.Resolve<ITriggerCollection>().Add(runtime);
        }

        private static void EnsureNotInstalled(ServiceRegistry services)
        {
            
        }
    }
}