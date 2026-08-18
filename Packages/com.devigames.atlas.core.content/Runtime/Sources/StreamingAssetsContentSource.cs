using System;
using System.IO;
using System.Threading.Tasks;

using DeviGames.Atlas.Core.Content.Interfaces;

using UnityEngine;

namespace DeviGames.Atlas.Core.Content.Sources
{
    public sealed class StreamingAssetsContentSource :
        IContentSource
    {
        private readonly string _rootPath;

        public StreamingAssetsContentSource(
            string rootPath)
        {
            if (string.IsNullOrWhiteSpace(
                    rootPath))
            {
                throw new ArgumentException(
                    "Root path cannot be empty.",
                    nameof(rootPath));
            }

            _rootPath =
                rootPath;
        }

        public async Task<string> LoadAsync(
            string contentId)
        {
            if (string.IsNullOrWhiteSpace(
                    contentId))
            {
                throw new ArgumentException(
                    "Content ID cannot be empty.",
                    nameof(contentId));
            }

            string path =
                Path.Combine(
                    _rootPath,
                    contentId + ".json");

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(
                    $"Content package '{contentId}' " +
                    $"was not found.",
                    path);
            }

            return await File.ReadAllTextAsync(
                path);
        }
    }
}