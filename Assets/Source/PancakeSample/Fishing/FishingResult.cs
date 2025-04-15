namespace PancakeSample.Fishing
{
    public class FishingResult
    {
        public string FishName;
        public float FishWeight;

        public string GetResultLabel()
        {
            return $"You caught a {FishWeight:F1} kg {FishName}!";
        }
    }
}