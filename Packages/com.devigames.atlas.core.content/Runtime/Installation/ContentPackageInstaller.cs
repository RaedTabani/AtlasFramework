using System;
using DeviGames.Atlas.Core.Content.Models;
using DeviGames.Atlas.Core.Content.Validation;
using DeviGames.Atlas.Core.Content.Interfaces;
using DeviGames.Atlas.Core.Missions.Models;
using DeviGames.Atlas.Core.Missions.Services;
using DeviGames.Atlas.Core.Objectives.Models;
using DeviGames.Atlas.Core.Objectives.Services;

namespace DeviGames.Atlas.Core.Content.Installation
{
    public sealed class ContentPackageInstaller : IContentPackageConsumer
    {
        public int Order => 100;
        private readonly ContentPackageValidator
            _validator;
        private readonly ContentPackagePreflight
            _preflight;

        private readonly ObjectiveService
            _objectiveService;

        private readonly MissionService
            _missionService;

        public ContentPackageInstaller(
            ContentPackageValidator validator,
            ContentPackagePreflight preflight,
            ObjectiveService objectiveService,
            MissionService missionService)
        {
            _validator =
                validator
                ?? throw new ArgumentNullException(
                    nameof(validator));

            _preflight =
                preflight
                ?? throw new ArgumentNullException(
                    nameof(preflight));

            _objectiveService =
                objectiveService
                ?? throw new ArgumentNullException(
                    nameof(objectiveService));

            _missionService =
                missionService
                ?? throw new ArgumentNullException(
                    nameof(missionService));
        }

        public void Install(
            ContentPackageData package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(
                    nameof(package));
            }

            ContentValidationResult validation =
                _validator.Validate(
                    package);

            if (!validation.IsValid)
            {
                throw new InvalidOperationException(
                    BuildValidationMessage(
                        package,
                        validation));
            }

            ContentValidationResult preflight =
                _preflight.Validate(
                    package);

            if (!preflight.IsValid)
            {
                throw new InvalidOperationException(
                    BuildValidationMessage(
                        package,
                        preflight));
            }

            // No runtime mutation happened before this point.

            InstallObjectives(
                package);

            InstallMissions(
                package);
        }

        private void InstallObjectives(
            ContentPackageData package)
        {
            foreach (ObjectiveContentData data
                     in package.Objectives)
            {
                var definition =
                    new ObjectiveDefinition(
                        id:
                            data.Id,
                        displayName:
                            data.DisplayName,
                        description:
                            data.Description,
                        targetValue:
                            data.TargetValue);

                _objectiveService.Register(
                    definition);
            }
        }

        private void InstallMissions(
            ContentPackageData package)
        {
            foreach (MissionContentData data
                    in package.Missions)
            {
                var definition =
                    new MissionDefinition(
                        id:
                            data.Id,
                        displayName:
                            data.DisplayName,
                        description:
                            data.Description,
                        objectiveIds:
                            data.ObjectiveIds,
                        introSequenceId:
                            data.IntroSequenceId,
                        outroSequenceId:
                            data.OutroSequenceId);

                _missionService.Register(
                    definition);
            }
        }

        private static string BuildValidationMessage(
            ContentPackageData package,
            ContentValidationResult validation)
        {
            string packageId =
                string.IsNullOrWhiteSpace(
                    package.PackageId)
                    ? "<unknown>"
                    : package.PackageId;

            return
                $"Content package '{packageId}' " +
                $"failed validation:{Environment.NewLine}" +
                string.Join(
                    Environment.NewLine,
                    validation.Errors);
        }
    }
}