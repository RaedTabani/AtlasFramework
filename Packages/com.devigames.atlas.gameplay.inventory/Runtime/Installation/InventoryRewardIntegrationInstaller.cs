using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Rewards.Registry;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Gameplay.Inventory.Interfaces;
using DeviGames.Atlas.Gameplay.Inventory.Rewards;

namespace DeviGames.Atlas.Gameplay.Inventory.Installation
{
    public sealed class InventoryRewardIntegrationInstaller : IAtlasInstaller
    {
        public void Install(AtlasInstallationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            ServiceRegistry services = context.Services;

            IInventoryService inventoryService = services.Resolve<IInventoryService>();
            RewardHandlerRegistry rewardHandlers = services.Resolve<RewardHandlerRegistry>();

            rewardHandlers.Register( new InventoryRewardHandler(inventoryService));
        }
    }
}