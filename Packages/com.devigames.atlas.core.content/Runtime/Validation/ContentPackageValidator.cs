using System;
using System.Collections.Generic;
using DeviGames.Atlas.Core.Content.Models;

namespace DeviGames.Atlas.Core.Content.Validation
{
    public sealed class ContentPackageValidator
    {
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

            ValidatePackage(
                package,
                result);

            ValidateObjectives(
                package,
                result);

            ValidateMissions(
                package,
                result);

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
    }
}