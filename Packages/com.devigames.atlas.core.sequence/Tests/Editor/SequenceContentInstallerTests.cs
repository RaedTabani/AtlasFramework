using System;

using DeviGames.Atlas.Core.Content.Models;
using DeviGames.Atlas.Core.Sequence.Collections;
using DeviGames.Atlas.Core.Sequence.Content;
using DeviGames.Atlas.Core.Sequence.Models;
using DeviGames.Atlas.Core.Sequence.Parsing;

using NUnit.Framework;

namespace DeviGames.Atlas.Core.Sequence.Tests
{
    public sealed class SequenceContentInstallerTests
    {
        private SequenceDefinitionCollection _collection;
        private SequenceContentInstaller _installer;

        [SetUp]
        public void SetUp()
        {
            _collection =
                new SequenceDefinitionCollection();

            var registry =
                new SequenceStepContentConverterRegistry();

            registry.Register(
                new ShowTextStepContentConverter());

            registry.Register(
                new WaitForContinueStepContentConverter());

            _installer =
                new SequenceContentInstaller(
                    _collection,
                    registry);
        }

        [Test]
        public void Install_WithSequence_RegistersDefinition()
        {
            ContentPackageData package =
                CreatePackage(
                    new SequenceContentData
                    {
                        Id = "sequence.test"
                    });

            _installer.Install(
                package);

            SequenceDefinition definition =
                _collection.Get(
                    "sequence.test");

            Assert.IsNotNull(
                definition);

            Assert.AreEqual(
                "sequence.test",
                definition.Id);
        }

        [Test]
        public void Install_WithSteps_CreatesCorrectStepDefinitions()
        {
            ContentPackageData package =
                CreatePackage(
                    new SequenceContentData
                    {
                        Id = "sequence.test",
                        Steps = new[]
                        {
                            new SequenceStepContentData
                            {
                                Type = "show-text",
                                Text = "Hello Atlas."
                            },
                            new SequenceStepContentData
                            {
                                Type = "wait-for-continue"
                            }
                        }
                    });

            _installer.Install(
                package);

            SequenceDefinition definition =
                _collection.Get(
                    "sequence.test");

            Assert.AreEqual(
                2,
                definition.Steps.Count);

            Assert.IsInstanceOf<ShowTextStepDefinition>(
                definition.Steps[0]);

            Assert.IsInstanceOf<WaitForContinueStepDefinition>(
                definition.Steps[1]);

            var showText =
                (ShowTextStepDefinition)
                definition.Steps[0];

            Assert.AreEqual(
                "Hello Atlas.",
                showText.Text);
        }

        [Test]
        public void Install_WithMultipleSequences_RegistersAll()
        {
            ContentPackageData package =
                CreatePackage(
                    new SequenceContentData
                    {
                        Id = "sequence.one"
                    },
                    new SequenceContentData
                    {
                        Id = "sequence.two"
                    });

            _installer.Install(
                package);

            Assert.AreEqual(
                2,
                _collection.Definitions.Count);

            Assert.IsTrue(
                _collection.TryGet(
                    "sequence.one",
                    out _));

            Assert.IsTrue(
                _collection.TryGet(
                    "sequence.two",
                    out _));
        }

        [Test]
        public void Install_WithNullPackage_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => _installer.Install(
                    null));
        }

        [Test]
        public void Install_WithEmptySequenceId_Throws()
        {
            ContentPackageData package =
                CreatePackage(
                    new SequenceContentData
                    {
                        Id = string.Empty
                    });

            Assert.Throws<InvalidOperationException>(
                () => _installer.Install(
                    package));
        }

        [Test]
        public void Install_WithStepWithoutType_Throws()
        {
            ContentPackageData package =
                CreatePackage(
                    new SequenceContentData
                    {
                        Id = "sequence.test",
                        Steps = new[]
                        {
                            new SequenceStepContentData()
                        }
                    });

            Assert.Throws<InvalidOperationException>(
                () => _installer.Install(
                    package));
        }

        [Test]
        public void Install_WithUnknownStepType_Throws()
        {
            ContentPackageData package =
                CreatePackage(
                    new SequenceContentData
                    {
                        Id = "sequence.test",
                        Steps = new[]
                        {
                            new SequenceStepContentData
                            {
                                Type = "unknown"
                            }
                        }
                    });

            Assert.Throws<InvalidOperationException>(
                () => _installer.Install(
                    package));
        }

        [Test]
        public void Install_WithShowTextWithoutText_Throws()
        {
            ContentPackageData package =
                CreatePackage(
                    new SequenceContentData
                    {
                        Id = "sequence.test",
                        Steps = new[]
                        {
                            new SequenceStepContentData
                            {
                                Type = "show-text"
                            }
                        }
                    });

            Assert.Throws<InvalidOperationException>(
                () => _installer.Install(
                    package));
        }

        [Test]
        public void Install_WithDuplicateSequenceId_Throws()
        {
            ContentPackageData package =
                CreatePackage(
                    new SequenceContentData
                    {
                        Id = "sequence.test"
                    },
                    new SequenceContentData
                    {
                        Id = "sequence.test"
                    });

            Assert.Throws<InvalidOperationException>(
                () => _installer.Install(
                    package));
        }

        private static ContentPackageData CreatePackage(
            params SequenceContentData[] sequences)
        {
            return new ContentPackageData
            {
                PackageId = "content.test",
                Sequences = sequences
            };
        }
    }
}