using System;

using DeviGames.Atlas.Core.Sequence.Models;
using DeviGames.Atlas.Core.Sequence.Parsing;
using DeviGames.Atlas.Core.Sequence.Services;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace DeviGames.Atlas.Core.Sequence.Tests
{
    public sealed class SequenceDefinitionParserTests
    {
        private SequenceDefinitionParser _parser;

        [SetUp]
        public void SetUp()
        {
            var registry =
                new SequenceStepDefinitionParserRegistry();

            registry.Register(
                new ShowTextStepDefinitionParser());

            registry.Register(
                new WaitForContinueStepDefinitionParser());

            _parser =
                new SequenceDefinitionParser(
                    registry);
        }

        [Test]
        public void Parse_WithValidSequence_CreatesDefinition()
        {
            JObject json = JObject.Parse(@"
            {
                ""Id"": ""sequence.mission.escape.intro"",
                ""Steps"": [
                    {
                        ""Type"": ""show-text"",
                        ""Text"": ""You wake up inside the house...""
                    },
                    {
                        ""Type"": ""wait-for-continue""
                    },
                    {
                        ""Type"": ""show-text"",
                        ""Text"": ""Find a way out.""
                    },
                    {
                        ""Type"": ""wait-for-continue""
                    }
                ]
            }");

            SequenceDefinition definition =
                _parser.Parse(
                    json);

            Assert.AreEqual(
                "sequence.mission.escape.intro",
                definition.Id);

            Assert.AreEqual(
                4,
                definition.Steps.Count);

            Assert.IsInstanceOf<ShowTextStepDefinition>(
                definition.Steps[0]);

            Assert.IsInstanceOf<WaitForContinueStepDefinition>(
                definition.Steps[1]);

            Assert.IsInstanceOf<ShowTextStepDefinition>(
                definition.Steps[2]);

            Assert.IsInstanceOf<WaitForContinueStepDefinition>(
                definition.Steps[3]);
        }

        [Test]
        public void Parse_PreservesShowTextContent()
        {
            JObject json = JObject.Parse(@"
            {
                ""Id"": ""sequence.test"",
                ""Steps"": [
                    {
                        ""Type"": ""show-text"",
                        ""Text"": ""Hello from Atlas.""
                    }
                ]
            }");

            SequenceDefinition definition =
                _parser.Parse(
                    json);

            var step =
                (ShowTextStepDefinition)
                definition.Steps[0];

            Assert.AreEqual(
                "Hello from Atlas.",
                step.Text);
        }

        [Test]
        public void Parse_WithNoSteps_CreatesEmptySequence()
        {
            JObject json = JObject.Parse(@"
            {
                ""Id"": ""sequence.empty""
            }");

            SequenceDefinition definition =
                _parser.Parse(
                    json);

            Assert.AreEqual(
                0,
                definition.Steps.Count);
        }

        [Test]
        public void Parse_WithNullJson_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => _parser.Parse(
                    null));
        }

        [Test]
        public void Parse_WithMissingId_Throws()
        {
            JObject json = JObject.Parse(@"
            {
                ""Steps"": []
            }");

            Assert.Throws<InvalidOperationException>(
                () => _parser.Parse(
                    json));
        }

        [Test]
        public void Parse_WithEmptyId_Throws()
        {
            JObject json = JObject.Parse(@"
            {
                ""Id"": """",
                ""Steps"": []
            }");

            Assert.Throws<InvalidOperationException>(
                () => _parser.Parse(
                    json));
        }

        [Test]
        public void Parse_WithNonArraySteps_Throws()
        {
            JObject json = JObject.Parse(@"
            {
                ""Id"": ""sequence.test"",
                ""Steps"": {}
            }");

            Assert.Throws<InvalidOperationException>(
                () => _parser.Parse(
                    json));
        }

        [Test]
        public void Parse_WithStepWithoutType_Throws()
        {
            JObject json = JObject.Parse(@"
            {
                ""Id"": ""sequence.test"",
                ""Steps"": [
                    {
                        ""Text"": ""Hello""
                    }
                ]
            }");

            Assert.Throws<InvalidOperationException>(
                () => _parser.Parse(
                    json));
        }

        [Test]
        public void Parse_WithUnknownStepType_Throws()
        {
            JObject json = JObject.Parse(@"
            {
                ""Id"": ""sequence.test"",
                ""Steps"": [
                    {
                        ""Type"": ""does-not-exist""
                    }
                ]
            }");

            Assert.Throws<InvalidOperationException>(
                () => _parser.Parse(
                    json));
        }

        [Test]
        public void Parse_WithNonObjectStep_Throws()
        {
            JObject json = JObject.Parse(@"
            {
                ""Id"": ""sequence.test"",
                ""Steps"": [
                    ""invalid""
                ]
            }");

            Assert.Throws<InvalidOperationException>(
                () => _parser.Parse(
                    json));
        }
    }
}