using System;

using DeviGames.Atlas.Core.Sequence.Collections;
using DeviGames.Atlas.Core.Sequence.Models;

using NUnit.Framework;

namespace DeviGames.Atlas.Core.Sequence.Tests
{
    public sealed class SequenceDefinitionCollectionTests
    {
        private SequenceDefinitionCollection _collection;

        [SetUp]
        public void SetUp()
        {
            _collection =
                new SequenceDefinitionCollection();
        }

        [Test]
        public void NewCollection_IsEmpty()
        {
            Assert.AreEqual(
                0,
                _collection.Definitions.Count);
        }

        [Test]
        public void Add_WithValidDefinition_RegistersDefinition()
        {
            var definition =
                new SequenceDefinition(
                    "sequence.test");

            _collection.Add(
                definition);

            Assert.AreEqual(
                1,
                _collection.Definitions.Count);

            Assert.AreSame(
                definition,
                _collection.Get(
                    "sequence.test"));
        }

        [Test]
        public void Add_WithNullDefinition_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => _collection.Add(
                    null));
        }

        [Test]
        public void Add_WithEmptyId_Throws()
        {
            var definition =
                new SequenceDefinition(
                    string.Empty);

            Assert.Throws<ArgumentException>(
                () => _collection.Add(
                    definition));
        }

        [Test]
        public void Add_WithDuplicateId_Throws()
        {
            _collection.Add(
                new SequenceDefinition(
                    "sequence.test"));

            Assert.Throws<InvalidOperationException>(
                () => _collection.Add(
                    new SequenceDefinition(
                        "sequence.test")));
        }

        [Test]
        public void TryGet_WithKnownId_ReturnsDefinition()
        {
            var definition =
                new SequenceDefinition(
                    "sequence.test");

            _collection.Add(
                definition);

            bool result =
                _collection.TryGet(
                    "sequence.test",
                    out SequenceDefinition resolved);

            Assert.IsTrue(
                result);

            Assert.AreSame(
                definition,
                resolved);
        }

        [Test]
        public void TryGet_WithUnknownId_ReturnsFalse()
        {
            bool result =
                _collection.TryGet(
                    "unknown",
                    out SequenceDefinition resolved);

            Assert.IsFalse(
                result);

            Assert.IsNull(
                resolved);
        }

        [Test]
        public void Get_WithUnknownId_Throws()
        {
            Assert.Throws<InvalidOperationException>(
                () => _collection.Get(
                    "unknown"));
        }
    }
}