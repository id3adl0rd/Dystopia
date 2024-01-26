using UnityEngine;

[CreateAssetMenu(fileName = "GenerationParameters_", menuName = "GenerationParams/GenerationParameters")]
public class RandomGenerationData : ScriptableObject
{
    public int iterations = 10, walkLength = 10;
    public bool startRandomlyEachIteration = true;
}