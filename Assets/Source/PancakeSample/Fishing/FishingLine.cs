using UnityEngine;

namespace PancakeSample.Fishing
{
    public class FishingLine : MonoBehaviour
    {
        [SerializeField] 
        private Transform fishingRodEnd;

        [SerializeField] 
        private FishingBobber bobber;
        
        [SerializeField]
        private LineRenderer lineRenderer;

        private void Update()
        {
            if (bobber.IsFishing)
            {
                lineRenderer.SetPosition(0, fishingRodEnd.position);
                lineRenderer.SetPosition(1, bobber.transform.position);
            }
            else
            {
                lineRenderer.SetPosition(0, Vector3.zero);
                lineRenderer.SetPosition(1, Vector3.zero);
            }
        }
    }
}