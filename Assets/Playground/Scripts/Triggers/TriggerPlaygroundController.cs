using UnityEngine;
using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Triggers.Events;
using DeviGames.Atlas.Gameplay.Inventory.Interfaces;

namespace DeviGames.Playground.Trigger
{
    public class TriggerPlaygroundController : MonoBehaviour
    {

        private IInventoryService _inventory;
        private bool _isInitialized;

        public void Initialize(IInventoryService inventory)
        {
            _inventory = inventory;
            _isInitialized = _inventory != null;
        }

        void OnEnable()
        {
            EventBus.Subscribe<TriggerActivatedEvent>(OnTriggerActivated);
        }
        void OnDisable()
        {
            EventBus.Unsubscribe<TriggerActivatedEvent>(OnTriggerActivated);
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Debug.Log("Press A or B to Add or Subtract key to Inventory");
        }

        // Update is called once per frame
        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.A))
            {
                _inventory.Add("key",1);
            }

            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.B))
            {
                _inventory.Remove("key",1);
            }
        }

        private static void OnTriggerActivated(TriggerActivatedEvent eventData)
        {
            if (eventData.TriggerId !="playground.inventory.collect-three-keys")
            {
                return;
            }

            UnityEngine.Debug.Log("Collected three keys. Trigger activated.");
        }
    }
}
