using UnityEngine;

using DeviGames.Atlas.Core.Unlocks.Interfaces;

namespace DeviGames.Playground.Debugger
{
    public class PlaygroundDebugger : MonoBehaviour
    {
        private IUnlockService _unlockService; 
        
        public void Initialize(IUnlockService _unlockService)
        {
            this._unlockService = _unlockService;
        }
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.U))
            {
                Debug.Log(_unlockService.IsUnlocked("mission.playground.chapter-02"));
            }
        }
    }
}