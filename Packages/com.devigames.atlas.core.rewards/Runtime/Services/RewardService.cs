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
        private readonly RewardHandlerRegistry _handlerRegistry;

        private readonly List<MissionRewardBinding> _missionBindings =  new();
        private readonly Dictionary<string, RewardDefinition> _rewards = new(StringComparer.Ordinal);

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
        public void Register(RewardDefinition reward)
        {
            if (reward == null)
            {
                throw new ArgumentNullException(nameof(reward));
            }

            if (!_rewards.TryAdd(reward.Id, reward))
            {
                throw new InvalidOperationException(
                    $"Reward '{reward.Id}' is already registered.");
            }
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

        private void OnMissionCompleted(MissionCompletedEvent eventData)
        {
            for (int index = 0; index < _missionBindings.Count; index++)
            {
                MissionRewardBinding binding = _missionBindings[index];

                if (!string.Equals(binding.MissionId, eventData.MissionId, StringComparison.Ordinal))
                {
                    continue;
                }

                if (!_rewards.TryGetValue(binding.RewardId, out RewardDefinition reward))
                {
                    throw new InvalidOperationException(
                        $"Reward '{binding.RewardId}' is not registered.");
                }

                Grant(reward);
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