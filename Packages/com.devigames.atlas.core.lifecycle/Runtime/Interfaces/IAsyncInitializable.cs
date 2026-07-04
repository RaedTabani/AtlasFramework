using System.Threading.Tasks;

namespace DeviGames.Atlas.Core.Lifecycle.Interfaces
{
    public interface IAsyncInitializable
    {
        Task InitializeAsync();
    }
}