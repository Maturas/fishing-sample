using System;
using PancakeSample.Fishing;
using PancakeSample.UI;
using UnityEngine;

namespace PancakeSample.Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] 
        private PlayerCamera playerCamera;
        public PlayerCamera Camera => playerCamera;
        
        [SerializeField]
        private PlayerMovement playerMovement;
        public PlayerMovement Movement => playerMovement;
        
        [SerializeField]
        private PlayerInputController playerInputController;
        public PlayerInputController InputController => playerInputController;
        
        [SerializeField]
        private FishingResultUI fishingResultUI;
        
        [SerializeField]
        private FishingBobber fishingBobber;

        [SerializeField] 
        private Transform fishingRodEnd;
        public Transform FishingRodEnd => fishingRodEnd;

        private bool _wasPressed;
        private float _holdStartTime;

        public event Action<FishingResult> OnCaughtFishEvent;

        private void Awake()
        {
            playerInputController.OnPrimaryActionEvent += OnPrimaryAction;
        }

        private void OnDestroy()
        {
            playerInputController.OnPrimaryActionEvent -= OnPrimaryAction;
        }

        public void OnCaughtFish(FishingResult fishingResult)
        {
            OnCaughtFishEvent?.Invoke(fishingResult);
        }

        private void OnPrimaryAction(bool pressed)
        {
            if (_wasPressed == pressed) 
                return;

            if (!_wasPressed && pressed)
            {
                _holdStartTime = Time.time;
            }
            else if (_wasPressed && !pressed)
            {
                fishingBobber.OnInputAction(Time.time - _holdStartTime);
                _holdStartTime = 0.0f;
            }
            
            _wasPressed = pressed;
        }
        
        public bool CanMove => !fishingBobber.IsFishing;
    }
}