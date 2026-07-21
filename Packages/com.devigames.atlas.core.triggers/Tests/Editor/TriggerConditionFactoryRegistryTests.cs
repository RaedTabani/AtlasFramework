using NUnit.Framework;
using DeviGames.Atlas.Core.Triggers.Factories;
using System;
using System.Collections.Generic;

namespace DeviGames.Atlas.Core.Triggers.Test
{
    public sealed class TriggerConditionFactoryRegistryTests
    {
        private TriggerConditionFactoryRegistry _registry;

        [SetUp]
        public void SetUp()
        {
            _registry =
                new TriggerConditionFactoryRegistry();
        }

        [Test]
        public void Register_AddsFactory()
        {
            var factory =
                new BooleanConditionFactory();

            _registry.Register(factory);

            Assert.That(
                _registry.Count,
                Is.EqualTo(1));

            Assert.That(
                _registry.Contains(factory.Type),
                Is.True);
        }

        [Test]
        public void Register_DuplicateType_Throws()
        {
            _registry.Register(
                new BooleanConditionFactory());

            Assert.Throws<InvalidOperationException>(
                () => _registry.Register(
                    new BooleanConditionFactory()));
        }

        [Test]
        public void Get_UnknownType_Throws()
        {
            Assert.Throws<KeyNotFoundException>(
                () => _registry.Get(
                    "unknown"));
        }

        [Test]
        public void Unregister_RemovesFactory()
        {
            var factory =
                new BooleanConditionFactory();

            _registry.Register(factory);

            bool removed =
                _registry.Unregister(
                    factory.Type);

            Assert.That(removed, Is.True);
            Assert.That(_registry.Count, Is.Zero);
        }

        [Test]
        public void Clear_RemovesAllFactories()
        {
            _registry.Register(
                new BooleanConditionFactory());

            _registry.Clear();

            Assert.That(_registry.Count, Is.Zero);
        }
    }
}