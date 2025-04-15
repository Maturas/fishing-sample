using UnityEngine;

namespace PancakeSample.Fishing
{
    public class FishingWater : MonoBehaviour
    {
        private FishingBobber _cachedBobber;
        
        [SerializeField] private FishingResultsConfig resultsConfig;
        [SerializeField] private float minBaitWaitTime = 3f;
        [SerializeField] private float maxBaitWaitTime = 8f;
        [SerializeField] private float minBaitDuration = 1.5f;
        [SerializeField] private float maxBaitDuration = 3f;
        [SerializeField] private float baitForce = 1f;
        [SerializeField] private float baitInterval = 0.2f;
        
        private float _nextBaitTime;
        private float _baitEndTime;
        private float _nextBaitPushTime;

        private bool _isBaiting;
        
        private void Update()
        {
            if (_cachedBobber == null)
                return;
                
            var currentTime = Time.time;
            
            if (_isBaiting)
            {
                if (currentTime >= _baitEndTime)
                {
                    _isBaiting = false;
                    _nextBaitTime = currentTime + Random.Range(minBaitWaitTime, maxBaitWaitTime);
                }
                else if (currentTime >= _nextBaitPushTime)
                {
                    _cachedBobber.MyRigidbody.AddForce(Vector3.up * baitForce, ForceMode.Impulse);
                    _nextBaitPushTime = currentTime + baitInterval;
                }
            }
            else if (currentTime >= _nextBaitTime)
            {
                _isBaiting = true;
                _baitEndTime = currentTime + Random.Range(minBaitDuration, maxBaitDuration);
                _nextBaitPushTime = currentTime;
            }
        }
        
        public void StartFishing(FishingBobber bobber)
        {
            _cachedBobber = bobber;
            _isBaiting = false;
            _nextBaitTime = Time.time + Random.Range(minBaitWaitTime, maxBaitWaitTime);
        }
        
        public FishingResult TryCatchFish()
        {
            var caughtFish = _isBaiting;
            
            _cachedBobber = null;
            _isBaiting = false;
            
            return caughtFish ? resultsConfig.GetRandomResult() : null;
        }
    }
}