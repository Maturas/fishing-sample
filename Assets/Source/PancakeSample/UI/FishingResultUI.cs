using System;
using PancakeSample.Fishing;
using PancakeSample.Player;
using TMPro;
using UnityEngine;

namespace PancakeSample.UI
{
    public class FishingResultUI : MonoBehaviour
    {
        [SerializeField]
        private PlayerBehaviour player;
        
        [SerializeField] 
        private TextMeshProUGUI label;

        [SerializeField] 
        private float autoCleanDelay = 5.0f;

        private float _autoCleanerStartTime;

        private void Awake()
        {
            player.OnCaughtFishEvent += OnFishCaught;
        }

        private void OnDestroy()
        {
            if (player)
                player.OnCaughtFishEvent -= OnFishCaught;
        }

        private void OnFishCaught(FishingResult result)
        {
            SetLabel(result.GetResultLabel());
        }

        private void SetLabel(string text)
        {
            label.text = text;
            _autoCleanerStartTime = Time.time;
        }
        
        private void Update()
        {
            if (_autoCleanerStartTime > 0.0f && Time.time - _autoCleanerStartTime > autoCleanDelay)
            {
                SetLabel(string.Empty);
                _autoCleanerStartTime = 0.0f;
            }
        }
    }
}