using System;
using System.Threading.Tasks;

using DeviGames.Atlas.Core.Content.Collections;
using DeviGames.Atlas.Core.Content.Interfaces;
using DeviGames.Atlas.Core.Content.Models;
using DeviGames.Atlas.Core.Content.Serialization;

namespace DeviGames.Atlas.Core.Content.Loading
{
    public sealed class ContentPackageLoader
    {
        private readonly IContentSource
            _contentSource;

        private readonly ContentJsonParser
            _parser;

        private readonly ContentPackageConsumerCollection
            _consumers;

        public ContentPackageLoader(
            IContentSource contentSource,
            ContentJsonParser parser,
            ContentPackageConsumerCollection consumers)
        {
            _contentSource =
                contentSource
                ?? throw new ArgumentNullException(
                    nameof(contentSource));

            _parser =
                parser
                ?? throw new ArgumentNullException(
                    nameof(parser));

            _consumers =
                consumers
                ?? throw new ArgumentNullException(
                    nameof(consumers));
        }

        public async Task<ContentPackageData> LoadAsync(
            string contentId)
        {
            if (string.IsNullOrWhiteSpace(
                    contentId))
            {
                throw new ArgumentException(
                    "Content ID cannot be empty.",
                    nameof(contentId));
            }

            string json =
                await _contentSource.LoadAsync(
                    contentId);

            ContentPackageData package =
                _parser.Parse(
                    json);

            ValidatePackageIdentity(
                contentId,
                package);

            InstallPackage(
                package);

            return package;
        }

        private static void ValidatePackageIdentity(
            string requestedContentId,
            ContentPackageData package)
        {
            if (!string.Equals(
                    requestedContentId,
                    package.PackageId,
                    StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    $"Requested content package " +
                    $"'{requestedContentId}', but loaded " +
                    $"package identifies itself as " +
                    $"'{package.PackageId}'.");
            }
        }

        private void InstallPackage(
            ContentPackageData package)
        {
            var consumers =
                _consumers.Consumers;

            for (int index = 0;
                 index < consumers.Count;
                 index++)
            {
                consumers[index].Install(
                    package);
            }
        }
    }
}