namespace DeviGames.Atlas.Unity.Scenes.Models
{
    public enum MissionLaunchResult
    {
        Success = 0,

        MissionNotFound = 1,
        MissingSceneKey = 2,
        MissingContentKey = 3,

        ContentDownloadFailed = 4,
        MissionStartRejected = 5,
        SceneLoadFailed = 6
    }
}