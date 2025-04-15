using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PancakeSample.Player
{
    public class PlayerInputController : MonoBehaviour
    {
        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public event Action<bool> OnPrimaryActionEvent;

        private void SetDefaults()
        {
            Move = Vector2.zero;
            Look = Vector2.zero;
        }
        
        private void Awake()
        {
            GetComponent<PlayerInput>().ActivateInput();
        }

        private void OnEnable()
        {
            SetDefaults();
        }

        private void OnMove(InputValue value)
        {
            Move = value.Get<Vector2>();
        }
        
        private void OnLook(InputValue value)
        {
            Look = value.Get<Vector2>();
        }
        
        private void OnPrimaryAction(InputValue value)
        {
            OnPrimaryActionEvent?.Invoke(value.isPressed);
        }
    }
}