using System;

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
    }
}