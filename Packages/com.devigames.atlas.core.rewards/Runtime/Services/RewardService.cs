using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Lifecycle.Interfaces;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Rewards.Interfaces;
using DeviGames.Atlas.Core.Rewards.Models;
using DeviGames.Atlas.Core.Rewards.Registry;

namespace DeviGames.Atlas.Core.Rewards.Services
{
    public sealed class RewardService :
        IInitializable,
        IShutdownable
    {
        private readonly RewardHandlerRegistry
            _handlerRegistry;

        private readonly List<MissionRewardBinding>
            _missionBindings =
                new();

        public RewardService(
            RewardHandlerRegistry handlerRegistry)
        {
            _handlerRegistry =
                handlerRegistry
                ?? throw new ArgumentNullException(
                    nameof(handlerRegistry));
        }

        public void Initialize()
        {
            EventBus.Subscribe<MissionCompletedEvent>(
                OnMissionCompleted);
        }

        public void Shutdown()
        {
            EventBus.Unsubscribe<MissionCompletedEvent>(
                OnMissionCompleted);
        }

        public void AddMissionReward(
            MissionRewardBinding binding)
        {
            if (binding == null)
            {
                throw new ArgumentNullException(
                    nameof(binding));
            }

            _missionBindings.Add(
                binding);
        }

        private void OnMissionCompleted(
            MissionCompletedEvent eventData)
        {
            for (int index = 0;
                 index < _missionBindings.Count;
                 index++)
            {
                MissionRewardBinding binding =
                    _missionBindings[index];

                if (!string.Equals(
                        binding.MissionId,
                        eventData.MissionId,
                        StringComparison.Ordinal))
                {
                    continue;
                }

                Grant(
                    binding.Reward);
            }
        }

        private void Grant(
            RewardDefinition reward)
        {
            IRewardHandler handler =
                _handlerRegistry.Resolve(
                    reward.Type);

            handler.Grant(
                reward);
        }
    }
}