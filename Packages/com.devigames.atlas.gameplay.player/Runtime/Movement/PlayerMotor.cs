using System;

using DeviGames.Atlas.Gameplay.Player.Configuration;
using DeviGames.Atlas.Gameplay.Player.Input;

using UnityEngine;

namespace DeviGames.Atlas.Gameplay.Player.Movement
{
    public sealed class PlayerMotor :
        MonoBehaviour
    {
        [SerializeField]
        private CharacterController _characterController;

        [SerializeField]
        private MonoBehaviour _playerInputSource;

        [SerializeField]
        private PlayerMovementSettings _settings;

        private IPlayerInput _playerInput;

        private float _verticalVelocity;

        private void Awake()
        {
            if (_characterController == null)
            {
                throw new InvalidOperationException(
                    "Character Controller is not assigned.");
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
            Move();
        }

        private void Move()
        {
            Vector2 input =
                _playerInput.Move;

            Vector3 movement =
                transform.right * input.x +
                transform.forward * input.y;

            if (movement.sqrMagnitude > 1f)
            {
                movement.Normalize();
            }

            float speed =
                _playerInput.RunHeld
                    ? _settings.RunSpeed
                    : _settings.WalkSpeed;

            if (_characterController.isGrounded &&
                _verticalVelocity < 0f)
            {
                _verticalVelocity =
                    -2f;
            }

            _verticalVelocity +=
                _settings.Gravity *
                Time.deltaTime;

            Vector3 velocity =
                movement * speed;

            velocity.y =
                _verticalVelocity;

            _characterController.Move(
                velocity *
                Time.deltaTime);
        }
    }
}