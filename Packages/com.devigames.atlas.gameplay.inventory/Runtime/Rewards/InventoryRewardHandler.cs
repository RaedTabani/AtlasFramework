using System;

using DeviGames.Atlas.Core.Rewards.Interfaces;
using DeviGames.Atlas.Core.Rewards.Models;
using DeviGames.Atlas.Gameplay.Inventory.Interfaces;

namespace DeviGames.Atlas.Gameplay.Inventory.Rewards
{
    public sealed class InventoryRewardHandler : IRewardHandler
    {
        public const string RewardType = "inventory.item";

        private readonly IInventoryService _inventoryService;

        public string Type => RewardType;

        public InventoryRewardHandler(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
        }

        public bool Grant(RewardDefinition reward)
        {
            if (reward == null)
            {
                throw new ArgumentNullException(nameof(reward));
            }

            return _inventoryService.Add(reward.TargetId, reward.Amount);
        }
    }
}