using System;

namespace DeviGames.Atlas.Core.Diagnostics.Save
{
    public sealed class SaveFileInfo
    {
        public string Key { get; }

        public string FileName { get; }

        public string FullPath { get; }

        public long SizeBytes { get; }

        public DateTime LastModifiedUtc { get; }

        public SaveFileInfo(
            string key,
            string fileName,
            string fullPath,
            long sizeBytes,
            DateTime lastModifiedUtc)
        {
            Key = key;
            FileName = fileName;
            FullPath = fullPath;
            SizeBytes = sizeBytes;
            LastModifiedUtc = lastModifiedUtc;
        }
    }
}