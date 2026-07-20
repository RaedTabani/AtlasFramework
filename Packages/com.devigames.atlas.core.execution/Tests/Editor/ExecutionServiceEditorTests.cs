using NUnit.Framework;
using System;
using System.Collections.Generic;
using DeviGames.Atlas.Core.Execution.Systems;
using DeviGames.Atlas.Core.Execution.Interfaces;
using DeviGames.Atlas.Core.Execution.Models;
using DeviGames.Atlas.Core.Execution.Services;

namespace DeviGames.Atlas.Core.Execution.Tests
{
    public sealed class ExecutionServiceEditorTests
    {
        [Test]
        public void Add_WhenObjectHasNoExecutionContract_Throws()
        {
            var systems =
                new SystemCollection();

            Assert.Throws<ArgumentException>(
                () => systems.Add(new object()));
        }
        [Test]
        public void Add_WhenSystemIsNull_Throws()
        {
            var systems =
                new SystemCollection();

            Assert.Throws<ArgumentNullException>(
                () => systems.Add(null));
        }
        [Test]
        public void Add_WhenSameInstanceAddedTwice_AddsOnce()
        {
            var systems =
                new SystemCollection();

            var system =
                new TestUpdateSystem();

            systems.Add(system);
            systems.Add(system);

            Assert.AreEqual(
                1,
                systems.Count);
        }

        [Test]
        public void Add_WhenDifferentInstancesAdded_AddsBoth()
        {
            var systems =
                new SystemCollection();

            systems.Add(
                new TestUpdateSystem());

            systems.Add(
                new TestUpdateSystem());

            Assert.AreEqual(
                2,
                systems.Count);
        }

        [Test]
        public void Update_InvokesRegisteredUpdatable()
        {
            var systems =
                new SystemCollection();

            var system =
                new TestUpdateSystem();

            systems.Add(system);

            var execution =
                new ExecutionService(
                    systems);

            var context =
                new ExecutionContext(
                    0.016f,
                    10);

            execution.Update(context);

            Assert.AreEqual(
                1,
                system.UpdateCount);

            Assert.AreEqual(
                context.DeltaTime,
                system.LastContext.DeltaTime);

            Assert.AreEqual(
                context.Frame,
                system.LastContext.Frame);
        }
        [Test]
        public void LateUpdate_DoesNotInvokeUpdateOnlySystem()
        {
            var systems =
                new SystemCollection();

            var system =
                new TestUpdateSystem();

            systems.Add(system);

            var execution =
                new ExecutionService(
                    systems);

            execution.LateUpdate(
                new ExecutionContext(
                    0.016f,
                    10));

            Assert.AreEqual(
                0,
                system.UpdateCount);
        }
        [Test]
        public void Execution_InvokesEveryImplementedPhase()
        {
            var systems =
                new SystemCollection();

            var system =
                new TestMultiPhaseSystem();

            systems.Add(system);

            var execution =
                new ExecutionService(
                    systems);

            var context =
                new ExecutionContext(
                    0.016f,
                    10);

            execution.Update(context);
            execution.FixedUpdate(context);
            execution.LateUpdate(context);

            Assert.AreEqual(
                1,
                system.UpdateCount);

            Assert.AreEqual(
                1,
                system.FixedUpdateCount);

            Assert.AreEqual(
                1,
                system.LateUpdateCount);
        }
        [Test]
        public void Update_PreservesCollectionOrder()
        {
            var calls =
                new List<string>();

            var systems =
                new SystemCollection();

            systems.Add(
                new OrderedUpdateSystem(
                    "A",
                    calls));

            systems.Add(
                new OrderedUpdateSystem(
                    "B",
                    calls));

            var execution =
                new ExecutionService(
                    systems);

            execution.Update(
                new ExecutionContext(
                    0.016f,
                    1));

            CollectionAssert.AreEqual(
                new[] { "A", "B" },
                calls);
        }
    }


}
