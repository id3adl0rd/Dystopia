using UnityEngine;

namespace Dungeons.Params.DungeonObject
{
    [CreateAssetMenu]
    public class DungeonObjectSO : ScriptableObject
    {
        public float _chance;
        public GameObject _gameObject;
    }
}