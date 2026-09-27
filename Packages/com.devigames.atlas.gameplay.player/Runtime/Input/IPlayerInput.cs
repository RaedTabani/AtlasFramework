using UnityEngine;

namespace DeviGames.Atlas.Gameplay.Player.Input
{
    public interface IPlayerInput
    {
        Vector2 Move { get; }

        Vector2 Look { get; }

        bool RunHeld { get; }

        bool InteractPressed { get; }
    }
}