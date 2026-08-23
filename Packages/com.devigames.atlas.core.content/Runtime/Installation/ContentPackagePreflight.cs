using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Content.Models;
using DeviGames.Atlas.Core.Content.Validation;
using DeviGames.Atlas.Core.Missions.Interfaces;
using DeviGames.Atlas.Core.Objectives.Interfaces;

namespace DeviGames.Atlas.Core.Content.Installation
{
    public sealed class ContentPackagePreflight
    {
        private readonly IObjectiveCollection
            _objectiveCollection;

        private readonly IMissionCollection
            _missionCollection;

        public ContentPackagePreflight(
            IObjectiveCollection objectiveCollection,
            IMissionCollection missionCollection)
        {
            _objectiveCollection =
                objectiveCollection
                ?? throw new ArgumentNullException(
                    nameof(objectiveCollection));

            _missionCollection =
                missionCollection
                ?? throw new ArgumentNullException(
                    nameof(missionCollection));
        }

        public ContentValidationResult Validate(
            ContentPackageData package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(
                    nameof(package));
            }

            var result =
                new ContentValidationResult();

            ValidateObjectives(
                package,
                result);

            ValidateMissions(
                package,
                result);

            HashSet<string> missionIds = BuildMissionIdSet(package);
            HashSet<string> rewardIds = BuildRewardIdSet(package);

            ValidateMissionRewardReferences(
                package,
                missionIds,
                rewardIds,
                result);

            return result;
        }

        private void ValidateObjectives(
            ContentPackageData package,
            ContentValidationResult result)
        {
            if (package.Objectives == null)
            {
                return;
            }

            foreach (ObjectiveContentData objective
                     in package.Objectives)
            {
                if (objective == null ||
                    string.IsNullOrWhiteSpace(
                        objective.Id))
                {
                    continue;
                }

                if (_objectiveCollection.TryGet(
                        objective.Id,
                        out _))
                {
                    result.AddError(
                        $"Objective '{objective.Id}' " +
                        "is already installed.");
                }
            }
        }

        private void ValidateMissions(
            ContentPackageData package,
            ContentValidationResult result)
        {
            if (package.Missions == null)
            {
                return;
            }

            foreach (MissionContentData mission
                     in package.Missions)
            {
                if (mission == null ||
                    string.IsNullOrWhiteSpace(
                        mission.Id))
                {
                    continue;
                }

                if (_missionCollection.TryGet(
                        mission.Id,
                        out _))
                {
                    result.AddError(
                        $"Mission '{mission.Id}' " +
                        "is already installed.");
                }
            }
        }

        private static HashSet<string> BuildRewardIdSet(
            ContentPackageData package)
        {
            var ids = new HashSet<string>(StringComparer.Ordinal);

            foreach (RewardContentData reward in package.Rewards)
            {
                if (!string.IsNullOrWhiteSpace(reward.Id))
                {
                    ids.Add(reward.Id);
                }
            }

            return ids;
        }

        private static HashSet<string> BuildMissionIdSet(
            ContentPackageData package)
        {
            var ids = new HashSet<string>(StringComparer.Ordinal);

            foreach (MissionContentData mission in package.Missions)
            {
                if (!string.IsNullOrWhiteSpace(mission.Id))
                {
                    ids.Add(mission.Id);
                }
            }

            return ids;
        }

        private static void ValidateMissionRewardReferences(
            ContentPackageData package,
            HashSet<string> missionIds,
            HashSet<string> rewardIds,
            ContentValidationResult result)
        {
            foreach (MissionRewardBindingData binding in package.MissionRewardBindings)
            {
                if (!missionIds.Contains(binding.MissionId))
                {
                    result.AddError(
                        $"Mission reward binding references unknown mission '{binding.MissionId}'.");
                }

                if (!rewardIds.Contains(binding.RewardId))
                {
                    result.AddError(
                        $"Mission reward binding references unknown reward '{binding.RewardId}'.");
                }
            }
        }
    }
}