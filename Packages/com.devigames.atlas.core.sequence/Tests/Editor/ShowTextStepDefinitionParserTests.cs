using System;

using DeviGames.Atlas.Core.Sequence.Models;
using DeviGames.Atlas.Core.Sequence.Parsing;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace DeviGames.Atlas.Core.Sequence.Tests
{
    public sealed class ShowTextStepDefinitionParserTests
    {
        private ShowTextStepDefinitionParser _parser;

        [SetUp]
        public void SetUp()
        {
            _parser =
                new ShowTextStepDefinitionParser();
        }

        [Test]
        public void Type_ReturnsShowTextType()
        {
            Assert.AreEqual(
                ShowTextStepDefinition.StepType,
                _parser.Type);
        }

        [Test]
        public void Parse_WithValidJson_CreatesDefinition()
        {
            JObject json = JObject.Parse(@"
            {
                ""Type"": ""show-text"",
                ""Text"": ""Escape the house.""
            }");

            SequenceStepDefinition result =
                _parser.Parse(
                    json);

            Assert.IsInstanceOf<ShowTextStepDefinition>(
                result);

            var definition =
                (ShowTextStepDefinition)result;

            Assert.AreEqual(
                "Escape the house.",
                definition.Text);
        }

        [Test]
        public void Parse_WithNullJson_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => _parser.Parse(
                    null));
        }

        [Test]
        public void Parse_WithMissingText_Throws()
        {
            JObject json = JObject.Parse(@"
            {
                ""Type"": ""show-text""
            }");

            Assert.Throws<InvalidOperationException>(
                () => _parser.Parse(
                    json));
        }

        [Test]
        public void Parse_WithEmptyText_Throws()
        {
            JObject json = JObject.Parse(@"
            {
                ""Type"": ""show-text"",
                ""Text"": """"
            }");

            Assert.Throws<InvalidOperationException>(
                () => _parser.Parse(
                    json));
        }
    }
}