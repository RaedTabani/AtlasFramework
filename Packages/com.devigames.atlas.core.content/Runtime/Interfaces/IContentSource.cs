using System.Threading.Tasks;

namespace DeviGames.Atlas.Core.Content.Interfaces
{
    public interface IContentSource
    {
        Task<string> LoadAsync(
            string contentId);
    }
}