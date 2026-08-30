using System.Threading.Tasks;

namespace DeviGames.Atlas.Unity.Scenes.Interfaces
{
    public interface ISceneService
    {
        string ActiveSceneName { get; }

        Task LoadAsync(string sceneName);
    }
}