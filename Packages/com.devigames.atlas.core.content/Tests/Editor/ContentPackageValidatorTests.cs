using NUnit.Framework;

using DeviGames.Atlas.Core.Content.Models;
using DeviGames.Atlas.Core.Content.Validation;

namespace DeviGames.Atlas.Core.Content.Tests
{
    public sealed class ContentPackageValidatorTests
    {
        private ContentPackageValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator =
                new ContentPackageValidator();
        }

        [Test]
        public void Validate_ValidPackage_IsValid()
        {
            ContentPackageData package =
                CreateValidPackage();

            ContentValidationResult result =
                _validator.Validate(
                    package);

            Assert.That(
                result.IsValid,
                Is.True);

            Assert.That(
                result.Errors,
                Is.Empty);
        }

        [Test]
        public void Validate_EmptyPackageId_IsInvalid()
        {
            ContentPackageData package =
                CreateValidPackage();

            package.PackageId = "";

            ContentValidationResult result =
                _validator.Validate(
                    package);

            Assert.That(
                result.IsValid,
                Is.False);

            Assert.That(
                result.Errors,
                Does.Contain(
                    "Package ID cannot be empty."));
        }

        [Test]
        public void Validate_InvalidVersion_IsInvalid()
        {
            ContentPackageData package =
                CreateValidPackage();

            package.Version = 0;

            ContentValidationResult result =
                _validator.Validate(
                    package);

            Assert.That(
                result.IsValid,
                Is.False);

            Assert.That(
                result.Errors,
                Does.Contain(
                    "Package version must be at least 1."));
        }

        [Test]
        public void Validate_DuplicateObjectiveId_IsInvalid()
        {
            ContentPackageData package =
                CreateValidPackage();

            package.Objectives =
                new[]
                {
                    new ObjectiveContentData
                    {
                        Id =
                            "objective.same",

                        DisplayName =
                            "Objective A",

                        Description =
                            "",

                        TargetValue =
                            1
                    },

                    new ObjectiveContentData
                    {
                        Id =
                            "objective.same",

                        DisplayName =
                            "Objective B",

                        Description =
                            "",

                        TargetValue =
                            1
                    }
                };

            package.Missions =
                new[]
                {
                    new MissionContentData
                    {
                        Id =
                            "mission.test",

                        DisplayName =
                            "Mission",

                        Description =
                            "",

                        ObjectiveIds =
                            new[]
                            {
                                "objective.same"
                            }
                    }
                };

            ContentValidationResult result =
                _validator.Validate(
                    package);

            Assert.That(
                result.IsValid,
                Is.False);

            Assert.That(
                result.Errors,
                Does.Contain(
                    "Duplicate objective ID 'objective.same'."));
        }

        [Test]
        public void Validate_ObjectiveTargetBelowOne_IsInvalid()
        {
            ContentPackageData package =
                CreateValidPackage();

            package.Objectives[0].TargetValue =
                0;

            ContentValidationResult result =
                _validator.Validate(
                    package);

            Assert.That(
                result.IsValid,
                Is.False);

            Assert.That(
                result.Errors,
                Does.Contain(
                    "Objective 'objective.a' must have a target value of at least 1."));
        }

        [Test]
        public void Validate_DuplicateMissionId_IsInvalid()
        {
            ContentPackageData package =
                CreateValidPackage();

            package.Missions =
                new[]
                {
                    new MissionContentData
                    {
                        Id =
                            "mission.same",

                        DisplayName =
                            "Mission A",

                        Description =
                            "",

                        ObjectiveIds =
                            new[]
                            {
                                "objective.a"
                            }
                    },

                    new MissionContentData
                    {
                        Id =
                            "mission.same",

                        DisplayName =
                            "Mission B",

                        Description =
                            "",

                        ObjectiveIds =
                            new[]
                            {
                                "objective.b"
                            }
                    }
                };

            ContentValidationResult result =
                _validator.Validate(
                    package);

            Assert.That(
                result.IsValid,
                Is.False);

            Assert.That(
                result.Errors,
                Does.Contain(
                    "Duplicate mission ID 'mission.same'."));
        }

        [Test]
        public void Validate_MissionWithoutObjectives_IsInvalid()
        {
            ContentPackageData package =
                CreateValidPackage();

            package.Missions[0].ObjectiveIds =
                System.Array.Empty<string>();

            ContentValidationResult result =
                _validator.Validate(
                    package);

            Assert.That(
                result.IsValid,
                Is.False);

            Assert.That(
                result.Errors,
                Does.Contain(
                    "Mission 'mission.test' must contain at least one objective."));
        }

        [Test]
        public void Validate_MissionReferencesUnknownObjective_IsInvalid()
        {
            ContentPackageData package =
                CreateValidPackage();

            package.Missions[0].ObjectiveIds =
                new[]
                {
                    "objective.does-not-exist"
                };

            ContentValidationResult result =
                _validator.Validate(
                    package);

            Assert.That(
                result.IsValid,
                Is.False);

            Assert.That(
                result.Errors,
                Does.Contain(
                    "Mission 'mission.test' references unknown objective 'objective.does-not-exist'."));
        }

        [Test]
        public void Validate_MissionContainsDuplicateObjectiveId_IsInvalid()
        {
            ContentPackageData package =
                CreateValidPackage();

            package.Missions[0].ObjectiveIds =
                new[]
                {
                    "objective.a",
                    "objective.a"
                };

            ContentValidationResult result =
                _validator.Validate(
                    package);

            Assert.That(
                result.IsValid,
                Is.False);

            Assert.That(
                result.Errors,
                Does.Contain(
                    "Mission 'mission.test' contains duplicate objective 'objective.a'."));
        }

        [Test]
        public void Validate_NullPackage_Throws()
        {
            Assert.Throws<
                System.ArgumentNullException>(
                    () =>
                        _validator.Validate(
                            null));
        }

        private static ContentPackageData
            CreateValidPackage()
        {
            return new ContentPackageData
            {
                Version =
                    1,

                PackageId =
                    "package.test",

                Objectives =
                    new[]
                    {
                        new ObjectiveContentData
                        {
                            Id =
                                "objective.a",

                            DisplayName =
                                "Objective A",

                            Description =
                                "First objective.",

                            TargetValue =
                                3
                        },

                        new ObjectiveContentData
                        {
                            Id =
                                "objective.b",

                            DisplayName =
                                "Objective B",

                            Description =
                                "Second objective.",

                            TargetValue =
                                1
                        }
                    },

                Missions =
                    new[]
                    {
                        new MissionContentData
                        {
                            Id =
                                "mission.test",

                            DisplayName =
                                "Test Mission",

                            Description =
                                "Test mission.",

                            ObjectiveIds =
                                new[]
                                {
                                    "objective.a",
                                    "objective.b"
                                }
                        }
                    }
            };
        }
    }
}