using System;
using NUnit.Framework;
using DeviGames.Atlas.Core.Missions.Collections;
using DeviGames.Atlas.Core.Missions.Models;
using DeviGames.Atlas.Core.Missions.Runtime;

public sealed class MissionCollectionTests
{
    private MissionCollection _collection;

    [SetUp]
    public void SetUp()
    {
        _collection =
            new MissionCollection();
    }

    [Test]
    public void Add_ValidMission_AddsMission()
    {
        MissionRuntime mission =
            CreateMission(
                "mission.a");

        _collection.Add(
            mission);

        Assert.That(
            _collection.Count,
            Is.EqualTo(1));

        Assert.That(
            _collection.Contains(
                mission),
            Is.True);
    }

    [Test]
    public void Add_DuplicateId_Throws()
    {
        _collection.Add(
            CreateMission(
                "mission.a"));

        Assert.Throws<InvalidOperationException>(
            () => _collection.Add(
                CreateMission(
                    "mission.a")));
    }

    [Test]
    public void TryGet_ExistingId_ReturnsSameMission()
    {
        MissionRuntime mission =
            CreateMission(
                "mission.a");

        _collection.Add(
            mission);

        bool found =
            _collection.TryGet(
                "mission.a",
                out MissionRuntime result);

        Assert.That(found, Is.True);
        Assert.That(result, Is.SameAs(mission));
    }

    [Test]
    public void Remove_ExistingMission_RemovesMission()
    {
        MissionRuntime mission =
            CreateMission(
                "mission.a");

        _collection.Add(
            mission);

        bool removed =
            _collection.Remove(
                mission);

        Assert.That(removed, Is.True);
        Assert.That(_collection.Count, Is.Zero);
    }

    [Test]
    public void Remove_DifferentInstanceWithSameId_DoesNotRemove()
    {
        MissionRuntime owned =
            CreateMission(
                "mission.a");

        MissionRuntime other =
            CreateMission(
                "mission.a");

        _collection.Add(
            owned);

        bool removed =
            _collection.Remove(
                other);

        Assert.That(removed, Is.False);

        Assert.That(
            _collection.Contains(
                owned),
            Is.True);
    }

    [Test]
    public void Missions_PreserveInsertionOrder()
    {
        MissionRuntime a =
            CreateMission(
                "mission.a");

        MissionRuntime b =
            CreateMission(
                "mission.b");

        MissionRuntime c =
            CreateMission(
                "mission.c");

        _collection.Add(a);
        _collection.Add(b);
        _collection.Add(c);

        Assert.That(
            _collection.Missions[0],
            Is.SameAs(a));

        Assert.That(
            _collection.Missions[1],
            Is.SameAs(b));

        Assert.That(
            _collection.Missions[2],
            Is.SameAs(c));
    }

    [Test]
    public void Clear_RemovesEverything()
    {
        _collection.Add(
            CreateMission(
                "mission.a"));

        _collection.Add(
            CreateMission(
                "mission.b"));

        _collection.Clear();

        Assert.That(
            _collection.Count,
            Is.Zero);
    }

    private static MissionRuntime CreateMission(
        string id)
    {
        return new MissionRuntime(
            new MissionDefinition(
                id: id,
                displayName: id,
                description: string.Empty,
                objectiveIds:
                    new[]
                    {
                        $"{id}.objective"
                    }));
    }
}