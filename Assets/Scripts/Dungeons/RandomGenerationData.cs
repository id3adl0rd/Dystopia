using UnityEngine;

[CreateAssetMenu(fileName = "GenerationParameters_", menuName = "GenerationParams/GenerationParameters")]
public class RandomGenerationData : ScriptableObject
{
    [field: SerializeField] public int iterations = 10;
    [field: SerializeField] public int walkLength = 10;
    [field: SerializeField] public bool startRandomlyEachIteration = true;
}