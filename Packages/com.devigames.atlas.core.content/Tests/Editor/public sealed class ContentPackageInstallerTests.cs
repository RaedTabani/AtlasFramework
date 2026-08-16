using System;

using NUnit.Framework;

using DeviGames.Atlas.Core.Content.Installation;
using DeviGames.Atlas.Core.Content.Models;
using DeviGames.Atlas.Core.Content.Validation;

using DeviGames.Atlas.Core.Missions.Collections;
using DeviGames.Atlas.Core.Missions.Factories;
using DeviGames.Atlas.Core.Missions.Services;
using DeviGames.Atlas.Core.Missions.Models;

using DeviGames.Atlas.Core.Objectives.Collections;
using DeviGames.Atlas.Core.Objectives.Factories;
using DeviGames.Atlas.Core.Objectives.Services;
using DeviGames.Atlas.Core.Objectives.Models;

namespace DeviGames.Atlas.Core.Content.Tests
{
    public sealed class ContentPackageInstallerTests
    {
        private ObjectiveCollection
            _objectiveCollection;

        private ObjectiveService
            _objectiveService;

        private MissionCollection
            _missionCollection;

        private MissionService
            _missionService;

        private ContentPackageInstaller
            _installer;

        [SetUp]
        public void SetUp()
        {
            _objectiveCollection =
                new ObjectiveCollection();

            var objectiveFactory =
                new ObjectiveFactory();

            _objectiveService =
                new ObjectiveService(
                    objectiveFactory,
                    _objectiveCollection);

            _missionCollection =
                new MissionCollection();

            var missionFactory =
                new MissionFactory();

            _missionService =
                new MissionService(
                    missionFactory,
                    _missionCollection);

            var validator =
                new ContentPackageValidator();
            
            var preflight =
                new ContentPackagePreflight(
                    _objectiveCollection,
                    _missionCollection);

            _installer =
                new ContentPackageInstaller(
                    validator,
                    preflight,
                    _objectiveService,
                    _missionService);
        }

        [Test]
        public void Install_ValidPackage_InstallsObjectives()
        {
            ContentPackageData package =
                CreateValidPackage();

            _installer.Install(
                package);

            Assert.That(
                _objectiveCollection.Count,
                Is.EqualTo(2));

            Assert.That(
                _objectiveCollection.TryGet(
                    "objective.a",
                    out _),
                Is.True);

            Assert.That(
                _objectiveCollection.TryGet(
                    "objective.b",
                    out _),
                Is.True);
        }

        [Test]
        public void Install_ValidPackage_InstallsMissions()
        {
            ContentPackageData package =
                CreateValidPackage();

            _installer.Install(
                package);

            Assert.That(
                _missionCollection.Count,
                Is.EqualTo(1));

            Assert.That(
                _missionCollection.TryGet(
                    "mission.test",
                    out var mission),
                Is.True);

            Assert.That(
                mission.ObjectiveCount,
                Is.EqualTo(2));

            Assert.That(
                mission.ContainsObjective(
                    "objective.a"),
                Is.True);

            Assert.That(
                mission.ContainsObjective(
                    "objective.b"),
                Is.True);
        }

        [Test]
        public void Install_ValidPackage_CreatesCorrectObjectiveRuntimeData()
        {
            ContentPackageData package =
                CreateValidPackage();

            _installer.Install(
                package);

            var objective =
                _objectiveCollection.Get(
                    "objective.a");

            Assert.That(
                objective.DisplayName,
                Is.EqualTo(
                    "Objective A"));

            Assert.That(
                objective.TargetValue,
                Is.EqualTo(3));

            Assert.That(
                objective.CurrentValue,
                Is.Zero);

            Assert.That(
                objective.IsCompleted,
                Is.False);
        }

        [Test]
        public void Install_InvalidPackage_Throws()
        {
            ContentPackageData package =
                CreateValidPackage();

            package.Missions[0].ObjectiveIds =
                new[]
                {
                    "objective.missing"
                };

            Assert.Throws<
                InvalidOperationException>(
                    () =>
                        _installer.Install(
                            package));
        }

        [Test]
        public void Install_InvalidPackage_DoesNotInstallObjectives()
        {
            ContentPackageData package =
                CreateValidPackage();

            package.Missions[0].ObjectiveIds =
                new[]
                {
                    "objective.missing"
                };

            try
            {
                _installer.Install(
                    package);
            }
            catch (InvalidOperationException)
            {
                // Expected.
            }

            Assert.That(
                _objectiveCollection.Count,
                Is.Zero);
        }

        [Test]
        public void Install_InvalidPackage_DoesNotInstallMissions()
        {
            ContentPackageData package =
                CreateValidPackage();

            package.Missions[0].ObjectiveIds =
                new[]
                {
                    "objective.missing"
                };

            try
            {
                _installer.Install(
                    package);
            }
            catch (InvalidOperationException)
            {
                // Expected.
            }

            Assert.That(
                _missionCollection.Count,
                Is.Zero);
        }

        [Test]
        public void Install_ValidPackage_PreservesMissionObjectiveOrder()
        {
            ContentPackageData package =
                CreateValidPackage();

            _installer.Install(
                package);

            var mission =
                _missionCollection.Get(
                    "mission.test");

            Assert.That(
                mission.Definition.ObjectiveIds[0],
                Is.EqualTo(
                    "objective.a"));

            Assert.That(
                mission.Definition.ObjectiveIds[1],
                Is.EqualTo(
                    "objective.b"));
        }
        [Test]
        public void Install_ObjectiveAlreadyExists_Throws()
        {
            _objectiveService.Register(
                new ObjectiveDefinition(
                    id:
                        "objective.a",
                    displayName:
                        "Existing Objective",
                    description:
                        "",
                    targetValue:
                        1));

            ContentPackageData package =
                CreateValidPackage();

            Assert.Throws<InvalidOperationException>(
                () =>
                    _installer.Install(
                        package));
        }
        [Test]
        public void Install_ObjectiveCollision_DoesNotPartiallyInstallPackage()
        {
            _objectiveService.Register(
                new ObjectiveDefinition(
                    id:
                        "objective.b",
                    displayName:
                        "Existing Objective",
                    description:
                        "",
                    targetValue:
                        1));

            ContentPackageData package =
                CreateValidPackage();

            Assert.Throws<InvalidOperationException>(
                () =>
                    _installer.Install(
                        package));

            // objective.a came before objective.b in our package.
            // Without preflight it could have been installed
            // before objective.b caused the failure.

            Assert.That(
                _objectiveCollection.TryGet(
                    "objective.a",
                    out _),
                Is.False);

            Assert.That(
                _missionCollection.TryGet(
                    "mission.test",
                    out _),
                Is.False);

            // The content that existed BEFORE the attempted
            // installation must remain untouched.
            Assert.That(
                _objectiveCollection.TryGet(
                    "objective.b",
                    out _),
                Is.True);
        }
        [Test]
        public void Install_MissionCollision_DoesNotInstallPackageObjectives()
        {
            _missionService.Register(
                new MissionDefinition(
                    id:
                        "mission.test",
                    displayName:
                        "Existing Mission",
                    description:
                        "",
                    objectiveIds:
                        new[]
                        {
                            "objective.existing"
                        }));

            ContentPackageData package =
                CreateValidPackage();

            Assert.Throws<InvalidOperationException>(
                () =>
                    _installer.Install(
                        package));

            Assert.That(
                _objectiveCollection.Count,
                Is.Zero);

            Assert.That(
                _missionCollection.Count,
                Is.EqualTo(1));
        }

        [Test]
        public void Install_NullPackage_Throws()
        {
            Assert.Throws<
                ArgumentNullException>(
                    () =>
                        _installer.Install(
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
                                "Collect three things.",

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
                                "Finish the task.",

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
                                "Complete both objectives.",

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