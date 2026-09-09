using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Lifecycle.Interfaces;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Rewards.Events;
using DeviGames.Atlas.Gameplay.Progression.Interfaces;
using DeviGames.Atlas.Gameplay.Progression.Models;

namespace DeviGames.Atlas.Gameplay.Progression.Services
{
    public sealed class MissionResultService :
        IMissionResultService,
        IInitializable,
        IShutdownable
    {
        private readonly Dictionary<string, List<MissionRewardResult>>
            _pendingRewards =
                new(StringComparer.Ordinal);

        public MissionResult CurrentResult { get; private set; }

        public bool HasResult =>
            CurrentResult != null;

        public void Initialize()
        {
            EventBus.Subscribe<MissionCompletedEvent>(
                OnMissionCompleted);
            
            EventBus.Subscribe<MissionFailedEvent>(
                OnMissionFailed);

            EventBus.Subscribe<MissionRewardGrantedEvent>(
                OnMissionRewardGranted);
        }

        public void Shutdown()
        {
            EventBus.Unsubscribe<MissionCompletedEvent>(
                OnMissionCompleted);

            EventBus.Unsubscribe<MissionFailedEvent>(
                OnMissionFailed);

            EventBus.Unsubscribe<MissionRewardGrantedEvent>(
                OnMissionRewardGranted);
        }

        public bool TryGetResult(
            out MissionResult result)
        {
            result =
                CurrentResult;

            return result != null;
        }

        public void Clear()
        {
            CurrentResult =
                null;

            _pendingRewards.Clear();
        }

        private void OnMissionCompleted(
            MissionCompletedEvent eventData)
        {
            if (string.IsNullOrWhiteSpace(
                    eventData.MissionId))
            {
                return;
            }

            CurrentResult =
                new MissionResult(
                    eventData.MissionId,
                    true);

            ApplyPendingRewards(
                CurrentResult);
        }

        private void OnMissionRewardGranted(
            MissionRewardGrantedEvent eventData)
        {
            if (string.IsNullOrWhiteSpace(
                    eventData.MissionId))
            {
                return;
            }

            var reward =
                new MissionRewardResult(
                    eventData.RewardId,
                    eventData.Type,
                    eventData.TargetId,
                    eventData.Amount);

            if (CurrentResult != null &&
                string.Equals(
                    CurrentResult.MissionId,
                    eventData.MissionId,
                    StringComparison.Ordinal))
            {
                CurrentResult.AddReward(
                    reward);

                return;
            }

            AddPendingReward(
                eventData.MissionId,
                reward);
        }

        private void AddPendingReward(
            string missionId,
            MissionRewardResult reward)
        {
            if (!_pendingRewards.TryGetValue(
                    missionId,
                    out List<MissionRewardResult> rewards))
            {
                rewards =
                    new List<MissionRewardResult>();

                _pendingRewards.Add(
                    missionId,
                    rewards);
            }

            rewards.Add(
                reward);
        }

        private void ApplyPendingRewards(
            MissionResult result)
        {
            if (!_pendingRewards.TryGetValue(
                    result.MissionId,
                    out List<MissionRewardResult> rewards))
            {
                return;
            }

            for (int index = 0;
                 index < rewards.Count;
                 index++)
            {
                result.AddReward(
                    rewards[index]);
            }

            _pendingRewards.Remove(
                result.MissionId);
        }

        private void OnMissionFailed(
            MissionFailedEvent eventData)
        {
            if (string.IsNullOrWhiteSpace(eventData.MissionId))
            {
                return;
            }

            CurrentResult =
                new MissionResult(
                    eventData.MissionId,
                    false);

            ApplyPendingRewards(
                CurrentResult);
        }
    }
}