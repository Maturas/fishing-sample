using PancakeSample.Player;
using UnityEngine;

namespace PancakeSample.Fishing
{
    public class FishingBobber : MonoBehaviour
    {
        [SerializeField]
        private float minThrowForce = 2.0f;

        [SerializeField] 
        private float maxThrowForce = 60.0f;

        [SerializeField] 
        private float maxThrowHoldTime = 5.0f;

        [SerializeField] 
        private PlayerBehaviour player;
        
        public bool IsFishing { get; private set; }
        public Rigidbody MyRigidbody { get; private set; }

        private Collider _collider;
        private FishingWater _cachedWater;
        
        private void Awake()
        {
            MyRigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            
            SwitchFishingMode(false);
        }

        private void Update()
        {
            if (!IsFishing)
            {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
        }

        public void OnInputAction(float holdTime)
        {
            if (!IsFishing)
            {
                var normalizedHoldTime = Mathf.Clamp01(holdTime / maxThrowHoldTime);
                Throw(normalizedHoldTime);
            }
            else
            {
                var caughtFish = _cachedWater.TryCatchFish();
                if (caughtFish != null)
                {
                    player.OnCaughtFish(caughtFish);
                }
                
                SwitchFishingMode(false);
            }
        }

        private void Throw(float normalizedHoldTime)
        {
            SwitchFishingMode(true);
            
            var throwForce = Mathf.Lerp(minThrowForce, maxThrowForce, normalizedHoldTime);
            MyRigidbody.AddForce(player.Camera.CameraTransform.forward * throwForce, ForceMode.Impulse);
        }

        private void SwitchFishingMode(bool isFishing)
        {
            if (isFishing)
            {
                transform.SetParent(null);
            }
            else
            {
                transform.SetParent(player.FishingRodEnd, false);
            }
            
            MyRigidbody.isKinematic = !isFishing;
            MyRigidbody.useGravity = isFishing;
            MyRigidbody.constraints = isFishing ? RigidbodyConstraints.None : RigidbodyConstraints.FreezeAll;
            _collider.enabled = isFishing;
            
            _cachedWater = null;
            
            IsFishing = isFishing;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_cachedWater && other.gameObject == _cachedWater.gameObject)
                return;
            
            if (other.gameObject.CompareTag("Water"))
            {
                _cachedWater = other.gameObject.GetComponent<FishingWater>();
                _cachedWater.StartFishing(this);
            }
            else
            {
                SwitchFishingMode(false);
            }
        }
    }
}