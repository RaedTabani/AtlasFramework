using NUnit.Framework;
using DeviGames.Atlas.Core.Objectives.Models;
using DeviGames.Atlas.Core.Objectives.Runtime;
using System;
public sealed class ObjectiveRuntimeTests
{
    [Test]
    public void Constructor_StartsActiveWithZeroProgress()
    {
        var runtime =
            CreateRuntime(
                targetValue: 5);

        Assert.That(
            runtime.CurrentValue,
            Is.Zero);

        Assert.That(
            runtime.State,
            Is.EqualTo(
                ObjectiveState.Active));

        Assert.That(
            runtime.IsCompleted,
            Is.False);
    }

    [Test]
    public void AddProgress_BelowTarget_ReturnsProgressed()
    {
        var runtime =
            CreateRuntime(
                targetValue: 5);

        ObjectiveUpdateResult result =
            runtime.AddProgress(2);

        Assert.That(
            result,
            Is.EqualTo(
                ObjectiveUpdateResult.Progressed));

        Assert.That(
            runtime.CurrentValue,
            Is.EqualTo(2));

        Assert.That(
            runtime.IsCompleted,
            Is.False);
    }

    [Test]
    public void AddProgress_ReachesTarget_ReturnsCompleted()
    {
        var runtime =
            CreateRuntime(
                targetValue: 3);

        ObjectiveUpdateResult result =
            runtime.AddProgress(3);

        Assert.That(
            result,
            Is.EqualTo(
                ObjectiveUpdateResult.Completed));

        Assert.That(
            runtime.CurrentValue,
            Is.EqualTo(3));

        Assert.That(
            runtime.IsCompleted,
            Is.True);
    }

    [Test]
    public void AddProgress_ExceedsTarget_ClampsAndCompletes()
    {
        var runtime =
            CreateRuntime(
                targetValue: 3);

        ObjectiveUpdateResult result =
            runtime.AddProgress(10);

        Assert.That(
            result,
            Is.EqualTo(
                ObjectiveUpdateResult.Completed));

        Assert.That(
            runtime.CurrentValue,
            Is.EqualTo(3));
    }

    [Test]
    public void AddProgress_WhenCompleted_DoesNothing()
    {
        var runtime =
            CreateRuntime(
                targetValue: 1);

        runtime.AddProgress(1);

        ObjectiveUpdateResult result =
            runtime.AddProgress(1);

        Assert.That(
            result,
            Is.EqualTo(
                ObjectiveUpdateResult.None));

        Assert.That(
            runtime.CurrentValue,
            Is.EqualTo(1));
    }

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-10)]
    public void AddProgress_NonPositiveAmount_DoesNothing(
        int amount)
    {
        var runtime =
            CreateRuntime(
                targetValue: 5);

        ObjectiveUpdateResult result =
            runtime.AddProgress(amount);

        Assert.That(
            result,
            Is.EqualTo(
                ObjectiveUpdateResult.None));

        Assert.That(
            runtime.CurrentValue,
            Is.Zero);
    }

    [Test]
    public void Definition_EmptyId_Throws()
    {
        Assert.Throws<ArgumentException>(
            () => new ObjectiveDefinition(
                id: "",
                displayName: "Test",
                description: "",
                targetValue: 1));
    }

    [Test]
    public void Definition_InvalidTarget_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new ObjectiveDefinition(
                id: "test",
                displayName: "Test",
                description: "",
                targetValue: 0));
    }
    private static ObjectiveRuntime CreateRuntime(
        int targetValue)
    {
        return new ObjectiveRuntime(
            new ObjectiveDefinition(
                id: "test.objective",
                displayName: "Test Objective",
                description: "Test description.",
                targetValue: targetValue));
    }
}