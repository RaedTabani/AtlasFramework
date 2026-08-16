using NUnit.Framework;

using DeviGames.Atlas.Core.Content.Models;
using DeviGames.Atlas.Core.Content.Serialization;

namespace DeviGames.Atlas.Core.Content.Tests
{
    public sealed class ContentJsonParserTests
    {
        [Test]
        public void Parse_ValidJson_ReturnsContentPackage()
        {
            const string json =
                @"{
                    ""Version"": 1,
                    ""PackageId"": ""playground.chapter-01"",
                    ""Objectives"": [
                        {
                            ""Id"": ""objective.collect-keys"",
                            ""DisplayName"": ""Collect Keys"",
                            ""Description"": ""Collect three keys."",
                            ""TargetValue"": 3
                        }
                    ],
                    ""Missions"": [
                        {
                            ""Id"": ""mission.escape"",
                            ""DisplayName"": ""Escape"",
                            ""Description"": ""Escape the area."",
                            ""ObjectiveIds"": [
                                ""objective.collect-keys""
                            ]
                        }
                    ]
                }";

            var parser =
                new ContentJsonParser();

            ContentPackageData data =
                parser.Parse(
                    json);

            Assert.That(
                data.PackageId,
                Is.EqualTo(
                    "playground.chapter-01"));

            Assert.That(
                data.Version,
                Is.EqualTo(1));

            Assert.That(
                data.Objectives.Length,
                Is.EqualTo(1));

            Assert.That(
                data.Objectives[0].Id,
                Is.EqualTo(
                    "objective.collect-keys"));

            Assert.That(
                data.Objectives[0].TargetValue,
                Is.EqualTo(3));

            Assert.That(
                data.Missions.Length,
                Is.EqualTo(1));

            Assert.That(
                data.Missions[0].ObjectiveIds,
                Does.Contain(
                    "objective.collect-keys"));
        }

        [Test]
        public void Parse_EmptyJson_Throws()
        {
            var parser =
                new ContentJsonParser();

            Assert.That(
                () => parser.Parse(""),
                Throws.ArgumentException);
        }
        [Test]
        public void JsonUtility_ContentPackage_RoundTrips()
        {
            var original =
                new ContentPackageData
                {
                    PackageId =
                        "package.test",

                    Objectives =
                        new[]
                        {
                            new ObjectiveContentData
                            {
                                Id =
                                    "objective.test",

                                DisplayName =
                                    "Test",

                                Description =
                                    "Test objective",

                                TargetValue =
                                    3
                            }
                        },

                    Missions =
                        new[]
                        {
                            new MissionContentData
                            {
                                Id =
                                    "mission.test",

                                DisplayName =
                                    "Mission",

                                Description =
                                    "Test mission",

                                ObjectiveIds =
                                    new[]
                                    {
                                        "objective.test"
                                    }
                            }
                        }
                };

            string json =
                UnityEngine.JsonUtility.ToJson(
                    original);

            ContentPackageData restored =
                UnityEngine.JsonUtility.FromJson<
                    ContentPackageData>(
                        json);

            Assert.That(
                restored.PackageId,
                Is.EqualTo(
                    "package.test"));

            Assert.That(
                restored.Objectives.Length,
                Is.EqualTo(1));

            Assert.That(
                restored.Missions[0].ObjectiveIds[0],
                Is.EqualTo(
                    "objective.test"));
        }
    }
}