using UnityEngine;
using DeviGames.Atlas.Core.Events;
public class EventBusDemo : MonoBehaviour
{
    private void OnEnable()
    {
        EventBus.Subscribe<PlayerDiedEvent>(OnPlayerDied);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<PlayerDiedEvent>(OnPlayerDied);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventBus.Publish(new PlayerDiedEvent(42));
        }
    }

    private void OnPlayerDied(PlayerDiedEvent e)
    {
        Debug.Log("Player died: " + e.PlayerId);
    }
}

public readonly struct PlayerDiedEvent
{
    public readonly int PlayerId;

    public PlayerDiedEvent(int playerId)
    {
        PlayerId = playerId;
    }
}