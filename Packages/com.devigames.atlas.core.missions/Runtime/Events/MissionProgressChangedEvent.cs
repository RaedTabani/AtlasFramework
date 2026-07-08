public readonly struct MissionProgressChangedEvent
{
    public string MissionId { get; }

    public MissionProgressChangedEvent(string missionId)
    {
        MissionId = missionId;
    }
}