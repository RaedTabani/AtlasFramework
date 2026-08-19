using DeviGames.Atlas.Gameplay.WorldState.Models;
namespace DeviGames.Atlas.Gameplay.WorldState.Interfaces
{
    public interface IWorldStateService
    {
        bool Get(
            string key);

        bool Set(
            string key,
            bool value);

        bool Contains(
            string key);

        WorldStateData CreateSnapshot();

        void Load(
            WorldStateData data);
    }
}