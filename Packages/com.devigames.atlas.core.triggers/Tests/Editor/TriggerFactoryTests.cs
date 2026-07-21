using NUnit.Framework;
using DeviGames.Atlas.Core.Triggers.Factories;
using DeviGames.Atlas.Core.Triggers.Models;
using DeviGames.Atlas.Core.Triggers.Interfaces;
using DeviGames.Atlas.Core.Triggers.Runtime;
using DeviGames.Atlas.Core.Services;
using System.Collections.Generic;

namespace DeviGames.Atlas.Core.Triggers.Test
{
    public sealed class TriggerFactoryTests
    {
        private ServiceRegistry _services;
        private TriggerConditionFactoryRegistry _conditionFactories;
        private TriggerFactory _triggerFactory;

        [SetUp]
        public void SetUp()
        {
            _services =
                new ServiceRegistry();

            _conditionFactories =
                new TriggerConditionFactoryRegistry();

            _conditionFactories.Register(
                new BooleanConditionFactory());

            _triggerFactory =
                new TriggerFactory(
                    _conditionFactories,
                    new TriggerBuildContext(
                        _services));
        }

        [Test]
        public void Create_BuildsRuntime()
        {
            var definition =
                new TriggerDefinition(
                    "test-trigger",
                    repeatable: false,
                    new BooleanConditionDefinition(
                        true));

            TriggerRuntime runtime =
                _triggerFactory.Create(
                    definition);

            Assert.That(runtime, Is.Not.Null);

            Assert.That(
                runtime.Definition,
                Is.SameAs(definition));

            Assert.That(
                runtime.Condition,
                Is.TypeOf<BooleanCondition>());

            Assert.That(
                runtime.State,
                Is.EqualTo(
                    TriggerState.Waiting));
        }

        [Test]
        public void Create_UnknownConditionType_Throws()
        {
            var definition =
                new TriggerDefinition(
                    "unknown-trigger",
                    repeatable: false,
                    new UnknownConditionDefinition());

            Assert.Throws<KeyNotFoundException>(
                () => _triggerFactory.Create(
                    definition));
        }
    }

    public sealed class UnknownConditionDefinition :
        ITriggerConditionDefinition
    {
        public string Type =>
            "test.unknown";
    }
}