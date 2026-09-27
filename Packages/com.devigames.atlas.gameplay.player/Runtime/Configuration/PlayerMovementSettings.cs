using UnityEngine;

namespace DeviGames.Atlas.Gameplay.Player.Configuration
{
    [CreateAssetMenu(
        fileName = "PlayerMovementSettings",
        menuName = "DeviGames/Atlas/Player/Movement Settings")]
    public sealed class PlayerMovementSettings :
        ScriptableObject
    {
        [SerializeField]
        private float _walkSpeed = 4f;

        [SerializeField]
        private float _runSpeed = 7f;

        [SerializeField]
        private float _gravity = -20f;

        [SerializeField]
        private float _lookSensitivity = 0.15f;

        [SerializeField]
        private float _minimumLookAngle = -80f;

        [SerializeField]
        private float _maximumLookAngle = 80f;

        public float WalkSpeed =>
            _walkSpeed;

        public float RunSpeed =>
            _runSpeed;

        public float Gravity =>
            _gravity;

        public float LookSensitivity =>
            _lookSensitivity;

        public float MinimumLookAngle =>
            _minimumLookAngle;

        public float MaximumLookAngle =>
            _maximumLookAngle;
    }
}