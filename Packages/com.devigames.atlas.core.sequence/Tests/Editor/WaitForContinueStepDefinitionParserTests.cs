using System;

using DeviGames.Atlas.Core.Sequence.Models;
using DeviGames.Atlas.Core.Sequence.Parsing;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace DeviGames.Atlas.Core.Sequence.Tests
{
    public sealed class WaitForContinueStepDefinitionParserTests
    {
        private WaitForContinueStepDefinitionParser _parser;

        [SetUp]
        public void SetUp()
        {
            _parser =
                new WaitForContinueStepDefinitionParser();
        }

        [Test]
        public void Type_ReturnsWaitForContinueType()
        {
            Assert.AreEqual(
                WaitForContinueStepDefinition.StepType,
                _parser.Type);
        }

        [Test]
        public void Parse_WithValidJson_CreatesDefinition()
        {
            JObject json = JObject.Parse(@"
            {
                ""Type"": ""wait-for-continue""
            }");

            SequenceStepDefinition result =
                _parser.Parse(
                    json);

            Assert.IsInstanceOf<WaitForContinueStepDefinition>(
                result);
        }

        [Test]
        public void Parse_WithNullJson_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => _parser.Parse(
                    null));
        }
    }
}