using System;

using DeviGames.Atlas.Core.Sequence.Models;
using DeviGames.Atlas.Core.Sequence.Interfaces;
using DeviGames.Atlas.Core.Sequence.Services;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace DeviGames.Atlas.Core.Sequence.Tests
{
    public sealed class SequenceStepDefinitionParserRegistryTests
    {
        private SequenceStepDefinitionParserRegistry _registry;

        [SetUp]
        public void SetUp()
        {
            _registry =
                new SequenceStepDefinitionParserRegistry();
        }

        [Test]
        public void Register_WithValidParser_AllowsParserToBeResolved()
        {
            var parser =
                new TestParser(
                    "test");

            _registry.Register(
                parser);

            ISequenceStepDefinitionParser resolved =
                _registry.Get(
                    "test");

            Assert.AreSame(
                parser,
                resolved);
        }

        [Test]
        public void Register_WithNullParser_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => _registry.Register(
                    null));
        }

        [Test]
        public void Register_WithEmptyType_Throws()
        {
            var parser =
                new TestParser(
                    string.Empty);

            Assert.Throws<ArgumentException>(
                () => _registry.Register(
                    parser));
        }

        [Test]
        public void Register_DuplicateType_Throws()
        {
            _registry.Register(
                new TestParser(
                    "test"));

            Assert.Throws<InvalidOperationException>(
                () => _registry.Register(
                    new TestParser(
                        "test")));
        }

        [Test]
        public void TryGet_WithRegisteredType_ReturnsTrue()
        {
            var parser =
                new TestParser(
                    "test");

            _registry.Register(
                parser);

            bool result =
                _registry.TryGet(
                    "test",
                    out ISequenceStepDefinitionParser resolved);

            Assert.IsTrue(
                result);

            Assert.AreSame(
                parser,
                resolved);
        }

        [Test]
        public void TryGet_WithUnknownType_ReturnsFalse()
        {
            bool result =
                _registry.TryGet(
                    "unknown",
                    out ISequenceStepDefinitionParser resolved);

            Assert.IsFalse(
                result);

            Assert.IsNull(
                resolved);
        }

        [Test]
        public void Get_WithUnknownType_Throws()
        {
            Assert.Throws<InvalidOperationException>(
                () => _registry.Get(
                    "unknown"));
        }

        private sealed class TestParser :
            ISequenceStepDefinitionParser
        {
            public string Type { get; }

            public TestParser(
                string type)
            {
                Type =
                    type;
            }

            public SequenceStepDefinition Parse(
                JObject json)
            {
                return null;
            }
        }
    }
}