using UnityEngine;
using NUnit.Framework;
using System.Threading.Tasks;
using System.IO;

namespace DeviGames.Atlas.Core.Content.Sources.Tests
{
    public class StreamingAssetsContentSourceTests
    {
        [Test]
        public async Task LoadAsync_ExistingPackage_ReturnsJson()
        {
            string root =
                Path.Combine(
                    Application.streamingAssetsPath,
                    "Atlas",
                    "Content");

            var source =
                new StreamingAssetsContentSource(
                    root);

            string json =
                await source.LoadAsync(
                    "playground.chapter-01");

            Assert.That(
                json,
                Is.Not.Null.And.Not.Empty);

            Assert.That(
                json,
                Does.Contain(
                    "\"PackageId\""));

            Assert.That(
                json,
                Does.Contain(
                    "playground.chapter-01"));
        }
        [Test]
        public void LoadAsync_MissingPackage_Throws()
        {
            string root =
                Path.Combine(
                    Application.streamingAssetsPath,
                    "Atlas",
                    "Content");

            var source =
                new StreamingAssetsContentSource(
                    root);

            Assert.ThrowsAsync<FileNotFoundException>(
                async () =>
                    await source.LoadAsync(
                        "package.does-not-exist"));
        }
    }
}
