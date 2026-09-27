using System;

using DeviGames.Atlas.Gameplay.Player.Input;

using UnityEngine;
using UnityEngine.InputSystem;

namespace DeviGames.Atlas.Unity.Player.Input
{
    public sealed class UnityPlayerInput :
        MonoBehaviour,
        IPlayerInput
    {
        [SerializeField]
        private InputActionReference _moveAction;

        [SerializeField]
        private InputActionReference _lookAction;

        [SerializeField]
        private InputActionReference _runAction;

        [SerializeField]
        private InputActionReference _interactAction;

        public Vector2 Move =>
            _moveAction.action.ReadValue<Vector2>();

        public Vector2 Look =>
            _lookAction.action.ReadValue<Vector2>();

        public bool RunHeld =>
            _runAction.action.IsPressed();

        public bool InteractPressed =>
            _interactAction.action.WasPressedThisFrame();

        private void Awake()
        {
            ValidateActions();
        }

        private void OnEnable()
        {
            _moveAction.action.Enable();
            _lookAction.action.Enable();
            _runAction.action.Enable();
            _interactAction.action.Enable();
        }

        private void OnDisable()
        {
            _moveAction.action.Disable();
            _lookAction.action.Disable();
            _runAction.action.Disable();
            _interactAction.action.Disable();
        }

        private void ValidateActions()
        {
            if (_moveAction == null)
            {
                throw new InvalidOperationException(
                    "Move action is not assigned.");
            }

            if (_lookAction == null)
            {
                throw new InvalidOperationException(
                    "Look action is not assigned.");
            }

            if (_runAction == null)
            {
                throw new InvalidOperationException(
                    "Run action is not assigned.");
            }

            if (_interactAction == null)
            {
                throw new InvalidOperationException(
                    "Interact action is not assigned.");
            }
        }
    }
}