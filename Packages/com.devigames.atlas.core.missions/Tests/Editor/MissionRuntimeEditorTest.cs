using UnityEngine;
using NUnit.Framework;
using DeviGames.Atlas.Core.Missions.Models;
using DeviGames.Atlas.Core.Missions.Runtime;

namespace DeviGames.Atlas.Core.Missions.Tests.Editor
{
    public class MissionRuntimeEditorTest 
    {
        [Test]
        public void Constructor_StartsActive()
        {
            MissionRuntime runtime =
                CreateMission();

            Assert.That(
                runtime.State,
                Is.EqualTo(
                    MissionState.Active));

            Assert.That(
                runtime.CompletedObjectiveCount,
                Is.Zero);

            Assert.That(
                runtime.IsCompleted,
                Is.False);
        }

        [Test]
        public void NotifyObjectiveCompleted_KnownObjective_RecordsCompletion()
        {
            MissionRuntime runtime =
                CreateMission();

            MissionUpdateResult result =
                runtime.NotifyObjectiveCompleted(
                    "objective.a");

            Assert.That(
                result,
                Is.EqualTo(
                    MissionUpdateResult.ObjectiveCompleted));

            Assert.That(
                runtime.CompletedObjectiveCount,
                Is.EqualTo(1));

            Assert.That(
                runtime.IsObjectiveCompleted(
                    "objective.a"),
                Is.True);
        }

        [Test]
        public void NotifyObjectiveCompleted_UnknownObjective_DoesNothing()
        {
            MissionRuntime runtime =
                CreateMission();

            MissionUpdateResult result =
                runtime.NotifyObjectiveCompleted(
                    "objective.unknown");

            Assert.That(
                result,
                Is.EqualTo(
                    MissionUpdateResult.None));

            Assert.That(
                runtime.CompletedObjectiveCount,
                Is.Zero);
        }

        [Test]
        public void NotifyObjectiveCompleted_SameObjectiveTwice_CountsOnce()
        {
            MissionRuntime runtime =
                CreateMission();

            runtime.NotifyObjectiveCompleted(
                "objective.a");

            MissionUpdateResult second =
                runtime.NotifyObjectiveCompleted(
                    "objective.a");

            Assert.That(
                second,
                Is.EqualTo(
                    MissionUpdateResult.None));

            Assert.That(
                runtime.CompletedObjectiveCount,
                Is.EqualTo(1));
        }
        [Test]
        public void NotifyObjectiveCompleted_FinalObjective_CompletesMission()
        {
            MissionRuntime runtime =
                CreateMission();

            runtime.NotifyObjectiveCompleted(
                "objective.a");

            MissionUpdateResult result =
                runtime.NotifyObjectiveCompleted(
                    "objective.b");

            Assert.That(
                result,
                Is.EqualTo(
                    MissionUpdateResult.Completed));

            Assert.That(
                runtime.IsCompleted,
                Is.True);

            Assert.That(
                runtime.CompletedObjectiveCount,
                Is.EqualTo(2));
        }

        private static MissionRuntime CreateMission()
        {
            return new MissionRuntime(
                new MissionDefinition(
                    id: "mission.test",
                    displayName: "Test Mission",
                    description: "",
                    objectiveIds:
                        new[]
                        {
                            "objective.a",
                            "objective.b"
                        }));
        }
    }
}
