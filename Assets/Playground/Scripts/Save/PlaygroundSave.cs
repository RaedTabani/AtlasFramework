using UnityEngine;

using DeviGames.Atlas.Core.Save.Services;

namespace DeviGames.Playground.Debugger
{
    public class PlaygroundSave: MonoBehaviour
    {
        private SaveGameCoordinator _saveGameCoordinator; 
        
        public void Initialize(SaveGameCoordinator saveGameCoordinator)
        {
            this._saveGameCoordinator = saveGameCoordinator;
        }
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.S))
            {
                _saveGameCoordinator.SaveAsync();
            }
        }
    }
}