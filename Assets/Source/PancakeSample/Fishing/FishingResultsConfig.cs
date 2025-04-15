using UnityEngine;

namespace PancakeSample.Fishing
{
    [CreateAssetMenu(fileName = "FishingResultsConfig", menuName = "Pancake/Fishing/FishingResultsConfig")]
    public class FishingResultsConfig : ScriptableObject
    {
        public string[] FishNames;
        public float MinFishWeight;
        public float MaxFishWeight;

        public FishingResult GetRandomResult()
        {
            return new FishingResult
            {
                FishName = FishNames[Random.Range(0, FishNames.Length)],
                FishWeight = Random.Range(MinFishWeight, MaxFishWeight)
            };
        }
    }
}