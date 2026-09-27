using System;

using DeviGames.Atlas.Gameplay.Player.Configuration;
using DeviGames.Atlas.Gameplay.Player.Input;

using UnityEngine;

namespace DeviGames.Atlas.Gameplay.Player.Look
{
    public sealed class PlayerLook :
        MonoBehaviour
    {
        [SerializeField]
        private Transform _cameraRoot;

        [SerializeField]
        private MonoBehaviour _playerInputSource;

        [SerializeField]
        private PlayerMovementSettings _settings;

        private IPlayerInput _playerInput;

        private float _verticalRotation;

        private void OnEnable()
        {
            Cursor.lockState =
                CursorLockMode.Locked;

            Cursor.visible =
                false;
        }

        private void OnDisable()
        {
            Cursor.lockState =
                CursorLockMode.None;

            Cursor.visible =
                true;
        }
        
        private void Awake()
        {
            if (_cameraRoot == null)
            {
                throw new InvalidOperationException(
                    "Camera Root is not assigned.");
            }

            if (_playerInputSource == null)
            {
                throw new InvalidOperationException(
                    "Player Input Source is not assigned.");
            }

            _playerInput =
                _playerInputSource as IPlayerInput;

            if (_playerInput == null)
            {
                throw new InvalidOperationException(
                    "Player Input Source must implement IPlayerInput.");
            }

            if (_settings == null)
            {
                throw new InvalidOperationException(
                    "Player Movement Settings are not assigned.");
            }
        }

        private void Update()
        {
            Look();
        }

        private void Look()
        {
            Vector2 input =
                _playerInput.Look;

            float horizontalRotation =
                input.x *
                _settings.LookSensitivity;

            float verticalRotation =
                input.y *
                _settings.LookSensitivity;

            transform.Rotate(
                Vector3.up,
                horizontalRotation);

            _verticalRotation -=
                verticalRotation;

            _verticalRotation =
                Mathf.Clamp(
                    _verticalRotation,
                    _settings.MinimumLookAngle,
                    _settings.MaximumLookAngle);

            _cameraRoot.localRotation =
                Quaternion.Euler(
                    _verticalRotation,
                    0f,
                    0f);
        }
    }
}