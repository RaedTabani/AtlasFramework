using System;
using NUnit.Framework;
using DeviGames.Atlas.Core.Missions.Factories;
using DeviGames.Atlas.Core.Missions.Models;
using DeviGames.Atlas.Core.Missions.Runtime;

public sealed class MissionFactoryTests
{
    [Test]
    public void Create_ValidDefinition_CreatesRuntime()
    {
        var factory =
            new MissionFactory();

        var definition =
            new MissionDefinition(
                id: "mission.test",
                displayName: "Test Mission",
                description: "",
                objectiveIds:
                    new[]
                    {
                        "objective.a"
                    });

        MissionRuntime runtime =
            factory.Create(
                definition);

        Assert.That(runtime, Is.Not.Null);

        Assert.That(
            runtime.Definition,
            Is.SameAs(definition));
    }

    [Test]
    public void Create_NullDefinition_Throws()
    {
        var factory =
            new MissionFactory();

        Assert.Throws<ArgumentNullException>(
            () => factory.Create(null));
    }
}