using System;
using System.Collections.Generic;
using DeviGames.Atlas.Core.Content.Models;

namespace DeviGames.Atlas.Core.Content.Validation
{
    public sealed class ContentPackageValidator
    {
        public ContentValidationResult Validate(ContentPackageData package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            var result = new ContentValidationResult();

            ValidatePackage(package, result);
            ValidateObjectives(package, result);
            ValidateMissions(package, result);

            ValidateRewards(package, result);
            ValidateMissionRewardBindings(package, result);

            return result;
        }

        private static void ValidatePackage(
            ContentPackageData package,
            ContentValidationResult result)
        {
            if (string.IsNullOrWhiteSpace(
                    package.PackageId))
            {
                result.AddError(
                    "Package ID cannot be empty.");
            }

            if (package.Version < 1)
            {
                result.AddError(
                    "Package version must be at least 1.");
            }
        }

        private static void ValidateObjectives(
            ContentPackageData package,
            ContentValidationResult result)
        {
            if (package.Objectives == null)
            {
                result.AddError(
                    "Objectives collection cannot be null.");

                return;
            }

            var objectiveIds =
                new HashSet<string>(
                    StringComparer.Ordinal);

            foreach (ObjectiveContentData objective
                     in package.Objectives)
            {
                if (objective == null)
                {
                    result.AddError(
                        "Objective entry cannot be null.");

                    continue;
                }

                if (string.IsNullOrWhiteSpace(
                        objective.Id))
                {
                    result.AddError(
                        "Objective ID cannot be empty.");

                    continue;
                }

                if (!objectiveIds.Add(
                        objective.Id))
                {
                    result.AddError(
                        $"Duplicate objective ID " +
                        $"'{objective.Id}'.");
                }

                if (string.IsNullOrWhiteSpace(
                        objective.DisplayName))
                {
                    result.AddError(
                        $"Objective '{objective.Id}' " +
                        "must have a display name.");
                }

                if (objective.TargetValue < 1)
                {
                    result.AddError(
                        $"Objective '{objective.Id}' " +
                        "must have a target value of at least 1.");
                }
            }
        }

        private static void ValidateMissions(
            ContentPackageData package,
            ContentValidationResult result)
        {
            if (package.Missions == null)
            {
                result.AddError(
                    "Missions collection cannot be null.");

                return;
            }

            var availableObjectiveIds =
                new HashSet<string>(
                    StringComparer.Ordinal);

            foreach (ObjectiveContentData objective
                     in package.Objectives)
            {
                if (objective != null &&
                    !string.IsNullOrWhiteSpace(
                        objective.Id))
                {
                    availableObjectiveIds.Add(
                        objective.Id);
                }
            }

            var missionIds =
                new HashSet<string>(
                    StringComparer.Ordinal);

            foreach (MissionContentData mission
                     in package.Missions)
            {
                if (mission == null)
                {
                    result.AddError(
                        "Mission entry cannot be null.");

                    continue;
                }

                if (string.IsNullOrWhiteSpace(
                        mission.Id))
                {
                    result.AddError(
                        "Mission ID cannot be empty.");

                    continue;
                }

                if (!missionIds.Add(
                        mission.Id))
                {
                    result.AddError(
                        $"Duplicate mission ID " +
                        $"'{mission.Id}'.");
                }

                if (string.IsNullOrWhiteSpace(
                        mission.DisplayName))
                {
                    result.AddError(
                        $"Mission '{mission.Id}' " +
                        "must have a display name.");
                }

                if (mission.ObjectiveIds == null ||
                    mission.ObjectiveIds.Length == 0)
                {
                    result.AddError(
                        $"Mission '{mission.Id}' " +
                        "must contain at least one objective.");

                    continue;
                }

                var missionObjectiveIds =
                    new HashSet<string>(
                        StringComparer.Ordinal);

                foreach (string objectiveId
                         in mission.ObjectiveIds)
                {
                    if (string.IsNullOrWhiteSpace(
                            objectiveId))
                    {
                        result.AddError(
                            $"Mission '{mission.Id}' " +
                            "contains an empty objective ID.");

                        continue;
                    }

                    if (!missionObjectiveIds.Add(
                            objectiveId))
                    {
                        result.AddError(
                            $"Mission '{mission.Id}' " +
                            $"contains duplicate objective " +
                            $"'{objectiveId}'.");
                    }

                    if (!availableObjectiveIds.Contains(
                            objectiveId))
                    {
                        result.AddError(
                            $"Mission '{mission.Id}' " +
                            $"references unknown objective " +
                            $"'{objectiveId}'.");
                    }
                }
            }
        }
        private static void ValidateItemCollectedBindings(
            ContentPackageData package,
            ContentValidationResult result)
        {
            if (package.ItemCollectedObjectiveBindings == null)
                return;

            var objectiveIds =
                BuildObjectiveIdSet(
                    package);

            foreach (ItemCollectedObjectiveBindingData binding
                    in package.ItemCollectedObjectiveBindings)
            {
                if (binding == null)
                {
                    result.AddError(
                        "Item objective binding cannot be null.");

                    continue;
                }

                if (string.IsNullOrWhiteSpace(
                        binding.ObjectiveId))
                {
                    result.AddError(
                        "Item objective binding must reference an objective.");

                    continue;
                }

                if (!objectiveIds.Contains(
                        binding.ObjectiveId))
                {
                    result.AddError(
                        $"Item binding references unknown objective " +
                        $"'{binding.ObjectiveId}'.");
                }

                if (string.IsNullOrWhiteSpace(
                        binding.ItemId))
                {
                    result.AddError(
                        $"Item binding for objective " +
                        $"'{binding.ObjectiveId}' has no item ID.");
                }

                if (binding.ProgressAmount < 1)
                {
                    result.AddError(
                        $"Item binding for objective " +
                        $"'{binding.ObjectiveId}' must have " +
                        $"a positive progress amount.");
                }
            }
        }
        private static HashSet<string> BuildObjectiveIdSet(
            ContentPackageData package)
        {
            var objectiveIds =
                new HashSet<string>(
                    StringComparer.Ordinal);

            if (package.Objectives == null)
            {
                return objectiveIds;
            }

            foreach (ObjectiveContentData objective
                    in package.Objectives)
            {
                if (objective == null)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(
                        objective.Id))
                {
                    continue;
                }

                objectiveIds.Add(
                    objective.Id);
            }

            return objectiveIds;
        }

        private static void ValidateRewards(
            ContentPackageData package,
            ContentValidationResult result)
        {
            var rewardIds = new HashSet<string>(StringComparer.Ordinal);

            foreach (RewardContentData reward in package.Rewards)
            {
                if (string.IsNullOrWhiteSpace(reward.Id))
                {
                    result.AddError(
                        "Reward ID cannot be empty.");

                    continue;
                }

                if (!rewardIds.Add(reward.Id))
                {
                    result.AddError(
                        $"Duplicate reward ID '{reward.Id}'.");
                }

                if (string.IsNullOrWhiteSpace(reward.Type))
                {
                    result.AddError(
                        $"Reward '{reward.Id}' has an empty type.");
                }

                if (string.IsNullOrWhiteSpace(reward.TargetId))
                {
                    result.AddError(
                        $"Reward '{reward.Id}' has an empty target ID.");
                }

                if (reward.Amount <= 0)
                {
                    result.AddError(
                        $"Reward '{reward.Id}' must have an amount greater than zero.");
                }
            }
        }

        private static void ValidateMissionRewardBindings(
            ContentPackageData package,
            ContentValidationResult result)
        {
            foreach (MissionRewardBindingData binding in package.MissionRewardBindings)
            {
                if (string.IsNullOrWhiteSpace(binding.MissionId))
                {
                    result.AddError(
                        "Mission reward binding has an empty mission ID.");
                }

                if (string.IsNullOrWhiteSpace(binding.RewardId))
                {
                    result.AddError(
                        "Mission reward binding has an empty reward ID.");
                }
            }
        }
    }
}