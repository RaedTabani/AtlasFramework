using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Triggers.Factories;
using DeviGames.Atlas.Core.Triggers.Interfaces;
using DeviGames.Atlas.Core.Triggers.Models;
using DeviGames.Atlas.Core.Triggers.Registry;
using DeviGames.Atlas.Core.Triggers.Systems;
using DeviGames.Atlas.Core.Triggers.Runtime;
using DeviGames.Atlas.Core.Triggers.Content;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Core.Services.Interfaces;
using DeviGames.Atlas.Core.Rewards.Registry;
using DeviGames.Atlas.Gameplay.Inventory.Rewards;
using DeviGames.Atlas.Gameplay.Inventory.Interfaces;
using DeviGames.Atlas.Gameplay.Inventory.Services;
using DeviGames.Atlas.Gameplay.Inventory.Content;
using DeviGames.Atlas.Gameplay.Inventory.Triggers;
using DeviGames.Atlas.Gameplay.Inventory.Models;

namespace DeviGames.Atlas.Gameplay.Inventory.Installation
{
    public sealed class InventoryInstaller :
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

            var inventoryService = new InventoryService();

            services.Register<IInventoryService>(inventoryService);

            var registry = services.Resolve<ITriggerConditionFactoryRegistry>();

            registry.Register(new InventoryQuantityConditionFactory(inventoryService));

            services.Resolve<TriggerContentConditionAdapterRegistry>().Register(new InventoryQuantityTriggerContentAdapter());
            
            RewardHandlerRegistry rewardHandlers = services.Resolve<RewardHandlerRegistry>();

            rewardHandlers.Register(
                new InventoryRewardHandler(
                    services.Resolve<IInventoryService>()));
        }

        private static void EnsureNotInstalled(
            ServiceRegistry services)
        {
            if (services.TryResolve<IInventoryService>(
                    out _))
            {
                throw new InvalidOperationException(
                    "Atlas Inventory Service has already been installed.");
            }

        }
    }
}