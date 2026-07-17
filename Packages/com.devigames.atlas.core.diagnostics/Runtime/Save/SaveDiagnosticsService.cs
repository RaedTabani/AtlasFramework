using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System;

namespace DeviGames.Atlas.Core.Diagnostics.Save
{
    public sealed class SaveDiagnosticsService
        : ISaveDiagnosticsService
    {
        private readonly string _saveDirectory;

        private readonly List<SaveFileInfo> _saveFiles =
            new();

        public SaveDiagnosticsService(
            string saveDirectory)
        {
            _saveDirectory = saveDirectory;

            Refresh();
        }

        public IReadOnlyList<SaveFileInfo> GetSaveFiles()
        {
            return _saveFiles;
        }

        public void Refresh()
        {
            _saveFiles.Clear();

            if (!Directory.Exists(_saveDirectory))
            {
                return;
            }

            foreach (string path in Directory.GetFiles(
                        _saveDirectory,
                        "*.json"))
            {
                FileInfo fileInfo = new(path);

                _saveFiles.Add(
                    new SaveFileInfo(
                        key: Path.GetFileNameWithoutExtension(path),
                        fileName: fileInfo.Name,
                        fullPath: fileInfo.FullName,
                        sizeBytes: fileInfo.Length,
                        lastModifiedUtc: fileInfo.LastWriteTimeUtc));
            }

            _saveFiles.Sort(
                (left, right) =>
                    string.Compare(
                        left.FileName,
                        right.FileName,
                        StringComparison.OrdinalIgnoreCase));
        }

        public async Task<string> ReadRawAsync(
            string key)
        {
            string path = Path.Combine(
                _saveDirectory,
                $"{key}.json");

            if (!File.Exists(path))
            {
                return string.Empty;
            }

            return await File.ReadAllTextAsync(path);
        }
    }
}