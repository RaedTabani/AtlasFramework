namespace DeviGames.Atlas.Gameplay.Events
{
    public readonly struct AreaEnteredEvent
    {
        public string AreaId { get; }

        public AreaEnteredEvent(string areaId)
        {
            AreaId = areaId;
        }
    }
}