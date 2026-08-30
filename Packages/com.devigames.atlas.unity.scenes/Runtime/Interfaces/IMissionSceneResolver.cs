namespace DeviGames.Atlas.Unity.Scenes.Interfaces
{
    public interface IMissionSceneResolver
    {
        bool TryGetSceneName(
            string missionId,
            out string sceneName);
    }
}