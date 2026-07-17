using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviGames.Atlas.Core.Diagnostics.Save
{
    public interface ISaveDiagnosticsService
    {
        IReadOnlyList<SaveFileInfo> GetSaveFiles();

        Task<string> ReadRawAsync(string key);

        void Refresh();
    }
}