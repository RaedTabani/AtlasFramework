using System.Threading.Tasks;

namespace DeviGames.Atlas.Core.Save.Interfaces
{
    public interface ISaveParticipant
    {
        string Key { get; }

        Task SaveAsync();

        Task LoadAsync();
    }
}